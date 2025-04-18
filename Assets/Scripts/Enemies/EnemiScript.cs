using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiScript : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    [SerializeField]
    private Transform Finish;
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        if (NavMeshAgent != null )
        {
            NavMeshAgent.SetDestination(Finish.position);
        }
    }

   
}
