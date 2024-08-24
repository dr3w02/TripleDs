using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PullingPushing : MonoBehaviour
    {
        public Transform boxAttachPoint;
        public Transform orientation;
        public float distance;

        public LayerMask mask;

        GameObject box;
        public bool isAttached;

        //Raycast out from the player
        //Look for 'pushable' layer.
        //Select right click
        //Attach the box as a child.
        //Add Rigidbody drag to US so we cant just fling it around.

        private void Update()
        {
            HandleInput(); // Check for input and handle attaching/detaching the box
        }

        void HandleInput()
        {
            // Check if the right mouse button is pressed down
            if (Mouse.current.rightButton.isPressed)
            {
                if (!isAttached)
                {
                    CheckForPushable(); // Try to attach a box if not already attached
                }
            }
            // Check if the right mouse button is released
            else if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                if (isAttached)
                {
                    UnattachBoxFromPlayer(); // Detach the box if already attached
                }
            }
        }

        void CheckForPushable()
        {
            RaycastHit hit;
            Vector3 rayOrigin = orientation.position + new Vector3(0, 0.7f, 0);
            Vector3 rayDirection = orientation.forward;

            Debug.DrawRay(rayOrigin, rayDirection * distance, Color.yellow);

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, distance, mask))
            {
                if (hit.collider != null)
                {
                    AttachBoxToPlayer(hit.collider.gameObject); // Attach the box if one is found
                }
            }
        }

        void AttachBoxToPlayer(GameObject hit)
        {
            isAttached = true;

            box = hit;
            hit.transform.parent = transform;
            hit.transform.position = boxAttachPoint.position;

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.drag = 20f; // Adjust drag value as needed
                rb.angularDrag = 20f; // Adjust angular drag value as needed
            }
            //add rigidbody drag
            //play pull Animation
        }

        void UnattachBoxFromPlayer()
        {
            if (box == null) return;

            // Remove the box from being a child of the player
            box.transform.parent = null;

            // Reset Rigidbody drag
            Rigidbody rb = box.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.drag = 0f;
                rb.angularDrag = 0.05f; // Reset to default or as needed
            }

            // Play detach animation if needed
            isAttached = false;
            box = null;
            //Remove the box parent.
            //Remove the drag
            //Animation
        }
    }

}
