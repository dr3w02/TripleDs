using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
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
        public FinalBossAnim reset;
       
        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        public bool resetAllLamps;
     
        public GameObject bb;
        public GameObject DeathPoint;

        public bool AnimationPlay;

        public GameObject Lamp1;
        public GameObject Lamp2;
        public GameObject Lamp3;
        public GameObject Lamp4;
        public GameObject Lamp5;
        public GameObject Lamp6;

        public GameObject player;

        public bool gameOver;

        public GameObject Enemy;
        public List<GameObject> KeysList;
        

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


            if(Lamps.Count > 0)
            {

                Lamps[randomIndex].SetActive(true);
                Lamps.RemoveAt(randomIndex);
            }

            //Lamps[randomIndex].SetActive(true);


            //Lamps.RemoveAt(randomIndex);
            
         

            if (Lamps.Count == 0)
            {
                // LampLast = true;


               
                StartCoroutine(WaitBetweenFadeInOut());

                //StartCoroutine(waitTime());

            }
          
           

        }

        //public void ResetAllLamps()
       // {
        //    ResetAllLamps(Lamp1, Lamp2, Lamp3, Lamp4, Lamp5, Lamp6);


       // }
        
        public IEnumerator waitTime()
        {

            Lamp1.SetActive(false);
            Lamp2.SetActive(false);
            Lamp3.SetActive(false);
            Lamp4.SetActive(false);
            Lamp5.SetActive(false);
            Lamp6.SetActive(false);


            Debug.Log("lampcount0");
            Debug.Log("Animation Death Playingg");
            bb.transform.position = DeathPoint.transform.position;
            bb.SetActive(true);
            Enemy.SetActive(false);
            player.SetActive(false);
            BlackBeakAnim.SetBool("Dying", true);
            yield return new WaitForSeconds(5f);
            
           
            Debug.Log("Animation Death Play");

            StartCoroutine(WaitBetweenFadeInOut());
            gameOver = true;


        }
        private void Update()
        {


            if (resetAllLamps)
            {
                ResetAllLamps(Lamp1, Lamp2, Lamp3, Lamp4, Lamp5, Lamp6);

               
            }
            else
            {
                return;
            }
            if (!resetAllLamps)
            {
                ActivateRandomObject();
            }

            if (AnimationPlay)
            {
                
            }

       
         

       
       
           
        }
        

        
        public void ResetAllLamps(GameObject Lamp1, GameObject Lamp2, GameObject Lamp3, GameObject Lamp4, GameObject Lamp5, GameObject Lamp6)
        {
            
             
            Lamps.Remove(Lamp1);
            Lamps.Remove(Lamp2);
            Lamps.Remove(Lamp3);
            Lamps.Remove(Lamp4);
            Lamps.Remove(Lamp5);
            Lamps.Remove(Lamp6);
      
                    
                Lamps.Add(Lamp1);
                Lamps.Add(Lamp2);
                Lamps.Add(Lamp3);
                Lamps.Add(Lamp4);
                Lamps.Add(Lamp5);
                Lamps.Add(Lamp6);

         
            resetAllLamps = false;
            

        }

        public GameObject startingPoint;

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
                    DeathCamBB.SetActive(true);
                }
            }
          

            //RESET PLAYER POSTION AND TURN OFF SLEEP HERE

            if (gameOver)
            {

                reset.bossReset = false;


                DeathCamBB.SetActive(false);
                player.SetActive(false);
                BlackBeakAnim.SetBool("Die", false);
                


                gameOver = false;


                SceneManager.LoadScene(2);

            }
            else
            {
               
                yield return new WaitForSeconds(fadeWaitTime);

                fadeOut = true;
                while (fadeOut)
                {
                    if (myUIGroup.alpha > 0)
                    {
                        bb.transform.position = DeathPoint.transform.position;
                        myUIGroup.alpha -= Time.deltaTime;
                        yield return null; // Wait for the next frame
                        
                        //GetComponent<NavMeshAgent>().isStopped = false;
                        Debug.Log("Startedlamspawn");
                        StartCoroutine(waitTime());
                       
                        //turn on bb moving here and turn off camera here
                        //BlackBeakAnim.SetBool("Die", false);
                    }
                    else
                    {
                        fadeOut = false;
                      
                        StopCoroutine(WaitBetweenFadeInOut());
                    }
                }

              

            }

             
    }
    }
}
