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

     

        public GameObject SleepCam;

        [Header("BlackBeak")]

        public Animator BlackBeakAnim;
        public GameObject Bosscam;
        public GameObject BB;

        [Header("WayPoint")]
        public List<GameObject> waypointsbbIntro;
        int index = 0;
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
        [SerializeField] private float shakeTime = 2f;


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
           
            BB.transform.position = waypointsbbIntro[index].transform.position;

            BB.SetActive(false);
        }

        public void Update()
        {
            ShakeCamera(shakeIntensity, shakeTime);
        }

        public void StartBlackBeakIntro()
        {
            // Disable sleeping script and show BlackBeak
            SleepScript.enabled = false;
            BB.SetActive(true);
            Bosscam.SetActive(true);
            SleepCam.SetActive(false);

            // Start BlackBeak movement and animation
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


            BlackBeakAnim.SetBool("walking", true);

         
            while (index < waypointsbbIntro.Count)
            {
                Vector3 destination = waypointsbbIntro[index].transform.position;

                while (Vector3.Distance(BB.transform.position, destination) > 0.1f)
                {
                    BB.transform.position = Vector3.MoveTowards(BB.transform.position, destination, speed * Time.deltaTime);
                    yield return null;
                }

                index++;

                if (index == waypointsbbIntro.Count)
                {
                    BlackBeakAnim.SetBool("walking", false);

                    BlackBeakAnim.SetBool("Evil", true);


                    yield return new WaitForSeconds(shakeTime);


                    CamShake = true;

                    yield return new WaitForEndOfFrame();


                    
                   

                    StartCoroutine(FadeInOut());
                    yield break;
                }
            }
        }


      
     

        private IEnumerator FadeInOut()
        {

            CamShake = false;
            BB.SetActive(false);
            Bosscam.SetActive(false);

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
