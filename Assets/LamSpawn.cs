using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class LamSpawn : MonoBehaviour
    {

        // List to store GameObjects
        public List<GameObject> gameObjects;
        public GameObject BB;
        public GameObject DeathCamBB;

        public Animator BlackBeakAnim;

        public NavMeshAgent agent;

        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;

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

                BlackBeakAnim.SetBool("Die", true);

                StartCoroutine(FadeIn());
            }

        }

        private IEnumerator FadeIn()
        {


            Debug.Log("Switch Camera");

            fadeIn = true;

            while (fadeIn)
            {
                if (myUIGroup.alpha < 1)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    //yield return null; // Wait for the next frame
                }
                else
                {
                    fadeIn = false;
                }
            }

            //mCharacter.SetActive(false);

            yield return new WaitForSeconds(fadeWaitTime);

          //Switch scenes here to you win /credits!!!!!!!!!!!!!!!!!!!!!!!!!
            

        }
    }
}
