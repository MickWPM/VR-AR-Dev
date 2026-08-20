using UnityEngine;

public class FishFeeder : MonoBehaviour
{
    [SerializeField] private Fish droppedFlakePrefab;
    public float spawnRange = 2f;
    public float spawnDelayMin, spawnDelayMax;
    public int spawnNumMin, spawnNumMax;
    private void Start()
    {
        FeedFish();
    }

    private async void FeedFish()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(spawnDelayMin, spawnDelayMax));
            int numToSpawn = Random.Range(spawnNumMin, spawnNumMax+1);
            SpawnFlakes(numToSpawn);
        }
    }

    void SpawnFlakes(int numFlakes)
    {
        for (int i = 0; i < numFlakes; i++) 
        {
            Vector3 spawnPos = transform.position + 
                new Vector3(
                    Random.Range(-spawnRange/2f, spawnRange/2f), 
                    Random.Range(-0.1f, 0.1f), 
                    Random.Range(-spawnRange / 2f, spawnRange /2f)
                    );

            Instantiate(droppedFlakePrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRange, 0.1f, spawnRange));
    }
}
