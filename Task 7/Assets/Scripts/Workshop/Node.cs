using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public List<Spring> springs;
    public float mass = 1f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 position;

    public bool dynamic = true;

    private void Awake()
    {
        springs = new List<Spring>();
        position = transform.position;
    }
    public void AddSpring(Spring spring)
    {
        springs.Add(spring);
    }

    public void DoDynamics()
    {
        if (dynamic == false)
        {
            return;
        }
        Vector3 force = Vector3.zero;

        foreach (Spring spring in springs)
        {
            force += spring.GetForce(transform);
        }

        //force += mass * 2.81f * (-Vector3.up);
        Vector3 a = force / mass;
        velocity += Cloth.deltaTime * a * 0.97f;
        position += Cloth.deltaTime * velocity;
    }

    public void StepTime()
    {
        transform.position = position;
    }
}