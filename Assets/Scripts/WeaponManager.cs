using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public int weaponUsed = 0;
    public bool isSwitching = false;
    public GameObject weapon;
    public GameObject knife;
    public Text weaponAmmoText;
    public int weaponAmmoCurrent;
    public int weaponAmmoConstraint;
    public int weaponAmmoReload;

    void Update()
    {
        weaponAmmoText.text = weaponAmmoCurrent + " / " + weaponAmmoReload;
        if (weaponUsed == 1 && isSwitching)
        {
            isSwitching = false;
            StartCoroutine(Swtiching());
        }
        else if (weaponUsed == 0 && isSwitching)
        {
            isSwitching = false;
            StartCoroutine(Swtiching());
        }
    }



    IEnumerator Swtiching()
    {
        if (weaponUsed == 1)
        {
            yield return new WaitForSeconds(.5f);
            knife.SetActive(true);
            weapon.SetActive(false);
        }
        else if (weaponUsed == 0)
        {
            yield return new WaitForSeconds(.5f);
            knife.SetActive(false);
            weapon.SetActive(true);
        }
    }

}
