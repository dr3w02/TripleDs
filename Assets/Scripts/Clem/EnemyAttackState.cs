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
        [SerializeField] private float fadeWaitTime = 4f;
        [SerializeField] private float clemWaitTime = 3f;
        [SerializeField] private float BBWaitTime = 3f;

        

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
                enemy.BlackBeackKillCam.SetActive(true);

            

                enemy.StartCoroutine(WaitBetweenFadeInOutClem());


                Debug.Log("Switch Camera");// Use the stored reference to set active



            }

            else if (enemy.CompareTag("EnemyClem"))
            {

                agent.GetComponent<NavMeshAgent>().isStopped = true;
                enemy.clemDeathCam.SetActive(true);



                enemy.StartCoroutine(WaitBetweenFadeInOutClem());



            }


        }
   
        public IEnumerable WaitBetweenFadeInOutBB()
        {
            
                yield return new WaitForSeconds(BBWaitTime);

                enemy.mCharacter.SetActive(false);
          

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


         
                //Debug.Log("RespawnPlayer");
               // // Wait for the specified amount of time
               // yield return new WaitForSeconds(fadeWaitTime);
                //enemy.mCharacter.SetActive(true);

               // enemy.BlackBeackKillCam.SetActive(false);
                //enemy.clemDeathCam.SetActive(false);


                fadeOut = true;
                while (fadeOut)
                {
                    if (enemy.myUIGroup.alpha > 0)
                    {
                        enemy.myUIGroup.alpha -= Time.deltaTime;
                        yield return null;  // Wait for the next frame
                        enemy.BlackBeackDeathCam.SetActive(false);
                        enemy.mCharacter.SetActive(true);
                        enemy.characterMain.Enabled();
                        agent.GetComponent<NavMeshAgent>().isStopped = false;
                }
                    else
                    {
                        fadeOut = false;
                    }
                }


            
        }
    
        public IEnumerator WaitBetweenFadeInOutClem()
        {
            
                yield return new WaitForSeconds(clemWaitTime);

                enemy.mCharacter.SetActive(false);
               

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

            // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);

                fadeOut = true;
                while (fadeOut)
                {
                    if (enemy.myUIGroup.alpha > 0)
                    {
                        enemy.myUIGroup.alpha -= Time.deltaTime;
                        yield return null;  // Wait for the next frame
                        enemy.clemDeathCam.SetActive(false);
              
                        enemy.mCharacter.SetActive(true);
                        enemy.characterMain.Enabled();
                        agent.GetComponent<NavMeshAgent>().isStopped = false;
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




