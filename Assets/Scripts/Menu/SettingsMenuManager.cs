using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    
    public GameObject settingsMenu;

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

    public void OnSettingsButtonClick()
    {
            settingsMenu.SetActive(true);
    }

    public void OnBackButtonClick()
    {
            settingsMenu.SetActive(false);
    }

    public void OnResetButtonClick()
    {
        fovSlider.value = 60;
        mainCamera.fieldOfView = 60;
        fovValueText.text = "FOV: 60";
        musicToggle.isOn = true;
        musicSource.Play();
    }
    
}
