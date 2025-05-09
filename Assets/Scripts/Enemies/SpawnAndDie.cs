using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnAndDie : MonoBehaviour
{
    private EnemiScript enemyScript;
    private PlayerMovement playerMovement;
    public float morehp = 1.5f; 

    public ScriptableOBJ[] enemies;
    public Transform spawnPoint;

    public GameObject winMenu;

    public Image TimebeforeWaveBar;
    public Image TimebeforeWave;

    public float spawnInterval = 2f;
    public int enemiesPerWave = 10;
    public float timeBetweenWaves = 10f;
    public int maxWaves;

    private int spawnedInWave = 0;
    private bool spawningWave = false;

    void Start()
    {
        winMenu.SetActive(false);
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        int waveCount = 0;
        while (true)
        {
            if (maxWaves > 0 && waveCount >= maxWaves)
            {
                WinGame();
                yield break;
            }

            spawnedInWave = 0;
            spawningWave = true;

            while (spawnedInWave < enemiesPerWave)
            {
                SpawnRandomEnemy();
                spawnedInWave++;
                yield return new WaitForSeconds(spawnInterval);
            }

            spawningWave = false;


            while (FindObjectsOfType<EnemiScript>().Length > 0)
            {
                yield return null;
            }


            if (TimebeforeWaveBar != null)
            {
                TimebeforeWaveBar.gameObject.SetActive(true);
                TimebeforeWaveBar.fillAmount = 1f;
            }
            if (TimebeforeWave != null)
            {
                TimebeforeWave.gameObject.SetActive(true);
            }

            float timer = timeBetweenWaves;
            while (timer > 0f)
            {
                if (TimebeforeWaveBar != null)
                    TimebeforeWaveBar.fillAmount = timer / timeBetweenWaves;
                timer -= Time.deltaTime;
                yield return null;
            }

            if (TimebeforeWaveBar != null)
            {
                TimebeforeWaveBar.fillAmount = 0f;
                TimebeforeWaveBar.gameObject.SetActive(false);
            }
            if (TimebeforeWave != null)
            {
                TimebeforeWave.gameObject.SetActive(false);
            }

            enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * 1.5f);
            morehp *= 2f;
            spawnInterval *= 0.9f;
            waveCount++;
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

    void WinGame()
    {
        Time.timeScale = 0f;
        winMenu.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        if (pm != null)
        {
            pm.canMove = false;
        }
    }
}