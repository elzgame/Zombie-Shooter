using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public int weaponDamage;
    public Animator animator;
    public Camera playerCamera;
    Vector3 moveDirection = Vector3.zero;
    public Transform weaponBulletPoint;
    public AudioClip weaponShootSound;
    public AudioClip weaponReloadSound;
    public bool isCanShoot = true;
    public float reloadTime;
    public GameObject crosshair;
    private Vector3 crosshairScale;
    private bool isCompleteEmptyAmmo;
    public WeaponManager weaponManager;
    public Button changeWeaponButton;


    void Start()
    {
        weaponDamage = PlayerPrefs.GetInt("weaponDamage", 10);
        animator = GetComponent<Animator>();
        weaponManager.weaponAmmoCurrent = weaponManager.weaponAmmoConstraint;
        crosshairScale = crosshair.transform.localScale;
    }

    void OnEnable()
    {
        animator.SetInteger("Switch", 2);
    }

    public void AnimateUpDone()
    {
        animator.SetInteger("Switch", 0);
    }

    void Update()
    {
        if (weaponManager.weaponAmmoCurrent <= 0)
            weaponManager.weaponAmmoCurrent = 0;
        if (weaponManager.weaponAmmoCurrent <= 0 && isCanShoot)
        {
            isCanShoot = false;
            // Reload
            if (weaponManager.weaponAmmoReload > 0)
            {
                StartCoroutine(Reload());
            }
            else
            {
                isCompleteEmptyAmmo = true;
            }
        }
        if (isCompleteEmptyAmmo == true && weaponManager.weaponAmmoReload > 0)
        {
            isCanShoot = true;
            isCompleteEmptyAmmo = false;
        }
    }



    IEnumerator Reload()
    {
        changeWeaponButton.interactable = false;
        animator.SetBool("Reload", true);
        GameManager.audioSource.PlayOneShot(weaponReloadSound);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reload", false);
        changeWeaponButton.interactable = true;
        isCanShoot = true;
        if (weaponManager.weaponAmmoReload < weaponManager.weaponAmmoConstraint)
        {
            weaponManager.weaponAmmoCurrent += weaponManager.weaponAmmoReload;
            weaponManager.weaponAmmoReload = 0;
        }
        else if (weaponManager.weaponAmmoReload >= weaponManager.weaponAmmoConstraint)
        {
            weaponManager.weaponAmmoCurrent += weaponManager.weaponAmmoConstraint;
            weaponManager.weaponAmmoReload -= weaponManager.weaponAmmoConstraint;
        }
    }


    public void ShootStart()
    {
        GameManager.audioSource.PlayOneShot(weaponShootSound);
        weaponManager.weaponAmmoCurrent--;
    }

    public void ShootDone()
    {
        animator.SetBool("Shoot", false);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            if (hit.transform.tag == "Zombie")
            {
                StartCoroutine(CrosshairHit());
                moveDirection = hit.transform.gameObject.GetComponent<Rigidbody>().transform.position - transform.position;
                moveDirection = new Vector3(moveDirection.x, moveDirection.y + 0.75f, moveDirection.z);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 50f);
                hit.transform.gameObject.GetComponent<Zombie>().zombieHealth -= weaponDamage;
            }
        }
    }

    IEnumerator CrosshairHit()
    {
        crosshair.GetComponent<Image>().color = Color.red;
        crosshair.gameObject.transform.localScale = new Vector3(crosshair.gameObject.transform.localScale.x + 0.2f, crosshair.gameObject.transform.localScale.y + 0.2f, crosshair.gameObject.transform.localScale.z);
        yield return new WaitForSeconds(0.1f);
        crosshair.GetComponent<Image>().color = Color.black;
        crosshair.gameObject.transform.localScale = crosshairScale;
        StopCoroutine(CrosshairHit());
    }



}
