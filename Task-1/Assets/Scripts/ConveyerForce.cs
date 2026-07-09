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


}
