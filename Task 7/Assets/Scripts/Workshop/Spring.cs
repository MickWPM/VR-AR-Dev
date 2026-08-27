using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spring : MonoBehaviour
{
    public Transform end1;
    public Transform end2;
    public float stiffness;
    private float restLength;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        restLength = Vector3.Distance(end1.position, end2.position);
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, end1.position);
        lineRenderer.SetPosition(1, end2.position);
    }

    //public Vector3 GetForce(Transform end)
    //{
    //    Vector3 d = (end1.position - end2.position).normalized;
    //    float length = (end1.position - end2.position).magnitude;


    //}

    public Vector3 GetForce(Transform t)
    {
        Vector3 d = (end1.position - end2.position).normalized;
        float length = (end1.position - end2.position).magnitude;
        Vector3 force = -stiffness * (length - restLength) * d;
        if (t == end2)
        {
            force = -force;
        }
        return force / 2f;
    }
}
