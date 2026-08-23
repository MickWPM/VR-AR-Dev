using UnityEngine;

public class LightsabreCuttingEffect : MonoBehaviour
{
    [SerializeField] private AudioSource cuttingSound;
    [SerializeField] private ParticleSystem cutParticleSystem;
    [SerializeField] private LightsabreCutting cuttingScript;

    float timeSinceAudio = 0;
    [SerializeField] private float audioFadeTime = 2f;


    [ContextMenu("Test cut")]
    private void TestCut()
    {
        CutAt(transform.position);
    }

    private void CutAt(Vector3 worldPos)
    {
        ShowCutVisuals(worldPos);
        PlayCutAudioEffect();
    }

    private void ShowCutVisuals(Vector3 worldPos)
    {
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = worldPos;

        cutParticleSystem.Emit(emitParams, 1);
    }

    private bool audioPlaying = false;
    private async void PlayCutAudioEffect()
    {
        timeSinceAudio = 0;
        if (audioPlaying) return;
        audioPlaying = true;
        cuttingSound.Play();

        while (timeSinceAudio < audioFadeTime)
        {
            timeSinceAudio += Time.deltaTime;
            await Awaitable.EndOfFrameAsync();
        }
        cuttingSound.Stop();
        audioPlaying = false;
    }


    private void OnEnable()
    {
        cuttingScript.CutHappenedEvent += CutAt;
    }
    private void OnDisable()
    {
        cuttingScript.CutHappenedEvent -= CutAt;
    }

}
