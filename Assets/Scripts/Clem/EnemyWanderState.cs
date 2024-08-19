using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyWanderState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Vector3 startpoint;
        readonly float wanderRadius;
        PlayerDetector playerDetector;

        public List<Transform> wayPoint;

        public int currentWayPointIndex = 0;
        public bool isItBlackBeak;
        void Start()
        {
            isItBlackBeak = GameObject.FindGameObjectWithTag("EnemyBB");
            Debug.Log("BB");

        }
        public EnemyWanderState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator)
        {
           

            if (enemy == null)
            {
                Debug.LogError("Enemy is null in EnemyWanderState constructor.");
                return;
            }

            if (isItBlackBeak)
            {

                Debug.Log("Reading WayPoints");
                wayPoint = enemy.listForBB;
                Debug.Log("WayPoint list count: " + wayPoint.Count);
                WalkingBB();

            }

            else
            {
                this.agent = agent;
                this.startpoint = enemy.transform.position;
                this.wanderRadius = wanderRadius;

                Debug.Log("enemie wander stre plays");

            }


        }
  

      

    
        void WalkingBB()
        {
            if (wayPoint == null || wayPoint.Count == 0)
            {
                Debug.LogWarning("WayPoint list is empty or null.");
                return;
            }

            float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, enemy.transform.position);

            if (distanceToWayPoint <= 3 ) 
            {
                currentWayPointIndex = (currentWayPointIndex +1) % wayPoint.Count;
            }

            agent.SetDestination(wayPoint[currentWayPointIndex].position);




        }

        

        public override void OnEnter()
        {
            Debug.Log(message: "WANDER");
            animator.CrossFade(WalkHash, crossFadeDuration);

            //animator.CrossFade(WalkHash, crossFadeDuration);
        }

        public override void Update()
        {
            if (!isItBlackBeak)
            {
                if (HasReachedDestination())
                {
                    //find a new destination
                    //mayne find out how to change this is we want her to have a set route !!!!!!!!!!!!!!!

                    var randomDirection = Random.insideUnitSphere * wanderRadius;
                    randomDirection += startpoint;
                    NavMeshHit hit;
                    NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, areaMask: 1);

                    var finalPosition = hit.position;

                    agent.SetDestination(finalPosition); /// all of these start pos and final pos maybe ad more for route 
                }
            }


            //check for player detection
        }

        bool HasReachedDestination()
        {
            
            return !agent.pathPending
                && agent.remainingDistance <= agent.stoppingDistance
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);

            //if it has no more path it finds a new place to move to 
            // maybe add looks and move on here 
        }
    }
    


    
}

