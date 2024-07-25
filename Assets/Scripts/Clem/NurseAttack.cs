using UnityEngine;
using UnityEngine.AI;
using Cinemachine;


public class NurseAttack : MonoBehaviour
{
   public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer;


    //Attacking 

    public float timeBetweenAttacks;

    bool alreadyAttacked;

    //States 

    public float sightRange, attackRange;

    public bool playerInSightRange, playerInAttackRange;

    public void Update()
    {
        //Check For Sight and Attack Range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);



        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInAttackRange && playerInSightRange) AttackPlayer();



    }

    private void Awake()
    {
        player = GameObject.Find("ClemNurse").transform;

        agent = GetComponent<NavMeshAgent>();
    }

    private void Patrolling()
    {

    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

    }

    private void AttackPlayer()
    {
        //Make sure enemy doesnt move 

        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked) 
        {
            //Attack anim here 


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        
        }
    }

    private void OnDrewGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        // reset here 
    }


}
