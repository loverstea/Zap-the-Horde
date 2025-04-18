using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject tutorialMenuPanel;
    public GameObject settingsPanel;

    void Start()
    {
        tutorialMenuPanel.SetActive(false); 
    }

    public void OnPlayButtonClick()
    {
        tutorialMenuPanel.SetActive(true);
    }
    public void OnPlaySettingsClick()
    {
        settingsPanel.SetActive(true);

    }
    public void OnCloseSettingsButtonClick()
    {
        settingsPanel.SetActive(false);
    }

    public void OnTutorialButtonClick()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnLevelsButtonClick()
    {
        SceneManager.LoadScene("Map-1");
    }
    public void OnExitButtonClick()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
