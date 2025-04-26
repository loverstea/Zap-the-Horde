using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Transform finish;

    public GameManager gameManager;

    [SerializeField]
    private int damage = 1;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (navMeshAgent != null && finish != null)
        {
            navMeshAgent.SetDestination(finish.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack();
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
