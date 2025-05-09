using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsMenu : MonoBehaviour
{
    public Dropdown qualityDropdown;

    void Start()
    {
        // Очистити попередні опції
        qualityDropdown.ClearOptions();

        // Отримати назви рівнів якості
        string[] qualityNames = QualitySettings.names;
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(qualityNames));

        // Встановити поточне значення
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true); // true – одразу застосувати
    }
}
