using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Levers : MonoBehaviour, IInteractable
    {
        public enum LeverType { Lever4, Lever1, Lever5, Other }
        public LeverType leverType;

        [SerializeField] private string _prompt = "Pull Lever";
        public string InteractionPrompt => _prompt;

        public Animator leverAnimator;
        public Collider leverCollider;
        public AudioClip leverSoundEffect;

        public int LeverDownHash { get; private set; }
        public int LeverUpHash { get; private set; }

        private bool _isLeverDown = false;
        private bool _isLeverUp = false;


        void Awake()
        {
            LeverDownHash = Animator.StringToHash("LeverDown");
            LeverUpHash = Animator.StringToHash("LeverUp");
        }

        public bool Interact(Interactor interactor)
        {
            if (_isLeverDown)
            {
                // reset the lever to the up position
                Debug.Log($"{leverType} Lever Up");

                ResetAnimatorParameters();
                leverAnimator.SetTrigger(LeverUpHash);

                leverCollider.isTrigger = false;

                PlaySoundEffect();

                _isLeverDown = false; // lever is now up
            }
            else
            {
                // Pull the lever down
                Debug.Log($"{leverType} Lever Down");

                ResetAnimatorParameters();
                leverAnimator.SetTrigger(LeverDownHash);

                leverCollider.isTrigger = true;

                PlaySoundEffect();

                _isLeverDown = true; // lever is now down

                // register the lever pull in the Bathroom script
                Bathroom bathroom = FindObjectOfType<Bathroom>();
                bathroom?.RegisterLeverPull(this);
            }

            return true;
        }

        /*
        public bool Interact(Interactor interactor)
        {
            if (_isLeverDown)
            {
                // Reset the lever to the up position
                Debug.Log($"{leverType} Lever Up");

                ResetAnimatorParameters();
                leverAnimator.SetTrigger("LeverUp");

                leverCollider.isTrigger = false;

                PlaySoundEffect();

                _isLeverDown = false; // mark the lever as up
            }
            else
            {
                // Pull the lever down
                Debug.Log($"{leverType} Lever Down");

                ResetAnimatorParameters();
                leverAnimator.SetTrigger("LeverDown");

                leverCollider.isTrigger = true;

                PlaySoundEffect();

                _isLeverDown = true; // mark the lever as down
            }

            return true;
        }
        */
        private void ResetAnimatorParameters()
        {
            leverAnimator.ResetTrigger("LeverDown");
            leverAnimator.ResetTrigger("LeverUp");
        }

        private void PlaySoundEffect()
        {
            if (leverSoundEffect)
            {
                AudioSource.PlayClipAtPoint(leverSoundEffect, transform.position);
            }
        }

        public bool IsLeverDown()
        {
            return _isLeverDown;
        }

        public LeverType GetLeverType()
        {
            return leverType;
        }

        // New method to reset lever state
        public void ResetLeverState()
        {
            Debug.Log($"{leverType} Lever Reset");

            ResetAnimatorParameters();
            leverAnimator.SetTrigger(LeverUpHash);

            leverCollider.isTrigger = false;

            _isLeverDown = false;
        }
    }
}