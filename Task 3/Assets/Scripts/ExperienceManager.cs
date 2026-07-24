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


    private bool spawnComplete = false;
    public void SpawnComplete()
    {
        spawnComplete = true;
    }

    private int spawned, crashed, landed;
    public void PlaneSpawned()
    {
        ++spawned;
        PlaneSpawnEvent?.Invoke();
        CheckGameOver();
    }

    public void PlaneCrashed()
    {
        ++crashed;
        PlaneCrashEvent?.Invoke();
        CheckGameOver();
    }
    public void PlaneLanded()
    {
        ++landed;
        PlaneLandEvent.Invoke();
        CheckGameOver();
    }

    public UnityEvent GameOverEvent;
    private void CheckGameOver()
    {
        if (!spawnComplete) return;
        if (crashed + landed == spawned)
        {
            GameOverEvent?.Invoke();
            this.enabled = false;
        }
    }
}
