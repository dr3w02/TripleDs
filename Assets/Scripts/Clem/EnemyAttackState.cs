using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;

namespace Platformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] private float fadeWaitTime = 3f;
        [SerializeField] private float clemWaitTime = 3f;
        [SerializeField] private float BBWaitTime = 2f;

        

        public EnemyAttackState(NurseCodeOffice enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;


        }

       
        public override void OnEnter()
        {

            //GameObject clemDeathCam = GameObject.FindGameObjectWithTag("EnemyClemCam");
            //Debug.Log("Found GameObject with tag 'EnemyClemCam': " + clemDeathCam.name);
            Debug.Log("Attackin in State");

            //if (GameObject.FindWithTag("EnemyBB"))
            //{

            //nursecodeoffice.BlackBeakDeath.Priority = 200;
            // }

            enemy.characterMain.TurnOffMovement();

            animator.CrossFade(AttackHash, crossFadeDuration);

            if (enemy.CompareTag("EnemyBB"))
            {
                agent.GetComponent<NavMeshAgent>().isStopped = true;
                Debug.Log("attackstatebbStopped");


                enemy.BlackBeackKillCam.SetActive(true);
                enemy.StartCoroutine(WaitBetweenFadeInOutBB());
                Debug.Log("Switch Camera");// Use the stored reference to set active

            }

            else if (enemy.CompareTag("EnemyClem"))
            {

               agent.GetComponent<NavMeshAgent>().isStopped = true;
                Debug.Log("attackstateclemStopped");
                enemy.clemDeathCam.SetActive(true);
                enemy.StartCoroutine(WaitBetweenFadeInOutClem());

            }


        }
   
        public IEnumerator WaitBetweenFadeInOutBB()
        {
           

            yield return new WaitForSeconds(2f);
           
            yield return new WaitForSeconds(BBWaitTime);

            Debug.Log("Switch Camera");// Use the stored reference to set active

                fadeIn = true;
                while (fadeIn)
                {
                    if (enemy.myUIGroup.alpha < 1)
                    {
                        enemy.myUIGroup.alpha += Time.deltaTime;
                        yield return null;  // Wait for the next frame
                        enemy.mCharacter.SetActive(true);
                        enemy.respawn.RespawnPlayer();
                }
                    else
                    {
                        fadeIn = false;
                    }
                }

            enemy.mCharacter.SetActive(false);
            //Debug.Log("RespawnPlayer");
            // // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);
         
            enemy.BlackBeackKillCam.SetActive(false);
            enemy.mCharacter.SetActive(true);

            enemy.ResetBossFight.ResetWholeBossFight();

            fadeOut = true;
                while (fadeOut)
                {
                    if (enemy.myUIGroup.alpha > 0)
                    {
                        enemy.myUIGroup.alpha -= Time.deltaTime;
                        yield return null;  // Wait for the next frame

                        enemy.characterMain.Enabled();
                        agent.GetComponent<NavMeshAgent>().isStopped = false;
                        Debug.Log("Stoppedattackstate bb respawn");

                }
                    else
                    {
                        fadeOut = false;
                    }

                  
                }



        }
    
        public IEnumerator WaitBetweenFadeInOutClem()
        {
                yield return new WaitForSeconds(2f);
                enemy.mCharacter.SetActive(false);
                yield return new WaitForSeconds(clemWaitTime);

              
               

                Debug.Log("Switch Camera");// Use the stored reference to set active

                fadeIn = true;
                while (fadeIn)
                {
                    if (enemy.myUIGroup.alpha < 1)
                    {
                        enemy.myUIGroup.alpha += Time.deltaTime;
                        yield return null;  // Wait for the next frame

                    }
                    else
                    {
                        fadeIn = false;
                    }
                }



            enemy.respawn.RespawnPlayer();
            enemy.clemDeathCam.SetActive(false);

            enemy.mCharacter.SetActive(true);
           
            // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);

                fadeOut = true;
                while (fadeOut)
                {
                    if (enemy.myUIGroup.alpha > 0)
                    {
                        enemy.myUIGroup.alpha -= Time.deltaTime;
                        yield return null;  // Wait for the next frame
                        enemy.characterMain.Enabled();
                        agent.GetComponent<NavMeshAgent>().isStopped = false;
                    Debug.Log("Stoppedclemstarted");
                }
                    else
                    {
                        fadeOut = false;
                    }
                }


            

        }
    



        public override void Update()
        {
            agent.SetDestination(player.position);
            enemy.Attack();
        }

        public void ShowUI()
        {
            fadeIn = true;
        }

        public void HideUI()
        {
            fadeOut = true;
        }
    }
}




