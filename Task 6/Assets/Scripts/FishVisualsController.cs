using UnityEngine;

public class FishVisualsController : MonoBehaviour
{
    private Material fishMat;
    private const string OFFSET_KEYWORD = "_TimeOffset", SPEED_KEYWORD = "_SwimSpeed";
    [SerializeField] private float minShaderSpeed = 5, fastShaderSpeed = 20, turboShaderSpeed = 50;
    
    private Fish fishScript;

    private void Awake()
    {
        var mr = gameObject.GetComponentInChildren<Renderer>();
        fishMat = mr.material;
        var timeOffset = Random.Range(0f, 1f);
        fishMat.SetFloat(OFFSET_KEYWORD, timeOffset);

        fishScript = gameObject.GetComponent<Fish>();
        fishScript.MoveRateUpdatedEvent += FishScript_MoveRateUpdatedEvent;
    }

    private void OnDisable()
    {
        fishScript.MoveRateUpdatedEvent -= FishScript_MoveRateUpdatedEvent;
    }

    private void FishScript_MoveRateUpdatedEvent(Fish.MoveRate moveRate)
    {
        float shaderSpeed;
        switch (moveRate)
        {
            case Fish.MoveRate.Slow:
                shaderSpeed = minShaderSpeed;
                break;
            case Fish.MoveRate.Fast:
                shaderSpeed = fastShaderSpeed;
                break;
            case Fish.MoveRate.Turbo:
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
