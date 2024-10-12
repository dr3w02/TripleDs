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
        public CinemachineVirtualCamera VirtualCam;
        private CinemachineBasicMultiChannelPerlin perlinNoise;


        [SerializeField] private float shakeIntensity = 5f;
        [SerializeField] private float shakeTime = 5f;
        public bool CamShake;


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


        public GameObject Lamps;

        public Transform A;
        public Transform B;




        [Header("Animators")]
        public Animator AnimatorBBIntro;
        public Animator FourtySixAnims;

        public bool PlayingAnim;

        public bool movePlayer;

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
            }

        }

    

    private void Update()
        {
            if (movePlayer)
            {
                Movement();

            }
        

           
            
             ShakeCamera(shakeIntensity, shakeTime);
            


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

            //Vector3 destination = SleepPoint.transform.position;


            //Vector3 newPos = Vector3.MoveTowards(mCharacter.transform.position, SleepPoint.transform.position, speed * Time.deltaTime);


            //mCharacter.transform.position = newPos;


            //loat distance = Vector3.Distance(mCharacter.transform.position, destination);



            Vector3 destination = new Vector3(SleepPoint.transform.position.x, mCharacter.transform.position.y, SleepPoint.transform.position.z);

            Vector3 newPos = Vector3.MoveTowards(mCharacter.transform.position, destination, speed * Time.deltaTime);

            mCharacter.transform.position = newPos;

            // Now check the distance, but only for the X and Z axes (ignore Y)
            float distance = Vector3.Distance(new Vector3(mCharacter.transform.position.x, 0, mCharacter.transform.position.z),

                                              new Vector3(SleepPoint.transform.position.x, 0, SleepPoint.transform.position.z));




            if (distance <= 0.1f) 
            {
                Debug.Log("sTOPMoving!");
                FourtySixAnims.SetBool("isWalking", false);
                FourtySixAnims.SetBool("isRunning", false);
                FourtySixAnims.SetBool("isSleeping", true);

                SleepCam.SetActive(true);

                StartCoroutine(SleepWait());

                movePlayer = false;
              
            }

            //Debug.Log("Character Position: " + mCharacter.transform.position);
            //Debug.Log("SleepPoint Position: " + SleepPoint.transform.position);

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

        private IEnumerator SleepWait()
        {
            yield return new WaitForSeconds(5f);

            PlayingAnim = true;

            BB.SetActive(true);
            Bosscam.SetActive(true);
            SleepCam.SetActive(false);

            Debug.LogWarning("StartingAgain!!!");
            // SleepScript.enabled = false;



            PlayingAnim = true;

            StartCoroutine(MoveToCheckpoint());


           
          


        }

    


        public bool Reset;
        private IEnumerator MoveToCheckpoint()
        {
            if (PlayingAnim)
            {
                Vector3 destination = B.position;


                Vector3 newPos = Vector3.MoveTowards(BB.transform.position, destination, speed * Time.deltaTime);


                BB.transform.position = newPos;


                float distanceBB = Vector3.Distance(BB.transform.position, destination);


                Debug.Log("Character Position: " + BB.transform.position);
                Debug.Log("SleepPoint Position: " + destination);


                if (distanceBB == 0)
                {
                    PlayingAnim = false;

                    BlackBeakAnim.SetBool("walking", false);

                    BlackBeakAnim.SetBool("Evil", true);


                    CamShake = true;

                    yield return new WaitForEndOfFrame();


                    StartCoroutine(WaitBetweenFadeInOut());


                    Reset = true;
                
                }

                else
                {
                    BlackBeakAnim.SetBool("walking", true);

                }
            }
            
             

        }




        public void ShakeCamera(float intensity, float shakeTime)
        {


            if (CamShake == true)
            {
                 perlinNoise.m_AmplitudeGain = intensity;
            }



            else if (CamShake == false)
            {
                perlinNoise.m_AmplitudeGain = 0f;
            }


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
           
            BB.SetActive(false);
            Bosscam.SetActive(false);
            BossFight.SetActive(true);
            Lamps.SetActive(true);

            FourtySixAnims.SetBool("isWakeUp", true);

            yield return new WaitForSeconds(3);

            FourtySixAnims.SetBool("isWakeUp", false);

            FourtySixAnims.SetBool("isSleeping", false);

            mainScript.Enabled();

        }

    }
}


        
    


        

