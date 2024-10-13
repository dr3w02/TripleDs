﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class EnemyWanderState : EnemyBaseState
    {
        private NavMeshAgent agent;
       // private readonly WayPoints Wait;
       private readonly Vector3 startpoint;
        private readonly float wanderRadius;


        public bool isStopped;
        private Vector3 destination;
        public int currentWayPointIndex = 0;
        

        private List<Transform> wayPoints = new List<Transform>();

        public EnemyWanderState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator)
        {
            Debug.Log("Wander");
            if (enemy == null)
            {
                Debug.LogError("Enemy is null");
                return;
            }

            this.agent = agent;

            this.startpoint = enemy.transform.position;
            this.wanderRadius = wanderRadius;


            // Debug.Log("Enemy wander state initialized.");
            animator.CrossFade(WalkHash, crossFadeDuration);
        }


        public void Start()
        {

            Transform wayPointsObject = GameObject.FindGameObjectWithTag("Waypoint").transform;

            foreach (Transform t in wayPointsObject)
            {
                wayPoints.Add(t);
            }

            if (wayPoints.Count == 0)
            {
                Debug.LogError("No waypoints found!");
                return;
            }

        }

     

        public void WalkingBB()
        {
            if (wayPoints.Count == 0) return;

      
            float distanceToWayPoint = Vector3.Distance(wayPoints[currentWayPointIndex].position, agent.transform.position);

            if (!enemy.smashed)
            {
                agent.speed = enemy.speed;

                if (distanceToWayPoint <= 3f)
                {

                    currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
                    //Debug.Log("Changing waypoint");

                }

                agent.SetDestination(wayPoints[currentWayPointIndex].position);

                Vector3 directionToTarget = wayPoints[currentWayPointIndex].position - agent.transform.position;

                directionToTarget.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, targetRotation, Time.deltaTime * 300f);
            }

           
           //else if (enemy.smashed)
           //{
           //     heardANoise();
          // }
        }

        private IEnumerator heardANoise()
        {
            agent.SetDestination(enemy.transform.position);

            animator.SetBool("Looking",true);

            yield return new WaitForEndOfFrame();

            animator.SetBool("Looking", false);

            agent.speed = 6;

            agent.SetDestination(enemy.mCharacter.transform.position);
        }


        public override void Update()
        {
            

            if (enemy.CompareTag("EnemyBB"))
            {
                WalkingBB();
            }
         

            else if (!enemy.CompareTag("EnemyBB"))
            {
                WanderRandom();
            }


        
        }


        private void WanderRandom()
        {
            agent.speed = enemy.speed;

            if (HasReachedDestination())
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += startpoint;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
                var finalPosition = hit.position;
                agent.SetDestination(hit.position);
            }
        }


       // private IEnumerator WaitAtCheckpoint()
       // {
         //   Stop the enemys movement
         //   agent.isStopped = true;

         //   Wait for seconds
         //  yield return new WaitForSeconds(2f);
          //  Debug.Log("waiting");

         //   Quaternion lookDir = Quaternion.LookRotation(wayPoints[currentWayPointIndex].position);


         //   Resume the movement to the next waypoint
         //   agent.isStopped = false;
          //  currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
          //  agent.SetDestination(wayPoints[currentWayPointIndex].position);
        //}


        private bool HasReachedDestination()
        {
            return !agent.pathPending &&
                   agent.remainingDistance <= agent.stoppingDistance &&
                   (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }
    }
   
}


  

    


