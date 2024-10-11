using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class FinalBossAnim : MonoBehaviour, IInteractable
    {
        [Header("Interactables")]

        [SerializeField] private string _prompt;
        public GameObject InteractionImagePrompt => null;

        public string InteractionPrompt => _prompt;



        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        [SerializeField] private float WaitTime = 2f;



        [Header("CameraShake")]
        [SerializeField] private float shakeIntensity = 5f;
        [SerializeField] private float shakeTime = 5f;

        [Header("MainCharacter")]

        public RBController mainScript;

        public GameObject mCharacter;
        public Transform orentation;
        public Transform SleepPoint;
    

        public GameObject SleepCam;
        public bool sleeping;


        [Header("BB")]

        public float speed = 0.5f;


        [Header("Animators")]
        public Animator AnimatorBBIntro;
        public Animator FourtySixAnims;

        public bool PlayingAnim;

        public bool movePlayer;
        private void Update()
        {
            if (movePlayer)
            {
                Movement();

            }

           
        }

        public void Movement()
        {
            mainScript.TurnOffMovement();

            FourtySixAnims.SetBool("isWalking", true);

          
            Debug.Log("Moving!");

            Vector3 destination = SleepPoint.transform.position;



            mCharacter.transform.position = orentation.transform.position;


            Vector3 newPos = Vector3.MoveTowards(orentation.transform.position, SleepPoint.transform.position, speed * Time.deltaTime);

            orentation.transform.position = newPos;



            float distance = Vector3.Distance(orentation.transform.position, destination);


            if (distance <= 0.0001) 
            {
                Debug.Log("sTOPMoving!");
                FourtySixAnims.SetBool("isWalking", false);
                FourtySixAnims.SetBool("isRunning", false);
                FourtySixAnims.SetBool("isSleeping", true);

                SleepCam.SetActive(true);

                StartCoroutine(SleepWait());

                movePlayer = false;
              
            }

      

            Vector3 directionToTarget = SleepPoint.transform.position - mCharacter.transform.position;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            mCharacter.transform.rotation = Quaternion.RotateTowards(mCharacter.transform.rotation, targetRotation, Time.deltaTime * 300f);
        }
        public bool Interact(Interactor interactor)
        {

            mainScript.TurnOffMovement();

            movePlayer = true;

    

            return true;

        }


        public IEnumerator WaitBetweenFadeInOut()
        {

            //mCharacter.SetActive(false);
            yield return new WaitForSeconds(WaitTime);



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

        private IEnumerator SleepWait()
        {
            yield return new WaitForSeconds(5f);
            PlayingAnim = true;


        }
    }
}


        
    


        

