using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
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

        public NavMeshAgent agent;
        public Transform player;
        public NurseCodeOffice enemy;
        public PlayerDetector Detector;

    

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

            if (ShakedCount == 2)
            {
                if (enemy.CompareTag("EnemyBB"))
                {
                    Debug.Log("MadeANoise") ;
                    StartCoroutine(Noisettack());

                }


            }

            return true;

        }

     


        private IEnumerator Noisettack()
        {

            agent.SetDestination(player.position);

            Detector.detectionRadius = 20f;
            Detector.innerDetectionRadius = 10f;
            Detector.attackRange = 10f;
            
            yield return new WaitForSeconds(5f);

            Detector.detectionRadius = 3.01f;
            Detector.attackRange = 2f;
            Detector.innerDetectionRadius = 1.62f;
        }

      
     


    }


}
