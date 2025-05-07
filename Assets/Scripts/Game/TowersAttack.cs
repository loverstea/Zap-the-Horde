using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersAttack : MonoBehaviour
{
    public float fireRate = 1f; 
    public float attackRange = 0f;
    public float nextFire = 0f;

    public int bulletDamage = 0;

    public GameObject RotatingPart;
    public GameObject Arrow;

    void Update()
    {
        
        if (Time.time >= nextFire)
        {
            Arrow.SetActive(true);
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));

            if (enemiesInRange.Length > 0)
            {
                Transform target = GetClosestToFinish(enemiesInRange);
                if (target != null)
                {
                    Vector3 direction = target.position - RotatingPart.transform.position;

    direction.y = 0;

    if (direction.sqrMagnitude > 0.01f) 
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

        RotatingPart.transform.rotation = lookRotation * Quaternion.Euler(-90f, 0f, 0f); 
    }

                    Attack(target);

                    nextFire = Time.time + fireRate;
                }
            }
        }
    }

        Transform GetClosestToFinish(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float highestProgress = -1f;
    
        foreach (Collider enemyCollider in enemies)
        {
            EnemiScript enemy = enemyCollider.GetComponent<EnemiScript>();
            if (enemy != null)
            {
                float progress = enemy.GetProgress();
                if (progress > highestProgress)
                {
                    highestProgress = progress;
                    closestEnemy = enemy.transform;
                }
            }
        }
    
        return closestEnemy;
    }

    void Attack(Transform target)
    {
        if (Time.timeScale != 0f)
        {
            EnemiScript enemy = target.GetComponent<EnemiScript>();
            if (enemy != null)
            {
                Arrow.SetActive(false);
                enemy.EnemyHp -= bulletDamage;
                if (enemy.EnemyHp <= 0)
                {
                    GameManager.instance.Coins += enemy.DropCoin;
                    Destroy(enemy.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}