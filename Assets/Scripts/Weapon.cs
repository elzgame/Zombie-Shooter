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
    public Text weaponAmmoText;
    public int weaponAmmoCurrent;
    public int weaponAmmoConstraint;
    public int weaponAmmoReload;
    public float reloadTime;
    public GameObject crosshair;
    private Vector3 crosshairScale;


    void Start()
    {
        weaponDamage = PlayerPrefs.GetInt("weaponDamage", 10);
        animator = GetComponent<Animator>();
        weaponAmmoCurrent = weaponAmmoConstraint;
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
        weaponAmmoText.text = weaponAmmoCurrent + " / " + weaponAmmoReload;
        if (weaponAmmoCurrent <= 0)
            weaponAmmoCurrent = 0;
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
                StartCoroutine(CrosshairHit());
                moveDirection = hit.transform.gameObject.GetComponent<Rigidbody>().transform.position - transform.position;
                moveDirection = new Vector3(moveDirection.x, moveDirection.y + 0.75f, moveDirection.z);
                Debug.Log(moveDirection);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 50f);
                hit.transform.gameObject.GetComponent<Zombie>().zombieHealth -= weaponDamage;
                Debug.Log(hit.transform.name + " : " + hit.transform.gameObject.GetComponent<Zombie>().zombieHealth);
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
