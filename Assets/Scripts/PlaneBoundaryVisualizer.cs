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
    }

    void Update()
    {
        var boundary = arPlane.boundary;
        if (boundary.Length > 0)
        {
            lineRenderer.positionCount = boundary.Length + 1;
            for (int i = 0; i < boundary.Length; i++)
            {
                lineRenderer.SetPosition(i, boundary[i]);
            }
            // 最後の点を最初に戻す
            lineRenderer.SetPosition(boundary.Length, boundary[0]);
        }
    }
}
