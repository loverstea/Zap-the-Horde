using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource musicSource;

    void Start()
    {
        // ��������� �������� ��������
        volumeSlider.value = musicSource.volume;

        // ����'����� ���� �������� �� �������
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }
}
