using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Towers : MonoBehaviour
{
    public GameObject archerTowerPrefab; // Префаб башни лучников
    public GameObject magicTowerPrefab;  // Префаб магической башни
    public LayerMask gridLayerMask;      // Слой для сетки (где можно размещать башни)
    public LayerMask invalidPlacementMask; // Слой объектов, с которыми нельзя пересекаться (например, дороги или другие башни)

    public List<Image> towerImages;      // Список изображений башен
    private int selectedTowerIndex = -1; // Индекс выбранного изображения (-1, если ничего не выбрано)

    private GameObject currentTower;     // Текущая башня, которую игрок перемещает
    private Camera mainCamera;           // Камера для определения позиции мыши

    public float cellSize = 4f;          // Размер клетки (по умолчанию 4x4)
    public Vector3 topLeftCorner;        // Верхний левый угол карты
    public Vector3 bottomRightCorner;    // Нижний правый угол карты

    public List<Vector3> predefinedCells; // Список сгенерированных клеток
    public List<Vector3> occupiedCells = new List<Vector3>(); // Список занятых клеток

    private bool canPlaceTower = true;   // Можно ли установить башню

    private Material originalMaterial;   // Оригинальный материал башни

    void Start()
    {
        mainCamera = Camera.main; // Получаем основную камеру
        GenerateCells();          // Генерируем клетки
    }

    void Update()
    {
        // Выбор башни
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectTower(archerTowerPrefab, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectTower(magicTowerPrefab, 1);
        }

        // Перемещение башни по сетке
        if (currentTower != null)
        {
            MoveTowerToGrid();
            CheckPlacementValidity(); // Проверяем, можно ли установить башню
        }

        // Установка башни на клетку
        if (Input.GetMouseButtonDown(0) && currentTower != null && canPlaceTower)
        {
            PlaceTower();
        }
    }

    void GenerateCells()
    {
        predefinedCells = new List<Vector3>();

        // Определяем диапазоны для генерации клеток
        float minX = Mathf.Min(topLeftCorner.x, bottomRightCorner.x);
        float maxX = Mathf.Max(topLeftCorner.x, bottomRightCorner.x);
        float minZ = Mathf.Min(topLeftCorner.z, bottomRightCorner.z);
        float maxZ = Mathf.Max(topLeftCorner.z, bottomRightCorner.z);

        // Генерация клеток на основе диапазонов
        for (float x = minX; x <= maxX; x += cellSize)
        {
            for (float z = maxZ; z >= minZ; z -= cellSize)
            {
                Vector3 cellPosition = new Vector3(x, topLeftCorner.y, z);
                predefinedCells.Add(cellPosition);
                Debug.Log($"Сгенерирована клетка: {cellPosition}");
            }
        }

        Debug.Log($"Сгенерировано {predefinedCells.Count} клеток.");
    }

    void MoveTowerToGrid()
    {
        // Получаем позицию мыши в мире
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayerMask))
        {
            // Находим ближайшую клетку
            Vector3 nearestCellPosition = FindNearestCell(hit.point);

            // Устанавливаем башню в центр ближайшей клетки
            currentTower.transform.position = nearestCellPosition;
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
        // Проверяем, пересекается ли башня с запрещёнными объектами
        Collider towerCollider = currentTower.GetComponent<Collider>();
        if (towerCollider == null)
        {
            Debug.LogError("У текущей башни отсутствует Collider!");
            canPlaceTower = false;
            return;
        }

        Vector3 boxCenter = currentTower.transform.position;
        Vector3 boxSize = towerCollider.bounds.size;

        // Проверяем пересечения с объектами из слоя invalidPlacementMask
        Collider[] colliders = Physics.OverlapBox(
            boxCenter,
            boxSize / 2f, // Половина размера для OverlapBox
            currentTower.transform.rotation,
            invalidPlacementMask
        );

        Renderer renderer = currentTower.GetComponent<Renderer>();

        // Проверяем, занята ли клетка
        if (occupiedCells.Contains(boxCenter))
        {
            Debug.Log("Клетка уже занята!");
            canPlaceTower = false;
            if (renderer != null)
            {
                renderer.material.color = new Color(1, 0, 0, 0.5f); // Красный цвет с прозрачностью
            }
            return;
        }

        if (colliders.Length > 0)
        {
            Debug.Log("Пересечение с запрещёнными объектами:");
            foreach (Collider col in colliders)
            {
                Debug.Log($"Пересечение с объектом: {col.gameObject.name}");
            }
            canPlaceTower = false;
            if (renderer != null)
            {
                renderer.material.color = new Color(1, 0, 0, 0.5f); // Красный цвет с прозрачностью
            }
        }
        else
        {
            canPlaceTower = true;
            if (renderer != null)
            {
                renderer.material.color = new Color(0, 1, 0, 0.5f); // Зелёный цвет с прозрачностью
            }
        }
    }

        void PlaceTower()
    {
        // Устанавливаем башню на текущую клетку
        if (selectedTowerIndex != -1)
        {
            ResetTowerImage(selectedTowerIndex); // Сбрасываем выделение изображения
            selectedTowerIndex = -1;            // Сбрасываем выбор
        }
    
        // Включаем коллайдер, чтобы игроки не могли проходить сквозь башню
        Collider towerCollider = currentTower.GetComponent<Collider>();
        if (towerCollider != null)
        {
            towerCollider.enabled = true;
        }
    
        // Добавляем текущую клетку в список занятых клеток
        Vector3 occupiedCell = currentTower.transform.position;
        if (!occupiedCells.Contains(occupiedCell))
        {
            occupiedCells.Add(occupiedCell);
            Debug.Log($"Клетка занята: {occupiedCell}");
        }
    
        // Возвращаем оригинальный материал
        Renderer renderer = currentTower.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material = originalMaterial; // Возвращаем оригинальный материал
        }
    
        // Сбрасываем текущую башню, чтобы больше не перемещать её
        currentTower = null;
    }

                void SelectTower(GameObject towerPrefab, int imageIndex)
        {
            // Если нажали на уже выбранное изображение, сбрасываем его
            if (selectedTowerIndex == imageIndex)
            {
                ResetTowerImage(imageIndex);
                selectedTowerIndex = -1; // Сбрасываем выбор
                Destroy(currentTower);   // Удаляем текущую башню
                return;
            }
        
            // Сбрасываем предыдущее выбранное изображение
            if (selectedTowerIndex != -1)
            {
                ResetTowerImage(selectedTowerIndex);
            }
        
            // Выделяем новое изображение
            if (imageIndex >= 0 && imageIndex < towerImages.Count)
            {
                HighlightTowerImage(imageIndex);
                selectedTowerIndex = imageIndex;
            }
        
            // Удаляем текущую башню, если она уже выбрана
            if (currentTower != null)
            {
                Destroy(currentTower);
            }
        
            // Создаём новую башню
            currentTower = Instantiate(towerPrefab);
            currentTower.GetComponent<Collider>().enabled = false; // Отключаем коллайдер для чертежа
        
            // Сохраняем оригинальный материал и делаем башню зелёной и прозрачной
            Renderer renderer = currentTower.GetComponentInChildren<Renderer>(); // Ищем Renderer в дочерних объектах
            if (renderer != null)
            {
                originalMaterial = renderer.material;
        
                // Создаём временный материал для чертежа
                Material transparentMaterial = new Material(Shader.Find("Standard"));
                transparentMaterial.color = new Color(0, 1, 0, 0.5f); // Зелёный цвет с прозрачностью
                transparentMaterial.SetFloat("_Mode", 3); // Устанавливаем режим прозрачности
                transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                transparentMaterial.SetInt("_ZWrite", 0);
                transparentMaterial.DisableKeyword("_ALPHATEST_ON");
                transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
                transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                transparentMaterial.renderQueue = 3000;
        
                renderer.material = transparentMaterial;
            }
            else
            {
                Debug.LogError("Renderer не найден на объекте башни или его дочерних объектах!");
            }
        }

    void HighlightTowerImage(int index)
    {
        Image towerImage = towerImages[index];
        towerImage.color = new Color(towerImage.color.r, towerImage.color.g, towerImage.color.b, 230f / 255f); // Прозрачность 230
        towerImage.rectTransform.sizeDelta = new Vector2(300, 300); // Размер 300x300
    }

    void ResetTowerImage(int index)
    {
        Image towerImage = towerImages[index];
        towerImage.color = new Color(towerImage.color.r, towerImage.color.g, towerImage.color.b, 130f / 255f); // Прозрачность 130
        towerImage.rectTransform.sizeDelta = new Vector2(250, 250); // Размер 250x250
    }
}