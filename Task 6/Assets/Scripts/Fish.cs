using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishType type;
    public FishType Type { get { return type; } }

    public enum FishType { Fish, Shark, FoodOnly}

    private void OnEnable()
    {
        FishBrain fishBrain = GetComponent<FishBrain>();
        if (fishBrain != null && fishBrain.enabled == false) fishBrain.enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;
    }
}
