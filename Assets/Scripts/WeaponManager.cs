using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public int x = 3;
    public Gun[] guns; // Tüm silahlarý buraya ekleyeceksiniz
    public Text ammoUi;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;

    [SerializeField] private Gun currentGun;
    private int currentGunIndex = 0;

    void Start()
    {
        SelectGun(0); 
    }

    void Update()
    {
        //SelectGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectGun(0);

            Debug.Log(currentGun + "SDFGSDGSFGSGF");

            currentGun.StartReloading(); // Start reloading the selected gun
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectGun(1);
            currentGun.StartReloading(); // Start reloading the selected gun
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectGun(2);
            currentGun.StartReloading(); // Start reloading the selected gun
            return;
        }


        //if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        //{
        //    if (currentGun != null)
        //    {
        //        currentGun.Shoot();
        //        nextTimeToFire = Time.time + 1f / fireRate; // Bir sonraki ateþ etme zamaný
        //    }
        //}



        if (Input.GetKeyDown(KeyCode.R))
        {
                if (currentGun != null)
                {
                    currentGun.StartReloading();
                }
            
        }


    }

    void SelectGun(int index)
        {
        if (index < 0 || index >= guns.Length) return;
        currentGun = guns[index];
            currentGunIndex = index;
            

        }

    }


