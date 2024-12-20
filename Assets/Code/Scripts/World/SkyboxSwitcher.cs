using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{
    [Header("Skyboxes")]
    public Material daySkybox;
    public Material nightSkybox;

    private bool isDay = true;

    void Update()
    {
        // Check for the "N" key press
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleSkybox();
        }
    }

    void ToggleSkybox()
    {
        // Toggle between day and night
        if (isDay)
        {
            RenderSettings.skybox = nightSkybox;
        }
        else
        {
            RenderSettings.skybox = daySkybox;
        }

        // Update the lighting to match the new skybox
        DynamicGI.UpdateEnvironment();

        // Toggle the state
        isDay = !isDay;
    }
}
