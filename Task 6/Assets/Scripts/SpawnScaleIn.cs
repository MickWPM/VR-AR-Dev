using UnityEngine;

public class SpawnScaleIn : MonoBehaviour
{
    public float startScaler = 0.01f, endScaler = 1.0f;
    public float scaleTime = 1.5f;
    public AnimationCurve scaleCurve;

    private Vector3 originalScale;
    private float t = 0;
    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Start()
    {
        AnimateScale();
    }

    private async void AnimateScale()
    {
        while (t < 1)
        {
            t += Time.deltaTime / scaleTime;
            float scaler = scaleCurve.Evaluate(t);
            Vector3 scale = originalScale * scaleCurve.Evaluate(t);
            transform.localScale = scale;
            await Awaitable.EndOfFrameAsync();
        }
        this.enabled = false;
    }
}
