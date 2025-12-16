using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    // ---------------- START GAME ----------------
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    private IEnumerator StartGameAfterSceneLoads()
    {
        yield return null;

        if (GameManager.Instance == null)
        {
            GameManager.Instance = FindObjectOfType<GameManager>();
        }

        GameManager.Instance?.StartGame();
    }

    // ---------------- EXIT GAME ----------------
    public void ExitGame()
    {
        Debug.Log("Exiting Game...");

        // This works in a built game
        Application.Quit();

#if UNITY_EDITOR
        // This makes Exit work in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
