using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeightRotationAdjuster : MonoBehaviour
{
    [Header("ボードオブジェクト")]
    [SerializeField] private GameObject _boardObj;

    [Header("各スライダー")]
    [SerializeField] private Slider _heightSlider;
    [SerializeField] private Slider _rotationSlider;

    [Header("BilliardRule")]
    [SerializeField] private BilliardRule _billiardRule;

    private bool _isHeightSliding = false;
    private bool _isRotationSliding = false;
    private float _beforeRotY;

    // ====== スライダーイベント ======

    public void OnHeightChanged()
    {
        if (!_isHeightSliding) return; // 押している間のみ反応

        Vector3 pos = _boardObj.transform.position;
        pos.y = _billiardRule.BaseBoardPos.y + _heightSlider.value;
        _boardObj.transform.position = pos;
    }

    public void OnRotationChanged()
    {
        if (!_isRotationSliding) return; // 押している間のみ反応
        float newY = _beforeRotY + _rotationSlider.value;
        _boardObj.transform.rotation = Quaternion.Euler(0, newY, 0);
    }

    // ====== EventTrigger から呼ばれる ======
    public void OnHeightPointerDown()
    {
        _isHeightSliding = true;
    }

    public void OnHeightPointerUp()
    {
        _isHeightSliding = false;
    }

    public void OnRotationPointerDown()
    {
        _isRotationSliding = true;
        _beforeRotY = _boardObj.transform.rotation.eulerAngles.y;
    }

    public void OnRotationPointerUp()
    {
        _isRotationSliding = false;
        _rotationSlider.value = 0;
    }
}
