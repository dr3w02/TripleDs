using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.Controls.AxisControl;

namespace Platformer
{
    public class LamSpawn : MonoBehaviour
    {

        // List to store GameObjects
        // List<GameObject> gameObjects;
        public List<GameObject> Lamps = new List<GameObject>();

        public GameObject DeathCamBB;

        public Animator BlackBeakAnim;
        public NurseCodeOffice agent;
       

        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        public bool resetAllLamps;

        public GameObject Lamp1;
        public GameObject Lamp2;
        public GameObject Lamp3;
        public GameObject Lamp4;
        public GameObject Lamp5;
        public GameObject Lamp6;
        public GameObject Lamp7;
        public GameObject Lamp8;
        public GameObject Lamp9;
        public GameObject Lamp10;

        public bool gameOver;

        void Start()
        {
            resetAllLamps = true;

         
        }

        
        public void ActivateRandomObject()
        {
         
           
         
            foreach (GameObject obj in Lamps)
            {
                obj.SetActive(false);
            }

            int randomIndex = Random.Range(0, Lamps.Count);


            Lamps[randomIndex].SetActive(true);


            Lamps.RemoveAt(randomIndex);

            if (Lamps.Count == 0)
            {

                agent.GetComponent<NavMeshAgent>().isStopped = true;
                DeathCamBB.SetActive(true);

                BlackBeakAnim.SetBool("Die", true);

                StartCoroutine(WaitBetweenFadeInOut());

                gameOver = true;



            }

        }
        private void Update()
        {
            if (resetAllLamps)
            {
                ResetAllLamps(Lamp1, Lamp2, Lamp3, Lamp4, Lamp5, Lamp6, Lamp7, Lamp8, Lamp9, Lamp10);
            }
            else
            {
                return;
            }

            if (!resetAllLamps)
            {
                ActivateRandomObject();
            }
        }


        public void ResetAllLamps(GameObject Lamp1, GameObject Lamp2, GameObject Lamp3, GameObject Lamp4, GameObject Lamp5, GameObject Lamp6, GameObject Lamp7, GameObject Lamp8, GameObject Lamp9, GameObject Lamp10)
        {

            Lamps.Remove(Lamp1);
            Lamps.Remove(Lamp2);
            Lamps.Remove(Lamp3);
            Lamps.Remove(Lamp4);
            Lamps.Remove(Lamp5);
            Lamps.Remove(Lamp6);
            Lamps.Remove(Lamp7);
            Lamps.Remove(Lamp8);
            Lamps.Remove(Lamp9);
            Lamps.Remove(Lamp10);
                    
                Lamps.Add(Lamp1);
                Lamps.Add(Lamp2);
                Lamps.Add(Lamp3);
                Lamps.Add(Lamp4);
                Lamps.Add(Lamp5);
                Lamps.Add(Lamp6);
                Lamps.Add(Lamp7);
                Lamps.Add(Lamp8);
                Lamps.Add(Lamp9);
                Lamps.Add(Lamp10);

                resetAllLamps = false;
            
        }
     
        public IEnumerator WaitBetweenFadeInOut()
        {

            //mCharacter.SetActive(false);
            yield return new WaitForSeconds(10f);
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

            if (gameOver)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

                gameOver = false;
            }
            else
            {
                yield return new WaitForSeconds(fadeWaitTime);

                fadeOut = true;
                while (fadeOut)
                {
                    if (myUIGroup.alpha > 0)
                    {
                        myUIGroup.alpha -= Time.deltaTime;
                        yield return null; // Wait for the next frame
                        agent.GetComponent<NavMeshAgent>().isStopped = false;
                        //GetComponent<NavMeshAgent>().isStopped = false;
                        Debug.Log("Startedlamspawn");

                        //turn on bb moving here and turn off camera here
                        BlackBeakAnim.SetBool("Die", false);
                    }
                    else
                    {
                        fadeOut = false;
                    }
                }
            
            }


        }
    }
}
