using UnityEngine;

public class RotationSetter : MonoBehaviour
{
	[SerializeField]private Vector3 targetRotation;
	[SerializeField] private bool inWorldSpace;

    private void LateUpdate()
    {
        if (inWorldSpace)
        {
            transform.rotation = Quaternion.Euler(targetRotation);
        } else
		{
			transform.localRotation = Quaternion.Euler(targetRotation);
		}
    }
}