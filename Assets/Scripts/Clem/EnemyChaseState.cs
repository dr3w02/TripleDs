﻿using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer 
{

    public class EnemyChaseState: EnemyBaseState
    {

        readonly NavMeshAgent agent;
        readonly Transform player;
        
       
        public EnemyChaseState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            if (!enemy.attacking)
            {
                this.agent = agent;
                this.player = player;
            }
         
            
        }

        public override void OnEnter()
        {
            if (!enemy.attacking)
            {
               
                Debug.Log("Chase");


                animator.CrossFade(RunHash, crossFadeDuration);
            }
   
        }
       
        public override void Update()
        {
            //float distanceToWayPoint = Vector3.Distance(wayPoints[currentWayPointIndex].position, agent.transform.position);
            agent.speed = 7;
            agent.SetDestination(player.position);
        }
    }
}
    




