using UnityEngine;
using UnityEngine.EventSystems;

public class ColourSampler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private Color myColour;
    public Color Colour => myColour;
    [SerializeField] private MeshRenderer mr;
    private void Awake()
    {
        SetupWithColour(myColour);
    }


    public void SetupWithColour(Color colour)
    {
        myColour = colour;
        mr.material.color = myColour;
    }

    public static event System.Action<Color> ColourChanged;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        ColourChanged?.Invoke(myColour);
        Debug.Log("Colour change clicked", gameObject);
    }
}
