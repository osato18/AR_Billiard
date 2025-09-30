using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject _lureObj;
    [SerializeField] private GameObject _castOriginObj;

    private GameObject _castedLure;
    private bool _casted = false;
    private bool _fishing = false;
    private float _biteTime;

    public void Cast(Vector3 posePos)
    {
        Vector3 castVector = posePos - _castOriginObj.transform.position;
        _castedLure = Instantiate(_lureObj, _castOriginObj.transform.position, Quaternion.identity);
        _casted = true;
        Rigidbody lureRb = _castedLure.GetComponent<Rigidbody>();
        lureRb.AddForce(castVector, ForceMode.Impulse);
        _fishing=true;
    }

    private void Fight()
    {
        //ルアー巻き上げ処理
        //釣ったor逃がした
        Destroy(_castedLure);
        _fishing =false;
    }
    void Update()
    {
        if (Input.touchCount > 0&&!_fishing)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hitlist = new List<ARRaycastHit>();
                if (_raycastManager.Raycast(touch.position, hitlist, TrackableType.AllTypes))
                {
                    Pose hitPose = hitlist[0].pose;
                    Cast(hitPose.position);
                }
            }
        }

        if (_casted)
        {
            _biteTime = Random.Range(0f, 5.0f);
            //Invoke("Fight", _biteTime);
            _casted=false;
        }
    }
}
