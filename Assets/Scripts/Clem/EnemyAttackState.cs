using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

namespace Platformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;

        NurseCodeOffice nurseCodeOffice;
        public EnemyAttackState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, Transform player): base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Attack");
            animator.CrossFade(AttackHash, crossFadeDuration);

            if (GameObject.FindWithTag("EnemyBB"))
            {
                nurseCodeOffice.BlackBeakDeath.Priority = 20;
            }
        }
        public override void Update()
        {
            
          agent.SetDestination(player.position);
          enemy.Attack();
            
        }
    }
}
    



