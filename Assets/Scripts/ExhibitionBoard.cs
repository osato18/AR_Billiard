using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ExhibitionBoard : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [Header("回転の中心（例：テーブル）")]
    [SerializeField] private Transform target;

    [Header("回転半径")]
    [SerializeField] private float distance = 2.0f;

    [Header("回転速度 (度/秒)")]
    [SerializeField] private float rotationSpeed = 30f;

    [Header("カメラの高さ")]
    [SerializeField] private float height = 1.0f;

    private float currentAngle = 0f;

    void Update()
    {
        // 回転角度を時間経過で増加
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        // 中心からのオフセット計算（水平円運動）
        Vector3 offset = new Vector3(
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * distance,
            height,
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * distance
        );

        // カメラ位置更新
        _camera.transform.position = target.position + offset;

        // 常に中心（テーブル）を見る
        _camera.transform.LookAt(target);
    }
}
