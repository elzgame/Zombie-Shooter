using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour
{
    private Animator animator;
    public int knifeDamage;
    public bool isCanKnife = true;
    public Camera playerCamera;
    Vector3 moveDirection = Vector3.zero;
    public GameObject crosshair;
    private Vector3 crosshairScale;
    public AudioClip knifeStabSound;


    void Start()
    {
        animator = GetComponent<Animator>();
        crosshairScale = crosshair.transform.localScale;
    }

    public void StabStart()
    {
        GameManager.audioSource.PlayOneShot(knifeStabSound);
        Debug.Log("Stab Start!");
    }

    public void StabDone()
    {
        animator.SetBool("Stab", false);
        Debug.Log("Stab Done!");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1f))
        {
            if (hit.transform.tag == "Zombie")
            {
                StartCoroutine(CrosshairHit());
                moveDirection = hit.transform.gameObject.GetComponent<Rigidbody>().transform.position - transform.position;
                moveDirection = new Vector3(moveDirection.x, moveDirection.y + 0.75f, moveDirection.z);
                Debug.Log(moveDirection);
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 50f);
                hit.transform.gameObject.GetComponent<Zombie>().zombieHealth -= knifeDamage;
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
