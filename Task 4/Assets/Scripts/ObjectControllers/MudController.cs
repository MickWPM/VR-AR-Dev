using System.Collections.Generic;
using UnityEngine;

public class MudController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float hydrationLevel = 0.1f;
    public MeshRenderer mudRenderer;
    private Material mudMat;
    

    private void Awake()
    {
        mudMat = mudRenderer.material;
        UpdateHydration();
    }

    private void UpdateHydration()
    {
        hydrationLevel = Mathf.Clamp01(hydrationLevel);
        mudMat.SetFloat("_WaterHeight", hydrationLevel);
    }

    public void ChangeWater(float waterPercentPerSecond)
    {
        Debug.Log("Adding water");
        hydrationLevel += waterPercentPerSecond * Time.deltaTime;
        UpdateHydration();
    }
}
