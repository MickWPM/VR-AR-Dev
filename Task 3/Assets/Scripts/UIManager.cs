using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    public GameObject gameOverGO;
    private int landings, crashes, activePlanes;
    private void Start()
    {
        gameOverGO.SetActive(false);
        UpdateAircraftSummary();
    }

    private void UpdateAircraftSummary()
    {
        summaryText.text = $"Planes in flight: {activePlanes}\r\n\r\nSafe Landings: {landings}\r\n\r\nAircraft lost to crashes: {crashes}";
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

    public void GameOver()
    {
        gameOverGO.SetActive(true);
        _ = FlashText();
    }

    async Awaitable FlashText()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(1);
            gameOverGO.SetActive(false);
            await Awaitable.WaitForSecondsAsync(1);
            gameOverGO.SetActive(true);
        }
    }
}
