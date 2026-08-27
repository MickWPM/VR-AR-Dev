using UnityEngine;

public class BaseRotationSetter : MonoBehaviour 
{
    public Transform rotationBase;
    public Transform target;

    public Vector3 directionVector;
    void Update()
    {
        Vector3 targetOnRotationPlane = new Vector3(target.position.x, rotationBase.position.y, target.position.z);
        Vector3 dir = targetOnRotationPlane - rotationBase.position;
        directionVector = dir;
    }
}
