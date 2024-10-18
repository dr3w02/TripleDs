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
        //private Vector3 destination;
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
            
        }

        public override void OnEnter()
        {
            Debug.Log("Chase");


            animator.CrossFade(WalkHash, crossFadeDuration);
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
           

            if (enemy.smashed)
            {

               heardANoise();
            }

        
     
        }

        public IEnumerator heardANoise()
        {
            
           // agent.GetComponent<NavMeshAgent>().isStopped = true;
            Debug.Log("Stoppedheardanoise");

            animator.SetBool("Looking",true);

            yield return new WaitForSeconds(3f);

            animator.SetBool("Looking", false);

            animator.CrossFade(WalkHash, crossFadeDuration);

            agent.SetDestination(enemy.mCharacter.transform.position);

            yield return new WaitForSeconds(3f);

            enemy.smashed = false;
            //agent.GetComponent<NavMeshAgent>().isStopped = false;
            Debug.Log("hearda noise started");
            agent.speed = 6;

            

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





        private bool HasReachedDestination()
        {
            return !agent.pathPending &&
                   agent.remainingDistance <= agent.stoppingDistance &&
                   (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }
    }
   
}


  

    


