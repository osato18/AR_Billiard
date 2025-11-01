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
    public IObservable<Unit> IsColliderSESub=> _isColliderSESub;

    void OnCollisionEnter(Collision collision)
    {
        _isColliderSESub.OnNext(Unit.Default);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
