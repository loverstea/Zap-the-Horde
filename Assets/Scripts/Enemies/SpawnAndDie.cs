using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndDie : MonoBehaviour
{
    public ScriptableOBJ[] enemies; 
    public Transform spawnPoint; 

    public float spawnInterval = 2f; 
    private float timer;
    public int count = 0;
    public Transform finishPoint;
    public int maxCount = 10;
    public int ways = 1;
    public double WayCoins;
    private float WaySWaiter;

    void Update()
    {
        if (count < maxCount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
            SpawnRandomEnemy();
            timer = 0f;
            }
            
        }

        else
        {
            ways++;
            WayCoins *= 1.5;
            WaySWaiter += Time.deltaTime;
            if (WaySWaiter >= 25)
            {    
                maxCount *= 2;
                count = maxCount;
            }
        }
    }

    void SpawnRandomEnemy()
    {
        int index = Random.Range(0, enemies.Length);
        ScriptableOBJ enemyData = enemies[index];

        GameObject enemy = Instantiate(enemyData.prefab, spawnPoint.position, Quaternion.identity);
        count--;
    }
}
