//Esther Namulen
using UnityEngine;
using System.Collections;

public class SkyboxSwitcher : MonoBehaviour
{
    [Header("Skyboxes")]
    public Material[] skyboxes;          // Assign 12 skyboxes here

    [Header("Timing")]
    public float switchInterval = 10f;   // Time between switches

    void Start()
    {
        if (skyboxes.Length == 0) return;
        // Start with a random skybox
        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Length)];
        StartCoroutine(SwitchSkyboxes());
    }

    IEnumerator SwitchSkyboxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);

            // pick a random skybox, avoid repeating the current one
            Material newSkybox;
            do
            {
                newSkybox = skyboxes[Random.Range(0, skyboxes.Length)];
            } while (newSkybox == RenderSettings.skybox);

            RenderSettings.skybox = newSkybox;
        }
    }
}
