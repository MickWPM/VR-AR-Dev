using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    [SerializeField] private float gravityAcceleration = 0.1f;
    private float gravityForceToApply;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null )
        {
            Debug.LogError("Bullet has no rigidbody", this.gameObject);
            this.enabled = false;
        }
        gravityForceToApply = Physics.gravity.magnitude - gravityAcceleration;
    }
    void Start()
    {
        rb.linearVelocity = transform.forward * fireSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Hit {collision.gameObject}");
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * gravityForceToApply, ForceMode.Acceleration);
    }
}
