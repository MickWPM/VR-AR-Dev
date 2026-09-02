using UnityEngine;
using UnityEngine.InputSystem;

public class ImageManager : MonoBehaviour
{
    public Texture2D baseImage;
    public MeshRenderer baseRenderer, quantisedRenderer;

    public int quantisedLevels = 3 * 4;
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

    public void UpdateQuantised()
    {
        quantisedRenderer.material.mainTexture = ImageSimplifier.GetQuantisedTexture(baseImage, quantisedLevels);    
    }
}
