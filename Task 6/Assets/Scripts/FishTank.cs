using UnityEngine;

public class FishTank : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Fish f = other.gameObject.GetComponentInParent<Fish>();
        if (f != null)
        {
            f.enabled = true;
        }
    }


    #region WorldHelper

    public BoxCollider fishtankExtentsCollider;
    public float extentsThreshold = 0.95f;
    public Vector3 RandomInsideTank()
    {
        Vector3 extents = extentsThreshold * fishtankExtentsCollider.size / 2f;

        Vector3 localPoint = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + fishtankExtentsCollider.center;

        return fishtankExtentsCollider.transform.TransformPoint(localPoint);
    }


    #endregion
}
