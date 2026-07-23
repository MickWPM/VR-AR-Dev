using UnityEngine;

public class PlaneCollisionDetection : MonoBehaviour
{
    public GameObject onImpactSpawn;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collision!", gameObject);
        Instantiate(onImpactSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject.transform.root.gameObject);
    }
}
