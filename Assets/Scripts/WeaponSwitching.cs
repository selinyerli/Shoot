using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;


    private void start() 
    {
        SelectWeapon();    
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1) 
            selectedWeapon = 0;
            else
            selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >=2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    private void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                Gun gun = weapon.GetComponent<Gun>();
                if (gun != null)
                {
                    gun.UpdateAmmoUI();
                    Debug.Log($"Selected weapon: {weapon.name} with {gun.currentAmmo}/{gun.maxAmmo} ammo.");
                }
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }

    }
}

