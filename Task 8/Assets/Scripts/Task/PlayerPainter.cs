using UnityEngine;

public class PlayerPainter : MonoBehaviour
{
    private bool colourSelected = false;
    public Color selectedColour = Color.white;
    public ImageManager imageManager;

    private void OnEnable()
    {
        ColourSampler.ColourChanged += ColourSampler_ColourChanged;
        ColourChannelMarker.ColourChannelMarkerClicked += ColourChannelMarker_ColourChannelMarkerClicked;
    }

    private void ColourChannelMarker_ColourChannelMarkerClicked(ColourChannelMarker marker)
    {
        if (colourSelected == false) return;
        imageManager.SetColour(marker.Channel, selectedColour);
    }

    private void OnDisable()
    {
        ColourSampler.ColourChanged -= ColourSampler_ColourChanged;
        ColourChannelMarker.ColourChannelMarkerClicked -= ColourChannelMarker_ColourChannelMarkerClicked;
    }

    private void ColourSampler_ColourChanged(Color newColour)
    {
        colourSelected = true;
        selectedColour = newColour;
    }
}
