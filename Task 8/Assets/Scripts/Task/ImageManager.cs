using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class ImageManager : MonoBehaviour
{
    public Texture2D baseImage;
    public MeshRenderer baseRenderer, quantisedRenderer;
    private Texture2D quantisedImage;
    //private Dictionary<int, Color> colourChannelColours;
    //private Dictionary<int, List<int>> colourChannelIndexes;
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
    }

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
    }
}
