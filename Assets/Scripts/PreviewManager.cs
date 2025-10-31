using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PreviewManager : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] private BilliardRule _billiardRule;
    [SerializeField] private GameObject _previewObj;
    [SerializeField] private GameObject _previewButton;

    private bool _isPreparing = true;
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        _billiardRule.SetBoardSub.Subscribe(_ =>
        {
            _isPreparing = false;
            _previewObj.SetActive(false);
        }).AddTo(this);
    }

    void Update()
    {
        if (_isPreparing)
        {
            // 例：画面中央からRayを飛ばす
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
            {
                // 平面にヒットした場合
                _previewButton.SetActive(true);
                _previewObj.SetActive(true);

                var hitPose = hits[0].pose; // ヒット地点の位置と回転
                _previewObj.transform.position = hitPose.position;

            }
            else
            {
                _previewButton.SetActive(false);
                _previewObj.SetActive(false);
            }
        }

    }
}
