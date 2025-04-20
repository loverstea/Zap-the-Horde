using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Towers : MonoBehaviour
{
    public GameObject archerTowerPrefab;
    public GameObject magicTowerPrefab;
    public LayerMask gridLayerMask;
    public LayerMask invalidPlacementMask;

    public List<Image> towerImages;
    private int selectedTowerIndex = -1;

    private GameObject currentTower;
    private Camera mainCamera;

    public float cellSize = 4f;
    public Vector3 topLeftCorner;
    public Vector3 bottomRightCorner;

    public List<Vector3> predefinedCells;
    public List<Vector3> occupiedCells = new List<Vector3>();

    private bool canPlaceTower = true;

    public GameObject placementIndicatorPrefab;
    private GameObject placementIndicator;

    public Material greenTransparentMaterial;
    public Material redTransparentMaterial;

    void Start()
    {
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
    {
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
            SelectTower(archerTowerPrefab, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectTower(magicTowerPrefab, 1);
        }

        if (currentTower != null)
        {
            MoveTowerToGrid();
            CheckPlacementValidity();
        }

        if (Input.GetMouseButtonDown(0) && currentTower != null && canPlaceTower)
        {
            PlaceTower();
        }
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
            {
                placementIndicator.transform.position = nearestCellPosition;
            }
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
        Collider towerCollider = currentTower.GetComponent<Collider>();
        if (towerCollider == null)
        {
            canPlaceTower = false;
            return;
        }

        Vector3 boxCenter = currentTower.transform.position;
        Vector3 boxSize = towerCollider.bounds.size;

        Collider[] colliders = Physics.OverlapBox(
            boxCenter,
            boxSize / 2f,
            currentTower.transform.rotation,
            invalidPlacementMask
        );

        if (occupiedCells.Contains(boxCenter) || colliders.Length > 0)
        {
            canPlaceTower = false;

            if (placementIndicator != null)
            {
                Renderer indicatorRenderer = placementIndicator.GetComponent<Renderer>();
                if (indicatorRenderer != null)
                {
                    indicatorRenderer.material = redTransparentMaterial;
                }
            }
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

        currentTower = null;
    }

    void SelectTower(GameObject towerPrefab, int imageIndex)
    {
        if (selectedTowerIndex == imageIndex)
        {
            ResetTowerImage(imageIndex);
            selectedTowerIndex = -1;
            Destroy(currentTower);
            if (placementIndicator != null)
            {
                Destroy(placementIndicator);
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
        {
            Destroy(currentTower);
        }

        currentTower = Instantiate(towerPrefab);
        currentTower.GetComponent<Collider>().enabled = false;

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
    }

    void ResetTowerImage(int index)
    {
        Image towerImage = towerImages[index];
        towerImage.color = new Color(towerImage.color.r, towerImage.color.g, towerImage.color.b, 130f / 255f);
        towerImage.rectTransform.sizeDelta = new Vector2(250, 250);
    }
}