using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public struct OBObjData
{
    public Collider obCollider;
    public Rigidbody obRb;
}
public class OBSubject : MonoBehaviour
{
    private readonly Subject<OBObjData> _obBall = new Subject<OBObjData>();
    public IObservable<OBObjData> OBBall => _obBall;

    private void OnTriggerExit(Collider other)
    {
        _obBall.OnNext(new OBObjData
        {
            obCollider = other,
            obRb=other.gameObject.GetComponent<Rigidbody>()
        });
    }
}
