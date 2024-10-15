using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace Platformer
{
    public class MusicBox : MonoBehaviour
    {
        [SerializeField] Image radialImage;
   
        public InputManager inputManager;
        public TimerController timer;
        private float fillSpeed = 5.0f;
        public float maxTime = 20.0f; // change here u gotta change the other one on timer

        public GameObject MusicBoxCollider;

        string Music = "MusicBox";

        public Transform Player { get; private set; }


        public void Start()
        {
            MusicBoxCollider = GameObject.FindGameObjectWithTag(Music);
            Player = GameObject.FindGameObjectWithTag("Player").transform;

            radialImage.fillAmount = 1;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                if (InputManager.Instance.GetHold())
                {
                    timer.MusicBoxWindUp();
                    //radialImage.fillAmount += Time.deltaTime * fillSpeed;
                }

            }
            // Clamp fillAmount between 0 and 1
            //radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                timer.MusicBoxWindDown();
            }

        }

        void Update()
        {
            timer.MusicBoxWindDown();
        }


    }
}
