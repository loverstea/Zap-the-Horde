using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private EnemiScript enemyScript;
    private PlayerMovement playerMovement;
    public int PlayerHp;
    public int MaxPlayerHp;

    public Image HpBar;
    public GameObject GameOverPanel;

    public int Coins;
    public TextMeshProUGUI Coinss;

    public Transform[] waypoints;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        Coinss.text = Coins.ToString();

        if (HpBar != null)
        {
            HpBar.fillAmount = Mathf.Clamp01((float)PlayerHp / MaxPlayerHp);
        }
        if (PlayerHp <= 0)
        {
            PlayerHp = 0;
            Time.timeScale = 0f;
            GameOverPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (playerMovement != null)
            {
                playerMovement.canMove = false;
            }
        }
    }
    private void Start()
    {
        MaxPlayerHp = 50; 
        PlayerHp = MaxPlayerHp;

        if (HpBar != null)
        {
            HpBar.fillAmount = 1f;
        }
    }
}
