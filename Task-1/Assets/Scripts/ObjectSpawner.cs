using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float spawnTimer = 3;
    private float spawnCountdown;
    public GameObject[] spawnPrefab;
    public Transform spawnLocation;

    private void Start()
    {
        spawnCountdown = spawnTimer;
    }

    void Update()
    {
        SpawnUpdate();
    }

    private void SpawnUpdate()
    {
        spawnCountdown -= Time.deltaTime;
        if (spawnCountdown > 0) return;

        spawnCountdown = spawnTimer;
        var spawnedObjcet = GetSpawnPrefab();

        var spawnedGO = Instantiate(spawnedObjcet, spawnLocation.position, Quaternion.identity); //Could add random rotation here too...
        //we dont need to do anything with the spawnedGO yet....
    }

    private GameObject GetSpawnPrefab()
    {
        return spawnPrefab[Random.Range(0, spawnPrefab.Length)];
    }
}
