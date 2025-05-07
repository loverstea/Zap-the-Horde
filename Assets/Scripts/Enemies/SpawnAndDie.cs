using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndDie : MonoBehaviour
{
    private EnemiScript enemyScript;
    public float morehp = 1.5f; 

    public ScriptableOBJ[] enemies;
    public Transform spawnPoint;

    public float spawnInterval = 2f;
    public int enemiesPerWave = 10;
    public float timeBetweenWaves = 10f;

    private int spawnedInWave = 0;
    private bool spawningWave = false;

    void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        while (true)
        {
            spawnedInWave = 0;
            spawningWave = true;

            while (spawnedInWave < enemiesPerWave)
            {
                SpawnRandomEnemy();
                spawnedInWave++;
                yield return new WaitForSeconds(spawnInterval);
            }

            spawningWave = false;
            yield return new WaitForSeconds(timeBetweenWaves);
            enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * 1.5f);
            morehp *= 2f;
            spawnInterval *= 0.9f;
        }
    }

    void SpawnRandomEnemy()
{
    int index = Random.Range(0, enemies.Length);
    ScriptableOBJ enemyData = enemies[index];
    GameObject enemyObj = Instantiate(enemyData.prefab, spawnPoint.position, Quaternion.identity);
    EnemiScript enemyScript = enemyObj.GetComponent<EnemiScript>();

    if (enemyScript != null)
    {
        enemyScript.EnemyHp = Mathf.CeilToInt(enemyData.HP * morehp);
    }
}
}