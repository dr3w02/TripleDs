using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Platformer
{
    public class BlackBeackIntro : MonoBehaviour
    {
        StartBossFight StartSequence;

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



        [Header("MainCharacter")]

        public GameObject mCharacter;/// <summary>
       
        public Animator mainAnim;

        void Start()
        {

            Bosscam.SetActive(false);
          
        }

        public void BBPlays()
        {
            StartCoroutine(PanBack());
        }


        private IEnumerator PanBack()
        {




            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame

                    mCharacter.SetActive(true);
                 

                    mainAnim.SetBool("Sleeping", true);

                    //GetComponent<NavMeshAgent>().isStopped = false;
                }
                else
                {
                    fadeOut = false;
                }
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
