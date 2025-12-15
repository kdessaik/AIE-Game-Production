//Made by Kambale Kibeho Dessai
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text scoreText;
    public TMP_Text bestText;
    public GameObject gameOverPanel;

    void Awake()
    {
       
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ShowGameOver(int score, int bestScore)
    {
        if (bestText != null)
            bestText.text = "Best: " + bestScore;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(1);




    }

    public void ExitMenu()
    {
        SceneManager.LoadScene(0);
    }
}
