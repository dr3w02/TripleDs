using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class BrakeTheVent : MonoBehaviour
    {

        public Animator VentAnim;

        public GameObject VentFixed;

        public GameObject VentBroken;

        public GameObject mCharacter;


        public GameObject PlayerDead;

        public ScrewDriver screw;

        public Respawn respawn;

        public Collider ventOutskirts;

        public Collider VentColliderTrigger;
       


        [SerializeField] private float WaitTime = 20f;

        // Handle the fade out and in
        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;


        public characterMovement characterMain;

       
        public GameObject VentDeathCam;

        public AudioSource ventFan;
        public AudioSource playerVentDeath;
        public AudioSource ventBreak1;
        public AudioSource ventBreak2;

        private bool hasVentBreak1Played = false;
        private bool hasVentBreak2Played = false;

        private void Start()
        {
            VentFixed.SetActive(true);
            VentAnim.SetBool("VentSpin", true);
          
            VentAnim.SetBool("Dead", false);

            ventOutskirts.isTrigger = false;
            
            VentBroken.SetActive(false);
            PlayerDead.SetActive(false);
            VentDeathCam.SetActive(false);

           // ventOutskirts = GetComponent<Collider>();


        }

        private void OnTriggerEnter(Collider VentColliderTrigger)
        {
            Debug.Log("Trigger entered by: " + VentColliderTrigger.gameObject.name);

            if (VentColliderTrigger.CompareTag("Player"))
            {
                if (screw.hasScrew == true)
                {
                    ventOutskirts.isTrigger = true;
                    VentFixed.SetActive(false);
                    VentAnim.SetBool("VentSpin", false);
                    VentBroken.SetActive(true);
                    VentDeathCam.SetActive(false);
                    screw.holdingScrew.gameObject.SetActive(true);
                    ventFan.Stop();

                    if (!hasVentBreak1Played)
                    {
                        ventBreak1.Play();
                        hasVentBreak1Played = true;
                    }
                    if (!hasVentBreak2Played)
                    {
                        ventBreak2.Play();
                        hasVentBreak2Played = true;
                    }
                }
            }

            if (VentColliderTrigger.CompareTag("Player"))
            {
                if (screw.hasScrew == false)
                {
                    VentDeathCam.SetActive(true);
                    VentFixed.SetActive(true);

                    VentAnim.SetBool("Dead", true);
                    VentAnim.SetBool("VentSpin", true);
                    mCharacter.SetActive(false);


                    PlayerDead.SetActive(true);

                    StartCoroutine(WaitBetweenFadeInOut());
                    playerVentDeath.Play();
                }

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

            respawn.RespawnPlayer();

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
                    VentDeathCam.SetActive(false);
                    mCharacter.SetActive(true);
                    characterMain.Enabled();
                    //GetComponent<NavMeshAgent>().isStopped = false;


                    VentFixed.SetActive(true);
                    screw.pickedUpScrew.gameObject.SetActive(true);
                    screw.holdingScrew.gameObject.SetActive(false);

                    VentAnim.SetBool("Dead", false);
                    VentAnim.SetBool("VentSpin", true);
                    PlayerDead.SetActive(false);
                }
                else
                {
                    fadeOut = false;
                }
            }
        }


    }
}
