using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsMenuManager : MonoBehaviour
{
   public GameObject PauseMenu;
   public GameObject LevelsMenu;

    void Start()
    {
        LevelsMenu.SetActive(false); 
    }
    public void OnBackButtonClick()
    {
        LevelsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }
    public void OnLevel1ButtonClick()
    {
        LevelsMenu.SetActive(false);
        SceneManager.LoadScene("Map-1");
    }
    public void OnTutorialButtonClick()
    {
        LevelsMenu.SetActive(false);
        SceneManager.LoadScene("Tutorial");
    }
}
    
