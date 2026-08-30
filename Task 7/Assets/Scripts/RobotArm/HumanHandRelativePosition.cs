using UnityEngine;

public class HumanHandRelativePosition : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    public Vector3 LocalPosition { get => GetLocalPosition(); }

    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float movementScale = 1f;


    public Vector3 GetLocalPosition()
    {
        Vector3 localPos = handTransform.localPosition;
        localPos *= movementScale;
        localPos.y += yOffset;
        return localPos;
    }
}
