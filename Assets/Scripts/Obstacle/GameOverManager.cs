using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI")]
    public GameObject gameOverUI; // Assign GameOverPanel here

    private void Awake()
    {
        // Safe Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Ensure Game Over UI is hidden at start
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        // Ensure game runs normally
        Time.timeScale = 1f;
    }

    // 🔴 Called when player loses all life
    public void GameOver()
    {
        // Stop everything
        Time.timeScale = 0f;

        // Show Game Over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    // 🔁 Restart current level
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏠 Go back to Main Menu
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Make sure Main Menu is index 0
    }
}
