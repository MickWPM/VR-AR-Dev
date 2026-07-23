using UnityEngine;
using UnityEngine.Events;

public class ExperienceManager : MonoBehaviour
{
    public UnityEvent PlaneCrashEvent;
    public UnityEvent PlaneLandEvent;
    public UnityEvent PlaneSpawnEvent;

    public static ExperienceManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one Experience Manager in scene.");
            Destroy(this.gameObject);
        }
    }

    public void PlaneSpawned()
    {
        PlaneSpawnEvent?.Invoke();
    }

    public void PlaneCrashed()
    {
        PlaneCrashEvent?.Invoke();
    }
    public void PlaneLanded()
    {
        PlaneLandEvent.Invoke();
    }
}
