using UnityEngine;
using UnityEngine.EventSystems;

public class ColourChannelMarker : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MeshRenderer channelMarkerMeshRenderer;
    [SerializeField] private ImageManager imageManager;

    private int myChannel = -1;
    public int Channel => myChannel;
    private Color myColor = Color.white;


    private void ImageManager_ColourChannelUpdated(int channel, Color newColour)
    {
        if (channel != myChannel) return;

        UpdateMarker(newColour);
    }

    private bool setupComplete = false;
    public void SetupMarker(int channel, Color colour, ImageManager imageManager)
    {
        this.myChannel = channel;
        this.myColor = colour;
        channelMarkerMeshRenderer.material.color = myColor;
        this.imageManager = imageManager;

        imageManager.ColourChannelUpdated += ImageManager_ColourChannelUpdated;
        setupComplete = true;
    }


    private void UpdateMarker(Color newColour)
    {
        if (!setupComplete) return;
        if (myColor == newColour) return;
        myColor = newColour;
        channelMarkerMeshRenderer.material.color = myColor;
    }



    private void OnDisable()
    {
        if (!setupComplete) return;
        imageManager.ColourChannelUpdated -= ImageManager_ColourChannelUpdated;
    }


    public static event System.Action<ColourChannelMarker> ColourChannelMarkerClicked;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        ColourChannelMarkerClicked?.Invoke(this);
    }

    public void ExternalSelectMarker()
    {
        ColourChannelMarkerClicked?.Invoke(this);
    }
}
