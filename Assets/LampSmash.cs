using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.Controls.AxisControl;


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

        public Animator BBAnimator;
        public LamSpawn LampSpawn;

      
        public BoxCollider boxcollider;
      
        public NurseCodeOffice enemy;

        public bool lampSway;

        

        private void OnValidate()
        {
            if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
            {
                gameObject.layer = LayerMask.NameToLayer("Interactable");
               
            }

        }


        public void Start()
        {
            LampAnimator.SetBool("Idel", true);

        }
       
        public bool Interact(Interactor interactor)
        {
            if (lampSway == false)
            {
                ShakedCount += 1;

                Debug.Log("Interacted");

                LampAnimator.SetBool("Rocking", true);
            }

            
            if (ShakedCount == 3)
            {
                
                lampSway = true;
               boxcollider.isTrigger = true;
               LampAnimator.SetBool("Rocking", false);
               LampAnimator.SetBool("Smashed", true);

                
                LampSpawn.ActivateRandomObject();

                ShakedCount = 0;
            }


            if (ShakedCount > 1)
            {
                if (enemy.CompareTag("EnemyBB"))
                {

                    StartCoroutine(heardANoise());
                    Debug.Log("MadeANoise") ;
                   // StartCoroutine(Noisettack());

                }


            }

            return true;

        }

        

        public IEnumerator heardANoise()
        {

            enemy.speed = 0f;

            Debug.Log("Stoppedheardanoise");

            enemy.smashed = true;

            //bbAnimator.SetBool("Looking", true);

            yield return new WaitForSeconds(8f);

            //bbAnimator.SetBool("Looking", false);

            enemy.smashed = false;
         

            Debug.Log("hearda noise started");
            //agent.speed = 6;

            //StopCoroutine(heardANoise());

        }


        //private IEnumerator Noisettack()
        //{


        //    yield return new WaitForSeconds(5f);

        //    enemy.smashed = false;

        //    StopCoroutine(Noisettack() );
        //}





    }


}
