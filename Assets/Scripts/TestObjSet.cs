using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjSet : MonoBehaviour
{
    Rigidbody _objRb;
    // Start is called before the first frame update
    void Start()
    {
        _objRb=gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _objRb.velocity=Vector3.zero;
    }
}
