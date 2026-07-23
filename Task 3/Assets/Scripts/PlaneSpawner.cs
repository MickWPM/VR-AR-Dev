using UnityEngine;
using UnityEngine.Events;

public class PlaneSpawner : MonoBehaviour
{
    public LandingStrip landingStrip;
    public GameObject[] prefabs;
    private SphereCollider[] colliders;

    public int numAircraftToSpawn = 100;
    private void Start()
    {
        colliders = GetComponentsInChildren<SphereCollider>();
        _ = SpawnAircraft();
    }

    public UnityEvent PlaneSpawnedEvent, SpawningCompleteEvent;
    async Awaitable SpawnAircraft()
    {
        for (int i = 0; i < numAircraftToSpawn; i++)
        {
            var delay =  Random.Range(3, 5);
            await Awaitable.WaitForSecondsAsync(delay);
            var prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

            var go = Instantiate(prefabToSpawn, GetSpawnPosition(), Quaternion.identity);
            PlaneSpawnedEvent?.Invoke();
            var pathManager = go.GetComponent<PathManager>();
            pathManager.SetupPath(landingStrip);
        }

        SpawningCompleteEvent?.Invoke();
    }

    Vector3 GetSpawnPosition()
    {
        var col = colliders[Random.Range(0, colliders.Length)];
        return Random.insideUnitSphere * col.radius + col.transform.position;
    }
}
