using UnityEngine;

public class StormController : MonoBehaviour
{
    public GameObject lightningParent;
    private float yPos;

    private void Start()
    {
        for (int i = 0; i < lightningParent.transform.childCount; i++)
        {
            _ = RunStorm(lightningParent.transform.GetChild(i).gameObject);
        }
    }


    public Vector2 lightningStrikeDelayRange = new Vector2(0.1f, 1f);
    public float lightningStrikePositionOffsetRange = 0.1f;
    async Awaitable RunStorm(GameObject lightningBolt)
    {
        lightningBolt.SetActive(false);
        yPos = lightningBolt.transform.localPosition.y;

        while (true)
        {
            var pos = new Vector3(
                Random.Range(-lightningStrikePositionOffsetRange, lightningStrikePositionOffsetRange),
                yPos,
                Random.Range(-lightningStrikePositionOffsetRange, lightningStrikePositionOffsetRange));
            var delay = Random.Range(lightningStrikeDelayRange.x, lightningStrikeDelayRange.y);

            lightningBolt.SetActive(false);
            lightningBolt.transform.localPosition = pos;
            lightningBolt.transform.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(-90, 90), -90));

            await Awaitable.WaitForSecondsAsync(delay);
            lightningBolt.SetActive(true);
            await Awaitable.WaitForSecondsAsync(0.3f);
            lightningBolt.SetActive(false);
        }
    }
}
