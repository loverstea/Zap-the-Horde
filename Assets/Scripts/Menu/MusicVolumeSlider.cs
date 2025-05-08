using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource musicSource;

    void Start()
    {
        // Початкове значення слайдера
        volumeSlider.value = musicSource.volume;

        // Прив'язати зміну слайдера до функції
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
