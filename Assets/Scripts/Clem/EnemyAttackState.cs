using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Platformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;


        [SerializeField] private float fadeWaitTime = 4f;
        [SerializeField] private float clemWaitTime = 3f;
        [SerializeField] private float BBWaitTime = 2f;

        

        public EnemyAttackState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;


        }

       
        public override void OnEnter()
        {
            enemy.attacking = true;
            //GameObject clemDeathCam = GameObject.FindGameObjectWithTag("EnemyClemCam");
            //Debug.Log("Found GameObject with tag 'EnemyClemCam': " + clemDeathCam.name);
            Debug.Log("Attackin in State");

            //if (GameObject.FindWithTag("EnemyBB"))
            //{

            //nursecodeoffice.BlackBeakDeath.Priority = 200;
            // }

            enemy.characterMain.TurnOffMovement();

 
            if (enemy.CompareTag("EnemyBB"))
            {
                agent.GetComponent<NavMeshAgent>().isStopped = true;
                Debug.Log("attackstatebbStopped");


                enemy.BlackBeackKillCam.SetActive(true);

                animator.CrossFade(AttackHash, crossFadeDuration);
                
                enemy.StartCoroutine(WaitBetweenFadeInOutClem(enemy.BlackBeackKillCam));
                Debug.Log("Switch Camera");// Use the stored reference to set active

            }

            else if (enemy.CompareTag("EnemyClem"))
            {

                agent.GetComponent<NavMeshAgent>().isStopped = true;
                Debug.Log("attackstateclemStopped");
                enemy.clemDeathCam.SetActive(true);
                animator.CrossFade(AttackHash, crossFadeDuration);
                //enemy.mChild.SetActive(false);
                enemy.StartCoroutine(WaitBetweenFadeInOutClem(enemy.clemDeathCam));

            }


        }
   
   
        public IEnumerator WaitBetweenFadeInOutClem(GameObject cam)
        {
            yield return new WaitForSeconds(1f);
            enemy.mChild.SetActive(false);
            yield return new WaitForSeconds(clemWaitTime);

               
                //agent.GetComponent<NavMeshAgent>().isStopped = true;

                enemy.fadeIn = true;
                while (enemy.fadeIn)
                {
                    if (enemy.myUIGroup.alpha < 1)
                    {
                        enemy.myUIGroup.alpha += Time.deltaTime;
                        yield return null;  // Wait for the next frame

                    }
                    else
                    {
                        enemy.fadeIn = false;
                    }
                }

           
            agent.GetComponent<NavMeshAgent>().isStopped = false;
           
            //cam.SetActive(false);
            enemy.clemDeathCam.SetActive(false);
            enemy.BlackBeackKillCam.SetActive(false);

         
            enemy.BBChild.SetActive(false);

            // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);
            enemy.respawn.RespawnPlayer();

            
            enemy.attacking = false;
            //enemy.mCharacter.SetActive(true);
            enemy.mChild.SetActive(true);

            enemy.fadeOut = true;

            

            while (enemy.fadeOut)
            {
                if (enemy.myUIGroup.alpha > 0)
                {
                    enemy.myUIGroup.alpha -= Time.deltaTime;
                    yield return null;

                    // Wait for the next frame
                    
                    
                }
               
                else
                {
                 

                    enemy.fadeOut = false;

                    enemy.BB.gameObject.SetActive(false);

                }
            }
         

            enemy.characterMain.Enabled();
            

            Debug.Log("Stoppedclemstarted");
        }
    



        public override void Update()
        {
            agent.SetDestination(player.position);
            enemy.Attack();
        }

        public void ShowUI()
        {
            enemy.fadeIn = true;
        }

        public void HideUI()
        {
            enemy.fadeOut = true;
        }
    }
}




