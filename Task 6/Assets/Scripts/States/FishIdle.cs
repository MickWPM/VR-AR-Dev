using UnityEngine;
using static FishBrain;

public class FishIdle : IState
{
    private Vector3 currentMoveLocation;
    private FishMotor motor;
    private System.Func<Vector3> RandomLocationInsideTank;
    private FleeTriggerScript fleeTrigger;

    public string StateName => "Idle";

    public FishIdle(FishMotor motor, FleeTriggerScript fleeTrigger, System.Func<Vector3> RandomLocationInsideTank)
    {
        this.motor = motor;
        this.fleeTrigger = fleeTrigger;
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
    }

    public void ExitState()
    {
        motor.ArrivedAtTargetEvent -= OnArrivedAtTarget;
        fleeTrigger.FleeTriggeredByFishEvent -= FleeTriggered;
    }

    public void UpdateState()
    {
        //Check for transitions: Do we need to flee? Have we found food?
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
}
