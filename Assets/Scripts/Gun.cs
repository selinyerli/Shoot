using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 1f;

    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;

    public Animator animator;
    public Text ammoUi;

    private bool readyToShoot = true; // Yeni deðiþken

    private void Start()
    {
        ResetAmmo();
        muzzleFlash.Stop();
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            if (readyToShoot)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        if (animator != null)
        {
            animator.SetBool("Reloading", true);
        }
        yield return new WaitForSeconds(.25f);
        if (animator != null)
        {
            animator.SetBool("Reloading", false);
        }
        yield return new WaitForSeconds(.25f);
        ResetAmmo();
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
    }

    public void StartReloading()
    {
        StopAllCoroutines();
        StartCoroutine(Reload());
    }

    public bool CanReload()
    {
        return !isReloading && currentAmmo < maxAmmo;
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(1f / fireRate);
        readyToShoot = true;
    }

    public void Shoot()
    {
        Debug.Log("Shoot method called at: ");
        if (!readyToShoot) return;
        readyToShoot = false;

        Debug.Log("Gun Shoot method called at: ");
        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateAmmoUI();
            StartCoroutine(MuzzleFlash());
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }

        StartCoroutine(ResetShoot()); 
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.Play();
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.Stop();
    }

    public void UpdateAmmoUI()
    {
        if (ammoUi != null)
        {
            ammoUi.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
        }
    }

    public void ResetAmmo()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }
}




