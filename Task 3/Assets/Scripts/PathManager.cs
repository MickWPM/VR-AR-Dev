using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
    public SplineContainer spline;

    private Transform ApproachLanding, StartLanding, MidLanding, EndLanding;
    public Transform[] waypoints;
    public SplineAnimate aircraftAnimator;
    public int nextWaypointIndex = 1;

    private bool setupComplete = false;
    private Vector3[] waypointPositions;

    public void SetupPath(LandingStrip landingStrip)
    {
        waypointPositions = new Vector3[waypoints.Length];

        ApproachLanding = landingStrip.ApproachLanding;
        StartLanding = landingStrip.StartLanding;
        MidLanding = landingStrip.MidLanding;
        EndLanding = landingStrip.EndLanding;

        waypoints[0].transform.position = transform.position;
        waypointPositions[0] = transform.position;

        Vector3 vectorToTarget = ApproachLanding.position - transform.position;
        float distance = vectorToTarget.magnitude;  
        Vector3 direction = vectorToTarget.normalized;

        int numWaypointsToMove = waypoints.Length - 1;
        float distanceToUseWhenPositioning = 0.9f;
        float distancePerWaypoint = distanceToUseWhenPositioning * distance / numWaypointsToMove;
        for (int i = 1; i < waypoints.Length; i++)
        {
            Vector3 newPos = i * distancePerWaypoint * direction + transform.position;
            waypoints[i].transform.position = newPos;
            waypointPositions[i] = newPos;
        }

        SetupSpline();
        Destroy(waypoints[0].gameObject);

        setupComplete = true;
    }

    private void SetupSpline()
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


    private void Update()
    {
        if (setupComplete == false) return;

        if (PathDirty())
            UpdateSpline();
    }

    private float dirtyThreshold = 0.05f;
    private bool PathDirty()
    {

        var targetSpline = spline.Spline;
        //As we are changing waypoints we need to ensure that we to scale our current position by absolute distance travelled.
        //Otherwise the aircraft will move along the spline as the total path length is shorter/longer
        //This is because the aircraft animate component uses a distance in the range 0-1 to represent the animation
        splineOriginalLength = spline.CalculateLength();
        aircraftAbsoluteSplineDistance = aircraftAnimator.NormalizedTime * splineOriginalLength;

        CheckKnotPassed(aircraftAbsoluteSplineDistance);


        bool isDirty = false;
        for (int i = nextWaypointIndex; i < waypointPositions.Length; i++)
        {
            if (Vector3.Distance(waypoints[i].transform.position, waypointPositions[i]) > dirtyThreshold)
            {
                isDirty = true;
                waypointPositions[i] = waypoints[i].transform.position;
            }
        }
        return isDirty;
    }

    private float splineOriginalLength;
    private float aircraftAbsoluteSplineDistance;
    public void UpdateSpline()
    {
        var targetSpline = spline.Spline;
        //.........

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
            aircraftAnimator.NormalizedTime = aircraftAbsoluteSplineDistance / newLength;
        }
    }


    private void CheckKnotPassed(float currentDist)
    {
        float nextKnotDist = GetDistanceToKnot(nextWaypointIndex);
        if (currentDist >= nextKnotDist)
        {
            if (nextWaypointIndex < waypoints.Length)
            {
                Destroy(waypoints[nextWaypointIndex].gameObject);
            }
            nextWaypointIndex++;

            if (nextWaypointIndex >= spline.Spline.Count)
            {
                nextWaypointIndex = spline.Spline.Count - 1;
                Destroy(this.gameObject);
                Debug.Log("TODO - RAISE EVENT THAT WE HAVE FINISHED THE LANDING");
            }
            else
            {
                Debug.Log($"TODO - RAISE AN EVENT... Passed a knot! Now heading towards knot {nextWaypointIndex}");
            }
        }
    }

    public void CleanupOnCollision()
    {
        foreach (var waypoint in waypoints)
        {
            if (waypoint != null)
            {
                Destroy(waypoint.gameObject);
            }
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
