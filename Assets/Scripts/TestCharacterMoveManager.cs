using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterMoveManager : MonoBehaviour
{
    [SerializeField] private GameObject _centerPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_centerPoint.transform.position, _centerPoint.transform.up, 360 / 10 * Time.deltaTime);
    }
}
