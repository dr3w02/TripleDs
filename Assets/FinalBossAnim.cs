using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class FinalBossAnim : MonoBehaviour, IInteractable
    {
        [Header("Interactables")]

            [SerializeField] RBController characterMove;
            public string InteractionPrompt => "LookAtBook";

            public GameObject InteractionImagePrompt => BookPromptCanvas;

            //public GameObject character;
            public GameObject BookCam;
            public GameObject BookPromptCanvas;
            public GameObject Player;
            public bool isZoomedIn;



            [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        [SerializeField] private float WaitTime = 2f;
      

        [Header("CameraShake")]
        public CinemachineVirtualCamera VirtualCam;
        public CinemachineVirtualCamera MainVirtualCam;
        private CinemachineBasicMultiChannelPerlin perlinNoise;
        private CinemachineBasicMultiChannelPerlin MainperlinNoise;


        [SerializeField] private float shakeIntensity = 2f;
        [SerializeField] private float shakeTime = 5f;
        public bool CamShake;

        private bool MainCamShake;

        [Header("MainCharacter")]

        public RBController mainScript;

        public GameObject mCharacter;

        public Transform orentation;
        public Transform SleepPoint;
    

        public GameObject SleepCam;
        public bool sleeping;

        public float speed = 2f;

        [Header("BB")]

        public Animator BlackBeakAnim;
        public GameObject Bosscam;
        public GameObject BB;
        public GameObject BossFight;
        public float speedBB = 3f;

        public GameObject Lamps;

      
        public Transform B;




        [Header("Animators")]
        public Animator AnimatorBBIntro;
        public Animator FourtySixAnims;

        public bool PlayingAnim;

        public bool movePlayer;

        public bool Reset;

        public void Start()
        {

            BossFight.SetActive(false);
            Lamps.SetActive(false);
            Bosscam.SetActive(false);
            BB.SetActive(false);


            CamShake = false;

            if (VirtualCam != null)
            {
                perlinNoise = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                MainperlinNoise = MainVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

       
            }

        }

   
    private void Update()
        {
            if (movePlayer)
            {
                Movement();

            }


            if (MainCamShake)
            {
                MainShakeCamera(shakeIntensity);
            }
          



            if (CamShake)
            {
                ShakeCamera(shakeIntensity, shakeTime);
            }
            else
            {
                perlinNoise.m_AmplitudeGain = 0f;
            }

            if (PlayingAnim)
            {
                MoveToCheckpoint();
            }

            if (Reset)
            {
               StartCoroutine(ResetForFight());
            }


        }

        public void Movement()
        {
            mainScript.TurnOffMovement();

            FourtySixAnims.SetBool("isWalking", true);

          
            Debug.Log("Moving!");

            Vector3 destination = new Vector3(SleepPoint.transform.position.x, mCharacter.transform.position.y, SleepPoint.transform.position.z);

            Vector3 newPos = Vector3.MoveTowards(mCharacter.transform.position, destination, speed * Time.deltaTime);

            mCharacter.transform.position = newPos;

         
            float distance = Vector3.Distance(new Vector3(mCharacter.transform.position.x, 0, mCharacter.transform.position.z), new Vector3(SleepPoint.transform.position.x, 0, SleepPoint.transform.position.z));

            //comparing the distance between without using the y axis 


            Vector3 directionToTarget = SleepPoint.transform.position - mCharacter.transform.position;
            directionToTarget.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            mCharacter.transform.rotation = Quaternion.RotateTowards(mCharacter.transform.rotation, targetRotation, Time.deltaTime * 300f);


            if (distance <= 0.1f) 
            {
                Debug.Log("sTOPMoving!");
                FourtySixAnims.SetBool("isWalking", false);
                FourtySixAnims.SetBool("isRunning", false);
                FourtySixAnims.SetBool("isSleeping", true);

                SleepCam.SetActive(true);

                StartCoroutine(SleepWait());
                StartCoroutine(WaitBetweenFadeInOut());

                movePlayer = false;

               
            }



        }

        public bool Interact(Interactor interactor)
        {

            mainScript.TurnOffMovement();

            movePlayer = true;

    

            return true;

        }

        private IEnumerator SleepWait()
        {
            yield return new WaitForSeconds(5f);

            PlayingAnim = true;

            BB.SetActive(true);
            Bosscam.SetActive(true);
            SleepCam.SetActive(false);

            Debug.LogWarning("StartingAgain!!!");
            // SleepScript.enabled = false;


        }

    



        private void MoveToCheckpoint()
        {
           
            if (PlayingAnim)
            {

                Debug.Log("bbMove");
                Vector3 destination = B.position;


                Vector3 newPos = Vector3.MoveTowards(BB.transform.position, destination, speedBB * Time.deltaTime);


                BB.transform.position = newPos;


                float distanceBB = Vector3.Distance(BB.transform.position, destination);


              

               

                if (distanceBB == 0)
                {
                    Debug.Log("Made it");
                    PlayingAnim = false;

                    BlackBeakAnim.SetBool("walking", false);

                 

                    BlackBeakAnim.SetBool("Evil", true);



                    StartCoroutine(WaitForAnimationPlayShakeCam());

                    MainCamShake = true;

                


               

                }

                else
                {
                    BlackBeakAnim.SetBool("walking", true);

                }
            }
            
             

        }




        public void ShakeCamera(float intensity, float shakeTime)
        {

          perlinNoise.m_AmplitudeGain = intensity;
            

        }
        public void MainShakeCamera(float intensity)
        {

            MainperlinNoise.m_AmplitudeGain = intensity;


        }

        public IEnumerator WaitForAnimationPlayShakeCam()
        {
            yield return new WaitForSeconds(2f);

            CamShake = true;

            yield return new WaitForSeconds(3f);

            Reset = true;

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



        public IEnumerator ResetForFight()
        {
            
            StartCoroutine(WaitBetweenFadeInOut());


            yield return new WaitForSeconds(4);

            BB.SetActive(false);
            Bosscam.SetActive(false);
            BossFight.SetActive(true);
            Lamps.SetActive(true);

            FourtySixAnims.SetBool("isWakeUp", true);

            yield return new WaitForSeconds(3);

            FourtySixAnims.SetBool("isWakeUp", false);

            FourtySixAnims.SetBool("isSleeping", false);

            mainScript.Enabled();

            Reset = false;

 


        }

    }
}


        
    


        

