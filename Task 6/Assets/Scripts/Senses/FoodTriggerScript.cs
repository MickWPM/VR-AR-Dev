using UnityEngine;

public class FoodTriggerScript : MonoBehaviour
{

    public Fish.FishType[] foodTypes;
    public event System.Action<Fish> FoodFoundEvent;

    private SphereCollider foodSensorCollider;

    private SphereCollider[] foodInRange;
    [SerializeField] private int maxFoodInRange = 5;
    private void Awake()
    {
        foodSensorCollider = GetComponent<SphereCollider>();
        foodInRange = new SphereCollider[maxFoodInRange];
    }

    private void OnTriggerEnter(Collider other)
    {
        Fish f = other.GetComponentInParent<Fish>();
        if (f == null) return;

        bool food = false;
        for (int i = 0; i < foodTypes.Length; i++)
        {
            if (f.Type == foodTypes[i])
            {
                food = true;
                break;
            }
        }

        if (food)
        {
            FoodFoundEvent?.Invoke(f);
        }
    }
}
