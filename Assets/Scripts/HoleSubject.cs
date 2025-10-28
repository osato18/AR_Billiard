using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public struct PocketedData
{
    public GameObject pocketedObj;
    public BallType pocketedBallType;
}
public class HoleSubject : MonoBehaviour
{
    private readonly Subject<PocketedData> _pocketedBall = new Subject<PocketedData>();
    public IObservable<PocketedData> PocketedBall => _pocketedBall;
    private void OnTriggerExit(Collider other)
    {
        BallNumber pocketedBallNumber = other.GetComponent<BallNumber>();
        _pocketedBall.OnNext(new PocketedData
        {
            pocketedObj = other.gameObject,
            pocketedBallType=pocketedBallNumber.MyBallNum
        });
    }
    private void OnDestroy()
    {
        _pocketedBall.Dispose();
    }
}
