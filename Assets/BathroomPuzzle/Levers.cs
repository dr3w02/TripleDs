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

        /*
        public bool Interact(Interactor interactor)
        {
            if (_isLeverDown) return false;

            Debug.Log($"{leverType} Lever Down");

            leverCollider.isTrigger = true;
            leverAnimator.SetTrigger(LeverDownHash);

            if (leverSoundEffect)
            {
                AudioSource.PlayClipAtPoint(leverSoundEffect, transform.position);
            }

            _isLeverDown = true;
            return true;
        }
        */

        public bool Interact(Interactor interactor)
        {
            if (_isLeverDown)
            {
                // the lever is currently down, so reset it to the up position
                Debug.Log($"{leverType} Lever Up");

                // reset LeverDown and activate LeverUp
                ResetAnimatorParameters();
                leverAnimator.SetTrigger("LeverUp");

                leverCollider.isTrigger = false;

                if (leverSoundEffect)
                {
                    AudioSource.PlayClipAtPoint(leverSoundEffect, transform.position);
                }

                _isLeverDown = false; // mark the lever as up
            }
            else
            {
                // the lever is currently up, so pull it down
                Debug.Log($"{leverType} Lever Down");

                // reset LeverUp and activate LeverDown
                ResetAnimatorParameters();
                leverAnimator.SetTrigger("LeverDown");

                leverCollider.isTrigger = true;

                if (leverSoundEffect)
                {
                    AudioSource.PlayClipAtPoint(leverSoundEffect, transform.position);
                }

                _isLeverDown = true; // mark the lever as down
            }

            return true;
        }

        private void ResetAnimatorParameters()
        {
            leverAnimator.ResetTrigger("LeverDown");
            leverAnimator.ResetTrigger("LeverUp");
        }

        public bool IsLeverDown()
        {
            return _isLeverDown;
        }

        public LeverType GetLeverType()
        {
            return leverType;
        }
    }
}
