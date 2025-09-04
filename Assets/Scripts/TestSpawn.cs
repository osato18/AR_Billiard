using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class TestSpawn : MonoBehaviour
{

    //ARRayCastのコード
    public TrackableType type;

    ARRaycastManager _raycastManager;
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    //テスト
    [SerializeField] private GameObject _spawnObj;
    [SerializeField] private GameObject _offsetObj;
    // Start is called before the first frame update
    void Start()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }

    public void Spawn()
    {
        if (_raycastManager.Raycast(Input.GetTouch(0).position, hitResults, TrackableType.AllTypes))
        {
            Instantiate(_spawnObj, hitResults[0].pose.position, Quaternion.Euler(0, -90, 0));  //オブジェクトの生成
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_offsetObj.transform.position, 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
    }
}