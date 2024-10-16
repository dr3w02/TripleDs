using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class LamSpawn : MonoBehaviour
    {

        // List to store GameObjects
        public List<GameObject> gameObjects;

        public GameObject DeathCamBB;

        public Animator BlackBeakAnim;

        public NavMeshAgent agent;

        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;



        void Start()
        {
       
            ActivateRandomObject();

        }

        public void ActivateRandomObject()
        {
         
            if (gameObjects.Count == 0)
            {
                Debug.LogError("No GameObjects in list");
                return;
            }

         
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(false);
            }

            int randomIndex = Random.Range(0, gameObjects.Count);

     
            gameObjects[randomIndex].SetActive(true);


            gameObjects.RemoveAt(randomIndex);

            if (gameObjects.Count == 0)
            {
                agent.enabled = false;

                DeathCamBB.SetActive(true);

                StartCoroutine(WaitTime());

                BlackBeakAnim.SetBool("Die", true);

               
            }

        }

        public IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(3f);
        }

        public IEnumerator WaitBetweenFadeInOut()
        {

            //mCharacter.SetActive(false);
            yield return new WaitForSeconds(5f);



            Debug.Log("Switch Camera");

            fadeIn = true;

            while (fadeIn)
            {
                if (myUIGroup.alpha < 1)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    yield return null; // Wait for the next frame
                }
                else
                {
                    fadeIn = false;
                }
            }

            //RESET PLAYER POSTION AND TURN OFF SLEEP HERE


            SceneManager.LoadScene("EndCredits");

            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame

                    //GetComponent<NavMeshAgent>().isStopped = false;

                    //turn on bb moving here and turn off camera here   
                }
                else
                {
                    fadeOut = false;
                }
            }


        }
    }
}
