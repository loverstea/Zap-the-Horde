using UnityEngine;
using TMPro; // ��� TMP_Dropdown

public class QualitySelector : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;

    void Start()
    {
        qualityDropdown.ClearOptions();

        // ������ ���� ����� � Unity
        string[] names = QualitySettings.names;
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(names));

        // ������� �����
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
