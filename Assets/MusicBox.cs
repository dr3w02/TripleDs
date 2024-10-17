using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Linq;

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
        bool playerInRange;
        string Music = "MusicBox";

        public Transform Player { get; private set; }


        public void Start()
        {
            MusicBoxCollider = GameObject.FindGameObjectWithTag(Music);
            Player = GameObject.FindGameObjectWithTag("Player").transform;

            radialImage.fillAmount = 1;

            customInputs = new CustomInputs();
            customInputs.CharacterControls.Hold.performed += OnHoldPerformed;
            customInputs.CharacterControls.Select.canceled += OnHoldCanceled;
            customInputs.Enable();
        }


        private void OnDisable()
        {
           // // Unsubscribe from the "Hold" action event and disable input actions
           customInputs.CharacterControls.Hold.performed -= OnHoldPerformed;
           customInputs.CharacterControls.Hold.canceled -= OnHoldCanceled;

           
        }

        public void OnHoldPerformed(InputAction.CallbackContext context)
        {
            isHolding = true;
        }

        public void OnHoldCanceled(InputAction.CallbackContext context)
        {
            isHolding = false;
        }

        public bool Holding()
        {
            return isHolding;
        }

        private void OnTriggerStay(Collider other)
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                timer.MusicBoxWindDown();
                playerInRange = false;
            }

        }

        void Update()
        {
            timer.MusicBoxWindDown();
            if (playerInRange && isHolding)
            {
                Debug.Log("Holding");
                // Wind up the music box
                timer.MusicBoxWindUp();
                radialImage.fillAmount += Time.deltaTime * fillSpeed / maxTime;
            }
            else
            {
                // If the player isn't holding then wind down music box 

                radialImage.fillAmount -= Time.deltaTime * fillSpeed / maxTime;
                radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);

            }
            // Clamp fillAmount between 0 and 1
            radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);

        }

     
    }

}

