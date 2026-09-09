using UnityEngine;

public class Coral : MonoBehaviour
{
    private Transform[] childSpawnPoints;
    private void Awake()
    {
        for (int i = 0; i < childSpawnPoints.Length; i++)
        {
            CreateTube(childSpawnPoints[i].position, childSpawnPoints[i].rotation.eulerAngles);
        }
    }

    private void CreateTube(Vector3 spawnPos, Vector3 initialDirection)
    {

    }
}
