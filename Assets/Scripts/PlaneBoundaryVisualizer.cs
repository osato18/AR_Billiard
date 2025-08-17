using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(LineRenderer))]
public class PlaneBoundaryVisualizer : MonoBehaviour
{
    private ARPlane arPlane;
    private LineRenderer lineRenderer;
    void Awake()
    {
        arPlane = GetComponent<ARPlane>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.loop = true; // 線を閉じるならtrue
        lineRenderer.useWorldSpace = false; // ★ローカル座標に変更
    }

    void Update()
    {
        var boundary = arPlane.boundary;
        if (boundary.Length > 0)
        {
            lineRenderer.positionCount = boundary.Length;
            for (int i = 0; i < boundary.Length; i++)
            {
                lineRenderer.SetPosition(i, boundary[i]);
            }
        }
    }

}
