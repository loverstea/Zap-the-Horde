using UnityEngine;
using System.Collections;

public class CannonAreaShooter : MonoBehaviour
{
    public float fireInterval = 10f;
    public float areaRadius = 8f;
    public int damage = 10;

    private Vector3 targetArea;
    private bool isActive = false;

    public void SetTargetArea(Vector3 area)
    {
        targetArea = area;
        isActive = true;
        StopAllCoroutines();
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (isActive)
        {
            HitArea();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private void HitArea()
    {
        Collider[] hits = Physics.OverlapSphere(targetArea, areaRadius, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            EnemiScript enemy = hit.GetComponent<EnemiScript>();
            if (enemy != null)
            {
                enemy.EnemyHp -= damage;
                if (enemy.EnemyHp <= 0)
                {
                    GameManager.instance.Coins += enemy.DropCoin;
                    Destroy(enemy.gameObject);
                }
            }
        }
    }

    public void StopShooting()
    {
        isActive = false;
        StopAllCoroutines();
    }
}
