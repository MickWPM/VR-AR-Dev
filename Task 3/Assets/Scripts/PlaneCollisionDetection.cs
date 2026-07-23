using UnityEngine;

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


    private void Collision()
    {
        Debug.Log("Collision!", gameObject);
        Instantiate(onImpactSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject.transform.root.gameObject);
    }

    private void SuccessfulLanding(RunwaySafeHanger hanger)
    {
        Debug.Log("Safe landing at hanger");
        Destroy(gameObject.transform.root.gameObject);
    }
}
