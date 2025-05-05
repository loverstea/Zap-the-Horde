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
    public int EnemyHp;
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
            EnemyHp = OBJ.HP;
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
        
        if (EnemyHp <= 0)
        {
            gameManager.Coins += DropCoin;  
            Object.Destroy(gameObject);
        }
    }
    public float GetProgress()
    {

    return currentIndex + (1f - Mathf.Clamp01(navMeshAgent.remainingDistance / navMeshAgent.stoppingDistance));
   }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack();
            EnemyHp = 0;
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
