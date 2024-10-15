using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class FallingClock : MonoBehaviour
    {
        public AudioSource tickingClock;

        //public AudioSource clockBigHit;
        //public AudioSource dingDong;

        [Header("CameraShake")]
        [SerializeField] private float shakeIntensity = 5f;
 

        public CinemachineVirtualCamera VirtualCam;
        private CinemachineBasicMultiChannelPerlin perlinNoise;
        public bool CamShake;

       
        [SerializeField] public Animator ClockAnim;

       

        private void Awake()
        {

            perlinNoise = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        }
        private void Start()
        {
            CamShake = false;

            ClockAnim.SetBool("FallingClock", false);

            if (VirtualCam != null)
            {
                perlinNoise = VirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            }
        }

        private void Update()
        {
            if (CamShake)
            {
                ShakeCamera(shakeIntensity);
            }
        }

        public void ShakeCamera(float intensity)
        {
            perlinNoise.m_AmplitudeGain = intensity;
        }
     
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("in trigger");
            if (other.CompareTag("Player"))
            {
                ClockAnim.SetBool("FallingClock", true);
                tickingClock.Stop();
                CamShake = true;

                //StartCoroutine(ShakeWait());
                //Destroy(ScriptableObject);
            }
        }

        public void StopShake()
        {
            perlinNoise.m_AmplitudeGain = 0f;
        }

        private IEnumerator ShakeWait()
        {
           
           Debug.Log("Shaking Cam");
            yield return new WaitForSeconds(2f);

            CamShake = false;


        }
        /*
        public void clockBigHitFunction()
        {
            clockBigHit.Play();
        }

        public void clockDingDongFunction()
        {
            dingDong.Play();
        }
        */
    }
}
