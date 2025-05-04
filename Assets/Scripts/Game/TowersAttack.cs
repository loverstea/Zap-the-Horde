using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform firePoint; // Точка выстрела
    public float fireRate = 1f; // Скорость стрельбы (в секундах)
    public float projectileSpeed = 10f; // Скорость снаряда
    public float attackRange = 8f; // Радиус атаки
    public LayerMask enemyLayer; // Слой врагов

    private float nextFireTime = 0f; // Время следующего выстрела

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

            if (enemiesInRange.Length > 0)
            {
                Transform target = GetClosestEnemy(enemiesInRange);
                if (target != null)
                {
                    FireProjectile(target);
                    nextFireTime = Time.time + fireRate;

                    // Отключаем префаб башни на время перезарядки
                    StartCoroutine(HandlePrefabVisibility());
                }
            }
        }
    }

    void FireProjectile(Transform target)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = (target.position - firePoint.position).normalized;
                rb.velocity = direction * projectileSpeed;
            }
        }
    }

    Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    IEnumerator HandlePrefabVisibility()
    {
        if (projectilePrefab != null)
        {
            projectilePrefab.SetActive(false); // Прячем префаб
            yield return new WaitForSeconds(fireRate); // Ждем время перезарядки
            projectilePrefab.SetActive(true); // Включаем префаб
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация радиуса атаки в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
