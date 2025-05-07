using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersAttackMagic : MonoBehaviour
{
    public float fireRate = 1f; 
    public float attackRange = 0f;
    public float nextFire = 0f;

    public int Damage = 0;

    void Update()
{
    if (Time.time >= nextFire)
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        if (enemiesInRange.Length > 0)
        {
            foreach (Collider enemyCollider in enemiesInRange)
            {
                Attack(enemyCollider.transform);
            }
            nextFire = Time.time + fireRate;
        }
    }
}
    void Attack(Transform target)
    {
        if (Time.timeScale != 0f)
        {
            EnemiScript enemy = target.GetComponent<EnemiScript>();
            if (enemy != null)
            {
                enemy.EnemyHp -= Damage;
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