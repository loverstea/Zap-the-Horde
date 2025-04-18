using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider fovSlider;
    public Text fovValueText;
    public Camera mainCamera;

    public Toggle musicToggle;
    public AudioSource musicSource;

    void Start()
    {
        // FOV
        fovSlider.onValueChanged.AddListener(UpdateFOV);
        fovSlider.value = mainCamera.fieldOfView;

        // Music
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
            musicSource.Pause(); // або .Stop()
        }
    }
}
