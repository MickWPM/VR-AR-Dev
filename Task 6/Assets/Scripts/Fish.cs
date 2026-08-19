using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishType type;
    public FishType Type { get { return type; } }

    private void Start()
    {
        Brain_Start();
    }

    private void Update()
    {
        Brain_Update();
        Motor_Update();
    }


    #region WorldHelper

    public BoxCollider fishtankExtentsCollider;
    public float extentsThreshold = 0.95f;
    public Vector3 WorldHelper_RandomInsideTank()
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

    #region Motor
    public Rigidbody rb;
    private Transform moveTargetTransform;
    private Vector3 moveTargetPosition;
    public void Motor_SetTarget(Transform targetTransform)
    {
        moveTargetTransform = targetTransform;
    }

    public void Motor_SetTarget(Vector3 targetPosition)
    {
        moveTargetTransform = null;
        moveTargetPosition = targetPosition;
    }

    public Vector3 CurrentTargetPosition()
    {
        return moveTargetTransform == null ? moveTargetPosition : moveTargetTransform.position;
    }

    private void Motor_Update()
    {
        Motor_UpdateMovement();
    }


    [SerializeField] private float rotationSpeedSlow = 90, rotationSpeedFast = 270, rotationSpeedTurbo = 520; 
    private float rotationSpeed;
    [SerializeField] private float arrivalThreshold = 0.05f;
    private float moveSpeed = 0;
    public void Motor_UpdateMovement()
    {
        Vector3 targetPos = moveTargetTransform == null ? moveTargetPosition : moveTargetTransform.position;
        Vector3 direction = currentMoveLocation - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );

        rb.linearVelocity = transform.forward * moveSpeed;
        if (Vector3.Distance(transform.position, targetPos) < arrivalThreshold)
        {
            ArrivedAtTargetEvent?.Invoke();
        }
    }

    public event System.Action ArrivedAtTargetEvent;
    public event System.Action<MoveRate> MoveRateUpdatedEvent;

    [SerializeField] float slowMoveSpeed, fastMoveSpeed, turboMoveSpeed;
    public void Motor_SetMoveRate(MoveRate moveRate)
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

    public enum MoveRate { Slow, Fast, Turbo}

    public MoveRate testMoveRate;
    [ContextMenu("Update move rate")]
    public void SetTestMoveRate()
    {
        Motor_SetMoveRate(testMoveRate);
    }
    #endregion


    #region Brain
    public enum CurrentState { Idle, ChasingFood, EatingFood, Fleeing, Dead}
    [SerializeField] CurrentState currentState = CurrentState.Idle;

    private void Brain_Start()
    {
        EnterIdle();
    }

    private void Brain_Update()
    {
        switch (currentState)
        {
            case CurrentState.Idle:
                UpdateIdle();
                break;
            case CurrentState.ChasingFood:
                break;
            case CurrentState.EatingFood:
                break;
            case CurrentState.Fleeing:
                break;
            case CurrentState.Dead:
                break;
            default:
                break;
        }
    }

    FleeTriggerScript fleeTrigger;
    private void EnterIdle()
    {
        currentState = CurrentState.Idle;
        currentMoveLocation = WorldHelper_RandomInsideTank();
        Motor_SetTarget(currentMoveLocation);
        Motor_SetMoveRate(MoveRate.Slow);
        ArrivedAtTargetEvent += OnArrivedAtTarget;


        fleeTrigger = gameObject.GetComponentInChildren<FleeTriggerScript>();
        if (fleeTrigger != null)
        {
            fleeTrigger.FleeTriggeredByFishEvent += FleeTriggered;
        }
    }

    private void FleeTriggered(Fish fish)
    {
        ExitIdle();
        fleeTrigger.FleeAllClearEvent += FleeAllClear;


        //EnterFlee();
        //temp do the work for flee
        //get flee pos....
        //for now just swim faster to a new random pos

        currentMoveLocation = WorldHelper_RandomInsideTank();
        Motor_SetTarget(currentMoveLocation);
        Motor_SetMoveRate(MoveRate.Turbo);
    }

    private void FleeAllClear()
    {
        //Exit flee
        fleeTrigger.FleeAllClearEvent -= FleeAllClear;
        //Enter idle
        EnterIdle();
    }

    private void ExitIdle()
    {
        ArrivedAtTargetEvent -= OnArrivedAtTarget;
        fleeTrigger.FleeTriggeredByFishEvent -= FleeTriggered;
    }
    private void OnArrivedAtTarget()
    {
        currentMoveLocation = WorldHelper_RandomInsideTank();
        Motor_SetTarget(currentMoveLocation);
    }

    private Vector3 currentMoveLocation = Vector3.zero;
    void UpdateIdle()
    {
        //TRANSITIONS: 1. Flee, 2. Chase food
        if (false)
        {
            ExitIdle();
        }

        //update movement
        //Nothing required here as we handle it in events
    }


    private Transform currentFoodTarget = null;
    private void EnterChase(Transform food)
    {
        if (currentFoodTarget == null)
        {
            EnterIdle();
            return;
        }
        currentState = CurrentState.ChasingFood;
        currentFoodTarget = food;
        //Clear targets etc?
    }

    private void ExitChase()
    {
        currentFoodTarget = null;
    }

    private bool ThreatNear()
    {
        return false;
    }

    #endregion

    public enum FishType { Fish, Shark}
}
