using UnityEngine;

public class FishVisualsController : MonoBehaviour
{
    private Material fishMat;
    private const string OFFSET_KEYWORD = "_TimeOffset", SPEED_KEYWORD = "_SwimSpeed";
    [SerializeField] private float minShaderSpeed = 5, fastShaderSpeed = 20, turboShaderSpeed = 50;

    private void Awake()
    {
        var mr = gameObject.GetComponentInChildren<Renderer>();
        fishMat = mr.material;
        var timeOffset = Random.Range(0f, 1f);
        fishMat.SetFloat(OFFSET_KEYWORD, timeOffset);
    }

    [SerializeField] Vector2 fastSwimSpeedRange = new Vector2(0.5f, 1f);
    public void SwimVelocityUpdated(float swimSpeed)
    {
        float shaderSpeed = minShaderSpeed;
        if (swimSpeed > fastSwimSpeedRange.x)
        {
            shaderSpeed = swimSpeed > fastSwimSpeedRange.y ? turboShaderSpeed : fastShaderSpeed;
        }

        fishMat.SetFloat(SPEED_KEYWORD, shaderSpeed);
    }

    [Range(0.01f, 2f)]
    public float testSpeed = 0.2f;
    [ContextMenu("Test swim speed")]
    public void TestSpeed()
    {
        SwimVelocityUpdated(testSpeed);
    }
}
