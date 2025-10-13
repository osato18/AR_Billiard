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
    private void PrepareShotPreview(Touch touch, RaycastHit hit)
    {
        _touchBeganPos = Camera.main.ScreenToWorldPoint(touch.position);  //手玉をタップした際の画面上座標→ワールド座標変換
        //プレビュー
        _showPreviewArrowObj.transform.SetParent(hit.collider.transform);
        _showPreviewArrowObj.transform.position = hit.collider.transform.position;
        _showPreviewArrowObj.SetActive(true);
    }
    private Vector3 ShotPreview(Vector3 beganPos, Vector3 movedPos)  //予告線
    {
        Vector3 screenVector = beganPos - Camera.main.ScreenToWorldPoint(movedPos);  //移動後位置→手玉タップ位置ベクトル
        screenVector = new Vector3(screenVector.x, 0.0f, screenVector.z);
        screenVector = screenVector.normalized;     //正規化
        //プレビュー
        Quaternion arrowRot = Quaternion.LookRotation(screenVector);
        _showPreviewArrowObj.transform.rotation = arrowRot;
        return screenVector;
    }

    private void AdjustShotPower()
    {
        if (_isIncreasing)
        {
            _shotPower += _deltaShotPower * Time.deltaTime;
            if (_shotPower >= _maxShotPower)
            {
                _shotPower = _maxShotPower;
                _isIncreasing = false;
            }
        }
        else
        {
            _shotPower -= _deltaShotPower * Time.deltaTime;
            if (_shotPower <= 0)
            {
                _shotPower = 0;
                _isIncreasing = true;
            }
        }
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
            Observable.EveryUpdate().TakeUntil(_controllerSub.EndedCueBall).Subscribe(_ =>
            {
                AdjustShotPower();
            }).AddTo(this);
        }).AddTo(this);     //GameObject破棄時に自動購読解除

        //_MovedCueBallを購読
        _controllerSub.MovedCueBall.Subscribe(movedData =>
        {
            _shotVector = ShotPreview(_touchBeganPos, movedData.MovedPos);
            _textObj2.text = Camera.main.ScreenToWorldPoint(movedData.MovedPos).ToString();
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
    private void Update()
    {
        //Debug用
        _textObj1.text = _touchBeganPos.ToString();
    }
}