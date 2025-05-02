using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public GameObject TowersMenu;
    public GameObject TowerUI;
    public GameObject currentTower;
    public Image upgradeProgressBar;
    public Image upgradeProgressBarImage;

    private bool isNearTower = false;
    private Towers towersScript;
    private Vector3 currentCellPosition;

    void Start()
    {
        TowersMenu.SetActive(false);
        TowerUI.SetActive(false);

        if (upgradeProgressBar != null)
        {
            upgradeProgressBar.fillAmount = 0f;
            upgradeProgressBarImage.gameObject.SetActive(false);
        }

        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
        {
            towersScript = eventSystem.GetComponent<Towers>();
        }
    }

    void Update()
    {
        if (isNearTower)
        {
            if (!TowersMenu.activeSelf)
            {
                TowerUI.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TowersMenu.SetActive(!TowersMenu.activeSelf);
                TowerUI.SetActive(!TowersMenu.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RemoveTower();
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                UpgradeTower();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                TowersMenu.SetActive(false);
                TowerUI.SetActive(false);
            }
        }
        else
        {
            TowerUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            isNearTower = true;
            currentTower = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            isNearTower = false;
            currentTower = null;

            TowersMenu.SetActive(false);
            TowerUI.SetActive(false);
        }
    }

    private void RemoveTower()
    {
        if (currentTower != null)
        {
            Vector3 towerPosition = currentTower.transform.position;

            Vector3 nearestCell = FindNearestCell(towerPosition);
            if (towersScript.occupiedCells.Contains(nearestCell))
            {
                towersScript.occupiedCells.Remove(nearestCell);
            }

            Destroy(currentTower);
            currentTower = null;
            isNearTower = false;

            TowersMenu.SetActive(false);
            TowerUI.SetActive(false);

            towersScript.towerRemovalSound.Play();
        }
    }

    private void UpgradeTower()
    {
        if (currentTower != null)
        {
            StartCoroutine(UpgradeTowerWithDelay());
        }
    }

    IEnumerator UpgradeTowerWithDelay()
    {
        if (upgradeProgressBar != null)
        {
            upgradeProgressBar.gameObject.SetActive(true);
            upgradeProgressBarImage.gameObject.SetActive(true);
            upgradeProgressBar.fillAmount = 1f;
        }

        float upgradeTime = 0f;

        if (currentTower.name.Contains(towersScript.archerTowerLevel1Prefab.name) ||
            currentTower.name.Contains(towersScript.magicTowerLevel1Prefab.name) ||
            currentTower.name.Contains(towersScript.IceTowerLevel1Prefab.name))
        {
            upgradeTime = 3f; // Время улучшения с 1 на 2 уровень
        }
        else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name) ||
                 currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name) ||
                 currentTower.name.Contains(towersScript.IceTowerLevel2Prefab.name))
        {
            upgradeTime = 5f; // Время улучшения с 2 на 3 уровень
        }
        else
        {
            if (upgradeProgressBar != null)
            {
                upgradeProgressBar.fillAmount = 0f;
                upgradeProgressBar.gameObject.SetActive(false);
                upgradeProgressBarImage.gameObject.SetActive(false);
            }
            yield break;
        }

        float elapsedTime = 0f;
        currentCellPosition = FindNearestCell(currentTower.transform.position);

        while (elapsedTime < upgradeTime)
        {
            Vector3 newCellPosition = FindNearestCell(currentTower.transform.position);

            if (newCellPosition != currentCellPosition)
            {
                if (upgradeProgressBar != null)
                {
                    upgradeProgressBar.fillAmount = 0f;
                    upgradeProgressBar.gameObject.SetActive(false);
                    upgradeProgressBarImage.gameObject.SetActive(false);
                }
                yield break;
            }

            elapsedTime += Time.deltaTime;
            if (upgradeProgressBar != null)
            {
                upgradeProgressBar.fillAmount = 1f - (elapsedTime / upgradeTime);
            }
            yield return null;
        }

        if (upgradeProgressBar != null)
        {
            upgradeProgressBar.fillAmount = 0f;
            upgradeProgressBar.gameObject.SetActive(false);
            upgradeProgressBarImage.gameObject.SetActive(false);
        }

        PerformUpgrade();
    }

    private void PerformUpgrade()
    {
        Vector3 position = currentTower.transform.position;
        Quaternion rotation = currentTower.transform.rotation;

        if (currentTower.name.Contains(towersScript.archerTowerLevel1Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.archerTowerLevel2Prefab, position, rotation);
            towersScript.towerUpgradeSound.Play();
        }
        else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.archerTowerLevel3Prefab, position, rotation);
            towersScript.towerUpgradeSound.Play();
        }
        else if (currentTower.name.Contains(towersScript.magicTowerLevel1Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.magicTowerLevel2Prefab, position, rotation);
            towersScript.towerUpgradeSound.Play();
        }
        else if (currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.magicTowerLevel3Prefab, position, rotation);
            towersScript.towerUpgradeSound.Play();
        }
        else if (currentTower.name.Contains(towersScript.IceTowerLevel1Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.IceTowerLevel2Prefab, position, rotation);
            towersScript.towerUpgradeSound.Play();
        }
        else if (currentTower.name.Contains(towersScript.IceTowerLevel2Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.IceTowerLevel3Prefab, position, rotation);
            towersScript.towerUpgradeSound.Play();
        }
        else if (currentTower.name.Contains(towersScript.archerTowerLevel3Prefab.name) ||
                 currentTower.name.Contains(towersScript.magicTowerLevel3Prefab.name) ||
                 currentTower.name.Contains(towersScript.IceTowerLevel3Prefab.name))
        {
            towersScript.TowerCancelSound.Play();
        }
    }

    private Vector3 FindNearestCell(Vector3 position)
    {
        Vector3 nearestCell = position;
        float minDistance = float.MaxValue;

        foreach (Vector3 cell in towersScript.occupiedCells)
        {
            float distance = Vector3.Distance(position, cell);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCell = cell;
            }
        }

        return nearestCell;
    }
}