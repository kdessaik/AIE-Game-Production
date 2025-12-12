using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private void Awake()
    {
        // Play menu music
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
            audio.Play();
    }
}
