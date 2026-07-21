using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    public LandingStrip landingStrip;
    public GameObject[] prefabs;
    private SphereCollider[] colliders;
    private void Start()
    {
        colliders = GetComponentsInChildren<SphereCollider>();
        _ = SpawnAircraft();
    }


    async Awaitable SpawnAircraft()
    {
        for (int i = 0; i < 10; i++)
        {
            var delay =  Random.Range(3, 5);
            await Awaitable.WaitForSecondsAsync(delay);
            var prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

            var go = Instantiate(prefabToSpawn, GetSpawnPosition(), Quaternion.identity);
            var pathManager = go.GetComponent<PathManager>();
            pathManager.SetupPath(landingStrip);
        }
    }

    Vector3 GetSpawnPosition()
    {
        var col = colliders[Random.Range(0, colliders.Length)];
        return Random.insideUnitSphere * col.radius + col.transform.position;
    }
}
