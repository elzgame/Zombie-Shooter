using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShootDone()
    {
        animator.SetBool("Shoot", false);
    }

}
