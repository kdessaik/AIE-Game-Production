using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // remove if not using TextMeshPro

public class MainMenu : MonoBehaviour
{
    public TMP_Text bestScoreText; // use Text if not using TMP

    void Start()
    {
        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScoreText != null) bestScoreText.text = "Best Score: " + best;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); // index 1 -> Game Scene
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}