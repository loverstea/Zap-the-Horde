using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public GameObject TowersMenu;
    public GameObject TowerUI;
    public GameObject currentTower;

    private bool isNearTower = false;
    private Towers towersScript;

    void Start()
    {
        TowersMenu.SetActive(false);
        TowerUI.SetActive(false);

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
        }
    }

    private void UpgradeTower()
    {
        if (currentTower != null)
        {
        Vector3 position = currentTower.transform.position;
        Quaternion rotation = currentTower.transform.rotation;

        if (currentTower.name.Contains(towersScript.archerTowerLevel1Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.archerTowerLevel2Prefab, position, rotation);
        }
        else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.archerTowerLevel3Prefab, position, rotation);
        }
        else if (currentTower.name.Contains(towersScript.magicTowerLevel1Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.magicTowerLevel2Prefab, position, rotation);
        }
        else if (currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name))
        {
            Destroy(currentTower);
            currentTower = Instantiate(towersScript.magicTowerLevel3Prefab, position, rotation);
        }
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