using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FishMotor : MonoBehaviour
{
    public Rigidbody rb;
    private Transform moveTargetTransform;
    private Vector3 moveTargetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    float overrideArrival = -1;
    public void SetTarget(Transform targetTransform, float overrideArrival = -1)
    {
        moveTargetTransform = targetTransform;
        this.overrideArrival = overrideArrival;
    }

    public void SetTarget(Vector3 targetPosition, float overrideArrival = -1)
    {
        moveTargetTransform = null;
        moveTargetPosition = targetPosition;
        this.overrideArrival = overrideArrival;
    }

    public Vector3 CurrentTargetPosition()
    {
        return moveTargetTransform == null ? moveTargetPosition : moveTargetTransform.position;
    }

    private void Update()
    {
        UpdateMovement();
    }


    [SerializeField] private float rotationSpeedSlow = 90, rotationSpeedFast = 270, rotationSpeedTurbo = 520;
    private float rotationSpeed;
    [SerializeField] private float arrivalThreshold = 0.05f;
    private float moveSpeed = 0;
    public void UpdateMovement()
    {
        moveTargetPosition = CurrentTargetPosition();
        Vector3 direction = moveTargetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );

        rb.linearVelocity = transform.forward * moveSpeed;
        float threshold = overrideArrival > 0 ? overrideArrival : arrivalThreshold;
        if (Vector3.Distance(transform.position, moveTargetPosition) < threshold)
        {
            ArrivedAtTargetEvent?.Invoke();
        } 
    }

    public event System.Action ArrivedAtTargetEvent;
    public event System.Action<MoveRate> MoveRateUpdatedEvent;

    [SerializeField] float slowMoveSpeed, fastMoveSpeed, turboMoveSpeed;
    public void SetMoveRate(MoveRate moveRate)
    {
        switch (moveRate)
        {
            case MoveRate.Slow:
                moveSpeed = slowMoveSpeed;
                rotationSpeed = rotationSpeedSlow;
                break;
            case MoveRate.Fast:
                moveSpeed = fastMoveSpeed;
                rotationSpeed = rotationSpeedFast;
                break;
            case MoveRate.Turbo:
                moveSpeed = turboMoveSpeed;
                rotationSpeed = rotationSpeedTurbo;
                break;
            default:
                moveSpeed = slowMoveSpeed;
                rotationSpeed = rotationSpeedSlow;
                Debug.LogWarning($"Unhandled move speed: {moveRate}", gameObject);
                break;
        }
        MoveRateUpdatedEvent?.Invoke(moveRate);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, CurrentTargetPosition());
    }

    public enum MoveRate { Slow, Fast, Turbo }
}
