using UnityEngine;

public class FishIdle : IState
{
    private Vector3 currentMoveLocation;
    private FishMotor motor;
    private System.Func<Vector3> RandomLocationInsideTank;
    private FleeTriggerScript fleeTrigger;
    private FoodTriggerScript foodTrigger;

    public string StateName => "Idle";

    public FishIdle(FishMotor motor, FleeTriggerScript fleeTrigger, FoodTriggerScript foodTrigger, System.Func<Vector3> RandomLocationInsideTank) 
    {
        this.motor = motor;
        this.fleeTrigger = fleeTrigger;
        this.foodTrigger = foodTrigger;
        this.RandomLocationInsideTank = RandomLocationInsideTank;
    }

    public void EnterState()
    {
        currentMoveLocation = RandomLocationInsideTank();
        motor.SetTarget(currentMoveLocation);
        motor.SetMoveRate(FishMotor.MoveRate.Slow);

        motor.ArrivedAtTargetEvent += OnArrivedAtTarget;
        if (fleeTrigger != null)
        {
            fleeTrigger.FleeTriggeredByFishEvent += FleeTriggered;
        }

        if (foodTrigger != null)
        {
            foodTrigger.FoodFoundEvent += FoodSeen;
        }
    }

    public void ExitState()
    {
        motor.ArrivedAtTargetEvent -= OnArrivedAtTarget; 
        if (fleeTrigger != null)
        {
            fleeTrigger.FleeTriggeredByFishEvent -= FleeTriggered;
        }

        if (foodTrigger != null)
        {
            foodTrigger.FoodFoundEvent -= FoodSeen;
        }
    }

    public void UpdateState()
    {
        //Check for transitions - currently this is handled by events...
    }


    private void OnArrivedAtTarget()
    {
        currentMoveLocation = RandomLocationInsideTank();
        motor.SetTarget(currentMoveLocation);
    }


    public event System.Action<Fish> IdleFleeTriggeredEvent;
    private void FleeTriggered(Fish fish)
    {
        IdleFleeTriggeredEvent?.Invoke(fish);
    }

    public event System.Action<Fish> FoodSeenEvent;
    private void FoodSeen(Fish fish)
    {
        FoodSeenEvent?.Invoke(fish);
    }
}
