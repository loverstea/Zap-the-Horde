using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{ 
    public GameObject TowersMenu;
    public GameObject TowerUI;
    public GameObject CurrentTower;

    private bool isNearTower = false; // Флаг, указывающий, находится ли игрок рядом с башней

    void Start()
    {
        TowersMenu.SetActive(false);
        TowerUI.SetActive(false);
    }

    void Update()
    {
        if (isNearTower)
        {
            // Показываем UI, если меню башни не активно
            if (!TowersMenu.activeSelf)
            {
                TowerUI.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Time.timeScale = 0f;

                // Переключаем состояние меню
                TowersMenu.SetActive(!TowersMenu.activeSelf);

                // Скрываем UI, если меню открыто
                TowerUI.SetActive(!TowersMenu.activeSelf);
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
            CurrentTower = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            isNearTower = false;
            CurrentTower = null;

Time.timeScale = 1f;
            // Скрываем меню и UI при выходе из зоны башни
            TowersMenu.SetActive(false);
            TowerUI.SetActive(false);
        }
    }
}