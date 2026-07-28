using UnityEngine;
using System.Collections.Generic;

public class VineController : MonoBehaviour
{
    public Transform growingVinesParent;
    private List<Material> vineMaterials;

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

        UpdateGrowth();
    }

    [SerializeField] private float growthTime = 2.5f;
    [SerializeField]private float growth = 0f;

    //public bool growTest = false;
    //private void Update()
    //{
    //    if (growTest)
    //    {
    //        Grow();
    //    }else
    //    {
    //        Shrink();
    //    }
    //}

    public void Grow()
    {
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
    }
}
