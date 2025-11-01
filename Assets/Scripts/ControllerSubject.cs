using UniRx;
using UnityEngine;
using System;

public struct TouchData
{
    public Touch Touch;
    public RaycastHit Hit;
    public Rigidbody CueBallRb;
    public Vector3 BeganPos;
    public Vector3 MovedPos;
    public Vector3 DiagonalUpVec;
    public bool IsTouch;
}
public class ControllerSubject : MonoBehaviour
{
    [Header("ARSessionOrigin配下のARCameraをセット")]
    [SerializeField] private Camera _arCamera;
    private Ray _touchRay;
    private Rigidbody _cueBallRb;
    private bool _isTouchCueBall = false;

    // UniRxのSubject：通知を流せるオブジェク
    private readonly Subject<TouchData> _beganCueBall=new Subject<TouchData>();
    private readonly Subject<TouchData> _movedCueBall = new Subject<TouchData>();
    private readonly Subject<TouchData> _endedCueBall = new Subject<TouchData>();
    private readonly Subject<bool> _nonTouchCueBall = new Subject<bool>();
    private readonly Subject<Unit> _isShotSESun=new Subject<Unit>();

    // 外部から購読できるように公開する（IObservableにする）
    public IObservable<TouchData> BeganCueBall =>_beganCueBall;
    public IObservable<TouchData> MovedCueBall => _movedCueBall;
    public IObservable<TouchData> EndedCueBall => _endedCueBall;
    public IObservable<bool> NonTouchCueBall => _nonTouchCueBall;
    public IObservable<Unit> IsShotSESub=>_isShotSESun;
    void Update()
    {
        if (Input.touchCount > 0)   //タッチ数カウント
        {
            Touch touch = Input.GetTouch(0);    //1本目のタッチ情報取得
            if (touch.phase == TouchPhase.Began)    //タッチ状態:タップ時
            {
                _touchRay = _arCamera.ScreenPointToRay(touch.position);    //ray発射

                if (Physics.Raycast(_touchRay, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("CueBall"))
                    {
                        _isTouchCueBall = true;
                        _cueBallRb = hit.rigidbody;
                        _beganCueBall.OnNext(new TouchData
                        {
                            Touch = touch,
                            Hit = hit,
                            IsTouch=_isTouchCueBall,
                        });
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && _isTouchCueBall)   //タッチ状態:指が動いた
            {
                _movedCueBall.OnNext(new TouchData
                {
                    CueBallRb = _cueBallRb,
                    MovedPos = touch.position,
                    DiagonalUpVec = _arCamera.transform.up+_arCamera.transform.forward,
                    IsTouch=_isTouchCueBall,
                });
            }

            else if (touch.phase == TouchPhase.Ended && _isTouchCueBall)
            {
                _isShotSESun.OnNext(Unit.Default);
                _endedCueBall.OnNext(new TouchData
                {
                    CueBallRb = _cueBallRb,
                });
                _isTouchCueBall = false;
            }
        }
        else
        {
            _nonTouchCueBall.OnNext(_isTouchCueBall);
        }

    }
    private void OnDestroy()
    {
        _beganCueBall.Dispose();
        _movedCueBall.Dispose();
        _endedCueBall.Dispose();
        _nonTouchCueBall.Dispose();
    }
}