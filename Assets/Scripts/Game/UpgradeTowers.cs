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

    public GameObject upgradeButton;

    public Image CannonattackImage;

    public Material yellowTransparentMaterial;

    private GameObject area; 

    private bool isNearTower = false;
    private Towers towersScript;
    private Vector3 currentCellPosition;
    private GameManager gameManager;
    public TextMeshProUGUI UpgradeText;

    public Camera AreaSelectCamera; 
    private bool isSelectingArea = false;
    private GameObject areaSelectorCube;
    private Vector3 selectedAreaPosition;
    private CannonAreaShooter cannonShooterScript;

    private Dictionary<GameObject, Vector3> cannonAreaPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, GameObject> cannonAreaVisuals = new Dictionary<GameObject, GameObject>();

    private float cannonAreaRadius = 8f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        TowersMenu.SetActive(false);
        TowerUI.SetActive(false);
        CannonattackImage.gameObject.SetActive(false);
        if (CannonattackImage != null)
            CannonattackImage.gameObject.SetActive(false);

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

            if (towersScript != null && currentTower != null && towersScript.CannonTowerPrefab != null &&
                currentTower.name.Contains(towersScript.CannonTowerPrefab.name))
            {
                if (CannonattackImage != null)
                    CannonattackImage.gameObject.SetActive(true);
                if (UpgradeText != null)
                    UpgradeText.gameObject.SetActive(false);
                if (upgradeProgressBar != null)
                    upgradeProgressBar.gameObject.SetActive(false);
                if (upgradeProgressBarImage != null)
                    upgradeProgressBarImage.gameObject.SetActive(false);
                if (upgradeButton != null)
                    upgradeButton.SetActive(false);

                if (Input.GetKeyDown(KeyCode.O) && !isSelectingArea)
                {
                    StartAreaSelection();
                }

                if (isSelectingArea)
                {
                    UpdateAreaSelectorCube();

                    if (Input.GetMouseButtonDown(0))
                    {
                        ConfirmAreaSelection();
                    }
                }
            }
            else
            {
                if (CannonattackImage != null)
                    CannonattackImage.gameObject.SetActive(false);
                if (UpgradeText != null)
                    UpgradeText.gameObject.SetActive(true);
                if (upgradeButton != null)
                    upgradeButton.SetActive(true);

                if (Input.GetKeyDown(KeyCode.U))
                {
                    UpgradeTower();
                }
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
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
                isSelectingArea = false;
                if (areaSelectorCube != null)
                {
                    Destroy(areaSelectorCube);
                    areaSelectorCube = null;
                }
                if (AreaSelectCamera != null)
                    AreaSelectCamera.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            UpdateUpgradeText();
        }
        else
        {
            TowerUI.SetActive(false);
            if (CannonattackImage != null)
                CannonattackImage.gameObject.SetActive(false);
            if (UpgradeText != null)
                UpgradeText.gameObject.SetActive(false);
            if (upgradeButton != null)
                upgradeButton.SetActive(false);
        }
    }

    private void UpdateUpgradeText()
    {
        if (UpgradeText == null)
            return;

        if (currentTower == null || towersScript == null)
        {
            UpgradeText.gameObject.SetActive(false);
            return;
        }

        if (towersScript.CannonTowerPrefab != null && currentTower.name.Contains(towersScript.CannonTowerPrefab.name))
        {
            UpgradeText.gameObject.SetActive(false);
            return;
        }

        UpgradeText.gameObject.SetActive(true);

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
            if (towersScript != null && towersScript.AreaofAttackPrefab != null && currentTower != null)
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


            if (towersScript != null && towersScript.CannonTowerPrefab != null &&
                !currentTower.name.Contains(towersScript.CannonTowerPrefab.name))
            {
                if (CannonattackImage != null)
                    CannonattackImage.gameObject.SetActive(false);
            }

            if (towersScript != null && currentTower != null && towersScript.CannonTowerPrefab != null &&
                currentTower.name.Contains(towersScript.CannonTowerPrefab.name))
            {
                if (cannonAreaVisuals.ContainsKey(currentTower) && cannonAreaVisuals[currentTower] != null)
                {
                    cannonAreaVisuals[currentTower].SetActive(true);
                }
                else if (cannonAreaPositions.ContainsKey(currentTower))
                {
                    Vector3 pos = cannonAreaPositions[currentTower];
                    GameObject areaVisual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    areaVisual.transform.position = pos;
                    areaVisual.transform.localScale = new Vector3(cannonAreaRadius * 2f, 0.1f, cannonAreaRadius * 2f);
                    areaVisual.GetComponent<Collider>().enabled = false;
                    Renderer rend = areaVisual.GetComponent<Renderer>();
                    if (rend != null && yellowTransparentMaterial != null)
                        rend.material = yellowTransparentMaterial;
                    cannonAreaVisuals[currentTower] = areaVisual;
                }
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

            foreach (var kvp in cannonAreaVisuals)
            {
                if (kvp.Value != null)
                    kvp.Value.SetActive(false);
            }
        }
    }

    public void RemoveTower()
    {
        if (currentTower != null)
        {
            Vector3 towerPosition = currentTower.transform.position;

            Vector3 nearestCell = (towersScript != null) ? towersScript.FindNearestCell(towerPosition) : towerPosition;
            if (towersScript != null && towersScript.occupiedCells.Contains(nearestCell))
            {
                towersScript.occupiedCells.Remove(nearestCell);
            }

            if (cannonAreaVisuals.ContainsKey(currentTower) && cannonAreaVisuals[currentTower] != null)
            {
                Destroy(cannonAreaVisuals[currentTower]);
                cannonAreaVisuals.Remove(currentTower);
            }
            if (cannonAreaPositions.ContainsKey(currentTower))
                cannonAreaPositions.Remove(currentTower);

            Destroy(currentTower);
            currentTower = null;
            isNearTower = false;

            TowersMenu.SetActive(false);
            TowerUI.SetActive(false);

            if (towersScript != null)
                towersScript.towerRemovalSound.Play();

            if (area != null)
            {
                Destroy(area);
                area = null;
            }
        }
    }

    public void UpgradeTower()
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
        if (towersScript == null || towersScript.occupiedCells == null || towersScript.occupiedCells.Count == 0)
            return position;

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

    private void StartAreaSelection()
    {
        isSelectingArea = true;

        Camera[] allCams = Camera.allCameras;
        foreach (Camera cam in allCams)
        {
            if (cam != AreaSelectCamera)
                cam.gameObject.SetActive(false);
        }
        if (AreaSelectCamera != null)
        {
            AreaSelectCamera.gameObject.SetActive(true);
            AreaSelectCamera.enabled = true;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (areaSelectorCube == null)
        {
            areaSelectorCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            areaSelectorCube.transform.localScale = new Vector3(towersScript.cellSize, 1f, towersScript.cellSize);
            Renderer rend = areaSelectorCube.GetComponent<Renderer>();
            if (rend != null && towersScript.yellowTransparentMaterial != null)
                rend.material = towersScript.yellowTransparentMaterial;
            areaSelectorCube.GetComponent<Collider>().enabled = false;
        }
    }

    private void UpdateAreaSelectorCube()
    {
        Ray ray = AreaSelectCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, towersScript.roadLayerMask))
        {
            Vector3 nearestCell = towersScript.FindNearestCell(hit.point);

            bool isOnRoad = false;
            Collider[] roadColliders = Physics.OverlapBox(
                nearestCell,
                new Vector3(towersScript.cellSize / 2f, 2f, towersScript.cellSize / 2f),
                Quaternion.identity,
                towersScript.roadLayerMask
            );
            if (roadColliders.Length > 0)
                isOnRoad = true;

            if (isOnRoad)
            {
                areaSelectorCube.transform.position = nearestCell;
                Renderer rend = areaSelectorCube.GetComponent<Renderer>();
                if (rend != null && towersScript.yellowTransparentMaterial != null)
                    rend.material = towersScript.yellowTransparentMaterial;
                areaSelectorCube.SetActive(true);
            }
            else
            {
                areaSelectorCube.SetActive(false);
            }
        }
        else
        {
            areaSelectorCube.SetActive(false);
        }
    }

    private void ConfirmAreaSelection()
    {
        isSelectingArea = false;
        if (AreaSelectCamera != null)
        {
            if (AreaSelectCamera.targetTexture != null)
            {
                AreaSelectCamera.targetTexture = null;
            }
            AreaSelectCamera.enabled = false;
            AreaSelectCamera.gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Camera[] allCams = Resources.FindObjectsOfTypeAll<Camera>();
            foreach (Camera cam in allCams)
            {
                cam.gameObject.SetActive(true);
                cam.enabled = true;
            }
        }
#endif

        Camera mainCamera = null;
        Camera[] allCamsPlay = Resources.FindObjectsOfTypeAll<Camera>();
        foreach (Camera cam in allCamsPlay)
        {
            if (cam.CompareTag("MainCamera"))
            {
                mainCamera = cam;
                cam.gameObject.SetActive(true);
                cam.enabled = true;
            }
            else
            {
                cam.gameObject.SetActive(false);
                cam.enabled = false;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        selectedAreaPosition = areaSelectorCube.transform.position;

        Collider[] roadColliders = Physics.OverlapBox(
            selectedAreaPosition,
            new Vector3(towersScript.cellSize / 2f, 2f, towersScript.cellSize / 2f),
            Quaternion.identity,
            towersScript.roadLayerMask
        );
        if (roadColliders.Length == 0)
        {
            Destroy(areaSelectorCube);
            isSelectingArea = false;
            return;
        }

        if (currentTower != null)
        {
            cannonAreaPositions[currentTower] = selectedAreaPosition;

            if (cannonAreaVisuals.ContainsKey(currentTower) && cannonAreaVisuals[currentTower] != null)
            {
                Destroy(cannonAreaVisuals[currentTower]);
            }
            GameObject areaVisual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            areaVisual.transform.position = selectedAreaPosition;
            areaVisual.transform.localScale = new Vector3(cannonAreaRadius * 2f, 0.1f, cannonAreaRadius * 2f);
            areaVisual.GetComponent<Collider>().enabled = false;
            Renderer rend = areaVisual.GetComponent<Renderer>();
            if (rend != null && yellowTransparentMaterial != null)
                rend.material = yellowTransparentMaterial;
            cannonAreaVisuals[currentTower] = areaVisual;
        }

        Destroy(areaSelectorCube);

        if (currentTower != null)
        {
            cannonShooterScript = currentTower.GetComponent<CannonAreaShooter>();
            if (cannonShooterScript == null)
                cannonShooterScript = currentTower.AddComponent<CannonAreaShooter>();
            cannonShooterScript.SetTargetArea(selectedAreaPosition);
        }
    }
}