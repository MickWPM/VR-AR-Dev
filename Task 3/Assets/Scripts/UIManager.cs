using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    private int landings, crashes, activePlanes;
    private void Start()
    {
        UpdateAircraftSummary();
    }

    private void UpdateAircraftSummary()
    {
        summaryText.text = $"Planes in flight: {activePlanes}\r\n\r\nSafe Landings: {landings}\r\n\r\nCrashes: {crashes}";
    }

    public void AircraftSpawned()
    {
        ++activePlanes;
        UpdateAircraftSummary();
    }

    public void AircraftCollision()
    {
        ++crashes;
        --activePlanes;
        UpdateAircraftSummary();
    }

    public void AircraftLanded()
    {
        ++landings;
        --activePlanes;
        UpdateAircraftSummary();
    }
}
