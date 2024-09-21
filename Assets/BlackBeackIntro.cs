using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Cinemachine;

namespace Platformer
{
    public class BlackBeackIntro : MonoBehaviour
    {
        StartBossFight StartSequence;


        public CinemachineVirtualCamera VirtualCam;
        private CinemachineBasicMultiChannelPerlin perlinNoise;

        public Transform A;
        public Transform B;

        public GameObject SleepCam;

        [Header("BlackBeak")]

        public Animator BlackBeakAnim;
        public GameObject Bosscam;
        public GameObject BB;

      
        
        
        public float speed = 0.5f;

        [Header("Fade")]

        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;

        [SerializeField] private float WaitTime = 2f;
        [SerializeField] private float WaitTimeBB = 2f;


        [Header("CameraShake")]
        [SerializeField] private float shakeIntensity = 5f;
        [SerializeField] private float shakeTime = 5f;


       [Header("MainCharacter")]

        public GameObject mCharacter;/// <summary>

        public StartBossFight SleepScript;
        public Animator mainAnim;
        public bool CamShake;

        private void Awake()
        {
            perlinNoise = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            CamShake = false;

          
        }
        void Start()
        {

            Bosscam.SetActive(false);
           
            BB.transform.position = A.transform.position;

            BB.SetActive(false);
        }

        public void Update()
        {
            ShakeCamera(shakeIntensity, shakeTime);
        }


        public void StartBlackBeakIntro()
        {
            Debug.LogWarning("StartingAgain!!!");
           // SleepScript.enabled = false;
            BB.SetActive(true);
            Bosscam.SetActive(true);
            SleepCam.SetActive(false);

        
            StartCoroutine(MoveToCheckpoint());
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
    
        private IEnumerator MoveToCheckpoint()
        {

            //yield return new WaitForSeconds(WaitTime);



         
                Vector3 destination = B.position;

                while (Vector3.Distance(BB.transform.position, destination) == 0f)
                {

                    BlackBeakAnim.SetBool("walking", true);

                    BB.transform.position = Vector3.MoveTowards(BB.transform.position, destination, speed * Time.deltaTime);

                    yield return null;
                }

            

               
                    BlackBeakAnim.SetBool("walking", false);

                    BlackBeakAnim.SetBool("Evil", true);


                    yield return new WaitForSeconds(shakeTime);


                    CamShake = true;



            yield return new WaitForEndOfFrame();


            StartCoroutine(FadeInOut());
                   
                
            
        }


      
     

        private IEnumerator FadeInOut()
        {

      

            Debug.Log("Switch Camera");

            fadeIn = true;

            while (fadeIn)
            {
                if (myUIGroup.alpha < 1)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    //yield return null; // Wait for the next frame
                }
                else
                {
                    fadeIn = false;
                }
            }

            CamShake = false;
            BB.SetActive(false);
            Bosscam.SetActive(false);

            mainAnim.SetBool("Sleeping", false);

            //mCharacter.SetActive(false);

            yield return new WaitForSeconds(fadeWaitTime);

            fadeOut = true;
            while (fadeOut)
            {
                if (myUIGroup.alpha > 0)
                {
                    myUIGroup.alpha -= Time.deltaTime;
                    yield return null; // Wait for the next frame
                    
                  
                   
                    //GetComponent<NavMeshAgent>().isStopped = false;

                }
                else
                {
                    fadeOut = false;
                }
            }

        }

        //private IEnumerator BossIntro()
        //{
           // Bosscam.SetActive(true);

          //  BlackBeakAnim.SetBool("walking", true);
           // BlackBeakAnim.SetBool("Evil", false);

           // yield return new WaitForSeconds(WaitTimeBB);
        //
            //BlackBeakAnim.SetBool("walking", false);

            //BlackBeakAnim.SetBool("Evil", true);


            //yield return new WaitForSeconds(WaitTime);


            //PanBack();

        //}
    }
}
