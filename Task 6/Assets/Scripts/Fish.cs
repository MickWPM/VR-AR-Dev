using UnityEngine;

[RequireComponent(typeof(FishMotor))]
public class Fish : MonoBehaviour
{
    [SerializeField] private FishType type;
    public FishType Type { get { return type; } }
    private FishMotor motor;

    private void Awake()
    {
        motor = gameObject.GetComponent<FishMotor>();
    }

    private void Start()
    {
        Brain_Start();
    }

    private void Update()
    {
        Brain_Update();
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
        motor.SetTarget(currentMoveLocation);
        motor.SetMoveRate(FishMotor.MoveRate.Slow);
        motor.ArrivedAtTargetEvent += OnArrivedAtTarget;

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
        motor.SetTarget(currentMoveLocation);
        motor.SetMoveRate(FishMotor.MoveRate.Turbo);
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
        motor.ArrivedAtTargetEvent -= OnArrivedAtTarget;
        fleeTrigger.FleeTriggeredByFishEvent -= FleeTriggered;
    }
    private void OnArrivedAtTarget()
    {
        currentMoveLocation = WorldHelper_RandomInsideTank();
        motor.SetTarget(currentMoveLocation);
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
