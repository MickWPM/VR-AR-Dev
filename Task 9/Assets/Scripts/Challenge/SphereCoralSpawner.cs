using UnityEngine;

public class SphereCoralSpawner : MonoBehaviour
{
    public Vector2Int numSpawnRange = new Vector2Int(2, 5);
    public float sphereRadius = 1f;
    public Transform budParent;
    private void Awake()
    {
        int numToSpawn = Random.Range(numSpawnRange.x, numSpawnRange.y);
        if (budParent ==  null ) budParent = transform;

        Debug.Log($"Spawning {numToSpawn} buds");
        for (int i = 0; i < numToSpawn; i++)
        {
            Vector3 dir = Random.onUnitSphere;
            dir.y = Mathf.Abs( dir.y );
            Vector3 localPos = dir * sphereRadius;
            var newGo = new GameObject("Coral bud");
            newGo.transform.SetParent( budParent );
            newGo.transform.localPosition = localPos;
            newGo.transform.up = dir;
        }
    }

}
