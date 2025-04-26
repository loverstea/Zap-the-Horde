using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int PlayerHp;

    public int Coins;

    public Text Hp;

    public Text Coinss;
        

    private void Start()
    {
        PlayerHp = 27; 
    }

    private void Update()
    {
        Hp.text = "Hp: " + PlayerHp;

        Coinss.text = "Coins: " + Coins;
    }



}
