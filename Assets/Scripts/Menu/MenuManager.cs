using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject LevelsMenuPanel;
    public GameObject settingsPanel;
    public GameObject MainMenuPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        settingsPanel.SetActive(false);
        LevelsMenuPanel.SetActive(false); 
        MainMenuPanel.SetActive(true);
    }

    public void OnPlayButtonClick()
    {
        LevelsMenuPanel.SetActive(true);
    }
    public void OnSettingsButtonClick()
    {
        MainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

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
