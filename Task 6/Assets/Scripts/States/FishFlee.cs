using System;
using UnityEngine;

public class FishFlee : IState
{
    public string StateName => "Flee";
    private Vector3 currentMoveLocation;
    private FishMotor motor;
    private FleeTriggerScript fleeTrigger;
    private System.Func<Vector3> RandomLocationInsideTank;
    private float moveArriveOverride = -1;

    public FishFlee(FishMotor motor, FleeTriggerScript fleeTrigger, Func<Vector3> randomLocationInsideTank)
    {
        this.motor = motor;
        this.fleeTrigger = fleeTrigger;
        RandomLocationInsideTank = randomLocationInsideTank;
    }
    
    public void SetMoveArriveOverride(float moveArriveOverride)
    {
        this.moveArriveOverride = moveArriveOverride;
    }

    public void EnterState()
    {
        fleeTrigger.FleeAllClearEvent += FleeAllClear;
        motor.ArrivedAtTargetEvent += OnArrivedAtTarget;

        currentMoveLocation = RandomLocationInsideTank();
        motor.SetTarget(currentMoveLocation, moveArriveOverride);
        motor.SetMoveRate(FishMotor.MoveRate.Turbo);
    }


    public void ExitState()
    {
        fleeTrigger.FleeAllClearEvent -= FleeAllClear;
        motor.ArrivedAtTargetEvent -= OnArrivedAtTarget;
    }
    private void OnArrivedAtTarget()
    {
        currentMoveLocation = RandomLocationInsideTank();
        motor.SetTarget(currentMoveLocation, moveArriveOverride);
    }

    public void UpdateState()
    {

    }

    public System.Action FleeAllClearEvent;
    private void FleeAllClear()
    {
        FleeAllClearEvent?.Invoke();
    }
}
