using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(FishMotor))]
public class FishBrain : MonoBehaviour
{
    public string currentStateDescription;
    private IState idleState, fleeState, foodSeekState;
    private IState currentState;
    private FishMotor motor;
    private FleeTriggerScript fleeTrigger;
    private FoodTriggerScript foodTrigger;
    [SerializeField] private float foodEatRangeOverride = -1;

    private void Awake()
    {
        motor = GetComponent<FishMotor>();
        fleeTrigger = GetComponentInChildren<FleeTriggerScript>();
        foodTrigger = GetComponentInChildren<FoodTriggerScript>();
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
        FishIdle idle = new FishIdle(motor, fleeTrigger, foodTrigger, WorldHelper_RandomInsideTank);
        if (fleeTrigger != null)
        {
            idle.IdleFleeTriggeredEvent += IdleFleeTriggered;
            FishFlee flee = new FishFlee(motor, fleeTrigger, WorldHelper_RandomInsideTank);
            flee.FleeAllClearEvent += FleeAllClear;
            fleeState = flee;
        }
        if (foodTrigger != null)
        {
            idle.FoodSeenEvent += FoodSeen;
            FishSeekFood foodSeek = new FishSeekFood(motor, fleeTrigger, WorldHelper_RandomInsideTank);
            foodSeek.ArrivedAtFoodEvent += ArrivedAtFood;
            foodSeek.LostFoodEvent += FoodLost;
            if (foodEatRangeOverride > 0)
            {
                foodSeek.SetFoodRangeOverride(foodEatRangeOverride);
            }
            foodSeekState = foodSeek;
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

    private void FoodSeen(Fish fish)
    {
        //TODO: HUNGER LOGIC SO WE DONT ALWAYS CHARGE AT ALL FOOD
        //TODO - logic here to confirm that fish is a valid food for this fish? 
        //This fish's sensor handles this though so from a *behaviour* point this is fine
        ((FishSeekFood)foodSeekState).SetFoodTarget(fish);
        EnterState(foodSeekState);
    }

    private void ArrivedAtFood(Fish fish)
    {
        Destroy(fish.gameObject);
        EnterState(idleState);  //Or do we want a pause for "eating"
    }
    private void FoodLost()
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
