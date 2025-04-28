using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemiScript : MonoBehaviour
{
    public ScriptableOBJ OBJ;

    private NavMeshAgent navMeshAgent;

    private GameManager gameManager;

    [SerializeField]
    private int damage;
    [SerializeField]
    private int Hp;
    [SerializeField]
    private int DropCoin;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (OBJ != null)
        {
            DropCoin = OBJ.Coins;
            Hp = OBJ.HP;
            damage = OBJ.Attacka;
        }
        gameManager = GameManager.instance;

    }

    void Update()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(gameManager.transform.position);
        }
        if (Hp <= 0)
        {
            gameManager.Coins += DropCoin;  
            Object.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack();
            Hp = 0;
        }
    }

    public void Attack()
    {
        if (gameManager != null)
        {
            gameManager.PlayerHp -= damage;
        }
    }
    
}
