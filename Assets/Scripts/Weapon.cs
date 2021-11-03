using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    private Animator animator;
    public Camera playerCamera;
    Vector3 moveDirection = Vector3.zero;
    public Transform weaponBulletPoint;


    void Start()
    {
        animator = GetComponent<Animator>();
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
                hit.transform.gameObject.GetComponent<Zombie>().zombieHealth -= damage;
                Debug.Log(hit.transform.name + " : " + hit.transform.gameObject.GetComponent<Zombie>().zombieHealth);
            }
        }
    }



}
