using UnityEngine;

public class IKTargetFollowOnPlane : MonoBehaviour
{
    [SerializeField]private Transform transformToEmulate;
    [SerializeField] private Transform grabberRotationTransform;
    private Vector3 transformStartPosition;
    private float localXOffset;

    public Transform TransformToEmulate { get => transformToEmulate; }//set => InitialiseTransform(value); }

    private void Awake()
    {
        localXOffset = transform.localPosition.x;
       // TransformToEmulate = transformToEmulate;
        transformStartPosition = transformToEmulate.position;
    }

    //void InitialiseTransform(Transform t)
    //{
    //    TransformToEmulate = t;
    //}

    private void Update()
    {
        Vector3 positionOnPlane = transformToEmulate.position;
        Vector3 localPosition = positionOnPlane - transformStartPosition;
        localPosition.x = localXOffset;
        transform.localPosition = localPosition;

        Vector3 emulatedRotation = new Vector3(transformToEmulate.localEulerAngles.x, 0, 0);
        grabberRotationTransform.localRotation = Quaternion.Euler(emulatedRotation);
    }
}
