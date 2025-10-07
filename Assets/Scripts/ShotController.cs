using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShotController : MonoBehaviour
{
    [Header("ARSessionOrigin配下のARCameraをセット")]
    [SerializeField] private Camera _arCamera;

    [Header("プレビュー矢印")]
    [SerializeField] private GameObject _previewArrowObj;

    [Header("飛ばす力")]
    [SerializeField] private float _shotPower;
    //Debug用
    [SerializeField] private TMP_Text _DebugText;
    [SerializeField] private TMP_Text _DebugText2;

    private Ray _touchRay;

    private Vector3 _touchBeganPos;
    private Vector3 _touchMovedPos;
    private Vector3 _shotVector;
    private Rigidbody _cueBallRb;

    private GameObject _showPreviewArrowObj;

    private bool _isTouchCueBall = false;

    private Vector3 ShotPreview(Vector3 beganPos, Vector3 movedPos)  //予告線
    {
        Vector3 screenVector = beganPos - movedPos;  //移動後位置→手玉タップ位置ベクトル
        screenVector = new Vector3(screenVector.x,0.0f,screenVector.y);
        screenVector = screenVector.normalized;     //正規化
        //プレビュー
        Quaternion arrowRot = Quaternion.LookRotation(screenVector);
        _showPreviewArrowObj.transform.rotation = arrowRot;
        return screenVector;                                        
    }
    private void Shot(Rigidbody cueBallRb,Vector3 forceVector)  //球を発射
    {
        forceVector = forceVector* _shotPower;
        cueBallRb.AddForce(forceVector, ForceMode.Impulse);
    }

    void Start()
    {
        _showPreviewArrowObj = Instantiate(_previewArrowObj);
        _showPreviewArrowObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //タッチしたオブジェクトを判定
        if (Input.touchCount > 0)   //タッチ数カウント
        {
            Touch touch = Input.GetTouch(0);    //1本目のタッチ情報取得
            if (touch.phase == TouchPhase.Began)    //タッチ状態:タップ時
            {
                _touchRay = _arCamera.ScreenPointToRay(touch.position);    //ray発射

                if (Physics.Raycast(_touchRay, out RaycastHit hit))
                {
                    Debug.Log("" + hit.collider.gameObject.name);
                    if (hit.collider.CompareTag("CueBall"))
                    {
                        _isTouchCueBall = true;
                        _touchBeganPos = touch.position;  //手玉をタップした際の画面上座標
                        _cueBallRb = hit.rigidbody;

                        //プレビュー
                        _showPreviewArrowObj.transform.parent = hit.collider.gameObject.transform;
                        _showPreviewArrowObj.transform.position=hit.collider.transform.position;
                        _showPreviewArrowObj.SetActive(true);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && _isTouchCueBall)   //タッチ状態:指が動いた
            {
                _touchMovedPos = touch.position;
                _shotVector = ShotPreview(_touchBeganPos, _touchMovedPos);
            }

            else if (touch.phase == TouchPhase.Ended && _isTouchCueBall)
            {
                Shot(_cueBallRb, _shotVector);
                _showPreviewArrowObj.SetActive(false);
                _isTouchCueBall = false;
            }
        }
    }
}