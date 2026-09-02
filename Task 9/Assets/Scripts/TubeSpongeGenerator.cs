using UnityEngine;
using UnityEngine.Splines;

public class TubeSpongeGenerator : MonoBehaviour
{
    public TubeSpongeSegment[] segments;
    private SplineContainer splineContainer;
    private Spline spline;
    public SplineExtrude extruder;
    UnityEngine.Splines.SplineMesh

    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        if ( splineContainer == null )
        {
            splineContainer = gameObject.AddComponent<SplineContainer>();
        }
        spline = splineContainer.Spline;
    }

 


    public bool showGizmoDebug = false;

    [ContextMenu("Create spline")]
    private void CreateSpline()
    {
        spline.Clear();
        SplineData<float> widthDataChannel = new SplineData<float>();
        widthDataChannel.PathIndexUnit = PathIndexUnit.Knot;

        for (int i = 0; i < segments.Length; i++)
        {
            Vector3 pos = transform.TransformPoint(segments[i].localPos);
            BezierKnot knot = new BezierKnot(pos);
            spline.Add(knot);
            widthDataChannel.Add(i, segments[i].size);
        }
        spline.Closed = false;
        spline.SetTangentMode(TangentMode.AutoSmooth);
        spline.SetFloatData("Width", widthDataChannel);


        extruder.Rebuild();
    }


    private void OnDrawGizmos()
    {
        if (segments == null || segments.Length < 2) return;
        for (int i = 1; i < segments.Length; i++)
        {
            Vector3 posA = transform.TransformPoint(segments[i - 1].localPos);
            Vector3 posB = transform.TransformPoint(segments[i].localPos);
            Gizmos.DrawLine(posA, posB);
        }

    }



    [System.Serializable]
    public struct TubeSpongeSegment
    {
        public Vector3 localPos;
        public float size;
    }
}
