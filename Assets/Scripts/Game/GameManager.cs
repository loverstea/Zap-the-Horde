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

    public Transform[] waypoints;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerHp = 27; 
    }
}
