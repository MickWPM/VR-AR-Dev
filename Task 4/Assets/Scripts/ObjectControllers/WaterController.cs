using UnityEngine;

public class WaterController : MonoBehaviour
{
    public GameObject steamGO;

    [Range(0, 1)]
    [SerializeField] private float waterLevel = 0.1f;
    public MeshRenderer waterRenderer;
    private Material waterMat;
    float steamLife = 0f;

    private void Awake()
    {
        steamGO.SetActive(false);
        waterMat = waterRenderer.material;
        UpdateWaterLevel();
    }

    private void Update()
    {
        steamLife -= Time.deltaTime;
        steamGO.SetActive(steamLife > 0);
    }
    private void UpdateWaterLevel()
    {
        waterLevel = Mathf.Clamp01(waterLevel);
        waterMat.SetFloat("_FillLevel", waterLevel);
    }

    public void ChangeWater(float waterPercentPerSecond)
    {
        waterLevel += waterPercentPerSecond * Time.deltaTime;
        UpdateWaterLevel();
    }

    public void SetSteamLife(float life)
    {
        steamLife = life;
    }
}
