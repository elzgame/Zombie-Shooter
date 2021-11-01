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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        agent.speed = zombieSpeed;
        agent.SetDestination(player.transform.position);
        animator.SetBool("Walk", true);

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
