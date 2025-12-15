using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the Game Scene
        SceneManager.LoadScene(1); // Game scene index

        // Start coroutine to wait for scene and GameManager
        StartCoroutine(StartGameAfterSceneLoads());
    }

    private IEnumerator StartGameAfterSceneLoads()
    {
        // Wait until the next frame to ensure the scene is loaded
        yield return null;

        // Find the GameManager in the scene
        if (GameManager.Instance == null)
        {
            GameManager.Instance = FindObjectOfType<GameManager>();
        }

        // Start the game
        GameManager.Instance?.StartGame();
    }
}
