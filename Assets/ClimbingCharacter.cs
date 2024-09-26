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

        public Vector3 ClimbOffset = new Vector3(0, 0, -0.8f);

        public float dropSpeed = 9.8f;

        public void Start()
        {



        }


        public void LateUpdate()
        {
            PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
            PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

            PlayerPrefs.SetFloat("PlayerXR", player.transform.rotation.x);
            PlayerPrefs.SetFloat("PlayerYR", player.transform.rotation.y);
            PlayerPrefs.SetFloat("PlayerZR", player.transform.rotation.z);
        }

        /*
        void AlignToLadder()
        {
            Debug.Log("allinging...");
            if (lastHit != null)
            {

                
                 

                    //Vector3 ladderForward = lastHit.transform.up;

                    //position


                    Vector3 lastHitPosition = lastHit.transform.position;


                    Vector3 newPosition = lastHitPosition;


                    player.transform.position = newPosition;

                    //player.transform.position = lastHitPosition;






                    Vector3 ladderRotation =  lastHit.transform.rotation.eulerAngles.normalized;



                    float ladderXRotation = -ladderRotation.x;
                    float ladderZRotation = 0f;
                    float ladderYRotation = -ladderRotation.y;


                    Vector3 currentRotation = player.transform.rotation.eulerAngles;
                    //Vector3 currentRotationThis = player.transform.rotation.eulerAngles;
                    float playerXRotation = PlayerPrefs.GetFloat("PlayerYR");


                    player.transform.rotation = Quaternion.Euler(ladderXRotation, ladderYRotation, ladderZRotation).normalized;

                    ///player.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotation.y, ladderZRotation).normalized;
                    //player.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotation.y, ladderZRotation);


                    Debug.LogWarning(lastHit.transform.position);
                    Debug.Log(player.transform.position);
                    // player.transform.position = lasthit.point;

                    Debug.Log("New Player Rotation: " + player.transform.transform);
                    Debug.Log("LadderRotation:" + ladderRotation);
                    //this.transform.rotation = Quaternion.Euler(0f, ladderYRotation, 0f);
                   
                    HandleClimbingMovement();
            }
            else
            {
                Debug.LogError("No valid lastHitTransform! Cannot align to ladder.");
         
            }
        }
        */
        private void CheckForLadder()
        {

            if (playerScript.isPullPressed)
            {


                if (!isClimbingLadder)
                {


                    //Vector3 ladderForward = lastHit.transform.forward;

                    //position


                    Vector3 lastHitPosition = lastHit.transform.position;
                    Vector3 newPosition = lastHitPosition + ClimbOffset;
                    player.transform.position = newPosition;



                    //Rotation



                    Vector3 ladderRotation = lastHit.transform.rotation.eulerAngles.normalized;



                    float ladderXRotation = -ladderRotation.x;
                    float ladderZRotation = 0f;
                    float ladderYRotation = -ladderRotation.y;


                    Vector3 currentRotation = player.transform.rotation.eulerAngles;




                    player.transform.rotation = Quaternion.Euler(ladderXRotation, ladderYRotation, ladderZRotation).normalized;



                    Debug.LogWarning(lastHit.transform.position);
                    Debug.LogWarning(player.transform.position);
                    // player.transform.position = lasthit.point;

                    isClimbingLadder = true;

                    HandleClimbingMovement();
                }


            }
            else
            {

                if (!playerScript.isPullPressed)
                {
                    isClimbingLadder = false;

                    DropLadder();
                }



            }


        }



        public void HandleClimbingMovement()
        {


            if (isClimbingLadder == true)
            {
                //OnSlope();



                Vector3 climbDirection = Vector3.up * climbSpeed * Time.deltaTime;


                // Move up if input is -1 climbing up
                if (playerScript.currentMovementInput.y == -1)
                {


                    //rb.MovePosition(player.transform.position + orientation.up * climbSpeed * Time.deltaTime);
                    //rb.AddForce(Vector3.up, ForceMode.Impulse);
                    rb.MovePosition(rb.position + climbDirection);
                    rb.AddForce(climbDirection, ForceMode.VelocityChange);
                    Debug.LogWarning("Climbingup");
                    // Debug.DrawRay(player.transform.position, Vector3.up * 100f, Color.yellow);
                    //Debug.LogWarning("CLIMBING UP");


                }
                // Move down if input is 1 climbing down


                else if (playerScript.currentMovementInput.y == 1)
                {


                    // rb.freezeRotation = true;

                    //rb.MovePosition(Vector3.down * climbSpeed * Time.deltaTime);

                    rb.MovePosition(rb.position - climbDirection);

                    rb.AddForce(climbDirection, ForceMode.VelocityChange);

                    //characterController.transform.position += Vector3.up / climbSpeed;
                    Debug.LogWarning("CLIMBING DOWN");
                }





            }


        }

        private void DropLadder()
        {

            Debug.LogWarning("Drop");

            isClimbingLadder = false;


            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));

            float playerXRotation = PlayerPrefs.GetFloat("PlayerXR");
            float playerYRotation = PlayerPrefs.GetFloat("PlayerYR");
            float playerZRotation = PlayerPrefs.GetFloat("PlayerZR");


            Quaternion playerRotation = Quaternion.Euler(playerXRotation, playerYRotation, playerZRotation);

            player.transform.rotation = playerRotation;


        }





        public float sphereRadius = 1.1f;
        public float maxDistance = -1.14f;
        RaycastHit hit;

        public Transform orientation;
        public Vector3 offset;

        void DetectClimbable()
        {
            if (Physics.SphereCast(orientation.transform.position + orientation.forward + offset, sphereRadius, orientation.forward, out hit, maxDistance, climbableLayer))
            {
                Debug.Log("Climbable object detected!" + hit.collider.name);
                // Store the hit information
                lastHitNormal = hit.normal;
                lastHit = hit.transform;

                ClimableFound = true;


                CheckForLadder();
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything.");

                ClimableFound = false;
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


        }
    }
}

