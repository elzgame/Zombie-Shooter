using UnityEngine;
using System;

public class AmmoBox : MonoBehaviour
{
    public int ammo;
    public AudioClip ammoSound;
    private WeaponManager weaponManager;

    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            weaponManager.weaponAmmoReload += ammo;
            GameManager.audioSource.PlayOneShot(ammoSound);
            Destroy(this.gameObject);
        }
    }

}
