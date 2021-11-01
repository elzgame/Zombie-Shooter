using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public int zombieHealth;
    public int zombieMoney;
    public float zombieSpeed;
    public int damage;
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;
    private bool isWalking;
    private bool isDying;
    [SerializeField]
    private GameObject zombieHand;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isWalking = true;
    }

    void Update()
    {
        var distance = Vector3.Distance(player.position, transform.position);
        // Debug.Log(distance);
        if (distance <= 2f && isAttacking == false && isDying == false)
        {
            isAttacking = true;
            isWalking = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            Debug.Log("Attacking");
        }
        else if (distance >= 2f && isDying == false)
        {
            isWalking = true;
            isAttacking = false;
        }

        if (isWalking && isAttacking == false && isDying == false)
        {
            isAttacking = false;
            isWalking = true;
            agent.speed = zombieSpeed;
            agent.SetDestination(player.transform.position);
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
            zombieHand.GetComponent<ZombieHand>().isHit = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetBool("Die", true);
            Debug.Log("Killed a zombie!");
        }

        if (zombieHealth <= 0) 
        {
            animator.SetBool("Die", true);
            Debug.Log("Killed a zombie!");
        }



        if (isDying)
        {
            agent.isStopped = true;
            isWalking = false;
            isAttacking = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
        }

        transform.LookAt(player);

    }

    public void ZombieAttack()
    {
        if (zombieHand.GetComponent<ZombieHand>().isHit == true)
        {
            PlayerStats.playerHealth -= damage;
        }
    }

    public void ZombieDie()
    {
        isDying = true;
    }

    public void ZombieDieMoney()
    {
        PlayerStats.playerMoney += zombieMoney;
        Destroy(this.gameObject);
    }

}
