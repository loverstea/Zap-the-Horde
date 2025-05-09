using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
   public GameObject PauseMenu;
   public GameObject SettingsMenu;
   public GameObject LevelsMenu;

   public PlayerMovement playerMovement;

   private bool isPaused = false;

   public AudioSource audioSource;


    void Start()
    {
        ResumeGame();
        PauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void OnResumeButtonClick()
    {
        ResumeGame();
    }
    public void OnSettingsButtonClick()
    {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    public void OnLevelsButtonClick()
    {
        PauseMenu.SetActive(false);
        LevelsMenu.SetActive(true);
    }
    public void OnMenuButtonClick()
    {
        PauseMenu.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
    public void OnZTHButtonClick()
    {
        audioSource.Play();
    }



    public void PauseGame()
    {
        isPaused = true;
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
    {
        playerMovement.canMove = false;
    }
    }

    public void ResumeGame()
    {
      if (PauseMenu.activeSelf)
        {
        isPaused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

        if (playerMovement != null)
        {
        playerMovement.canMove = true;
        }
        }
    }

}
