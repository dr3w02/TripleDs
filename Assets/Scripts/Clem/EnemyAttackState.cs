using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using Unity.VisualScripting;

namespace Platformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;

         


        public EnemyAttackState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
            
            enemy.clemDeathCam.SetActive(false);
            enemy.BlackBeackDeathCam.SetActive(false);
          
        }

        public override void OnEnter()
        {

            //GameObject clemDeathCam = GameObject.FindGameObjectWithTag("EnemyClemCam");
            //Debug.Log("Found GameObject with tag 'EnemyClemCam': " + clemDeathCam.name);
            Debug.Log("Attackingg");

            //if (GameObject.FindWithTag("EnemyBB"))
            //{

            //nursecodeoffice.BlackBeakDeath.Priority = 200;
            // }

       

            if (enemy.CompareTag("EnemyBB"))
            {
                //enemy.pausePlayer.TurnOffMovement();
                enemy.BlackBeackDeathCam.SetActive(true);
                Debug.Log("Switch Camera");// Use the stored reference to set active
                animator.CrossFade(AttackHash, crossFadeDuration);
              
            }

            else if (enemy.CompareTag("EnemyClem"))
            {
                //enemy.pausePlayer.TurnOffMovement();
                enemy.clemDeathCam.SetActive(true);
                Debug.Log("Switch Camera");// Use the stored reference to set active
                animator.CrossFade(AttackHash, crossFadeDuration);
                
            }
           
            
        }

        public override void Update()
        {
            agent.SetDestination(player.position);
            enemy.Attack();
        }
    }
}




