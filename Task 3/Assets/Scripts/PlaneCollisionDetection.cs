using UnityEngine;
using UnityEngine.Events;

public class PlaneCollisionDetection : MonoBehaviour
{
    public GameObject onImpactSpawn;
    private void OnTriggerEnter(Collider other)
    {
        var hanger = other.gameObject.GetComponent<RunwaySafeHanger>();
        Debug.Log("Hit : ", other.gameObject);
        if (hanger != null)
        {
            SuccessfulLanding(hanger);
        } else
        {
            Collision();
        }
    }

    public UnityEvent OnAircraftCollisionEvent;
    private void Collision()
    {
        Debug.Log("Collision!", gameObject);
        OnAircraftCollisionEvent?.Invoke();
        ExperienceManager.Instance.PlaneCrashed();
        Instantiate(onImpactSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject.transform.root.gameObject);
    }

    private void SuccessfulLanding(RunwaySafeHanger hanger)
    {
        Debug.Log("Safe landing at hanger");
        ExperienceManager.Instance.PlaneLanded();
        Destroy(gameObject.transform.root.gameObject);
    }
}
