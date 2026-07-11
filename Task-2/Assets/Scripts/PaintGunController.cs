using UnityEngine;

public class PaintGunController : MonoBehaviour
{
    [SerializeField] private Transform muzzleExit;
    [SerializeField] private Painter projectilePrefab;

    public Color paintingColour = Color.white;
    [SerializeField] private bool currentlySamplingColour = false;
    [SerializeField] private Color currentSampleColour = Color.white;
    [SerializeField] private Renderer colourPreviewRenderer;

    private void Awake()
    {
        colourPreviewRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool samplingColourThisFrame = CheckSampleColour();
        SetColourSampleState(samplingColourThisFrame);
    }

    public void SetColourSampleState(bool sampleColours)
    {
        if (sampleColours == currentlySamplingColour) return;

        currentlySamplingColour = sampleColours;
        currentSampleColour = paintingColour;
        colourPreviewRenderer.gameObject.SetActive(currentlySamplingColour);
    }

    private bool CheckSampleColour()
    {
        RaycastHit hit;

        if (Physics.Raycast(muzzleExit.position, muzzleExit.forward, out hit, Mathf.Infinity))
        {
            var sampleSurface = hit.collider.gameObject.GetComponent<ColourSamplingSurface>();
            if (sampleSurface == null) return false;

            DrawLaser(hit);

            var texCoord = hit.textureCoord;

            Renderer renderer = hit.collider.GetComponent<Renderer>();
            Texture2D tex = renderer.sharedMaterial.mainTexture as Texture2D;
            currentSampleColour = tex.GetPixelBilinear(texCoord.x, texCoord.y);
            colourPreviewRenderer.material.SetColor("_BaseColor", currentSampleColour);
            return true;
        }
        return false;
    }

    public void DrawLaser(RaycastHit hit)
    {
        Vector3 dir = hit.point - transform.position;
        Debug.DrawRay(muzzleExit.position, dir, Color.red);
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        if (currentlySamplingColour)
        {
            SetPaintingColour(currentSampleColour);
        }
        else
        {
            Fire(projectilePrefab);
        }
    }

    public void Fire(Painter prefab)
    {
        var painter = Instantiate(prefab, muzzleExit.transform.position, muzzleExit.transform.rotation) as Painter;
        painter.SetColour(paintingColour);
    }

    public event System.Action<Color> PaintingColourChangedEvent;
    public void SetPaintingColour(Color colour)
    {
        paintingColour = colour;
        PaintingColourChangedEvent?.Invoke(paintingColour);
    }
}
