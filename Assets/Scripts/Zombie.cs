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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isWalking = true;
    }

    void Update()
    {
        var distance = Vector3.Distance(player.position, transform.position);
        Debug.Log(distance);
        if (distance <= 2f && isAttacking == false)
        {
            isAttacking = true;
            isWalking = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            Debug.Log("Attacking");
        } else if(distance >= 2f) {
            isWalking = true;
        }

        if (isWalking)
        {
            isAttacking = false;
            isWalking = true;
            agent.speed = zombieSpeed;
            agent.SetDestination(player.transform.position);
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ZombieAttack();
            Debug.Log("Got hit!");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ZombieDie();
            Debug.Log("Killed a zombie!");
        }

        transform.LookAt(player);

    }

    public void ZombieAttack()
    {
        PlayerStats.playerHealth -= damage;
    }

    public void ZombieDie()
    {
        PlayerStats.playerMoney += zombieMoney;
    }

}
