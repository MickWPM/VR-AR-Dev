using System.Buffers.Text;
using UnityEngine;
using UnityEngine.Events;

public class Painter : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    private Color paintingColour = Color.white;
    public void SetColour(Color colour)
    {
        paintingColour = colour;
        objectRenderer.material.SetColor("_BaseColor", paintingColour);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var canvas = collision.gameObject.GetComponentInParent<PainterCanvas>();
        if (canvas == null)
        {
            Destroy(this.gameObject);
            return;
        }

        AddToCanvas(canvas);
    }

    public UnityEvent AddedToCanvasEvent;
    private void AddToCanvas(PainterCanvas canvas)
    {
        canvas.AddToCanvas(this.gameObject);
        AddedToCanvasEvent?.Invoke();
    }
}
