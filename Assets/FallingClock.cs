using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingClock : MonoBehaviour
    {
      
            public AudioSource ClockFallSound;
            public AudioSource GlassSmash;

        [Header("CameraShake")]
        [SerializeField] private float shakeIntensity = 20f;
        [SerializeField] private float shakeTime = 5f;
        public CinemachineVirtualCamera VirtualCam;
        private CinemachineBasicMultiChannelPerlin perlinNoise;
        public bool CamShake;

       
        [SerializeField] public Animator ClockAnim;

       

        private void Awake()
        {
            perlinNoise = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
          
            CamShake = false;


        }
        private void Start()
            {

                ClockAnim.SetBool("FallingClock", false);


            }

        private void Update()
        {
            ShakeCamera(shakeIntensity, shakeTime);
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

        private void OnTriggerExit(Collider other)
        {

                Debug.Log("in trigger");
                if (other.CompareTag("Player"))
                {


                    ClockAnim.SetBool("FallingClock", true);
                

                    ClockFallSound.Play();
                    

                    GlassSmash.Play();

                   StartCoroutine(ShakeWait());
                    //Destroy(ScriptableObject);
                }

        }

        private IEnumerator ShakeWait()
        {
            CamShake = true;

            yield return new WaitForSeconds(6f);

            CamShake = false;


        }

    }
}
