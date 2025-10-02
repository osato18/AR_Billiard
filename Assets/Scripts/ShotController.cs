using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShotController : MonoBehaviour
{
    [Header("ARSessionOrigin配下のARCameraをセット")]
    [SerializeField] private Camera _arCamera;

    private Ray _touchRay;
    private void Shot(Rigidbody cueBallRb)
    {
        cueBallRb.AddForce(_arCamera.transform.forward*5f, ForceMode.Impulse);
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