using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CueBallState
{
    Stop,
    Move,
}
public class CueBallBehavior : MonoBehaviour
{
    public CueBallState MyCueBallState;
    [SerializeField] private GameObject _cueBallPointer;
    private Rigidbody _cueBallRb;
    // Start is called before the first frame update
    void Start()
    {
        _cueBallRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cueBallRb.velocity.magnitude < 0.07)
        {
            MyCueBallState = CueBallState.Stop;
            _cueBallRb.velocity = Vector3.zero;
            _cueBallRb.angularVelocity = Vector3.zero;
            _cueBallPointer.transform.position = gameObject.transform.position;
            _cueBallPointer.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            MyCueBallState = CueBallState.Move;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            _cueBallPointer.SetActive(false);
        }
    }
}
