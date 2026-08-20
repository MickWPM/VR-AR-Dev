using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public FishBrain smallFishPreafb, sharkPrefab;
    public FishTank fishTank;
    public int maxFish = 25;
    public int fishBeforeShark = 7;
    private int spawnedFish = 0;
    private void Start()
    {
        DoFishSpawning();
    }

    private async void DoFishSpawning()
    {
        while (spawnedFish < maxFish)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(5, 5));
            spawnedFish++;
            FishBrain fishPrefab = spawnedFish % fishBeforeShark == 0 ? sharkPrefab : smallFishPreafb;
            SpawnFish(fishPrefab);
        }
    }

    void SpawnFish(FishBrain fishPrefab)
    {
        Vector3 spawnPos = fishTank.RandomInsideTank();
        FishBrain brain = Instantiate(fishPrefab, spawnPos, Quaternion.identity);
        brain.fishTank = fishTank;
    }

}
