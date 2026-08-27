using UnityEngine;

public class TransformTracker : MonoBehaviour
{
    public Transform transformToUpdate;
    public bool trackingTransform = true;
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
            transform.rotation = transformToUpdate.rotation;
        } else
        {
            transformToUpdate.position = transform.position;
            transformToUpdate.rotation = transform.rotation;
        }
        
    }
}
