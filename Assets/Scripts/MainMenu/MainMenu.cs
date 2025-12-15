using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Listen for when the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load the Player Development scene
        SceneManager.LoadScene(1); // game scene index
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) // ensure it’s the Player Development scene
        {
            // Find the GameManager in the new scene
            GameManager gm = GameManager.Instance;
            if (gm == null)
            {
                gm = FindObjectOfType<GameManager>();
                GameManager.Instance = gm; // set the static instance
            }

            // Start the game
            gm?.StartGame();

            // Stop listening
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // ------------------ Exit the game ------------------
    public void ExitGame()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}
