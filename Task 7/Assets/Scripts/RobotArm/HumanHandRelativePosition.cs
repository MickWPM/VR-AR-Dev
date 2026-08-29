using UnityEngine;

public class HumanHandRelativePosition : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    public Vector3 tmp_pos_output;
    public Vector3 LocalPosition { get => (handTransform.localPosition - transform.position); }
    private void Update()
    {
        tmp_pos_output = LocalPosition;
    }
}
