using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Platformer
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (PlayerDetector))]

    public class NurseCodeOffice : Entity
    {
        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Self] PlayerDetector playerDetector;
        [SerializeField, Child] Animator animator;
        NavMeshAgent navMeshAgent;
    
        [SerializeField] float wanderRadius = 10f; // changes how far enime is able to wander 

        [SerializeField] float timeBetweenAttacks = 1f;

        StateMachine stateMachine;

        CountdownTimer attackTimer;

        EnemyWanderState enemyWanderState;
        public int currentWayPointIndex = 0;
       

        public List<Transform> wayPoint;

        void OnValidate() => this.ValidateRefs();



        void Start()
        {
            attackTimer = new CountdownTimer(timeBetweenAttacks);
            stateMachine = new StateMachine();

            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);


            //Any(wanderState, new FuncPredicate (() => true)); // for testing PURPOSES setting it to always true!!!!!!!!!!!!!!!


            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

            Debug.Log("Wonder and chase");


            stateMachine.SetState(wanderState);


            navMeshAgent = GetComponent<NavMeshAgent>();

           
            StartCoroutine(WalkingBB());

        }



        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);




        void Update()
        {

            stateMachine.Update();

            attackTimer = new CountdownTimer(timeBetweenAttacks);

        }



        public IEnumerator WalkingBB()
        {
       
            if (wayPoint.Count == 0)
            {
                Debug.LogWarning("WayPoint list is empty or null.");

            }

            float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, transform.position);
          
            if (distanceToWayPoint <= 3)
            {

              
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;

                if (wayPoint[4])
                {
                    Quaternion lookDir = Quaternion.LookRotation(wayPoint[currentWayPointIndex].position);
                    yield return new WaitForSeconds(2);

                   
                    Debug.Log("Waiting 2 secs");
                }
            }

            agent.SetDestination(wayPoint[currentWayPointIndex].position);

        }

        
       
        
    


        void FixedUpdate()
        {
            stateMachine.FixedUpdate();

        }
        

        public void Attack()
        {
            if (attackTimer.IsRunning) return;

            attackTimer.Start();
            Debug.Log("Attacking");

        }
    }

}

