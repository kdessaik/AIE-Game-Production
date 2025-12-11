using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameplayMusicPlayer: MonoBehaviour
{
    [SerializeField] private AudioClip gameplayMusic;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        // 2D audio
        _audioSource.spatialBlend = 0f;

        // playloop
        _audioSource.loop = true;

        // playonawake
        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        if (gameplayMusic != null)
        {
            _audioSource.clip = gameplayMusic;

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("GameplayMusicPlayer: No gameplayMusic assigned!");
        }
    }
}
