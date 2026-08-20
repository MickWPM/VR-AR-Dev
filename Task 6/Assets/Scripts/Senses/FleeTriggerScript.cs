using UnityEngine;

public class FleeTriggerScript : MonoBehaviour
{
    public Fish.FishType[] fleeTypes;
    public event System.Action<Fish> FleeTriggeredByFishEvent;
    public event System.Action FleeAllClearEvent;
    public float fleeArriveDistanceOverride = -1f;

    private SphereCollider fleeCollider;

    private int fleeFishInRange = 0;

    private SphereCollider[] fleeFishCollidersInRange;
    [SerializeField] private int maxFleeInRange = 10;
    private void Awake()
    {
        fleeCollider = GetComponent<SphereCollider>();
        fleeFishCollidersInRange = new SphereCollider[maxFleeInRange];
    }

    private void OnTriggerEnter(Collider other)
    {
        Fish f = other.GetComponentInParent<Fish>();
        if (f == null) return;

        bool flee = false;
        for (int i = 0; i < fleeTypes.Length; i++)
        {
            if (f.Type == fleeTypes[i])
            {
                ++fleeFishInRange;
                flee = true;
                break;
            }
        }

        if (flee)
        {
            FleeTriggeredByFishEvent?.Invoke(f);
        }
    }

    [SerializeField] private LayerMask threatLayerMask;
    private void OnTriggerExit(Collider other)
    {
        Fish f = other.GetComponentInParent<Fish>();
        if (f == null) return;

        for (int i = 0; i < fleeTypes.Length; i++)
        {
            if (f.Type == fleeTypes[i])
            {
                --fleeFishInRange;
            }
        }

        if (fleeFishInRange > 0) return;

        FleeAllClearEvent?.Invoke();
    }
}
