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
    void Start()
    {
        MyCueBallState = CueBallState.Stop;
        _cueBallRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cueBallRb.velocity.magnitude < 0.07)
        {
            MyCueBallState = CueBallState.Stop;
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
