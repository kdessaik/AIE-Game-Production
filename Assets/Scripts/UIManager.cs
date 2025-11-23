using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text bestText;
    public GameObject gameOverPanel;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keeps UIManager between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Hide GameOver panel at the start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Update the in-game score
    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // Show GameOver panel and display the best score
    public void ShowGameOver(int score, int bestScore)
    {
        if (bestText != null)
            bestText.text = "Best: " + bestScore;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // Restart the current game scene
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Exit to main menu or quit application
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Editor
#else
            Application.Quit(); // Quits the built application
#endif
    }
}
