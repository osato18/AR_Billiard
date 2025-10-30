using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public enum CueBallState
{
    Stop,
    Move,
}
public class CueBallBehavior : MonoBehaviour
{
    private readonly Subject<CueBallState> _cueBallBehaviorSub = new Subject<CueBallState>();
    public IObservable<CueBallState> CueBallBehaviorSub => _cueBallBehaviorSub;
    public CueBallState MyCueBallState;
    [SerializeField] private GameObject _cueBallPointer;
    private Rigidbody _cueBallRb;

    private void StopBall()
    {
        _cueBallRb.velocity = Vector3.zero;
        _cueBallRb.angularVelocity = Vector3.zero;
    }
    void Start()
    {
        MyCueBallState = CueBallState.Stop;
        _cueBallRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cueBallRb.velocity.magnitude < 0.1)
        {
            MyCueBallState = CueBallState.Stop;
            StopBall();
            _cueBallBehaviorSub.OnNext(MyCueBallState);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            MyCueBallState = CueBallState.Move;
            _cueBallBehaviorSub.OnNext(MyCueBallState);
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    private void OnDestroy()
    {
        _cueBallBehaviorSub.Dispose();
    }
}
