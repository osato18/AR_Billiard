using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureManager : MonoBehaviour
{
    Rigidbody _lureRb;
    private void Start()
    {
        _lureRb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ARPlane"))
        {
            _lureRb.velocity=Vector3.zero;
        }
    }
}
