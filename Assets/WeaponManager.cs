using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int weaponUsed = 0;
    public bool isSwitching = false;
    public GameObject knife;
    public GameObject weapon;

    // Update is called once per frame
    void Update()
    {
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
