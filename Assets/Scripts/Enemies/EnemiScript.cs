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

    private int currentIndex = 0;

    private float stopDistance = 0.3f;

    [SerializeField]
    private int damage;
    [SerializeField]
    private int Hp;
    [SerializeField]
    private int DropCoin;


    private void Start()
    {
        gameManager = GameManager.instance;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (gameManager.waypoints.Length > 0 )
        {
            navMeshAgent.SetDestination(gameManager.waypoints[currentIndex].position);
        }
        if (OBJ != null)
        {
            DropCoin = OBJ.Coins;
            Hp = OBJ.HP;
            damage = OBJ.Attacka;
        }

    }

    void Update()
    {
        
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= stopDistance)
        {
            if (gameManager.waypoints.Length > currentIndex)
            {
                currentIndex = currentIndex + 1;
                navMeshAgent.SetDestination(gameManager.waypoints[currentIndex].position);
            }
            
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
