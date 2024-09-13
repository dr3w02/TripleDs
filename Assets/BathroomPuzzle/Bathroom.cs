using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Bathroom : MonoBehaviour
    {
        public GameObject PlayerDead;
        public Collider ElectricColliderTrigger;

        public GameObject Character;
        public characterMovement characterMain;

        public GameObject BathroomDeathCam;

        /*
        [SerializeField] private float WaitTime = 20f;

        // Handle the fade out and in
        [SerializeField] private bool fadeIn = false;
        [SerializeField] private bool fadeOut = true;
        [SerializeField] public CanvasGroup myUIGroup;
        [SerializeField] private float fadeWaitTime = 4f;
        */




        private void Start()
        {
            PlayerDead.SetActive(false);
            BathroomDeathCam.SetActive(false);
        }

        private void OnTriggerEnter(Collider ElectricColliderTrigger)
        {
            if (ElectricColliderTrigger.CompareTag("Player"))
            {
                Character.SetActive(false);
                PlayerDead.SetActive(true);
                BathroomDeathCam.SetActive(true);

                if (characterMain != null)
                {
                    characterMain.enabled = false;
                }
            }
        }
    }
}
