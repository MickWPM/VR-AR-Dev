using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public PaintGunController GunController;
    public int bullets = 3;
    public float delay = 5;
    private void Start()
    {
        FireTest();
    }

    private async Awaitable FireTest()
    {
        for (int i = 0; i < bullets; i++)
        {
            await Awaitable.WaitForSecondsAsync(delay);
            GunController.Fire();
        }
    }
}
