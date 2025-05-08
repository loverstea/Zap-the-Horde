using UnityEngine;
using TMPro; // для TMP_Dropdown

public class QualitySelector : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;

    void Start()
    {
        qualityDropdown.ClearOptions();

        // Список назв якості з Unity
        string[] names = QualitySettings.names;
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(names));

        // Поточна якість
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
