using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.InputSystem.Controls.AxisControl;

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

        public Door BossFightDoor;

        public bool ResetBossFight;
        public LamSpawn lampSpawn;

        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        [SerializeField] private float WaitTime = 2f;

        public bool ResetButtonbool;

        [Header("CameraShake")]
        public CinemachineVirtualCamera VirtualCam;
        public CinemachineVirtualCamera MainVirtualCam;
        private CinemachineBasicMultiChannelPerlin perlinNoise;
        private CinemachineBasicMultiChannelPerlin MainperlinNoise;


        [SerializeField] private float shakeIntensity = 1f;
       
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

        public Transform A;
        public Transform B;

        public BoxCollider BoxColliderForAnim;


        [Header("Animators")]
        public Animator AnimatorBBIntro;
        public Animator FourtySixAnims;

        public bool PlayingAnim;

        public bool movePlayer;

        public bool Reset;
       
        [Header("Audio")]
        public AudioSource bossMusic;


        public BoxCollider interactable1;
        public BoxCollider interactable2;
        public BoxCollider interactable3;

        public bool bossReset;

        public GameObject ResetButton;
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
            else if (!MainCamShake)
            {
                MainperlinNoise.m_AmplitudeGain = 0f;
            }


            if (CamShake)
            {
                ShakeCamera(shakeIntensity);
            }
            else if (!CamShake)
            {
                perlinNoise.m_AmplitudeGain = 0f;
            }


            if (PlayingAnim)
            {
                MoveToCheckpoint();
            }


     
            if(ResetButtonbool == true)
            {
                ResetButton.SetActive(false);
            }
            else if (ResetButtonbool == false) 
            {
                ResetButton.SetActive(true);
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


            if (distance <= 0.2f) 
            {
                Debug.Log("sTOPMoving!");
                FourtySixAnims.SetBool("isWalking", false);
                FourtySixAnims.SetBool("isRunning", false);
                FourtySixAnims.SetBool("isSleeping", true);

                SleepCam.SetActive(true);

                StartCoroutine(SleepWait());
               
                StartCoroutine(BossMusicStart());

                movePlayer = false;

               
            }
        }

        private IEnumerator BossMusicStart()
        {
            yield return new WaitForSeconds(5f);

            bossMusic.Play();
        }

        public bool Interact(Interactor interactor, GameObject player)
        {

            mainScript.TurnOffMovement();
            ResetButtonbool = true;
            movePlayer = true;

            return true;

        }

        private IEnumerator SleepWait()
        {
            StartCoroutine(WaitBetweenFadeInOut());

            yield return new WaitForSeconds(5f);

            PlayingAnim = true;

            BB.SetActive(true);
            Bosscam.SetActive(true);
            SleepCam.SetActive(false);

            BoxColliderForAnim.enabled = !BoxColliderForAnim.enabled;
            Debug.LogWarning("StartingAgain!!!");

            interactable1.enabled = !interactable1.enabled;
            interactable2.enabled = !interactable2.enabled;
            interactable3.enabled = !interactable3.enabled;

            // SleepScript.enabled = false;


        }

    



        private void MoveToCheckpoint()
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

        public void ShakeCamera(float intensity)
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

            StartCoroutine(ResetForFight());
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
            yield return new WaitForSeconds(fadeWaitTime);
            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame

                }
                else
                {
                    fadeOut = false;
                }
            }

            StopCoroutine(WaitBetweenFadeInOut());
        }

      

        public IEnumerator ResetForFight()
        {
            BossFight.transform.position = B.transform.position;
            bossReset = true;
            Debug.Log("RESET");
            StartCoroutine(WaitBetweenFadeInOut());
            yield return new WaitForSeconds(5);
            CamShake = false;
            MainCamShake = true;
            Bosscam.SetActive(false);
            BB.SetActive(false);


            BossFightDoor.m_ObjectCollider.isTrigger = false;
            BossFightDoor.doorisopen = false;
            //animator.SetBool(DoorOpenedHash, true);

            BossFight.SetActive(true);
            BossFight.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            Lamps.SetActive(true);
            FourtySixAnims.SetBool("isWakeUp", true);
            yield return new WaitForSeconds(3);
            FourtySixAnims.SetBool("isWakeUp", false);
            ResetButtonbool = false;
            FourtySixAnims.SetBool("isSleeping", false);

            mainScript.Enabled();



            BB.transform.position = A.transform.position;

        }


        public void FixedUpdate()
        {
           
        }


        public void ResetWholeBossFight()
        {
            BossFightDoor.m_ObjectCollider.isTrigger = true;
            BossFightDoor.doorisopen = true;
            //BossFight.SetActive(false);
           
            Reset = true;
            MainCamShake = false;

            mCharacter.SetActive(true);
            //BossFight.transform.position = B.transform.position;

            BoxColliderForAnim.enabled = !BoxColliderForAnim.enabled;

           
                Lamp.SetActive(true);
                Lamp1.SetActive(true);
                Lamp2.SetActive(true);
                Lamp3.SetActive(true);
                Lamp4.SetActive(true);
                Lamp5.SetActive(true);

            


            //lampSpawn.resetAllLamps = true;
            // Lamps.SetActive(false);
            //BossFight.SetActive(false);
            StartCoroutine(waitForLamps());


            bossMusic.Stop();
            interactable1.enabled = !interactable1.enabled;
            interactable2.enabled = !interactable2.enabled;
            interactable3.enabled = !interactable3.enabled;
            


        }

        public GameObject Lamp;
        public GameObject Lamp1;
        public GameObject Lamp2;
        public GameObject Lamp3;
        public GameObject Lamp4;
        public GameObject Lamp5;
    

        public IEnumerator waitForLamps()
        {
            yield return new WaitForSeconds(0.2f);
            //lampSpawn.ResetAllLamps();
            lampSpawn.resetAllLamps = true;
            Lamp.SetActive(false);
            Lamp1.SetActive(false);
            Lamp2.SetActive(false);
            Lamp3.SetActive(false);
            Lamp4.SetActive(false);
            Lamp5.SetActive(false);
         

            Lamps.SetActive(false);


            Reset = false;
            StopCoroutine(waitForLamps());

        }
       
    }
}


        
    


        

