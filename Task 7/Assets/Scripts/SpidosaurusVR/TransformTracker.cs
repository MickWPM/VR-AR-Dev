using UnityEngine;

public class TransformTracker : MonoBehaviour
{
    public Transform transformToUpdate;
    public bool trackingTransform = true;
    public bool matchRotation = true;
    public void SetToUpdatePosition()
    {
        trackingTransform = false;
    }

    public void SetToTrackPosition()
    {
        trackingTransform = true;
    }
    

    private void LateUpdate()
    {
        if (trackingTransform)
        {
            transform.position = transformToUpdate.position;
            if (matchRotation) transform.rotation = transformToUpdate.rotation;
        } else
        {
            transformToUpdate.position = transform.position;
            if (matchRotation) transformToUpdate.rotation = transform.rotation;
        }
        
    }
}
