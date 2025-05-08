using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoseMenu : MonoBehaviour
{
    public GameObject loseMenu;

    void Start()
    {
        loseMenu.SetActive(false);
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnMenuButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
