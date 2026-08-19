using UnityEngine;
using static FishBrain;

public class FishSeekFood : IState
{
    private Fish foodToSeek;
    private FishMotor motor;
    private System.Func<Vector3> RandomLocationInsideTank;
    private FleeTriggerScript fleeTrigger;

    public string StateName => "Seeking Food";

    public FishSeekFood(FishMotor motor, FleeTriggerScript fleeTrigger, System.Func<Vector3> RandomLocationInsideTank)
    {
        this.motor = motor;
        this.fleeTrigger = fleeTrigger;
        this.RandomLocationInsideTank = RandomLocationInsideTank;
    }

    public void SetFoodTarget(Fish food)
    {
        foodToSeek = food;
    }

    public void SetFoodRangeOverride(float foodRange)
    {
        foodEatRangeOverride = foodRange;
    }

    private float foodEatRangeOverride = -1;
    public void EnterState()
    {
        motor.SetTarget(foodToSeek.transform, foodEatRangeOverride);
        motor.SetMoveRate(FishMotor.MoveRate.Fast);

        motor.ArrivedAtTargetEvent += OnArrivedAtTarget;
        if (fleeTrigger != null)
        {
            fleeTrigger.FleeTriggeredByFishEvent += FleeTriggered;
        }
    }

    public void ExitState()
    {
        motor.ArrivedAtTargetEvent -= OnArrivedAtTarget;
        if (fleeTrigger != null)
        {
            fleeTrigger.FleeTriggeredByFishEvent -= FleeTriggered;
        }
    }

    public void UpdateState()
    {
        if(foodToSeek == null)
        {
            LostFoodEvent?.Invoke();
        }
    }


    public event System.Action LostFoodEvent;
    public event System.Action<Fish> ArrivedAtFoodEvent;
    private void OnArrivedAtTarget()
    {
        ArrivedAtFoodEvent?.Invoke(foodToSeek);
    }


    public event System.Action<Fish> IdleFleeTriggeredEvent;
    private void FleeTriggered(Fish fish)
    {
        IdleFleeTriggeredEvent?.Invoke(fish);
    }
}
