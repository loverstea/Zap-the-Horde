using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceTowerPower : MonoBehaviour
{
    public Image IcePowerImage;
    public Image IcePowerUI;
    public Image IcePowerImageCooldown;
    public float cooldownTime = 20f;
    public float slowDuration = 5f;
    public float slowAmount = 0.1f;

    private bool canUsePower = false;
    private bool isCooldown = false;

    void Start()
    {
        IcePowerImage.gameObject.SetActive(false);
        IcePowerUI.gameObject.SetActive(false);
        IcePowerImageCooldown.fillAmount = 0f;
    }

    void Update()
    {
        bool hasIceTower = FindIceTowerLevel3();

        if (!canUsePower && hasIceTower)
        {
            IcePowerImage.gameObject.SetActive(true);
            canUsePower = true;
        }

        if (canUsePower && !hasIceTower)
        {
            IcePowerImage.gameObject.SetActive(false);
            IcePowerUI.gameObject.SetActive(false);
            IcePowerImageCooldown.fillAmount = 0f;
            canUsePower = false;
            isCooldown = false;
            StopAllCoroutines();
        }

        if (canUsePower && !isCooldown && hasIceTower && Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(UseIcePower());
        }
    }

    bool FindIceTowerLevel3()
    {
        GameObject[] allTowers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (var tower in allTowers)
        {
            if (tower.name.Contains("IceTower-lvl3"))
                return true;
        }
        return false;
    }

    IEnumerator UseIcePower()
    {
        isCooldown = true;
        IcePowerImage.fillAmount = 1f;


        IcePowerUI.gameObject.SetActive(true);

        EnemiScript[] allEnemies = FindObjectsOfType<EnemiScript>();
        foreach (var enemy in allEnemies)
        {
            enemy.Slow(slowAmount, slowDuration);
        }

        float elapsed = 0f;
        while (elapsed < slowDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        IcePowerUI.gameObject.SetActive(false);

        elapsed = 0f;
        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            IcePowerImageCooldown.fillAmount = 1f - (elapsed / cooldownTime);
            yield return null;
        }
        IcePowerImageCooldown.fillAmount = 0f;
        isCooldown = false;
    }
}