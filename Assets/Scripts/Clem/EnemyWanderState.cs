using System.Collections;
using System.Collections.Generic;
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
        
        public GameObject isItBlackBeak;
        public bool isStopped;

        private Vector3 destination;
        

        public EnemyWanderState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator)
        {

            if (enemy == null)
            {
                Debug.LogError("Enemy is null in EnemyWanderState constructor.");
                return;
            }

            

            else
            {
                this.agent = agent;
                this.startpoint = enemy.transform.position;
                this.wanderRadius = wanderRadius;

                Debug.Log("enemie wander stre plays");

            }


        }

        public void MoveToPoint(Vector3 destination, NavMeshAgent agent)
        {
            this.destination = destination;
            agent.isStopped = false;
            agent.enabled = true;

            //debug = destination.ToString();

        }

        public override void OnEnter()
        {
            Debug.Log(message: "WANDER");
            animator.CrossFade(WalkHash, crossFadeDuration);

            //animator.CrossFade(WalkHash, crossFadeDuration);
        }


        public override void Update()
        {
           

            //FOR THE SMOOTH MOVEMENT
            Vector3 direction = agent.transform.DirectionTo(destination);

            float distance = Vector3.Distance(agent.transform.position, destination);
            //FOR THE SMOOTH MOVEMENT

            if (GameObject.FindWithTag("EnemyBB"))
            {

                Debug.Log("Reading WayPoints");
                enemy.WalkingBB();

                //Debug.Log("WayPoint list count: " + wayPoint.Count);
                //Debug.Log($"Current Waypoint Position: {wayPoint[currentWayPointIndex].position}");

            }


           else if (!GameObject.FindWithTag("EnemyBB"))
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

            Debug.Log("WayPoint list count: " + enemy.wayPoint.Count);
            //Debug.Log($"Current Waypoint Position: {wayPoint[currentWayPointIndex].position}");


            //check for player detection

            //make enemys turn alot smoother



            if (distance > 0.1)
            {
                LookToward(destination, distance);
                float distanceBasedSpeedModifier = 1.0f;
       

                Vector3 movement = agent.transform.forward * Time.deltaTime * distanceBasedSpeedModifier;
                agent.Move(movement);
            }
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

