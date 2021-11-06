using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public int zombieHealth;
    public int zombieMoney;
    public float zombieSpeed;
    public int zombieDamage;
    public Transform player;
    private NavMeshAgent agent;
    public Animator animator;
    private bool isAttacking;
    private bool isWalking;
    private bool isDying;
    public AudioClip soundZombieDie;
    public AudioClip[] soundZombieAttack;
    [SerializeField]
    private GameObject zombieHand;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<SC_FPSController>().gameObject.transform;
        isWalking = true;
    }

    void Update()
    {
        var distance = Vector3.Distance(player.position, transform.position);
        if (distance <= 2f && isAttacking == false && isDying == false)
        {
            isAttacking = true;
            isWalking = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
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
        
        if (zombieHealth <= 0)
        {
            animator.SetBool("Die", true);
        }

        if (isDying)
        {
            isDying = false;
            GameManager.audioSource.PlayOneShot(soundZombieDie);
            GameManager.zombieCountLeft--;
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
            PlayerStats.playerHealth -= zombieDamage;
            GameManager.audioSource.PlayOneShot(soundZombieAttack[Random.Range(0, soundZombieAttack.Length)]);
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
