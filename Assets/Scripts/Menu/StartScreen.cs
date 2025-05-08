using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class StartScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI Enter;
    public Image EnterImage;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        videoPlayer.loopPointReached += OnVideoEnd;

        StartCoroutine(ShowEnterText());
    }

    IEnumerator ShowEnterText()
    {
        Enter.gameObject.SetActive(false);
        EnterImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Enter.gameObject.SetActive(true);
        EnterImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(5); 
        Enter.gameObject.SetActive(false);
        EnterImage.gameObject.SetActive(false);
    }
    void OnVideoEnd(VideoPlayer vp)
    {
    SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
