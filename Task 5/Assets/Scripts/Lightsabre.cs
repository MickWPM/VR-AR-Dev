using UnityEngine;
using UnityEngine.InputSystem;

public class Lightsabre : MonoBehaviour
{
    public InputAction lightsabreToggleAction;

    [SerializeField] private SabreState sabreState = SabreState.Retracted;
    private LightsabreCutting cuttingScript;
    private LightsabreBeamVisuals beamVisuals;
    [SerializeField]private float extensionTime = 0.5f;
    private float currentExtensionPercent = 0;

    private void Awake()
    {
        lightsabreToggleAction.started += ToggleLightsabre;
        lightsabreToggleAction.Enable();

        cuttingScript = gameObject.GetComponent<LightsabreCutting>();
        beamVisuals = gameObject.GetComponentInChildren<LightsabreBeamVisuals>();
    }

    public event System.Action<SabreState> SabreStateChangedEvent;
    public event System.Action<float> SabreExtensionUpdatedEvent;
    private void Start()
    {
        cuttingScript.enabled = false;
        beamVisuals.enabled = false;

        SabreExtensionUpdatedEvent?.Invoke(currentExtensionPercent);
    }

    private void ToggleLightsabre(InputAction.CallbackContext context)
    {
        Debug.Log("ToggleLightsabre");
        if (sabreState == SabreState.Retracted)
        {
            ExtendSabre();
        } else if (sabreState == SabreState.Extended)
        {
            RetractSabre();
        }
    }

    private void ExtendSabre()
    {
        Debug.Log("ExtendSabre");
        sabreState = SabreState.Extending;
        SabreStateChangedEvent?.Invoke(sabreState);
        ChangeSabreState();
    }

    private void RetractSabre()
    {
        Debug.Log("RetractSabre");
        sabreState = SabreState.Retracting;
        SabreStateChangedEvent?.Invoke(sabreState);
        ChangeSabreState();
    }

    private async void ChangeSabreState()
    {
        Debug.Log("ChangeSabreState");
        bool complete = false;
        int mul = sabreState == SabreState.Retracting ? -1 : 1;
        while (!complete)
        {
            float delta = Time.deltaTime / extensionTime * mul;
            currentExtensionPercent += delta;
            complete = sabreState == SabreState.Retracting ? currentExtensionPercent <= 0 : currentExtensionPercent >= 1;
            currentExtensionPercent = Mathf.Clamp01(currentExtensionPercent);
            SabreExtensionUpdatedEvent?.Invoke(currentExtensionPercent);
            await Awaitable.EndOfFrameAsync();
        }
        sabreState = sabreState == SabreState.Retracting ? SabreState.Retracted : SabreState.Extended;
        SabreStateChangedEvent?.Invoke(sabreState);
    }

    public enum SabreState
    { 
        Retracted,
        Extended,
        Extending,
        Retracting
    }
}
