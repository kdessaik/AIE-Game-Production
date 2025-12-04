using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score = 0;
    public Text scoreText;   // assign UI text
    public GameObject gameOverUI; // assign a UI panel to show on game over

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        if (gameOverUI) gameOverUI.SetActive(false);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        if (gameOverUI != null) gameOverUI.SetActive(true);
        // optionally pause or stop spawning
        Time.timeScale = 0f;
    }

    // Optional restart
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
