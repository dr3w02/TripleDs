using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Platformer
{
    public class LampSmash : MonoBehaviour, IInteractable
    {
     

        [Header("Interactables")]

        [SerializeField] private string _prompt;
        public string InteractionPrompt => "LAMPHIT";

        public GameObject InteractionImagePrompt => null;

        public Animator LampAnimator;

        public int ShakedCount;

        public bool Smashed = false;
        public LamSpawn LampSpawn;



        private void OnValidate()
        {
            if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
            {
                gameObject.layer = LayerMask.NameToLayer("Interactable");
                gameObject.AddComponent<BoxCollider>();
            }

        }


        public void Start()
        {
            LampAnimator.SetBool("Idel", true);
          

        }
      
        public bool Interact(Interactor interactor)
        {
            if (Smashed == false)
            {
                ShakedCount += 1;

                Debug.Log("Interacted");


                LampAnimator.SetBool("Rocking", true);

            }


            if (ShakedCount == 3)
            {
                Smashed = true;

                LampAnimator.SetBool("Rocking", false);
               LampAnimator.SetBool("Smashed", true);

                    
               LampSpawn.ActivateRandomObject();

                ShakedCount = 0;
            }


            
            return false;

        }

       

      
     


    }


}
