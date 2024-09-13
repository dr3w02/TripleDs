using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class StartBossFight : MonoBehaviour, IInteractable
    {
        [Header("Interactables")]

        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;
        public bool interactable;

        [SerializeField] CameraManager cameraManager;
        

        [Header("MainCharacter")]

        public GameObject mCharacter;/// <summary>
        public characterMovement characterMain;
        characterMovement characterMovement;

        public GameObject SleepCam;
        public bool Sleeping;
        public Animator mainAnim;




        [Header("BlackBeak")]

        public Animator BlackBeakAnim;
        public GameObject Bosscam;
        public GameObject BB;

    


        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        [SerializeField] private float WaitTime = 0.5f;
        [SerializeField] private float WaitTimeBB = 2f;

        void Start()
        {

            Bosscam.SetActive(false);
            SleepCam.SetActive(false);
        }

        public bool Interact(Interactor interactor)
        {

            if (interactable == false)
            {
                mainAnim.SetBool("Sleeping", true);
                SleepCam.SetActive(true);

                StartCoroutine(SleepWait());

                Sleeping = true;
                BB.SetActive(false);
                characterMain.TurnOffMovement();


            }

            else
            {
                SleepCam.SetActive(false);
              
                interactable = false;
            }

            return true;


        }

        private IEnumerator SleepWait()
        {
            yield return new WaitForSeconds(5f);

        }
        private IEnumerator PanBack()
        {

          
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

            //characterMain.RespawnPlayer();
            //mCharacter.SetActive(false);
            // Wait for the specified amount of time
            BB.SetActive(true);

            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame
                    Bosscam.SetActive(false);
                    mCharacter.SetActive(true);
                    characterMain.Enabled();

                    mainAnim.SetBool("Sleeping", true);

                    //GetComponent<NavMeshAgent>().isStopped = false;
                }
                else
                {
                    fadeOut = false;
                }
            }

        }
        public void Update()
        {
            if(Sleeping == true)
            {
                BossIntro();
            }

         
         

        }

     
        private IEnumerator BossIntro()
        {
            Bosscam.SetActive(true);

            BlackBeakAnim.SetBool("walking", true);
            BlackBeakAnim.SetBool("Evil", false);

            yield return new WaitForSeconds(WaitTimeBB);

            BlackBeakAnim.SetBool("walking", false);

            BlackBeakAnim.SetBool("Evil", true);

            
            yield return new WaitForSeconds(WaitTime);


            PanBack();

        }
    }
}


  


    
   


