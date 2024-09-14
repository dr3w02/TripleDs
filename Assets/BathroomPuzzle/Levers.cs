using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Levers : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt = "Pull Lever";
        public string InteractionPrompt => _prompt;

        public Animator[] leverAnimators;
        public Collider[] levers;
        public AudioClip leverSoundEffect;

        private int LeverDownHash;
        private int LeverUpHash;

        void Awake()
        {
            LeverDownHash = Animator.StringToHash("LeverDown");
            LeverUpHash = Animator.StringToHash("LeverUp");
        }

        public bool Interact(Interactor interactor)
        {
            Debug.Log("Lever Down");

            for (int i = 0; i < levers.Length; i++)
            {
                levers[i].isTrigger = true;
                leverAnimators[i].SetBool(LeverDownHash, true);
            }

            if (leverSoundEffect)
            {
                AudioSource.PlayClipAtPoint(leverSoundEffect, transform.position);
            }

            return true;
        }
    }
}