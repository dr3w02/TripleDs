using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
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

        public FinalBossAnim reset;


        public BoxCollider boxcollider;
      
        public NurseCodeOffice enemy;

        public bool lampSway;

        public GameObject LampSmashed;

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
      
        public void Update()
        {
           
            if (reset.Reset == true)
            {
              
                Debug.Log("ResetLamps");
                
                lampSway = false;
                LampAnimator.SetBool("Idel", true);




            }
        }

        public bool Interact(Interactor interactor, GameObject player)
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

            

            yield return new WaitForSeconds(8f);


            enemy.smashed = false;
         

            Debug.Log("hearda noise started");
            
        }


       





    }


}
