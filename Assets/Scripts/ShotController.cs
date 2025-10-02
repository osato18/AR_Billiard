using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShotController : MonoBehaviour
{
    [Header("ARSessionOrigin配下のARCameraをセット")]
    [SerializeField] private Camera _arCamera;

    [Header("飛ばす力")]
    [SerializeField] private float _shotPower;
    //Debug用
    [SerializeField] private TMP_Text _DBAddFText;
    private Ray _touchRay;
    private void Shot(Rigidbody cueBallRb)
    {
        Vector3 _shotVec = new Vector3(_arCamera.transform.forward.x, 0.0f, _arCamera.transform.forward.z) * _shotPower;
        cueBallRb.AddForce(_shotVec, ForceMode.Impulse);
        _DBAddFText.text = _shotVec.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //タッチしたオブジェクトを判定
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _touchRay = _arCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(_touchRay, out RaycastHit hit))
                {
                    Debug.Log("" + hit.collider.gameObject.name);
                    if (hit.collider.CompareTag("CueBall"))
                    {
                        Debug.Log("CueBall!!!");
                        Shot(hit.rigidbody);
                    }
                }
            }
        }
    }
}