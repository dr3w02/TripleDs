using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Platformer
{
    public class GroundCheckOnCollision : MonoBehaviour
    {
        public float groundCheckDistance;


        //public float bufferCheckDistance = 0.3f;

        public Transform orentation;
        public RBController rbController;

        //Vector3 origin;

        RaycastHit hit;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            //groundCheckDistance = //0.2f + bufferCheckDistance;



            Debug.DrawRay(orentation.transform.position, -orentation.transform.up * groundCheckDistance, Color.magenta);

            //////////////////////////////////////////////////////////////////////////////////////Tester
            //if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            //{
            //   GetComponent<Rigidbody>().AddForce(transform.up * 3, ForceMode.Impulse);
            // }
            //////////////////////////////////////////////////////////////////////////////////////Tester

            if (Physics.Raycast(orentation.transform.position, -orentation.transform.up, out hit, groundCheckDistance))
            {//Where RayHitGround

              
                    rbController.SetGrounded(true);
                    // Debug.Log("Hit collider: " + hit.collider.name);
                
  
            }

            else// ray has not hit the ground
            {
              
                    rbController.SetGrounded(false);
                    // Debug.Log("Hit collider: " + hit.collider.name);
                
                //isGrounded = false;
                
               // Debug.Log("Raycast did not hit anything");

            }


            //if (!rbController.grounded)
            // {//
            //   Debug.LogError("No Ground");
            //  return;
            // }

        }


    }
}
