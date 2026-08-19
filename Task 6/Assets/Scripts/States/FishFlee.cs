using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FishFlee : IState
{
    public string StateName => "Flee";
    private Vector3 currentMoveLocation;
    private FishMotor motor;
    private FleeTriggerScript fleeTrigger;
    private System.Func<Vector3> RandomLocationInsideTank;

    public FishFlee(FishMotor motor, FleeTriggerScript fleeTrigger, Func<Vector3> randomLocationInsideTank)
    {
        this.motor = motor;
        this.fleeTrigger = fleeTrigger;
        RandomLocationInsideTank = randomLocationInsideTank;
    }

    public void EnterState()
    {
        fleeTrigger.FleeAllClearEvent += FleeAllClear;

        //temp do the work for flee
        //get flee pos....
        //for now just swim faster to a new random pos

        currentMoveLocation = RandomLocationInsideTank();
        motor.SetTarget(currentMoveLocation);
        motor.SetMoveRate(FishMotor.MoveRate.Turbo);
    }


    public void ExitState()
    {
        fleeTrigger.FleeAllClearEvent -= FleeAllClear;
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
