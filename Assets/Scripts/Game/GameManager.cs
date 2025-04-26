using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int PlayerHp;

    public int Coins;

    public Text Text;

    private void Start()
    {
        PlayerHp = 27; 
    }

    private void Update()
    {
        Text.text = "Hp: " + PlayerHp;
    }



}
