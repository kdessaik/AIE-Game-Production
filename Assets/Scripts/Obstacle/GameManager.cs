using UnityEngine;
using TMPro; // Required for TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score = 0;
    public TMP_Text scoreText; // Changed to TMP_Text

    [Header("Game Over")]
    public GameObject gameOverUI;
    public TMP_Text highScoreText; // Text for GameOverPanel high score

    [Header("Game State")]
    public bool isGameStarted = false;

    void Awake()
    {
       
    }

    void Start()
    {
        // -------------------------------
        // Pause the game at start
        // -------------------------------
        isGameStarted = false;       // prevent player & enemies from moving
        Time.timeScale = 0f;         // freeze all time-based movement

        // -------------------------------
        // UI Setup
        // -------------------------------
        UpdateScoreUI();             // make sure score text is correct, hidden by default

        if (gameOverUI != null)
            gameOverUI.SetActive(false); // hide GameOver panel

        if (scoreText != null)
            scoreText.gameObject.SetActive(false); // hide in-game score until game starts
    }

    // ---------------- SCORE ----------------
    public void AddScore(int amount)
    {
        if (!isGameStarted) return;

        score += amount;
        UpdateScoreUI();
        UIManager.instance?.UpdateScore(score);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = "Score: " + score;
        }
    }

    // ---------------- GAME OVER ----------------
    public void GameOver()
    {
        Time.timeScale = 0f;
        isGameStarted = false;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        int best = PlayerPrefs.GetInt("BestScore", 0);

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("BestScore", best);
            PlayerPrefs.Save(); // ensure it’s saved
        }

        // Update GameOverPanel high score
        if (highScoreText != null)
            highScoreText.text = "Best Score: " + best;

        UIManager.instance?.ShowGameOver(score, best);
    }

    // ---------------- START GAME ----------------
    public void StartGame()
    {
        Time.timeScale = 1f;
        isGameStarted = true;
        score = 0;
        UpdateScoreUI();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }
}
