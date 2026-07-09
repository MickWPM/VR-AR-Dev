using UnityEngine;

public class ConveyerForce : MonoBehaviour
{
    public float forcePower = 1.0f;
    private void OnCollisionStay(Collision collision)
    {
        var colliderGO = collision.gameObject;
        Rigidbody rb = colliderGO.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        //rb.AddForce(transform.forward * forcePower, ForceMode.VelocityChange);
        rb.linearVelocity = transform.forward * forcePower;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var colliderGO = collision.gameObject;
        Rigidbody rb = colliderGO.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        SetRotationConstraints(rb, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        var colliderGO = collision.gameObject;
        Rigidbody rb = colliderGO.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        SetRotationConstraints(rb, false);
    }

    private void SetRotationConstraints(Rigidbody rb, bool constraintEnabled)
    {
        return;
        rb.constraints = constraintEnabled ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.None;
    }
}
