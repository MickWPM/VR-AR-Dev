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
    [SerializeField] private float hungerTimer = 5f;
    private float currentHungerLevel;

    public FishTank fishTank;

    private void Awake()
    {
        Debug.Log("Fishbrain:AWAKE");
        currentHungerLevel = Random.Range(0f, 1f);
        motor = GetComponent<FishMotor>();
        if (motor.enabled == false) motor.enabled = true;
        fleeTrigger = GetComponentInChildren<FleeTriggerScript>();
        foodTrigger = GetComponentInChildren<FoodTriggerScript>();
    }

    private void OnEnable()
    {
        Debug.Log("Fishbrain:ENABLE");
        if (motor.enabled == false) motor.enabled = true;
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
        FishIdle idle = new FishIdle(motor, fleeTrigger, foodTrigger, fishTank.RandomInsideTank);
        if (fleeTrigger != null)
        {
            idle.IdleFleeTriggeredEvent += IdleFleeTriggered;
            FishFlee flee = new FishFlee(motor, fleeTrigger, fishTank.RandomInsideTank);
            flee.FleeAllClearEvent += FleeAllClear;
            flee.SetMoveArriveOverride(fleeTrigger.fleeArriveDistanceOverride);
            fleeState = flee;
        }
        if (foodTrigger != null)
        {
            idle.FoodSeenEvent += FoodSeen;
            FishSeekFood foodSeek = new FishSeekFood(motor, fleeTrigger, fishTank.RandomInsideTank);
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
        //We could add in hunger states here; depending on the hunger level we pursue or ignore
        if (currentHungerLevel < 1) return;

        //Note the fish's sensor handles determining if this is a suitable food
        //More advanced implementations could have additional logic here
        ((FishSeekFood)foodSeekState).SetFoodTarget(fish);
        EnterState(foodSeekState);
    }

    private void ArrivedAtFood(Fish fish)
    {
        Destroy(fish.gameObject);
        // We could extend this to have a satiation value for each fish (and a requirement)
        //For now we just set the hunger back to zero
        currentHungerLevel = 0; 

        EnterState(idleState);  //Future expansion could include "eating" state/animations etc
    }
    private void FoodLost()
    {
        EnterState(idleState);
    }
    #endregion

    private void Update()
    {
        currentHungerLevel += Time.deltaTime / hungerTimer;
        currentState.UpdateState();
    }

    public void EnterState(IState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
        currentStateDescription = currentState.StateName;
    }

}
