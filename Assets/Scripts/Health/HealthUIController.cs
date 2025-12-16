using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthUIController : MonoBehaviour
{
    // Drag your Health UI Panel here in the Inspector
    public GameObject healthUI;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Player Development scene index = 1
        if (healthUI != null)
        {
            healthUI.SetActive(scene.buildIndex == 1);
        }
    }
}
