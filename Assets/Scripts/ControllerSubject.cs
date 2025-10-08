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
    private readonly Subject<TouchData> _endedCueBall=new Subject<TouchData>();

    // 外部から購読できるように公開する（IObservableにする）
    public IObservable<TouchData> BeganCueBall =>_beganCueBall;
    public IObservable<TouchData> MovedCueBall => _movedCueBall;
    public IObservable<TouchData> EndedCueBall => _endedCueBall;

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
                            Hit = hit
                        });
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && _isTouchCueBall)   //タッチ状態:指が動いた
            {
                _movedCueBall.OnNext(new TouchData
                {
                    CueBallRb = _cueBallRb,
                    MovedPos = touch.position
                });
            }

            else if (touch.phase == TouchPhase.Ended && _isTouchCueBall)
            {
                _endedCueBall.OnNext(new TouchData
                {
                    CueBallRb = _cueBallRb,
                });
                _isTouchCueBall = false;
            }
        }

    }
    private void OnDestroy()
    {
        _beganCueBall.OnCompleted();
        _beganCueBall.Dispose();
        _movedCueBall.OnCompleted();
        _movedCueBall.Dispose();
        _endedCueBall.OnCompleted();
        _endedCueBall.Dispose();
    }
}