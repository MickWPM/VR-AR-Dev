using UnityEngine;

public class IKTargetFollowOnPlane : MonoBehaviour
{
    [SerializeField] private HumanHandRelativePosition handPositionTarget;
    [SerializeField] private Transform grabberRotationTransform;
    private Vector3 transformStartPosition;
    private float localXOffset;
    [SerializeField]private bool useMinThreshold = true;
    [SerializeField]private float zMinThreshold = 0.5f;

    private void Awake()
    {
        localXOffset = transform.localPosition.x;
    }


    private void Update()
    {
        Vector3 targetPosition = handPositionTarget.LocalPosition;
        Vector3 localPosition = targetPosition - transformStartPosition;
        localPosition.x = localXOffset;
        if (useMinThreshold && localPosition.z < zMinThreshold) localPosition.z = zMinThreshold;
        transform.position = localPosition;
    }
}
