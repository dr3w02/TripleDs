using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Platformer
{
    public class LampSmash : MonoBehaviour,IInteractable
    {
        [Header("Interactables")]

        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;

        public GameObject InteractionImagePrompt => null;

        public Animator LampAnimator;

        public static int ShakedCount;

        public bool Smashed;
        LamSpawn LampSpawn;

        public void Start()
        {
            LampAnimator.SetBool("Idel", true);
          

        }
        public bool Interact(Interactor interactor)
        {
            if (!Smashed)
            {
                ShakedCount += 1;

                Debug.Log("Interacted");


                LampAnimator.SetBool("Rocking", true);

            }


            if (ShakedCount == 3)
            {
               LampAnimator.SetBool("Rocking", false);
               LampAnimator.SetBool("Smashed", true);
               Smashed = true;
               
                LampSpawn.generateNewObj();


            }

            
            return true;

        }

       

      
     


    }


}
