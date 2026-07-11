using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    private Color paintingColour = Color.white;
    public void SetColour(Color colour)
    {
        paintingColour = colour;
        objectRenderer.material.SetColor("_BaseColor", paintingColour);
    }
}
