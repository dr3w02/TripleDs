using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

namespace Platformer
{

    public class ClimbingCharacter : MonoBehaviour
    {

        public Transform player;

        public Rigidbody rb;
        public RBController playerScript;

        public Animator animator;

        int isPullingHash;

        public bool isPullPressed;

        public float climbSpeed = 2f;


        public PlayerInput input;


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

        public void CheckForLadder(Climbable c)
        {
            Debug.Log("LADDER - CHECKING FOR LADDER ");
            if (isPullPressed)
            {
                //Called once
                if (!isClimbingLadder)
                {
                    playerScript.TurnOffMovement();
                    Debug.Log("Start Climbing");
                    playerScript.grounded = false;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    player.SetParent(c.attachPoint);
                    player.transform.localPosition = Vector3.zero;
                    player.transform.rotation = c.attachPoint.rotation;

                    //Vector3 ladderRotation = lastHit.transform.rotation.eulerAngles.normalized;
                    //float ladderXRotation = -ladderRotation.x;

                    //Vector3 currentRotation = player.transform.rotation.eulerAngles;
                    //player.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotation.y, currentRotation.z).normalized;

                    isClimbingLadder = true;

                    HandleClimbingMovement();

                }
                else
                {
                    Debug.Log("Already Climbing");
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

        public void OnPull(InputAction.CallbackContext context)
        {

            isPullPressed = context.ReadValueAsButton();
            // Debug.Log("PullPressed");

        }

        public void Awake()
        {
            isPullingHash = Animator.StringToHash("isPulling");

        }

        public void HandleAnimation()
        {

            bool isPulling = animator.GetBool(isPullingHash);

            //Pulling Controls-------------------------------
            if ((isPullPressed) && !isPulling)
            {

                animator.SetBool(isPullingHash, true);

                //Debug.Log("Pull animator on");


            }

            else if ((!isPullPressed) && isPulling)
            {
                animator.SetBool(isPullingHash, false);
                //Debug.Log("Pull Animator off");
            }
        }


        public void HandleClimbingMovement()
        {

            
            animator.SetBool(isPullingHash, true);
            // Move up if input is -1 climbing up
            if (playerScript.currentMovementInput.y == -1)
            {
                rb.velocity = orientation.up * climbSpeed;
            }
                
            if(!isPullPressed)
            {
                DropLadder();
            }
        }

        public void DropLadder()
        {
           
            playerScript.Enabled();
            animator.SetBool(isPullingHash, false);
            // Debug.Log("Drop");
            isClimbingLadder = false;
            //player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.y, 0);

            //player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));

            //float playerXRotation = PlayerPrefs.GetFloat("PlayerXR");
            //float playerYRotation = PlayerPrefs.GetFloat("PlayerYR");
            //float playerZRotation = PlayerPrefs.GetFloat("PlayerZR");

            //Quaternion playerRotation = Quaternion.Euler(playerXRotation, playerYRotation, playerZRotation);
            //player.transform.rotation = playerRotation;

            player.parent = null;
           

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
                ClimableFound = true;
                //CheckForLadder();
            }
            else
            {
                //Debug.LogWarning("LADDER - Raycast did not hit anything.");

                lastHitNormal = Vector3.zero;
                lastHit = null;
                ClimableFound = false;
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
            if (isClimbingLadder)
            {
                HandleClimbingMovement();
            }
            //else
            //{
            //    DetectClimbable();
            //}
        }
    }
}

