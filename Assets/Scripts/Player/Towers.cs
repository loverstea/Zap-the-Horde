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

    public float cellSize = 2f;          // Размер клетки (по умолчанию 2x2)

    private bool canPlaceTower = true;   // Можно ли установить башню

    private Material originalMaterial;   // Оригинальный материал башни
    public Material outlineMaterial;    // Материал для обводки

    void Start()
    {
        mainCamera = Camera.main; // Получаем основную камеру
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

        // Сохраняем оригинальный материал и применяем обводку
        Renderer renderer = currentTower.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            renderer.material = outlineMaterial; // Применяем материал обводки
        }
    }

    void MoveTowerToGrid()
    {
        // Получаем позицию мыши в мире
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayerMask))
        {
            // Привязываем башню к центру клетки
            Vector3 gridPosition = hit.point;
            gridPosition.x = Mathf.Floor(gridPosition.x / cellSize) * cellSize + cellSize / 2f; // Центр клетки по X
            gridPosition.z = Mathf.Floor(gridPosition.z / cellSize) * cellSize + cellSize / 2f; // Центр клетки по Z
            gridPosition.y = hit.point.y; // Сохраняем высоту
            currentTower.transform.position = gridPosition;
        }
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

    Collider[] colliders = Physics.OverlapBox(
        boxCenter,
        boxSize / 2f, // Половина размера для OverlapBox
        currentTower.transform.rotation,
        invalidPlacementMask
    );

    Renderer renderer = currentTower.GetComponent<Renderer>();
    if (colliders.Length > 0)
{
    Debug.Log("Пересечение с объектами:");
    foreach (Collider col in colliders)
    {
        Debug.Log(col.gameObject.name);
    }
    canPlaceTower = false;
    if (renderer != null)
    {
        renderer.material.color = Color.red; // Подсвечиваем башню красным
    }
}
    else
    {
        canPlaceTower = true;
        if (renderer != null)
        {
            renderer.material.color = Color.green; // Подсвечиваем башню зелёным
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

        // Возвращаем оригинальный материал
        Renderer renderer = currentTower.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = originalMaterial; // Возвращаем оригинальный материал
            renderer.material.color = Color.white; // Возвращаем цвет башни
        }

        // Сбрасываем текущую башню, чтобы больше не перемещать её
        currentTower = null;
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