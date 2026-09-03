using UnityEngine;

public class ColourSampleSpawner : MonoBehaviour
{
    public Color[] colours;
    public float ySpacing = 0.2f, xSpacing = 0.2f;
    public int maxInRow = 6;

    public ColourSampler colourSamplerPrefab;

    private void Awake()
    {
        SpawnSamplers();
    }

    private void SpawnSamplers()
    {
        int numRows = colours.Length % maxInRow;
        int spawnNum = 0;
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < maxInRow; j++)
            {
                var sampler = Instantiate(colourSamplerPrefab);
                sampler.transform.SetParent(this.transform);
                sampler.transform.localPosition = new Vector3(j * xSpacing, i * ySpacing, 0);
                sampler.transform.localRotation = Quaternion.identity;
                sampler.SetupWithColour(colours[spawnNum]);
                spawnNum++;
                if (spawnNum > colours.Length) return;
            }

        }
    }
}
