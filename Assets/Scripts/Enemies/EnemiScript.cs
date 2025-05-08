using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemiScript : MonoBehaviour
{
    public ScriptableOBJ OBJ;

    private NavMeshAgent navMeshAgent;

    private GameManager gameManager;
    
    public Image HpBar;

    public float speed = 3.5f;
    private float originalSpeed;

    private int currentIndex = 0;

    private float stopDistance = 0.3f;
    private int damage;
    [SerializeField]
    public int EnemyHp;
    [SerializeField]
    public int DropCoin;

    private void Start()
    {

        originalSpeed = speed;

        gameManager = FindObjectOfType<GameManager>();
        gameManager = GameManager.instance;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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
    
    public void Slow(float slowAmount, float slowDuration)
{
    StopCoroutine("SlowCoroutine");
    StartCoroutine(SlowCoroutine(slowAmount, slowDuration));
}

private IEnumerator SlowCoroutine(float slowAmount, float slowDuration)
{
    speed *= slowAmount;
    if (navMeshAgent != null)
        navMeshAgent.speed = speed;

    yield return new WaitForSeconds(slowDuration);

    speed = originalSpeed;
    if (navMeshAgent != null)
        navMeshAgent.speed = speed; 
}

    void Update()
    {
        
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= stopDistance)
        {
            if (gameManager.waypoints.Length > currentIndex + 1)
            {
                currentIndex++;
                navMeshAgent.SetDestination(gameManager.waypoints[currentIndex].position);
            }

    if (EnemyHp <= 0)
    {
        if (gameManager != null)
            gameManager.Coins += DropCoin;
        Destroy(gameObject);
    }
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
            Destroy(gameObject);
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
