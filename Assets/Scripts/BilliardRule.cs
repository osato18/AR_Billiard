using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class BilliardRule : MonoBehaviour
{
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
    [SerializeField] private int _playerLife;
    private BallType _targetBallType;


    public GameObject TargetBallObj;
    public int Score;


    void Start()
    {
        _targetBallType = BallType.Obj1;
        TargetBallObj=_ballObj[(int)_targetBallType];
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

    private void CompareBallType(PocketedData ballData)
    {
        Debug.Log("pocket!_" + _targetBallType.ToString());
        if (ballData.pocketedBallType == _targetBallType)
        {
            EvolveBallType(ballData.pocketedObj);
            Score += 1000;
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
            TargetBallObj=_ballObj[(int)_targetBallType];
        }
    }

    private void PockedBallTypeFoul()
    {
        if (_playerLife > 0)
        {
            Debug.Log("no_match");
            _playerLife--;
            Debug.Log("" + _playerLife);
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
        _boardObj.transform.position = _rayPointObj.transform.position + (_rayPointObj.transform.up / 3);
        _boardObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        // プレーン検出を停止し、既存プレーンを非表示にする
        _planeManager.enabled = false;
        foreach (var plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
    private void ReSetObj(OBObjData obObjData)
    {
        float randomNum_x = Random.Range(-0.5f, 0.5f);
        float randomNum_z = Random.Range(-0.5f, 0.5f);
        obObjData.obCollider.gameObject.SetActive(false);
        obObjData.obCollider.transform.position = _boardObj.transform.position + new Vector3(randomNum_x, 0.1f, randomNum_z);
        obObjData.obRb.velocity = Vector3.zero;
        obObjData.obRb.angularVelocity = Vector3.zero;
        obObjData.obCollider.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        Debug.Log("GameOver...");
    }
}
