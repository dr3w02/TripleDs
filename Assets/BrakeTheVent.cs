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
        string Vent = "Vent";
        public Collider ventOutskirts;


       


        [SerializeField] private float WaitTime = 0.5f;

        // Handle the fade out and in
        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;


        public characterMovement characterMain;

       
        public GameObject VentDeathCam;
        private void Start()
        {
            VentAnim.SetBool("VentSpin", true);
            VentAnim.SetBool("Dead", false);
            ventOutskirts.isTrigger = false;
            VentFixed.SetActive(true);

            VentBroken.SetActive(false);
            PlayerDead.SetActive(false);


            ventOutskirts = GetComponent<Collider>();


        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (screw.hasScrew == true)
                {
                    ventOutskirts.isTrigger = true;
                    VentFixed.SetActive(false);
                    VentAnim.SetBool("VentSpin", false);
                    VentBroken.SetActive(true);
                }
            }
            else if (screw.hasScrew == false)
            {
                VentAnim.SetBool("Dead", true);

                //mCharacter.SetActive(false);
                PlayerDead.SetActive(true);

                StartCoroutine(WaitBetweenFadeInOut());
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

            characterMain.RespawnPlayer();
            mCharacter.SetActive(false);
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
                }
                else
                {
                    fadeOut = false;
                }
            }
        }


    }
}
