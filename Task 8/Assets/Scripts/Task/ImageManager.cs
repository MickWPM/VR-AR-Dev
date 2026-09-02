using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class ImageManager : MonoBehaviour
{
    public Texture2D baseImage;
    public MeshRenderer baseRenderer, quantisedRenderer;
    private Texture2D quantisedImage;
    private Color[] colourChannelColours;
    private List<int>[] colourChannelIndexes;

    public int channelSelected = 1;
    public Color newColour = Color.yellow;

    [SerializeField]private int quantisedLevels = 6;
    private void Awake()
    {
        baseRenderer.material.mainTexture = baseImage;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            UpdateQuantised();
        }
    }

    [ContextMenu("Set Colour")]
    public void TestSetColour()
    {
        SetColour(channelSelected, newColour);
    }

    public event System.Action<int, Color> ColourChannelUpdated;
    public void SetColour(int channel,  Color newColour)
    {
        Color[] colours = quantisedImage.GetPixels();
        foreach (var index in colourChannelIndexes[channel])
        {
            colours[index] = newColour;
        }
        colourChannelColours[channel] = newColour;
        quantisedImage.SetPixels(colours);
        quantisedImage.Apply();
        ColourChannelUpdated?.Invoke(channel, newColour);
    }

    public event System.Action<Color[]> QuantisedImageChannelsUpdated;
    public void UpdateQuantised()
    {
        Dictionary<Color, List<int>> colourChannelPixels;
        (quantisedImage,colourChannelPixels)= ImageSimplifier.GetQuantisedTexture(baseImage, quantisedLevels);

        var colours = colourChannelPixels.Keys.ToArray<Color>();
        quantisedLevels = colours.Length;

        colourChannelColours = new Color[quantisedLevels];//new Dictionary<int, Color>();
        colourChannelIndexes = new List<int>[quantisedLevels]; //new Dictionary<int, List<int>>();
        for (int i = 0; i < quantisedLevels; i++)
        {
            Color c = colours[i];
            List<int> pixelIndexes = colourChannelPixels[c];
            
            colourChannelColours[i] = c;
            colourChannelIndexes[i] = pixelIndexes;

        }

        quantisedRenderer.material.mainTexture = quantisedImage;
        QuantisedImageChannelsUpdated?.Invoke(colourChannelColours);
    }

    //Better as a seperate class
    [SerializeField] private Transform markerTransformParent = null;
    [SerializeField] private ColourChannelMarker markerPrefab;
    private void ImageManager_QuantisedImageChannelsUpdated(Color[] colours)
    {
        //Lazy catch null parent
        if (markerTransformParent == null)
        {
            var Go = new GameObject("Marker parent");
            Go.transform.SetParent(this.transform);
            markerTransformParent = Go.transform;
        } 

        //Clear any previous markers
        while (markerTransformParent.childCount > 0)
        {
            var go = markerTransformParent.GetChild(0).gameObject;
            go.transform.SetParent(null);
            Destroy(go);
        }

        //Create our new markers
        for (int i = 0; i < colours.Length; i++)
        {
            var marker = Instantiate(markerPrefab);
            marker.name = $"Channel {i} marker";
            marker.transform.SetParent(markerTransformParent);
            marker.transform.localPosition = new Vector3(i * 0.2f, 0, 0);
            marker.SetupMarker(i, colours[i], this);
        }
    }

    private void OnEnable()
    {
        this.QuantisedImageChannelsUpdated += ImageManager_QuantisedImageChannelsUpdated;
    }


    private void OnDisable()
    {
        this.QuantisedImageChannelsUpdated += ImageManager_QuantisedImageChannelsUpdated;
    }
}
