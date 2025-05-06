using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int PlayerHp;

    public Text Hp;

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
    }
    private void Start()
    {
        PlayerHp = 27; 
    }
}
