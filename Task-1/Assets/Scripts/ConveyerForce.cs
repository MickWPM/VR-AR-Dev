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

        //This approach works fine too however the direct velocity control has a 
        //better 'feel' in testing. Left in here to highlight an alternative approach
        //rb.AddForce(transform.forward * forcePower, ForceMode.VelocityChange);
        
        rb.linearVelocity = transform.forward * forcePower;
    }


}
