using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float fireSpeed;
    [SerializeField] private float gravityAcceleration = 0.1f;
    [SerializeField] private GameObject baseGO;
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
        var canvas = collision.gameObject.GetComponentInParent<PainterCanvas>();
        if ( canvas == null)
        {
            Destroy(baseGO);
            return;
        }

        AddToCanvas(canvas);
        Debug.Log($"Hit {collision.gameObject}");
    }

    private void AddToCanvas(PainterCanvas canvas)
    {
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        baseGO.transform.SetParent(canvas.transform, true);
        this.enabled = false;
    }

    private void FixedUpdate()
    {
        if ( rb.isKinematic == false)
        {
            rb.AddForce(Vector3.up * gravityForceToApply, ForceMode.Acceleration);        
        }
    }

}
