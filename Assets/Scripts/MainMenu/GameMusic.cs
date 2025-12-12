using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private void Awake()
    {
        // Start playing game music only in game scene
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
            audio.Play();
    }
}