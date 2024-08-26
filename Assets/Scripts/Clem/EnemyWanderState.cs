using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyWanderState : EnemyBaseState
    {
        private NavMeshAgent agent;
        private readonly WayPoints Wait;
       private readonly Vector3 startpoint;
        private readonly float wanderRadius;
        private PlayerDetector playerDetector;

        public bool isStopped;
        private Vector3 destination;
        public int currentWayPointIndex = 0;
        

        private List<Transform> wayPoints = new List<Transform>();

        public EnemyWanderState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator)
        {

            if (enemy == null)
            {
                Debug.LogError("Enemy is null");
                return;
            }

            this.agent = agent;
            this.startpoint = enemy.transform.position;
            this.wanderRadius = wanderRadius;

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


            Debug.Log("Enemy wander state initialized.");
            animator.CrossFade(WalkHash, crossFadeDuration);
        }

        //public void MoveToPoint(Vector3 destination)
        //{
          //  this.destination = destination;
          //  agent.isStopped = false;
          //  agent.enabled = true;
           // agent.SetDestination(destination);
        //}

        public void WalkingBB()
        {
            if (wayPoints.Count == 0) return;

            float distanceToWayPoint = Vector3.Distance(wayPoints[currentWayPointIndex].position, agent.transform.position);

            if (distanceToWayPoint <= 3f)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
                Debug.Log("Changing waypoint");

          
          
            }



            agent.SetDestination(wayPoints[currentWayPointIndex].position);
        }

      

        public override void Update()
        {
            Vector3 direction = agent.transform.TransformDirection(destination);

            float distance = Vector3.Distance(agent.transform.position, destination);

            //FOR THE SMOOTH MOVEMENT^^^

            if (enemy.CompareTag("EnemyBB"))
            {
                WalkingBB();
            }
         

            else if (!enemy.CompareTag("EnemyBB"))
            {
                WanderRandom();
            }


            //if (distance > 0.1)
            //{
            //    //LookToward(destination, distance);
            //    float distanceBasedSpeedModifier = 1.0f;
                
            
            //    Vector3 movement = agent.transform.forward * Time.deltaTime * distanceBasedSpeedModifier;
               // agent.Move(movement);
           // }
        }


        private void WanderRandom()
        {
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


        private IEnumerator WaitAtCheckpoint()
        {
            // Stop the enemys movement
            agent.isStopped = true;
           
            // Wait for seconds
            yield return new WaitForSeconds(2f);
            Debug.Log("waiting");

            Quaternion lookDir = Quaternion.LookRotation(wayPoints[currentWayPointIndex].position);


            // Resume the movement to the next waypoint
            agent.isStopped = false;
            currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Count;
            agent.SetDestination(wayPoints[currentWayPointIndex].position);
        }


        private bool HasReachedDestination()
        {
            return !agent.pathPending &&
                   agent.remainingDistance <= agent.stoppingDistance &&
                   (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }
    }
}
  

    


