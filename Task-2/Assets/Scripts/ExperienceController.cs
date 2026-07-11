using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public PaintGunController GunController;
    private void Start()
    {
        FireTest();
    }

    private async Awaitable FireTest()
    {
        for (int i = 0; i < 3; i++)
        {
            await Awaitable.WaitForSecondsAsync(1f);
            GunController.Fire();
        }
    }
}
