using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemiScript enemy = other.GetComponent<EnemiScript>();
            if (enemy != null)
            {
                enemy.EnemyHp -= 1;
                if (enemy.EnemyHp <= 0)
                {
                    GameManager.instance.Coins += enemy.DropCoin;
                    Destroy(other.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}
