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
        public bool sleeping;
        public Animator mainAnim;


        [Header("BlackBeak")]

        public BlackBeackIntro bbIntro;



        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;

        [SerializeField] public CanvasGroup myUIGroup;

        [SerializeField] private bool fadeOut = true;
       
        [SerializeField] private float fadeWaitTime = 4f;


        [SerializeField] private float WaitTime = 0.5f;

        public void Start()
        {
            SleepCam.SetActive(false);
        }

        public bool Interact(Interactor interactor)
        {

            if (interactable == false)
            {


                sleeping = true;


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

            StartCoroutine(PanOut());

        }


        private IEnumerator PanOut()
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

          

            //mCharacter.SetActive(false);
            // Wait for the specified amount of time
            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame
                  
                    mCharacter.SetActive(true);
                    characterMain.Enabled();
                    //GetComponent<NavMeshAgent>().isStopped = false;

                    sleeping = false;
                    bbIntro.StartBlackBeakIntro();
                }
                else
                {
                    fadeOut = false;
                }
            }



        }
        public void Update()
        {
            if (sleeping == true)
            {
                mainAnim.SetBool("Sleeping", true);

                SleepCam.SetActive(true);

                StartCoroutine(SleepWait());
            }




        }
    }

     
      
}


  


    
   


