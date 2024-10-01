using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

namespace Platformer
{

    public class ClimbingCharacter : MonoBehaviour
    {

        public Transform player;

        public Rigidbody rb;
        public RBController playerScript;

        public Animator animator;


        public float climbSpeed = 2f;

        //things for climb and swing
        public bool isClimbingLadder;

        public LayerMask climbableLayer;


        public bool ClimableFound;

        public float rayDistance = 3;


        public RaycastHit lastHitTemp;
        public Vector3 lastHitNormal;
        Transform lastHit;



        public Vector3 collision = Vector3.zero;

        public Vector3 ClimbOffset = new Vector3(0, 0, 0);

        public float dropSpeed = 9.8f;

        public void Start()
        {
            PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

            PlayerPrefs.SetFloat("PlayerXR", player.transform.rotation.x);
            PlayerPrefs.SetFloat("PlayerYR", player.transform.rotation.y);
            PlayerPrefs.SetFloat("PlayerZR", player.transform.rotation.z);
        }

        private void CheckForLadder()
        {
            Debug.Log("LADDER - CHECKING FOR LADDER ");
            if (playerScript.isPullPressed)
            {
                //Called once
                if (!isClimbingLadder)
                {
                    Vector3 lastHitPosition = lastHit.transform.position;
                    Vector3 newPosition = lastHitPosition + ClimbOffset;
                    player.transform.position = newPosition;

                    Vector3 ladderRotation = lastHit.transform.rotation.eulerAngles.normalized;
                    float ladderXRotation = -ladderRotation.x;

                    Vector3 currentRotation = player.transform.rotation.eulerAngles;
                    player.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotation.y, currentRotation.z).normalized;

                    isClimbingLadder = true;
                    HandleClimbingMovement();

                }


            }
            else
            {
                //if (isClimbingLadder)
                //{
                //    isClimbingLadder = false;
                //    DropLadder();
                //}
            }


        }



        public void HandleClimbingMovement()
        {
            // Move up if input is -1 climbing up
            if (playerScript.currentMovementInput.y == -1)
            {
                rb.velocity = orientation.up * climbSpeed;
            }
                
            if(!playerScript.isPullPressed)
            {
                DropLadder();
            }
        }

        private void DropLadder()
        {
           // Debug.Log("Drop");
            isClimbingLadder = false;
            player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.y, 0);

            //player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));

            //float playerXRotation = PlayerPrefs.GetFloat("PlayerXR");
            //float playerYRotation = PlayerPrefs.GetFloat("PlayerYR");
            //float playerZRotation = PlayerPrefs.GetFloat("PlayerZR");

            //Quaternion playerRotation = Quaternion.Euler(playerXRotation, playerYRotation, playerZRotation);
            //player.transform.rotation = playerRotation;
        }

        public float sphereRadius = 1.1f;
        public float maxDistance;
        RaycastHit hit;

        public Transform orientation;
        public Vector3 offset;

        void DetectClimbable()
        {
            if (Physics.SphereCast(orientation.transform.position + orientation.forward + offset, sphereRadius, orientation.forward, out hit, maxDistance, climbableLayer))
            {
                Debug.Log("LADDER - Climbable object detected!" + hit.collider.name);
                // Store the hit information
                lastHitNormal = hit.normal;
                lastHit = hit.transform;

                CheckForLadder();
            }
            else
            {
                //Debug.LogWarning("LADDER - Raycast did not hit anything.");

                lastHitNormal = Vector3.zero;
                lastHit = null;

            }

            //Draws the max distance
            Debug.DrawRay(orientation.transform.position, orientation.forward * maxDistance, Color.yellow);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(orientation.position + orientation.forward + offset, sphereRadius); // Draw the sphere at the end
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            DetectClimbable();

            if (isClimbingLadder)
                HandleClimbingMovement();
        }
    }
}

