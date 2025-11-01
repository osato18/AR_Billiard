using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public enum BallType
{
    Obj1,
    Obj2,
    Obj3,
    Obj4,
    Obj5,
    Obj6,
    Cue,
}
public class BallNumber : MonoBehaviour
{
    public BallType MyBallNum;

    private readonly Subject<Unit> _isColliderSESub = new Subject<Unit>();
    public IObservable<Unit> IsColliderSESub => _isColliderSESub;

    private Rigidbody _ballRb;

    void OnCollisionEnter(Collision collision)
    {
        _isColliderSESub.OnNext(Unit.Default);
    }

    private void StopBall()
    {
        _ballRb.velocity = Vector3.zero;
        _ballRb.angularVelocity = Vector3.zero;
    }
    void Start()
    {
        _ballRb = gameObject.GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ballRb.velocity.magnitude < 0.07)
        {
            StopBall();
        }
    }
}
