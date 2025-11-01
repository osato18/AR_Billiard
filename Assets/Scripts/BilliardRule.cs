using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public struct ClearData
{
    public int PocketPoint;
    public int Life;
    public int TotalScore;
}

[RequireComponent(typeof(ARRaycastManager))]
public class BilliardRule : MonoBehaviour
{
    //UIManagerに通知
    private readonly Subject<int> _lifeSub = new Subject<int>();

    public IObservable<int> LifeSub => _lifeSub;
    private readonly Subject<int> _scoreSub = new Subject<int>();

    public IObservable<int> ScoreSub => _scoreSub;

    //PreviewManagerに通知
    private readonly Subject<Unit> _setBoardSub = new Subject<Unit>();
    public IObservable<Unit> SetBoardSub => _setBoardSub;


    //GameLoopに通知
    private readonly Subject<Unit> _gameStartSub = new Subject<Unit>();
    private readonly Subject<ClearData> _gameClearSub = new Subject<ClearData>();
    private readonly Subject<Unit> _gameOverSub = new Subject<Unit>();
    public IObservable<Unit> GameStartSub => _gameStartSub;
    public IObservable<ClearData> GameClearSub => _gameClearSub;
    public IObservable<Unit> GameOverSub => _gameOverSub;

    //SoundManagerへ通知
    private readonly Subject<Unit> _gameOverBgmSub = new Subject<Unit>();
    private readonly Subject<Unit> _gameClearBgmSub = new Subject<Unit>();
    private readonly Subject<Unit> _successSESub = new Subject<Unit>();
    private readonly Subject<Unit> _foulSESub = new Subject<Unit>();
    public IObservable<Unit> GameOverBgmSub => _gameOverBgmSub;
    public IObservable<Unit> GameClearBgmSub => _gameClearBgmSub;
    public IObservable<Unit> SuccessSESub => _successSESub;
    public IObservable<Unit> FoulSESub => _foulSESub;


    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private GameObject _rayPointObj;

    [Header("ボードオブジェクト")]
    [SerializeField] private GameObject _boardObj;

    [Header("ボールオブジェクト")]
    [SerializeField] private GameObject[] _ballObj;

    [Header("ホールのサブジェクトオブジェクト")]
    [SerializeField] private HoleSubject[] _holeSubj;

    [Header("OBサブジェクト")]
    [SerializeField] private OBSubject _OBSubj;

    [Header("ライフ")]
    [SerializeField] public int PlayerLife;
    private BallType _targetBallType;


    public GameObject TargetBallObj;
    public int Score;
    public Vector3 BaseBoardPos;
    public Quaternion BaseBoardRot;


    void Start()
    {
        _targetBallType = BallType.Obj1;
        TargetBallObj = _ballObj[(int)_targetBallType];
        //OBの購読
        _OBSubj.OBBall.Subscribe(obBall =>
        {
            PockedBallTypeFoul();
            ReSetObj(obBall);
        }).AddTo(this);

        //pocketの購読
        Observable.Merge(_holeSubj.Select(hole => hole.PocketedBall))
        .Subscribe(pocketBall =>
        {
            CompareBallType(pocketBall);
        }).AddTo(this);
    }

    public void OnChildHit(GameObject child, Collider other)
    {
        
    }


    private void CompareBallType(PocketedData ballData)
    {
        Debug.Log("pocket!_" + _targetBallType.ToString());
        if (ballData.pocketedBallType == _targetBallType)
        {
            Score += 1000;
            _scoreSub.OnNext(Score);
            _successSESub.OnNext(Unit.Default);
            if (ballData.pocketedBallType != BallType.Obj6)    //次の手玉番号に進む
            {
                EvolveBallType(ballData.pocketedObj);
            }
            else
            {
                ballData.pocketedObj.SetActive(false);
                GameClear();
            }
        }
        else
        {
            PockedBallTypeFoul();
        }
    }

    private void EvolveBallType(GameObject ballObj)
    {
        ballObj.SetActive(false);
        Debug.Log("match!");
        if (_targetBallType <= BallType.Obj6)
        {
            _targetBallType++;
            TargetBallObj = _ballObj[(int)_targetBallType];
        }
    }

    private void PockedBallTypeFoul()
    {
        _foulSESub.OnNext(Unit.Default);
        if (PlayerLife > 0)
        {
            Debug.Log("no_match");
            PlayerLife--;
            Debug.Log("" + PlayerLife);
            _lifeSub.OnNext(PlayerLife);
        }
        else
        {
            GameOver();
        }

    }

    public void SetBoard()
    {
        // レイポイント位置の少し上にボードを生成
        _boardObj.SetActive(true);
        _boardObj.transform.position = _rayPointObj.transform.position;
        _boardObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        //ボードのべ－ス座標・回転登録
        BaseBoardPos = _boardObj.transform.position;
        BaseBoardRot=_boardObj.transform.rotation;

        // プレーン検出を停止し、既存プレーンを非表示にする
        _planeManager.enabled = false;
        foreach (var plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        //PreviewManagerへ通知
        _setBoardSub.OnNext(Unit.Default);

        //GameLoopへ通知
        _gameStartSub.OnNext(Unit.Default);
    }
    private void ReSetObj(OBObjData obObjData)
    {
        float randomNum_x = UnityEngine.Random.Range(-0.2f, 0.2f);
        float randomNum_z = UnityEngine.Random.Range(-0.2f, 0.2f);
        obObjData.obCollider.gameObject.SetActive(false);
        obObjData.obCollider.transform.position = _boardObj.transform.position + new Vector3(randomNum_x, 0.1f, randomNum_z);
        obObjData.obRb.velocity = Vector3.zero;
        obObjData.obRb.angularVelocity = Vector3.zero;
        obObjData.obCollider.gameObject.SetActive(true);
    }

    private void GameClear()
    {
        _gameClearBgmSub.OnNext(Unit.Default);
        Debug.Log("GameClear!!!");
        //ゲームクリア通知（GameLoopへ）
        _gameClearSub.OnNext(new ClearData
        {
            PocketPoint = Score,
            Life = PlayerLife,
            TotalScore = Score + PlayerLife * 100,
        });
    }
    private void GameOver()
    {
        Debug.Log("GameOver...");
        //ゲームオーバー通知
        _gameOverBgmSub.OnNext(Unit.Default);
        _gameOverSub.OnNext(Unit.Default);
    }

    private void OnDestroy()
    {
        // UI通知
        _lifeSub.Dispose();
        _scoreSub.Dispose();

        // PreviewManager通知
        _setBoardSub.Dispose();

        // GameLoop通知
        _gameStartSub.Dispose();
        _gameClearSub.Dispose();
        _gameOverSub.Dispose();

        // SoundManager通知
        _gameOverBgmSub.Dispose();
        _gameClearBgmSub.Dispose();
        _successSESub.Dispose();
        _foulSESub.Dispose();
    }

}
