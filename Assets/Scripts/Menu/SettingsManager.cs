using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    public Slider fovSlider;
    public Text fovValueText;
    public Camera mainCamera;

    public Toggle musicToggle;
    public AudioSource musicSource;

    void Start()
    {
        settingsMenu.SetActive(false);

        fovSlider.onValueChanged.AddListener(UpdateFOV);
        fovSlider.value = mainCamera.fieldOfView;

        musicToggle.onValueChanged.AddListener(ToggleMusic);
        musicToggle.isOn = musicSource.isPlaying;
    }

    void UpdateFOV(float value)
    {
        mainCamera.fieldOfView = value;
        fovValueText.text = "FOV: " + Mathf.RoundToInt(value);
    }

    void ToggleMusic(bool isOn)
    {
        if (isOn)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }
    }

    public void OnBackButtonClick()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);

    }

    public void OnResetButtonClick()
    {
        fovSlider.value = 60; // Reset to default FOV
        mainCamera.fieldOfView = 60;
        fovValueText.text = "FOV: 60";
        musicToggle.isOn = true; // Reset music toggle to on
        musicSource.Play(); // Play music again
    }
    
}
