using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Towers : MonoBehaviour
{
    public GameObject archerTowerLevel1Prefab;
    public GameObject archerTowerLevel2Prefab;
    public GameObject archerTowerLevel3Prefab;

    public GameObject magicTowerLevel1Prefab;
    public GameObject magicTowerLevel2Prefab;
    public GameObject magicTowerLevel3Prefab;

    public GameObject IceTowerLevel1Prefab;
    public GameObject IceTowerLevel2Prefab;
    public GameObject IceTowerLevel3Prefab;

    public AudioSource towerUpgradeSound;
    public AudioSource towerRemovalSound;
    public AudioSource towerSelectionSound;
    public AudioSource TowerCancelSound;

    private GameManager gameManager;
    public Transform playerTransform;

    public LayerMask gridLayerMask;
    public LayerMask invalidPlacementMask;

    public List<Image> towerImages;
    private int selectedTowerIndex = -1;

    private GameObject currentTower;

    public GameObject Drawning;
    private Camera mainCamera;

    public float cellSize = 4f;
    public Vector3 topLeftCorner;
    public Vector3 bottomRightCorner;

    public List<Vector3> predefinedCells;
    public List<Vector3> occupiedCells = new List<Vector3>();

    private bool canPlaceTower = true;

    public GameObject placementIndicatorPrefab;
    private GameObject placementIndicator;

    public GameObject AreaofAttackPrefab;
    private GameObject currentAreaOfAttack;
    public float areaOfAttackRadiusArcher = 12f;
    public float areaOfAttackRadiusMagic = 8f;
    public float areaOfAttackRadiusIce = 6f;

    public Material greenTransparentMaterial;
    public Material yellowTransparentMaterial;

    private bool isBuildingTower = false;

    public Image buildProgressBar;
    public Image buildProgressBarImage;

    private Vector3 lastCameraPosition;
    private Vector3 currentCellPosition;

    public int AtcherTowerCost = 100;
    public int MagicTowerCost = 200;
    public int IceTowerCost = 170;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Drawning.SetActive(false);
        mainCamera = Camera.main;
        GenerateCells();

        Collider[] roadColliders = Physics.OverlapBox(
            (topLeftCorner + bottomRightCorner) / 2,
            (bottomRightCorner - topLeftCorner) / 2,
            Quaternion.identity,
            invalidPlacementMask
        );

        foreach (Collider roadCollider in roadColliders)
        {
            Vector3 roadPosition = roadCollider.transform.position;
            Vector3 nearestCell = FindNearestCell(roadPosition);
            if (!occupiedCells.Contains(nearestCell))
            {
                occupiedCells.Add(nearestCell);
            }
        }

        if (buildProgressBar != null)
        {
            buildProgressBar.fillAmount = 0f;
        }
        buildProgressBarImage.gameObject.SetActive(false);

        lastCameraPosition = mainCamera.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetBuildMode();

            Drawning.SetActive(false);
            if (currentAreaOfAttack != null)
            {
                Destroy(currentAreaOfAttack);
                currentAreaOfAttack = null;
            }
            if (currentTower != null)
            {
                Destroy(currentTower);
                currentTower = null;
            }

            if (placementIndicator != null)
            {
                Destroy(placementIndicator);
            }

            if (selectedTowerIndex != -1)
            {
                ResetTowerImage(selectedTowerIndex);
                selectedTowerIndex = -1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetBuildMode();

            if (currentAreaOfAttack != null)
            {
                Destroy(currentAreaOfAttack);
                currentAreaOfAttack = null;
            }
            if (currentTower != null)
            {
                Destroy(currentTower);
                currentTower = null;
            }

            if (placementIndicator != null)
            {
                Destroy(placementIndicator);
            }

            if (selectedTowerIndex != -1)
            {
                ResetTowerImage(selectedTowerIndex);
                selectedTowerIndex = -1;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectTower(archerTowerLevel1Prefab, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectTower(magicTowerLevel1Prefab, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectTower(IceTowerLevel1Prefab, 2);
        }

        if (currentTower != null && !isBuildingTower)
        {
            MoveTowerToGrid();
            CheckPlacementValidity();

            if (!canPlaceTower)
            {
                currentTower.SetActive(false);
                placementIndicator.SetActive(false);
                if (currentAreaOfAttack != null)
                    currentAreaOfAttack.SetActive(false); // только скрыть!
            }
            else
            {
                placementIndicator.SetActive(true);
                currentTower.SetActive(true);
                if (currentAreaOfAttack != null)
                    currentAreaOfAttack.SetActive(true);
            }
        }
        else
        {
            if (currentAreaOfAttack != null)
                currentAreaOfAttack.SetActive(false); // только скрыть!
        }

        if (Input.GetMouseButtonDown(0) && currentTower != null && canPlaceTower && !isBuildingTower)
        {
            StartCoroutine(BuildTowerWithDelay());
            if (currentAreaOfAttack != null)
            {
                Destroy(currentAreaOfAttack);
                currentAreaOfAttack = null;
            }
            Drawning.SetActive(false);
        }
    }

    IEnumerator BuildTowerWithDelay()
    {
        int towerCost = 0;

        if (currentTower != null && currentTower.name.Contains(archerTowerLevel1Prefab.name))
        {
            towerCost = AtcherTowerCost;
        }
        else if (currentTower != null && currentTower.name.Contains(magicTowerLevel1Prefab.name))
        {
            towerCost = MagicTowerCost;
        }
        else if (currentTower != null && currentTower.name.Contains(IceTowerLevel1Prefab.name))
        {
            towerCost = IceTowerCost;
        }

        if (gameManager.Coins < towerCost)
        {
            yield break;
        }

        if (buildProgressBar != null)
        {
            buildProgressBar.gameObject.SetActive(true);
            buildProgressBarImage.gameObject.SetActive(true);
            buildProgressBar.fillAmount = 1f;
        }

        float buildTime = 3f;
        float elapsedTime = 0f;

        if (currentTower == null)
        {
            yield break;
        }

        currentCellPosition = FindNearestCell(currentTower.transform.position);
        Vector3 initialCameraCell = FindNearestCell(mainCamera.transform.position);

        while (elapsedTime < buildTime)
        {
            if (currentTower == null)
            {
                if (buildProgressBar != null)
                {
                    buildProgressBar.fillAmount = 0f;
                    buildProgressBar.gameObject.SetActive(false);
                    buildProgressBarImage.gameObject.SetActive(false);
                }
                isBuildingTower = false;
                yield break;
            }

            Vector3 newCellPosition = FindNearestCell(currentTower.transform.position);
            Vector3 currentCameraCell = FindNearestCell(mainCamera.transform.position);

            if (newCellPosition != currentCellPosition || currentCameraCell != initialCameraCell)
            {
                if (buildProgressBar != null)
                {
                    buildProgressBar.fillAmount = 0f;
                    buildProgressBar.gameObject.SetActive(false);
                    buildProgressBarImage.gameObject.SetActive(false);
                }
                isBuildingTower = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;
            if (buildProgressBar != null)
            {
                buildProgressBar.fillAmount = 1f - (elapsedTime / buildTime);
            }
            yield return null;
        }

        if (buildProgressBar != null)
        {
            buildProgressBar.fillAmount = 0f;
            buildProgressBar.gameObject.SetActive(false);
            buildProgressBarImage.gameObject.SetActive(false);
        }

        gameManager.Coins -= towerCost;

        PlaceTower();
        isBuildingTower = false;
    }

    void GenerateCells()
    {
        predefinedCells = new List<Vector3>();

        float minX = Mathf.Min(topLeftCorner.x, bottomRightCorner.x);
        float maxX = Mathf.Max(topLeftCorner.x, bottomRightCorner.x);
        float minZ = Mathf.Min(topLeftCorner.z, bottomRightCorner.z);
        float maxZ = Mathf.Max(topLeftCorner.z, bottomRightCorner.z);

        for (float x = minX; x <= maxX; x += cellSize)
        {
            for (float z = maxZ; z >= minZ; z -= cellSize)
            {
                Vector3 cellPosition = new Vector3(x, topLeftCorner.y, z);
                predefinedCells.Add(cellPosition);
            }
        }
    }

    void MoveTowerToGrid()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayerMask))
        {
            Vector3 nearestCellPosition = FindNearestCell(hit.point);

            currentTower.transform.position = nearestCellPosition;

            if (placementIndicator != null)
                placementIndicator.transform.position = nearestCellPosition;
            if (currentAreaOfAttack != null)
                currentAreaOfAttack.transform.position = nearestCellPosition;
        }
    }

    Vector3 FindNearestCell(Vector3 position)
    {
        Vector3 nearestCell = Vector3.zero;
        float minDistance = float.MaxValue;

        foreach (Vector3 cellPosition in predefinedCells)
        {
            float distance = Vector3.Distance(position, cellPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCell = cellPosition;
            }
        }

        return nearestCell;
    }

    void CheckPlacementValidity()
    {
        if (currentTower == null)
        {
            canPlaceTower = false;
            return;
        }

        Collider towerCollider = currentTower.GetComponent<Collider>();
        if (towerCollider == null)
        {
            canPlaceTower = false;
            return;
        }

        Vector3 boxCenter = currentTower.transform.position;

        if (playerTransform == null)
        {
            canPlaceTower = false;
            return;
        }

        Vector3 playerPosition = playerTransform.position;

        Vector3 playerCell = FindNearestCell(playerPosition);

        List<Vector3> adjacentCells = GetAdjacentCells(playerCell);

        bool isInAdjacentCell = adjacentCells.Contains(FindNearestCell(boxCenter));
        bool isCellOccupied = occupiedCells.Contains(FindNearestCell(boxCenter));

        Collider[] colliders = Physics.OverlapBox(
            towerCollider.bounds.center,
            towerCollider.bounds.extents,
            currentTower.transform.rotation,
            invalidPlacementMask
        );

        if (!isInAdjacentCell || isCellOccupied || colliders.Length > 0)
        {
            canPlaceTower = false;
        }
        else
        {
            canPlaceTower = true;

            if (placementIndicator != null)
            {
                Renderer indicatorRenderer = placementIndicator.GetComponent<Renderer>();
                if (indicatorRenderer != null)
                {
                    indicatorRenderer.material = greenTransparentMaterial;
                }
            }
        }
    }

    List<Vector3> GetAdjacentCells(Vector3 centerCell)
    {
        List<Vector3> adjacentCells = new List<Vector3>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                Vector3 adjacentCell = new Vector3(
                    centerCell.x + x * cellSize,
                    centerCell.y,
                    centerCell.z + z * cellSize
                );
                if (predefinedCells.Contains(adjacentCell))
                {
                    adjacentCells.Add(adjacentCell);
                }
            }
        }

        return adjacentCells;
    }

    void PlaceTower()
    {
        if (selectedTowerIndex != -1)
        {
            ResetTowerImage(selectedTowerIndex);
            selectedTowerIndex = -1;
        }

        Collider towerCollider = currentTower.GetComponent<Collider>();
        if (towerCollider != null)
        {
            towerCollider.enabled = true;
        }

        Vector3 occupiedCell = currentTower.transform.position;
        if (!occupiedCells.Contains(occupiedCell))
        {
            occupiedCells.Add(occupiedCell);
        }

        if (placementIndicator != null)
        {
            Destroy(placementIndicator);
        }

        if (currentAreaOfAttack != null)
        {
            Destroy(currentAreaOfAttack);
            currentAreaOfAttack = null;
        }

        currentTower = null;
    }

    void SelectTower(GameObject towerPrefab, int imageIndex)
    {
        ResetBuildMode();

        if (selectedTowerIndex == imageIndex)
        {
            ResetTowerImage(imageIndex);
            selectedTowerIndex = -1;
            Destroy(currentTower);
            if (placementIndicator != null)
                Destroy(placementIndicator);
            if (currentAreaOfAttack != null)
            {
                Destroy(currentAreaOfAttack);
                currentAreaOfAttack = null;
            }
            return;
        }

        if (selectedTowerIndex != -1)
        {
            ResetTowerImage(selectedTowerIndex);
        }

        if (imageIndex >= 0 && imageIndex < towerImages.Count)
        {
            HighlightTowerImage(imageIndex);
            selectedTowerIndex = imageIndex;
        }

        if (currentTower != null)
            Destroy(currentTower);
        if (currentAreaOfAttack != null)
        {
            Destroy(currentAreaOfAttack);
            currentAreaOfAttack = null;
        }

        currentTower = Instantiate(towerPrefab);
        currentTower.GetComponent<Collider>().enabled = false;

        float radius = 3f;
        if (imageIndex == 0)
            radius = areaOfAttackRadiusArcher;
        else if (imageIndex == 1)
            radius = areaOfAttackRadiusMagic;
        else if (imageIndex == 2)
            radius = areaOfAttackRadiusIce;

        currentAreaOfAttack = Instantiate(AreaofAttackPrefab);
        currentAreaOfAttack.transform.position = currentTower.transform.position;
        currentAreaOfAttack.transform.localScale = new Vector3(radius * 2, 0.5f, radius * 2);

        Renderer rend = currentAreaOfAttack.GetComponent<Renderer>();
        if (rend != null && yellowTransparentMaterial != null)
            rend.material = yellowTransparentMaterial;

        if (placementIndicator == null)
        {
            placementIndicator = Instantiate(placementIndicatorPrefab);
            placementIndicator.transform.localScale = new Vector3(cellSize, 5f, cellSize);
        }
    }

    void HighlightTowerImage(int index)
    {
        Image towerImage = towerImages[index];
        towerImage.color = new Color(towerImage.color.r, towerImage.color.g, towerImage.color.b, 230f / 255f);
        towerImage.rectTransform.sizeDelta = new Vector2(300, 300);
        Drawning.SetActive(true);

        if (currentAreaOfAttack == null && currentTower != null)
        {
            float radius = 3f;
            if (index == 0)
                radius = areaOfAttackRadiusArcher;
            else if (index == 1)
                radius = areaOfAttackRadiusMagic;
            else if (index == 2)
                radius = areaOfAttackRadiusIce;

            currentAreaOfAttack = Instantiate(AreaofAttackPrefab);
            currentAreaOfAttack.transform.position = currentTower.transform.position;
            currentAreaOfAttack.transform.localScale = new Vector3(radius * 2, 0.5f, radius * 2);

            Renderer rend = currentAreaOfAttack.GetComponent<Renderer>();
            if (rend != null && yellowTransparentMaterial != null)
                rend.material = yellowTransparentMaterial;
        }
        if (currentAreaOfAttack != null)
            currentAreaOfAttack.SetActive(true);
        towerSelectionSound.Play();
    }

    void ResetTowerImage(int index)
    {
        Image towerImage = towerImages[index];
        towerImage.color = new Color(towerImage.color.r, towerImage.color.g, towerImage.color.b, 130f / 255f);
        towerImage.rectTransform.sizeDelta = new Vector2(250, 250);
        Drawning.SetActive(false);
        if (currentAreaOfAttack != null)
        {
            Destroy(currentAreaOfAttack);
            currentAreaOfAttack = null;
        }
        towerSelectionSound.Play();
    }

    void ResetBuildMode()
    {
        if (buildProgressBar != null)
        {
            buildProgressBar.fillAmount = 0f;
            buildProgressBar.gameObject.SetActive(false);
            buildProgressBarImage.gameObject.SetActive(false);
        }

        isBuildingTower = false;

        if (currentAreaOfAttack != null)
        {
            Destroy(currentAreaOfAttack);
            currentAreaOfAttack = null;
        }
        Drawning.SetActive(false);
    }
}