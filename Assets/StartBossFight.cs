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

        BlackBeackIntro bbIntro;







        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        [SerializeField] private float WaitTime = 0.5f;
        [SerializeField] private float WaitTimeBB = 2f;



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


            bbIntro.BBPlays();


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


  


    
   


