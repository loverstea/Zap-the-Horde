using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    
    public GameObject settingsMenu;
    public GameObject Graphics;
    public GameObject Audio;
    public GameObject Controls;
    public GameObject MainMenuPanel;


    public Slider fovSlider;
    public Text fovValueText;
    public Camera mainCamera;

    public Toggle musicToggle;
    public AudioSource musicSource;

    void Start()
    {
        settingsMenu.SetActive(false);

        fovSlider.minValue = 60;
        fovSlider.maxValue = 100;

        fovSlider.onValueChanged.AddListener(UpdateFOV);
        fovSlider.value = mainCamera.fieldOfView;

        musicToggle.onValueChanged.AddListener(ToggleMusic);
        musicToggle.isOn = musicSource.isPlaying;
    }

    void UpdateFOV(float value)
    {
        value = Mathf.Clamp(value, 30, 100);
        mainCamera.fieldOfView = value;
        fovValueText.text = " " + Mathf.RoundToInt(value);
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
    public void OnGraphicsButtonClick()
    {
        Graphics.SetActive(true);
        Audio.SetActive(false);
        Controls.SetActive(false);
    }
    public void OnAudioButtonClick()
    {
        Graphics.SetActive(false);
        Audio.SetActive(true);
        Controls.SetActive(false);
    }
    public void OnControlsButtonClick()
    {
        Graphics.SetActive(false);
        Audio.SetActive(false);
        Controls.SetActive(true);
    }

    public void OnBackButtonClick()
    {
            settingsMenu.SetActive(false);
            MainMenuPanel.SetActive(true);
    }

    public void OnResetButtonClick()
    {
        fovSlider.value = 60;
        mainCamera.fieldOfView = 60;
        fovValueText.text = " 60";
        musicToggle.isOn = true;
        musicSource.Play();
    }
}
