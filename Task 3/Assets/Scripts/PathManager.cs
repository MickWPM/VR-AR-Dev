using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
    public SplineContainer spline;

    public Transform ApproachLanding, StartLanding, MidLanding, EndLanding;
    public Transform[] waypoints;
    public SplineAnimate aircraftAnimator;
    public int nextWaypointIndex = 1;

    [ContextMenu("Setup spline")]
    public void SetupSpline()
    {
        var targetSpline = spline.Spline;
        targetSpline.Clear();

        void AddKnot(Transform t, bool autoKnot=true)
        {
            Vector3 localPosition = spline.transform.InverseTransformPoint(t.position);
            TangentMode mode = autoKnot ? TangentMode.AutoSmooth : TangentMode.Linear;
            targetSpline.Add(new BezierKnot(localPosition));
            
            int knotIndex = targetSpline.Count - 1;
            targetSpline.SetTangentMode(knotIndex, mode);
        }

        foreach (var wp in waypoints)
        {
            AddKnot(wp);
        }

        AddKnot(ApproachLanding);
        AddKnot(StartLanding);
        AddKnot(MidLanding, false);
        AddKnot(EndLanding, false); 
        
        Debug.Log($"Spline setup complete with {targetSpline.Count} knots.");
    }

    private void Awake()
    {
        SetupSpline();
    }
    private void Update()
    {
        UpdateSpline();
    }

    public void UpdateSpline()
    {
        var targetSpline = spline.Spline;

        //As we are changing waypoints we need to ensure that we to scale our current position by absolute distance travelled.
        //Otherwise the aircraft will move along the spline as the total path length is shorter/longer
        //This is because the aircraft animate component uses a distance in the range 0-1 to represent the animation
        float originalLength = spline.CalculateLength();
        float currentAbsoluteDistance = aircraftAnimator.NormalizedTime * originalLength;

        CheckKnotPassed(currentAbsoluteDistance);


        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i < nextWaypointIndex) continue;

            Vector3 localPosition = spline.transform.InverseTransformPoint(waypoints[i].position);

            BezierKnot currentKnot = targetSpline[i];
            currentKnot.Position = localPosition;
            targetSpline[i] = currentKnot;
        }

        float newLength = spline.CalculateLength();

        if (newLength > 0.001f)
        {
            aircraftAnimator.NormalizedTime = currentAbsoluteDistance / newLength;
        }
    }

    private void CheckKnotPassed(float currentDist)
    {
        float nextKnotDist = GetDistanceToKnot(nextWaypointIndex);
        if (currentDist >= nextKnotDist)
        {
            nextWaypointIndex++;

            if (nextWaypointIndex >= spline.Spline.Count)
            {
                nextWaypointIndex = spline.Spline.Count - 1;
            }

            Debug.Log($"TODO - RAISE AN EVENT... Passed a knot! Now heading towards knot {nextWaypointIndex}");
        }
    }

    private float GetDistanceToKnot(int knotIndex)
    {
        var targetSpline = spline.Spline;
        float distance = 0f;

        for (int i = 0; i < knotIndex; i++)
        {
            if (i < targetSpline.GetCurveCount())
            {
                distance += targetSpline.GetCurveLength(i);
            }
        }

        return distance;
    }
}
