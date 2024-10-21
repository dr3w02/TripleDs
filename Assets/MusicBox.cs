using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor.PackageManager.UI;

namespace Platformer
{
    public class MusicBox : MonoBehaviour
    {
        [SerializeField] Image radialImage;

        [SerializeField] public CustomInputs customInputs;
        private bool isHolding = false;
        public TimerController timer;
        private float fillSpeed = 5.0f;
        bool playerInRange;

        public Transform Player { get; private set; }

        public void Start()
        {
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
            if (!isHolding)
            {
                timer.MusicBoxWindDown();
                isHolding = false;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                isHolding = false;
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
            

            if (playerInRange && isHolding)
            {
                Debug.Log("Holding");
                // Wind up the music box
                timer.MusicBoxWindUp();
                radialImage.fillAmount += Time.deltaTime * fillSpeed / timer.maxTime;
          

            }

            else
            {
                // If the player isn't holding then wind down music box 
                timer.MusicBoxWindDown();
                radialImage.fillAmount -= Time.deltaTime * fillSpeed / timer.maxTime;
                radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);
              
            }
            // Clamp fillAmount between 0 and 1
            radialImage.fillAmount = Mathf.Clamp01(radialImage.fillAmount);

        }

     
    }

}

