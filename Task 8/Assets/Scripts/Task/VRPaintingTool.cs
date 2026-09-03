using UnityEngine;

public class VRPaintingTool : MonoBehaviour
{
    private Color currentColour = Color.white;

    private MeshRenderer mr;
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        mr.material.color = currentColour;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered from {other.gameObject.name}");
        ColourSampler sampler = other.gameObject.GetComponentInParent<ColourSampler>();
        if (sampler != null )
        {
            TriggeredSampler(sampler);
            return;
        }

        ColourChannelMarker marker = other.GetComponentInParent<ColourChannelMarker>();
        if (marker != null )
        {
            TriggeredChannelMarker(marker);
            return;
        }
    }

    private void TriggeredSampler(ColourSampler sampler)
    {
        currentColour = sampler.Colour;
        mr.material.color = currentColour;
        sampler.ExternalSelectColour();
    }
    private void TriggeredChannelMarker(ColourChannelMarker marker)
    {
        marker.ExternalSelectMarker();
    }
}
