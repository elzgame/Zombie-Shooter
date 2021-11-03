using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public int weaponDamage;
    private Animator animator;
    public Camera playerCamera;
    Vector3 moveDirection = Vector3.zero;
    public Transform weaponBulletPoint;
    public AudioClip weaponShootSound;
    public AudioClip weaponReloadSound;
    public bool isCanShoot = true;
    public Text weaponAmmoText;
    public int weaponAmmoCurrent;
    public int weaponAmmoConstraint;
    public int weaponAmmoReload;
    public float reloadTime;

    void Start()
    {
        weaponDamage = PlayerPrefs.GetInt("weaponDamage", 10);
        animator = GetComponent<Animator>();
        weaponAmmoCurrent = weaponAmmoConstraint;
    }

    void Update()
    {
        weaponAmmoText.text = weaponAmmoCurrent + " / " + weaponAmmoReload;
        if (weaponAmmoCurrent <= 0 && isCanShoot)
        {
            isCanShoot = false;
            // Reload
            if (weaponAmmoReload > 0)
            {
                // Can reload
                StartCoroutine(Reload());
            }
            else
            {
                // Can't reload
                Debug.Log("No ammo");
            }
        }
    }



    IEnumerator Reload()
    {
        Debug.Log("Reloading....");
        animator.SetBool("Reload", true);
        GameManager.audioSource.PlayOneShot(weaponReloadSound);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reload", false);
        isCanShoot = true;
        weaponAmmoCurrent += weaponAmmoConstraint;
        weaponAmmoReload -= weaponAmmoConstraint;
    }

    public void ShootStart()
    {
        GameManager.audioSource.PlayOneShot(weaponShootSound);
        weaponAmmoCurrent--;
    }

    public void ShootDone()
    {
        animator.SetBool("Shoot", false);
        Debug.Log("Shooting!");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            if (hit.transform.tag == "Zombie")
            {

                moveDirection = hit.transform.gameObject.GetComponent<Rigidbody>().transform.position - transform.position;
                moveDirection = new Vector3(moveDirection.x, moveDirection.y + 0.75f, moveDirection.z);
                Debug.Log(moveDirection);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 50f);
                hit.transform.gameObject.GetComponent<Zombie>().zombieHealth -= weaponDamage;
                Debug.Log(hit.transform.name + " : " + hit.transform.gameObject.GetComponent<Zombie>().zombieHealth);
            }
        }
    }



}
