using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject GunPrefab;
    public Transform firePoint;

    public float fireRate = 1f;
    public float bulletSpeed = 40f;
    public float bulletDamage = 0f;
    public float bulletLifetime = 2f;
    private float nextFireTime = 5f;

    private bool isGunSelected = false;

    public Image gunImage; 

    void Start()
    {
        if(GunPrefab.activeSelf)
        {
            GunPrefab.SetActive(false);
            isGunSelected = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !isGunSelected)
        {
            isGunSelected = true;
            GunPrefab.SetActive(true);
            HighlightGunImage();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isGunSelected)
        {
            isGunSelected = false;
            GunPrefab.SetActive(false);
            ResetGunImage();
        }
        
        if (Input.GetButtonDown("Fire1") && isGunSelected && GunPrefab.activeSelf)
        {
            FireBullet();
        }


        if(Input.GetKeyDown(KeyCode.Alpha1) && isGunSelected && GunPrefab.activeSelf)
        {
            isGunSelected = false;
            GunPrefab.SetActive(false);
            ResetGunImage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isGunSelected && GunPrefab.activeSelf)
        {
            isGunSelected = false;
            GunPrefab.SetActive(false);
            ResetGunImage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isGunSelected && GunPrefab.activeSelf)
        {
            isGunSelected = false;
            GunPrefab.SetActive(false);
            ResetGunImage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && isGunSelected && GunPrefab.activeSelf)
        {
            isGunSelected = false;
            GunPrefab.SetActive(false);
            ResetGunImage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && isGunSelected && GunPrefab.activeSelf)
        {
            isGunSelected = false;
            GunPrefab.SetActive(false);
            ResetGunImage();
        }
    }

void FireBullet()
{
    if (Time.timeScale != 0f)
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(90f, 0f, 0f);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Vector3 direction = (hit.point - firePoint.position).normalized;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = direction * bulletSpeed;
            }
            else
            {
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.velocity = firePoint.forward * bulletSpeed;
            }

            Destroy(bullet, bulletLifetime);
        }
    }
}

    void HighlightGunImage()
    {
        gunImage.color = new Color(gunImage.color.r, gunImage.color.g, gunImage.color.b, 230f / 255f);
        gunImage.rectTransform.sizeDelta = new Vector2(300, 300);
    }

    void ResetGunImage()
    {
        gunImage.color = new Color(gunImage.color.r, gunImage.color.g, gunImage.color.b, 130f / 255f);
        gunImage.rectTransform.sizeDelta = new Vector2(250, 250);
    }
}
