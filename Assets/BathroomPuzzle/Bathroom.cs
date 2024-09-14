using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Bathroom : MonoBehaviour, IInteractable
    {
        public GameObject PlayerDead;
        public Collider ElectricColliderTrigger;

        public GameObject Character;
        public characterMovement characterMain;

        public GameObject BathroomDeathCam;

        public Animator leverAnimator1, leverAnimator2, leverAnimator3, leverAnimator4, leverAnimator5, leverAnimator6;
        public Collider lever1, lever2, lever3, lever4, lever5, lever6;
        public AudioClip leverSoundEffect;

        [SerializeField] private string _prompt = "Pull Lever";
        private bool _isLeverActivated = false;

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

        public string InteractionPrompt => _prompt;

        public bool Interact(Interactor interactor)
        {
            if (_isLeverActivated) return false;
            ActivateLever();
            return true;
        }

        private void ActivateLever()
        {
            _isLeverActivated = true;

            if (leverAnimator1 != null) leverAnimator1.SetTrigger("ActivateLever");
            if (leverAnimator2 != null) leverAnimator2.SetTrigger("ActivateLever");
            if (leverAnimator3 != null) leverAnimator3.SetTrigger("ActivateLever");
            if (leverAnimator4 != null) leverAnimator4.SetTrigger("ActivateLever");
            if (leverAnimator5 != null) leverAnimator5.SetTrigger("ActivateLever");
            if (leverAnimator6 != null) leverAnimator6.SetTrigger("ActivateLever");

            if (leverSoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(leverSoundEffect, transform.position);
            }

        }
    }
}
