using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public int score = 0;
    public Text scoreText;

    [Header("Game Over")]
    public GameObject gameOverUI;

    [Header("Game State")]
    public bool isGameStarted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Pause the game at start
        Time.timeScale = 0f;
        isGameStarted = false;

        UpdateScoreUI();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (scoreText != null)
            scoreText.gameObject.SetActive(false); // Hide score initially
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
            PlayerPrefs.SetInt("BestScore", score);

        UIManager.instance?.ShowGameOver(score, PlayerPrefs.GetInt("BestScore"));
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
