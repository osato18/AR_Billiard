using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
public class ShotController : MonoBehaviour
{
    //Observerパターン
    [Header("ControllerSubjectのオブジェ")]
    [SerializeField] private ControllerSubject _controllerSub;

    [Header("ガイド矢印")]
    [SerializeField] private GameObject _guideArrowObj;

    [Header("プレビュー矢印")]
    [SerializeField] private GameObject _previewArrowObj;

    [Header("飛ばす力（最大値）")]
    [SerializeField] private float _maxShotPower;

    [Header("飛ばす力（増幅度）")]
    [SerializeField] private float _deltaShotPower;

    //Debug用
    [SerializeField] private TextMeshProUGUI _textObj1;
    [SerializeField] private TextMeshProUGUI _textObj2;

    private Vector3 _touchBeganPos;
    private Vector3 _shotVector;
    private GameObject _showPreviewArrowObj;
    private float _shotPower;
    private bool _isIncreasing = true;
    private GameObject _cueBallObj;
    private void PrepareShotPreview(Touch touch, RaycastHit hit)
    {
        _touchBeganPos = touch.position;  //手玉をタップした際の画面上座標
        //プレビュー
        //_showPreviewArrowObj.transform.SetParent(hit.collider.transform);
        _cueBallObj= hit.collider.gameObject;
        _showPreviewArrowObj.SetActive(true);
    }
    private Vector3 ShotPreview(Vector3 cameraForwardVec)  //予告線
    {
        Vector3 shotVector = new Vector3(cameraForwardVec.x, 0, cameraForwardVec.z);
        shotVector = shotVector.normalized;
        //プレビュー（方向）
        Quaternion arrowRot = Quaternion.LookRotation(shotVector);
        _showPreviewArrowObj.transform.rotation = arrowRot;
        //プレビュー（大きさ）
        _showPreviewArrowObj.transform.localScale = new Vector3(_shotPower, 1, _shotPower);
        //プレビュー（位置）
        _showPreviewArrowObj.transform.position = _cueBallObj.transform.position;
        return shotVector;
    }

    private float AdjustShotPower(Vector3 beganPos, Vector3 movedPos)
    {
        Vector3 screenVector = beganPos - movedPos;  //移動後位置→手玉タップ位置ベクトル
        return screenVector.magnitude / 600;  //規格化
    }
    private void Shot(Rigidbody cueBallRb, Vector3 forceVector)  //球を発射
    {
        forceVector = forceVector * _shotPower;
        cueBallRb.AddForce(forceVector, ForceMode.Impulse);
        _showPreviewArrowObj.SetActive(false);
    }

    void Start()
    {
        //_BeganCueBallを購読
        _controllerSub.BeganCueBall.Subscribe(beganData =>
        {
            _shotPower = 0.0f;
            PrepareShotPreview(beganData.Touch, beganData.Hit);

            ;           //指を離したら停止
            /*Observable.EveryUpdate().TakeUntil(_controllerSub.EndedCueBall).Subscribe(_ =>
            {

            }).AddTo(this);*/
        }).AddTo(this);     //GameObject破棄時に自動購読解除

        //_MovedCueBallを購読
        _controllerSub.MovedCueBall.Subscribe(movedData =>
        {
            _shotVector = ShotPreview(movedData.CameraForwardVec);
            _shotPower = AdjustShotPower(_touchBeganPos, movedData.MovedPos);
            //Debug用
            //_textObj1.text = _shotPower.ToString();
            //_textObj2.text = _previewArrowObj.ToString();
        }).AddTo(this);     //GameObject破棄時に自動購読解除

        //_EndedCueBallを購読
        _controllerSub.EndedCueBall.Subscribe(endedData =>
        {
            Shot(endedData.CueBallRb, _shotVector);
        }).AddTo(this);     //GameObject破棄時に自動購読解除

        //プレビュー矢印生成＆非表示
        _showPreviewArrowObj = Instantiate(_previewArrowObj);
        _showPreviewArrowObj.SetActive(false);
    }
}