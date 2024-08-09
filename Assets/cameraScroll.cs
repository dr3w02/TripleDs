using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class cameraScroll : MonoBehaviour
    {
        public Vector2 turn;
        public float sensitivity = 2f;

        // Threshold for detecting minimal movement
        public float movementThreshold = 0.1f;
        // Time it takes to smoothly slide back to the original position
        public float slideDuration = 1.0f;
        // Time the camera should stay in the current position after minimal movement
        public float holdTime = 2.0f;

        private Vector3 originalPosition = new Vector3(19.314f, 0f, 0);
        private Quaternion originalRotation;
        private bool slidingBack = false;
        private float inactivityTimer = 0f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            turn.x = 0f;
            turn.y = 0f;
            originalRotation = transform.localRotation;
        }

        private void Update()
        {
            Vector2 currentMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            if (currentMouseDelta.magnitude > movementThreshold)
            {
                // Mouse has moved beyond the threshold
                turn.x += currentMouseDelta.x * sensitivity;
                turn.y += currentMouseDelta.y * sensitivity;

                // Clamp the rotation values to the specified limits
                turn.x = Mathf.Clamp(turn.x, -8f, 8f);
                turn.y = Mathf.Clamp(turn.y, -12f, 12f);

                // Apply rotation directly
                transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);

                // Reset the inactivity timer and sliding back flag
                inactivityTimer = 0f;
                slidingBack = false;
            }
            else
            {
                // Increase inactivity timer when the mouse movement is minimal
                inactivityTimer += Time.deltaTime;

                if (inactivityTimer >= holdTime)
                {
                    // Start sliding back after hold time is reached
                    slidingBack = true;
                }
            }

            if (slidingBack)
            {
                // Slide back to the original position smoothly
                transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime / slideDuration);

                if (Quaternion.Angle(transform.localRotation, originalRotation) < 0.01f)
                {
                    // Stop sliding when close enough to the original rotation
                    transform.localRotation = originalRotation;
                    slidingBack = false;
                    inactivityTimer = 0f; // Reset timer when back to original position
                }
            }
        }
    }
}
