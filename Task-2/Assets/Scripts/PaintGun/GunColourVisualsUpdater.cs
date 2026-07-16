using UnityEngine;

public class GunColourVisualsUpdater : MonoBehaviour
{
    public PaintGunController gunController;
    public Renderer[] renderersToUpdate;

    private void Start()
    {
        GunController_PaintingColourChangedEvent(gunController.paintingColour);
    }

    private void GunController_PaintingColourChangedEvent(Color col)
    {
        foreach (Renderer renderer in renderersToUpdate)
        { 
            renderer.material.SetColor("_BaseColor", col);
        }
    }

    private void OnEnable()
    {
        gunController.PaintingColourChangedEvent += GunController_PaintingColourChangedEvent;
    }


    private void OnDisable()
    {
        gunController.PaintingColourChangedEvent -= GunController_PaintingColourChangedEvent;
    }
}
