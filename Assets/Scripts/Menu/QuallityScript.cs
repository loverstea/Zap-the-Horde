using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsMenu : MonoBehaviour
{
    public Dropdown qualityDropdown;

    void Start()
    {
        // �������� �������� �����
        qualityDropdown.ClearOptions();

        // �������� ����� ���� �����
        string[] qualityNames = QualitySettings.names;
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(qualityNames));

        // ���������� ������� ��������
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true); // true � ������ �����������
    }
}
