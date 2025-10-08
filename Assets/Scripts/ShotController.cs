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

    [Header("プレビュー矢印")]
    [SerializeField] private GameObject _previewArrowObj;

    [Header("飛ばす力")]
    [SerializeField] private float _shotPower;
    private Vector3 _touchBeganPos;
    private Vector3 _shotVector;

    private GameObject _showPreviewArrowObj;
    private void PrepareShotPreview(Touch touch,RaycastHit hit)
    {
        _touchBeganPos = touch.position;  //手玉をタップした際の画面上座標
        //プレビュー
        _showPreviewArrowObj.transform.SetParent(hit.collider.transform);
        _showPreviewArrowObj.transform.position = hit.collider.transform.position;
        _showPreviewArrowObj.SetActive(true);
    }
    private Vector3 ShotPreview(Vector3 beganPos, Vector3 movedPos)  //予告線
    {
        Vector3 screenVector = beganPos - movedPos;  //移動後位置→手玉タップ位置ベクトル
        screenVector = new Vector3(screenVector.x, 0.0f, screenVector.y);
        screenVector = screenVector.normalized;     //正規化
        //プレビュー
        Quaternion arrowRot = Quaternion.LookRotation(screenVector);
        _showPreviewArrowObj.transform.rotation = arrowRot;
        return screenVector;
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
        _controllerSub.BeganCueBall.Subscribe(beganData=>
        {
            PrepareShotPreview(beganData.Touch, beganData.Hit);
        }).AddTo(this);     //GameObject破棄時に自動購読解除

        //_MovedCueBallを購読
        _controllerSub.MovedCueBall.Subscribe(movedData =>
        {
            _shotVector = ShotPreview(_touchBeganPos, movedData.MovedPos);
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