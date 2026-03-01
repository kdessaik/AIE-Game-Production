using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuMusicPlayer: MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 0f;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        if (menuMusic != null)
        {
            _audioSource.clip = menuMusic;

            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
    }
}
