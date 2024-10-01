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
        public GameObject InteractionImagePrompt => null;
        public string InteractionPrompt => _prompt;
        public bool interactable;

        [SerializeField] CameraManager cameraManager;


        [Header("MainCharacter")]


        public Transform SleepPoint;


        public GameObject mCharacter;/// <summary>
        public RBController characterMain;
       

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

        public float speed = 0.5f;



        public void Start()
        {
            SleepCam.SetActive(false);
        }

        public bool Interact(Interactor interactor)
        {

            if (interactable == false)
            {
                characterMain.TurnOffMovement();

                mainAnim.SetBool("isWalking", true);

                mCharacter.transform.position = Vector3.MoveTowards(mCharacter.transform.position, SleepPoint.transform.position, speed * Time.deltaTime);

                if (Vector3.Distance(mCharacter.transform.position, SleepPoint.transform.position) == 0f)
                {
                    mainAnim.SetBool("isWalking", false);
                    mainAnim.SetBool("Sleeping", true);

                    SleepCam.SetActive(true);

                    StartCoroutine(SleepWait());

                    StartCoroutine(PanOut());
                }
               

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
            




        }
    }

     
      
}


  


    
   


