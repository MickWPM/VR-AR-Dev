using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Fusion;

public class ImageManager : NetworkBehaviour
{
    public Texture2D baseImage;
    public MeshRenderer baseRenderer, quantisedRenderer;
    private Texture2D quantisedImage;
    [SerializeField]private int quantisedLevels = 6;

    private Color[] colourChannelColours;
    private List<int>[] colourChannelIndexes;
    [SerializeField]private bool[] channelsSetup;

    public bool clearImageOnInit = true;
    private void Awake()
    {
        baseRenderer.material.mainTexture = baseImage;
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Runner.IsSharedModeMasterClient)
        {
            Debug.Log("IsSharedModeMasterClient");
            UpdateQuantised();
            if (clearImageOnInit) SetAllChannelsToColour(Color.white);
            channelsSetup = new bool[quantisedLevels];
            for (int i = 0; i < quantisedLevels; i++)
            {
                channelsSetup[i] = true;
            }

            ImageSetupCompleteEvent?.Invoke();
            SetupCompleteEvent?.Invoke();
        }
        else
        {
            Debug.Log("Not IsSharedModeMasterClient");
            DataUpdateRequestedRPC(Runner.LocalPlayer);
        }
    }


    public void SetAllChannelsToColour(Color col)
    {
        for (int i = 0; i < colourChannelIndexes.Length; i++)
        {
            SetColour(i, col);
        }
    }

    //RpcChannel.ReliableLargeData is not needed here but it is a simple way to fix mid-join updates
    //This results in slower updates for already connected clients
    //but it ensures that colour updates are not lost for clients mid connection
    //The alternative would be to implement a queued command system for join in progress clients to execute after setup is complete
    //This would give a better experience for already joined clients.
    [Rpc(RpcSources.All, RpcTargets.All, Channel = RpcChannel.ReliableLargeData)] 
    public void SetColourRPC(int channel, Color col)
    {
        ExecuteSetColour(channel, col);
    }

    public event System.Action<int, Color> ColourChannelUpdated;
    public void SetColour(int channel, Color newColour)
    {
        SetColourRPC(channel, newColour);
    }

    private void ExecuteSetColour(int channel, Color col)
    {
        if (colourChannelIndexes == null || colourChannelIndexes.Length <= channel || colourChannelIndexes[channel] == null) return;
        Color[] colours = quantisedImage.GetPixels();
        foreach (var index in colourChannelIndexes[channel])
        {
            colours[index] = col;
        }
        colourChannelColours[channel] = col;
        quantisedImage.SetPixels(colours);
        quantisedImage.Apply();
        ColourChannelUpdated?.Invoke(channel, col);
    }


    public int channelSelected = 1;
    public Color newColour = Color.yellow;

    [ContextMenu("Test set colour")]
    public void TestSetColour()
    {
        SetColour(channelSelected, newColour);
    }

    #region ClientOnly
    public event System.Action ImageSetupCompleteEvent;
    public UnityEngine.Events.UnityEvent SetupCompleteEvent;
    //Reliable large data needed here to pass the array
    [Rpc(RpcSources.All, RpcTargets.All, Channel = RpcChannel.ReliableLargeData)]
    public void SetColourChannelIndexesRPC([RpcTarget] PlayerRef targetPlayer, int numChannels, int channel, int[] indexesThisChannel, Color colourThisChannel)
    {
        if (Runner.IsSharedModeMasterClient == true) return;
        if (colourChannelColours == null || colourChannelColours.Length == 0)
        {
            colourChannelColours = new Color[numChannels];
            quantisedImage = new Texture2D(baseImage.width, baseImage.height);
            quantisedRenderer.material.mainTexture = quantisedImage;


            channelsSetup = new bool[numChannels];
            for (int i = 0; i < numChannels; i++)
            {
                channelsSetup[i] = false;
            }
        }
        if (colourChannelIndexes == null || colourChannelIndexes.Length == 0)
        {
            this.colourChannelIndexes = new List<int>[numChannels];
            for (int i = 0; i < numChannels; i++)
            {
                this.colourChannelIndexes[i] = new List<int>();
            }
        }
        this.colourChannelColours[channel] = colourThisChannel;
        this.colourChannelIndexes[channel] = new List<int>(indexesThisChannel);
        ExecuteSetColour(channel, colourThisChannel);
        QuantisedImageChannelsUpdated?.Invoke(colourChannelColours);

        channelsSetup[channel] = true;
        bool setupComplete = true;
        for (int i = 0; i < numChannels; i++)
        {
            if (channelsSetup[i] == false) setupComplete = false;
        }
        if (setupComplete)
        {
            Debug.Log("Setup is complete - game time");
            ImageSetupCompleteEvent?.Invoke();
            SetupCompleteEvent?.Invoke();
        }
    }

    #endregion

    #region ServerOnly

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void DataUpdateRequestedRPC(PlayerRef requester)
    {
        if (Runner.IsSharedModeMasterClient == false) return;
        for (int i = 0; i < quantisedLevels; i++)
        {
            SetColourChannelIndexesRPC(requester, quantisedLevels, i, colourChannelIndexes[i].ToArray(), colourChannelColours[i]);
        }
    }

    public event System.Action<Color[]> QuantisedImageChannelsUpdated;
    public void UpdateQuantised()
    {
        Dictionary<Color, List<int>> colourChannelPixels;
        (quantisedImage,colourChannelPixels)= ImageSimplifier.GetQuantisedTexture(baseImage, quantisedLevels);

        var colours = colourChannelPixels.Keys.ToArray<Color>();
        quantisedLevels = colours.Length;

        colourChannelColours = new Color[quantisedLevels];
        colourChannelIndexes = new List<int>[quantisedLevels];
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
    #endregion

    #region RefactorOpportunities
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
    #endregion

    private void OnEnable()
    {
        this.QuantisedImageChannelsUpdated += ImageManager_QuantisedImageChannelsUpdated;
    }


    private void OnDisable()
    {
        this.QuantisedImageChannelsUpdated -= ImageManager_QuantisedImageChannelsUpdated;
    }
}
