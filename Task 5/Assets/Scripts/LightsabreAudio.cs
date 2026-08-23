using UnityEngine;

public class LightsabreAudio : MonoBehaviour
{
    [SerializeField] private Lightsabre lightsabre;
    [SerializeField] private AudioSource lightsabreAudioSource;

    private void Awake()
    {
        lightsabre.SabreStateChangedEvent += this.Lightsabre_SabreStateChangedEvent;
    }

    public AudioClip extendAudio, retractAudio;
    private void Lightsabre_SabreStateChangedEvent(Lightsabre.SabreState newState)
    {
        switch (newState)
        {
            case Lightsabre.SabreState.Retracted:
                break;
            case Lightsabre.SabreState.Extended:
                lightsabreAudioSource.Play();
                break;
            case Lightsabre.SabreState.Extending:
                lightsabreAudioSource.Stop();
                lightsabreAudioSource.PlayOneShot(extendAudio);
                break;
            case Lightsabre.SabreState.Retracting:
                lightsabreAudioSource.Stop();
                lightsabreAudioSource.PlayOneShot(retractAudio);
                break;
            default:
                break;
        }
    }
}
