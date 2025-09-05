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

    [SerializeField] private GameObject _rayPointObj;
    [SerializeField] private GameObject _rayOriginObj;

    [SerializeField] private GameObject _raySpawnPoint;

    private RayPointManager _rayPointManager;

    private Ray _ray;

    // Start is called before the first frame update
    void Start()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _rayPointManager = _rayPointObj.GetComponent<RayPointManager>();
    }

    public void Spawn()
    {
        Instantiate(_spawnObj, _raySpawnPoint.transform.position, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_offsetObj.transform.position, 0.05f);
    }
    // Update is called once per frame
    void Update()
    {
        _ray = new Ray(_rayOriginObj.transform.position, _rayOriginObj.transform.forward);
        if (_raycastManager.Raycast(_ray, hitResults, TrackableType.AllTypes))
        {
            _rayPointManager.RayPointChanger(hitResults[0].pose.position);
        }
        Debug.DrawRay(_ray.origin, _ray.direction * 1000f, Color.cyan);
    }
}