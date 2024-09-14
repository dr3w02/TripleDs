using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Levers : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt = "Pull Lever";
        public string InteractionPrompt => _prompt;

        public Animator leverAnimator1, leverAnimator2, leverAnimator3, leverAnimator4, leverAnimator5, leverAnimator6;
        public Collider lever1, lever2, lever3, lever4, lever5, lever6;
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

            lever1 = GetComponent<Collider>();


            lever1.isTrigger = true;
            leverAnimator1.SetBool(LeverDownHash, true);

            return true;
        }
    }
}
