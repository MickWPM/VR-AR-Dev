using UnityEngine;
using System.Collections.Generic;

public class VineController : MonoBehaviour
{
    public Transform growingVinesParent;
    private List<Material> vineMaterials;
    public GameObject leafParticles;

    private void Awake()
    {
        MeshRenderer[] mrs = growingVinesParent.GetComponentsInChildren<MeshRenderer>();
        vineMaterials = new List<Material>();
        foreach (MeshRenderer mr in mrs)
        {
            foreach (Material m in mr.materials)
            {
                vineMaterials.Add(m);
            }
        }
        leafParticles.SetActive(false);
        UpdateGrowth();
    }

    [SerializeField] private float growthTime = 2.5f;
    [SerializeField]private float growth = 0f;

    public bool fullGrow = false;
    public bool fullWithdraw = false;
    private void Update()
    {
        if (fullGrow)
        {
            Grow();
            if(1f - growth < Mathf.Epsilon) fullGrow = false;
        }
        else if (fullWithdraw)
        {
            Shrink();
            if (growth < Mathf.Epsilon) fullWithdraw = false;
        }
    }

    public void FullGrow()
    {
        Debug.Log("Grow called");
        fullGrow = true;
    }
    public void FullWithdraw()
    {
        fullWithdraw = true;
    }

    public void Grow()
    {
        Debug.Log("Growing");
        float growthRate = 1 / growthTime;
        growth += Time.deltaTime * growthRate;
        UpdateGrowth();
    }

    public void Shrink()
    {
        float growthRate = 1 / growthTime;
        growth -= Time.deltaTime * growthRate;
        UpdateGrowth();
    }

    private void UpdateGrowth()
    {
        growth = Mathf.Clamp01(growth);

        foreach (var vineMat in vineMaterials)
        {
            vineMat.SetFloat("_Growth", growth);
        }


        leafParticles.SetActive(growth > 0.95f);
    }
}
