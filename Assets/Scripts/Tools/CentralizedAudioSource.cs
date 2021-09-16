using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CentralizedAudioSource : MonoBehaviour
{
    public static CentralizedAudioSource Instance { get; private set; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlaySoundClip(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
