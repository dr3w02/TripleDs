using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class LampSmash : MonoBehaviour
    {
        [Header("Interactables")]

        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;
        public bool interactable;


        public Animator LampAnimator;

        public int ShakedCount;
        public bool Interact(Interactor interactor)
        {
            if (interactable == false)
            {

                ShakedCount++;
                Debug.Log("Interacted");
                LampAnimator.SetBool("Rocking", true);

                if (ShakedCount == 3)
                {
                    LampAnimator.SetBool("Rocking", false);
                    LampAnimator.SetBool("Smashed", true);
                }

            }

            else
            {

                LampAnimator.SetBool("Idel", true);


            }


            return true;

        }
    }

}
