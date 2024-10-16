using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class MusicBox : MonoBehaviour
    {
        [SerializeField] Image radialImage;

        [SerializeField] public CustomInputs customInputs;
        private bool isHolding = false;
        public TimerController timer;
        private float fillSpeed = 5.0f;
        public float maxTime = 30.0f; // change here u gotta change the other one on timer

        public GameObject MusicBoxCollider;

        string Music = "MusicBox";

        public Transform Player { get; private set; }


        public void Start()
        {
            MusicBoxCollider = GameObject.FindGameObjectWithTag(Music);
            Player = GameObject.FindGameObjectWithTag("Player").transform;

            radialImage.fillAmount = 1;
        }

        private void OnEnable()
        {

           // customInputs.CharacterControls.Hold.performed += OnHoldPerformed;
            //customInputs.CharacterControls.Hold.canceled += OnHoldCanceled;
        }
        private void OnDisable()
        {
           // // Unsubscribe from the "Hold" action event and disable input actions
           // customInputs.CharacterControls.Hold.performed -= OnHoldPerformed;
           // customInputs.CharacterControls.Hold.canceled -= OnHoldCanceled;

           
        }

        private void OnHoldPerformed(InputAction.CallbackContext context)
        {
            isHolding = true;
        }

        private void OnHoldCanceled(InputAction.CallbackContext context)
        {
            isHolding = false;
        }

        public bool Holding()
        {
            return isHolding;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
               // if (RBController.Instance.GetHold())
              ///  {
               //    Debug.Log("Holding");
                  //Wind up the music box
               //     timer.MusicBoxWindUp();
               //     radialImage.fillAmount += Time.deltaTime * fillSpeed / maxTime;
              //  }

               // If the player isn't holding then wind down music box 
              // else
              //  {
                //    radialImage.fillAmount -= Time.deltaTime * fillSpeed / maxTime;
                //    radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);
               // }
            }

            // Clamp fillAmount between 0 and 1
            radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);
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

