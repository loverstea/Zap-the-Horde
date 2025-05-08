using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerMenu : MonoBehaviour
{
    public GameObject TowersMenu;
    public GameObject TowerUI;
    public GameObject currentTower;
    public Image upgradeProgressBar;
    public Image upgradeProgressBarImage;

    private GameObject area; 

    private bool isNearTower = false;
    private Towers towersScript;
    private Vector3 currentCellPosition;
    private GameManager gameManager;
    public TextMeshProUGUI UpgradeText;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

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

            UpdateUpgradeText();
        }
        else
        {
            TowerUI.SetActive(false);
        }
    }

    private void UpdateUpgradeText()
{
    if (currentTower == null)
    {
        return;
    }

    int upgradeCost = 0;

    if (currentTower.name.Contains(towersScript.archerTowerLevel1Prefab.name))
    {
        upgradeCost = 80;
    }
    else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name))
    {
        upgradeCost = 120;
    }
    else if (currentTower.name.Contains(towersScript.magicTowerLevel1Prefab.name))
    {
        upgradeCost = 160;
    }
    else if (currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name))
    {
        upgradeCost = 220;
    }
    else if (currentTower.name.Contains(towersScript.IceTowerLevel1Prefab.name))
    {
        upgradeCost = 100;
    }
    else if (currentTower.name.Contains(towersScript.IceTowerLevel2Prefab.name))
    {
        upgradeCost = 150;
    }
    else
    {
        UpgradeText.text = "MAX";
        return;
    }
    UpgradeText.text = "" + upgradeCost;
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Tower"))
    {
        isNearTower = true;
        currentTower = other.gameObject;
        if (towersScript.AreaofAttackPrefab != null && currentTower != null)
        {
            float radius = 3f;

            if (currentTower.name.Contains(towersScript.archerTowerLevel1Prefab.name))
                radius = 12f;
            else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name))
                radius = 18f;
            else if (currentTower.name.Contains(towersScript.archerTowerLevel3Prefab.name))
                radius = 24f;
            else if (currentTower.name.Contains(towersScript.magicTowerLevel1Prefab.name))
                radius = 8f;
            else if (currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name))
                radius = 10f;
            else if (currentTower.name.Contains(towersScript.magicTowerLevel3Prefab.name))
                radius = 12f;
            else if (currentTower.name.Contains(towersScript.IceTowerLevel1Prefab.name))
                radius = 8f;
            else if (currentTower.name.Contains(towersScript.IceTowerLevel2Prefab.name))
                radius = 10f;
            else if (currentTower.name.Contains(towersScript.IceTowerLevel3Prefab.name))
                radius = 12f;

            if (area != null)
            {
                Destroy(area);
                area = null;
            }

            area = Instantiate(towersScript.AreaofAttackPrefab);
            area.transform.position = currentTower.transform.position;
            area.transform.localScale = new Vector3(radius * 2, 0.5f, radius * 2);

            Renderer rend = area.GetComponent<Renderer>();
            if (rend != null && towersScript.yellowTransparentMaterial != null)
                rend.material = towersScript.yellowTransparentMaterial;
            area.SetActive(true);
        }
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

            if (area != null)
            {
                Destroy(area);
                area = null;
            }

            if (upgradeProgressBar != null)
        {
            upgradeProgressBar.fillAmount = 0f;
            upgradeProgressBar.gameObject.SetActive(false);
            upgradeProgressBarImage.gameObject.SetActive(false);
        }
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
    int upgradeCost = 0;

    if (currentTower.name.Contains(towersScript.archerTowerLevel1Prefab.name))
    {
        upgradeCost = 80;
    }
    else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name))
    {
        upgradeCost = 120;
    }
    else if (currentTower.name.Contains(towersScript.magicTowerLevel1Prefab.name))
    {
        upgradeCost = 160;
    }
    else if (currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name))
    {
        upgradeCost = 220;
    }
    else if (currentTower.name.Contains(towersScript.IceTowerLevel1Prefab.name))
    {
        upgradeCost = 100;
    }
    else if (currentTower.name.Contains(towersScript.IceTowerLevel2Prefab.name))
    {
        upgradeCost = 150;
    }

    if (gameManager.Coins < upgradeCost)
    {
        yield break;
    }

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
        upgradeTime = 3f; 
    }
    else if (currentTower.name.Contains(towersScript.archerTowerLevel2Prefab.name) ||
             currentTower.name.Contains(towersScript.magicTowerLevel2Prefab.name) ||
             currentTower.name.Contains(towersScript.IceTowerLevel2Prefab.name))
    {
        upgradeTime = 5f;
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
    Vector3 initialCameraCell = FindNearestCell(Camera.main.transform.position);

    while (elapsedTime < upgradeTime)
    {
        if (currentTower == null)
        {
            if (upgradeProgressBar != null)
            {
                upgradeProgressBar.fillAmount = 0f;
                upgradeProgressBar.gameObject.SetActive(false);
                upgradeProgressBarImage.gameObject.SetActive(false);
            }
            yield break;
        }

        Vector3 newCellPosition = FindNearestCell(currentTower.transform.position);
        Vector3 currentCameraCell = FindNearestCell(Camera.main.transform.position);

        if (newCellPosition != currentCellPosition || currentCameraCell != initialCameraCell)
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

    gameManager.Coins -= upgradeCost;

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