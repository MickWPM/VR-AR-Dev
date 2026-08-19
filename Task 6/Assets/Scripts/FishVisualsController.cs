using UnityEngine;

[RequireComponent(typeof(FishMotor))]
public class FishVisualsController : MonoBehaviour
{
    private Material fishMat;
    private const string OFFSET_KEYWORD = "_TimeOffset", SPEED_KEYWORD = "_SwimSpeed";
    [SerializeField] private float minShaderSpeed = 5, fastShaderSpeed = 20, turboShaderSpeed = 50;
    
    private FishMotor motor;

    private void Awake()
    {
        var mr = gameObject.GetComponentInChildren<Renderer>();
        fishMat = mr.material;
        var timeOffset = Random.Range(0f, 1f);
        fishMat.SetFloat(OFFSET_KEYWORD, timeOffset);
        
        motor = gameObject.GetComponent<FishMotor>();
        motor.MoveRateUpdatedEvent += FishScript_MoveRateUpdatedEvent;
    }

    private void OnDisable()
    {
        motor.MoveRateUpdatedEvent -= FishScript_MoveRateUpdatedEvent;
    }

    private void FishScript_MoveRateUpdatedEvent(FishMotor.MoveRate moveRate)
    {
        float shaderSpeed;
        switch (moveRate)
        {
            case FishMotor.MoveRate.Slow:
                shaderSpeed = minShaderSpeed;
                break;
            case FishMotor.MoveRate.Fast:
                shaderSpeed = fastShaderSpeed;
                break;
            case FishMotor.MoveRate.Turbo:
                shaderSpeed = turboShaderSpeed;
                break;
            default:
                shaderSpeed = minShaderSpeed;
                Debug.LogWarning($"Unhandled move rate: {moveRate}", gameObject);
                break;
        }

        fishMat.SetFloat(SPEED_KEYWORD, shaderSpeed);
    }
}
