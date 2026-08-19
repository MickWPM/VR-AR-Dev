using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(FishMotor))]
public class FishBrain : MonoBehaviour
{
    public string currentStateDescription;
    private IState idleState, fleeState;
    private IState currentState;
    private FishMotor motor;
    private FleeTriggerScript fleeTrigger;

    private void Awake()
    {
        motor = GetComponent<FishMotor>();
        fleeTrigger = GetComponentInChildren<FleeTriggerScript>();
    }

    private void Start()
    {
        SetupStates();

        currentState = idleState;
        currentState.EnterState();
        currentStateDescription = currentState.StateName;
    }

    private void SetupStates()
    {
        //Could refactor this to inject the flee trigger manually after construction (which then does the internal subcription
        //Currently its easy to pass a potential null - if null we dont flee
        FishIdle idle = new FishIdle(motor, fleeTrigger, WorldHelper_RandomInsideTank);
        if (fleeTrigger != null)
        {
            idle.IdleFleeTriggeredEvent += IdleFleeTriggered;
            FishFlee flee = new FishFlee(motor, fleeTrigger, WorldHelper_RandomInsideTank);
            flee.FleeAllClearEvent += FleeAllClear;
            fleeState = flee;
        }
        idleState = idle;

    }

#region StateTransitions
    private void IdleFleeTriggered(Fish fish)
    {
        EnterState(fleeState);
    }

    private void FleeAllClear()
    {
        EnterState(idleState);
    }
#endregion

    private void Update()
    {
        currentState.UpdateState();
    }

    public void EnterState(IState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
        currentStateDescription = currentState.StateName;
    }


    private Vector3 currentMoveLocation = Vector3.zero;


    //private Transform currentFoodTarget = null;
    //private void EnterChase(Transform food)
    //{
    //    if (currentFoodTarget == null)
    //    {
    //        EnterIdle();
    //        return;
    //    }
    //    currentStateEnum = CurrentState.ChasingFood;
    //    currentFoodTarget = food;
    //    //Clear targets etc?
    //}

    //private void ExitChase()
    //{
    //    currentFoodTarget = null;
    //}

    //private bool ThreatNear()
    //{
    //    return false;
    //}





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


}
