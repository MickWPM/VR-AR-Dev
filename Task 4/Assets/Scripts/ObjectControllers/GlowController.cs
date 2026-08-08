using UnityEngine;

public class GlowController : MonoBehaviour
{
    public float glowSizeIncrease = 0.2f;
    public float glowPeriod = 2f;
    public GameObject glowObject;
    private Vector3 baseGlowScale;
    bool expanding = true;
    float expansionPerSecond;
    float currentScaleIncrease;

    private void Start()
    {
        baseGlowScale = glowObject.transform.localScale;
        currentScaleIncrease = 0;
        expansionPerSecond = 2 * glowSizeIncrease / glowPeriod;
    }


    private void Update()
    {
        expansionPerSecond = 2 * glowSizeIncrease / glowPeriod;
        if (expanding)
        {
            Expand();
        } else
        {
            Contract();
        }
    }

    private void Expand()
    {
        currentScaleIncrease += expansionPerSecond * Time.deltaTime;
        UpdateVisual();
        if (currentScaleIncrease  > glowSizeIncrease)
        {
            expanding = false;
        }
    }

    private void Contract()
    {
        currentScaleIncrease -= expansionPerSecond * Time.deltaTime;
        UpdateVisual();
        if (currentScaleIncrease < 0.01f)
        {
            expanding = true;
        }
    }

    private void UpdateVisual()
    {
        glowObject.transform.localScale = baseGlowScale * (1 + currentScaleIncrease);
    }

}
