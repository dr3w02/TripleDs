using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


namespace Platformer
{
    public class RBController : MonoBehaviour
    {

        public Rigidbody rb;

        public CapsuleCollider playersCapsuleCollider;

        public Transform cam;

        public Transform player;

        [Header("Floats")]
        public float speed, maxForce, jumpForce;

        // calls to the input manager


        //private float lookRotation;
        public bool CanMove = true;

        [Header("Grounded")]
        public bool grounded;

        [Header("AnimHash")]
        int isWalkingHash;
        int isRunningHash;
       
        int isSelectedHash;
       
        int isCrouchingHash;

        [Header("Jump")]
        public bool HasJumped;
        bool isJumping = false;
        int isJumpingHash;
        bool isJumpPressed;

        [Header("Movement")]
        bool isMovementPressed;
        public Vector3 currentMovement;

        [Header("Run")]
        bool isRunPressed;
        Vector3 currentRunMovement;
        public float runMultiplier = 2.0f;

        [Header("Crouch")]
        public bool isCrouchPressed = false;
        public bool isCrouching = false;
        public bool crouched;
        public float crouchSpeed = 1;
        public float normalHeight = 2;
        public float crouchHeight = 0.5f;
        public Vector3 offset;


        [Header("Select")]

        public bool isSelectPressed;


        public Animator animator;


        public float rotationFactorPerFrame = 15.0f;


        public ClimbingCharacter characterClimb;

        //Dylan
        private bool isRightMouseButtonPressed;
        //Dylan

        public PlayerInput input;

        public void OnMove(InputAction.CallbackContext context)
        {


            currentMovementInput = context.ReadValue<Vector2>();


            animator.SetBool("isWalking", true);

            isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

        }

        public void OnSelect(InputAction.CallbackContext context)
        {

            isSelectPressed = context.ReadValueAsButton();


        }

        public void OnJump(InputAction.CallbackContext context)
        {
            isJumpPressed = context.ReadValueAsButton();


            Jump();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            isRunPressed = context.ReadValueAsButton();
        }

      


        private void Awake()
        {
            playersCapsuleCollider = GetComponent<CapsuleCollider>();



            isWalkingHash = Animator.StringToHash("isWalking");
            isRunningHash = Animator.StringToHash("isRunning");
            isJumpingHash = Animator.StringToHash("isJumping");
            isCrouchingHash = Animator.StringToHash("isCrouching");

            isSelectedHash = Animator.StringToHash("isSelected");
            // isRopeHash = Animator.StringToHash("isRope");
            //isLadderHash = Animator.StringToHash("isLadder");



        }
        public void FixedUpdate()
        {
            if (CanMove)
            {
                Move();
                handleRotation();
                handleAnimation();
                handleCrouch();

            }
            else if (!CanMove)
            {
                return;
            }


        }

        private void LateUpdate()
        {

            if (grounded)
            {
                animator.SetBool(isJumpingHash, false);
                HasJumped = false;
            }


        }

        void Jump()
        {
            if (CanMove)
            {
                if (isJumpPressed && HasJumped == false)
                {
                    Debug.Log("Jumping");

                    Vector3 jumpForces = Vector3.zero;

                    Debug.Log("jumping");

                    if (grounded)
                    {
                        jumpForces = Vector3.up * jumpForce;
                        animator.SetBool(isJumpingHash, true);
                        grounded = false;

                    }

                    rb.AddForce(jumpForces, ForceMode.VelocityChange);
                    HasJumped = true;
                }
            }

            else if (!CanMove)
            {
                return;
            }



        }
        public void OnNotHolding(InputAction.CallbackContext context)
        {

            InputManager.Instance.charging = false;
        }

        public void OnHolding(InputAction.CallbackContext context)
        {
            InputManager.Instance.charging = true;
        }

        public Vector3 moveDirection;


        public Vector2 currentMovementInput;

        public void Move()
        {
            if (characterClimb.isClimbingLadder)
            {
                return;
            }

            Vector3 moveDirection = CameraForward() + CameraRight();
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);


            targetVelocity = moveDirection * speed;

            //Align Direction

            if (!invertInputs)
            {
                targetVelocity.x = moveDirection.x * currentMovementInput.x * speed;
                targetVelocity.z = moveDirection.z * currentMovementInput.y * speed;
            }
            else
            {
                targetVelocity.x = moveDirection.x * currentMovementInput.y * speed;
                targetVelocity.z = moveDirection.z * currentMovementInput.x * speed;
            }

            currentMovement.x = targetVelocity.x;

            currentMovement.z = targetVelocity.z;

            currentRunMovement.x = targetVelocity.x * runMultiplier;

            currentRunMovement.z = targetVelocity.z * runMultiplier;


            // = transform.TransformDirection(targetVelocity);


            //Calculate forces

            Vector3 velocityChange = (targetVelocity - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

            Vector3.ClampMagnitude(velocityChange, maxForce);

            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            if (isRunPressed)
            {
                rb.MovePosition(rb.position + currentRunMovement * Time.deltaTime);
            }
            else
            {
                rb.MovePosition(rb.position + currentMovement * Time.deltaTime);
            }

            if (characterClimb.isClimbingLadder)
            {
                characterClimb.HandleClimbingMovement();

            }

        }


        public Vector3 CameraForward()
        {
            Vector3 camPos = cam.position;
            Vector3 camForward = cam.position + cam.forward;
            camPos.y = 0;
            camForward.y = 0;

            Vector3 direction = (camPos - camForward).normalized;

            return direction;


        }


        public Vector3 CameraRight()
        {
            Vector3 camPos = cam.position;
            Vector3 camRight = cam.position + cam.right;
            camPos.y = 0;
            camRight.y = 0;

            Vector3 direction = (camPos - camRight).normalized;

            return direction;
        }


        void handleRotation()
        {
            Vector3 positionToLookAt;

            positionToLookAt.x = currentMovement.x;
            positionToLookAt.y = 0;
            positionToLookAt.z = currentMovement.z;

            Quaternion currentRotation = transform.rotation;

            if (isMovementPressed)
            {
                if (characterClimb.isClimbingLadder)
                {
                    return;
                }

                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

                //if (invertInputs)
                //    targetRotation.eulerAngles = targetRotation.eulerAngles + new Vector3(0, 90, 0);

                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            }

        }

        public bool invertInputs;

        void handleAnimation()
        {
            bool isWalking = animator.GetBool(isWalkingHash);
            bool isRunning = animator.GetBool(isRunningHash);
           
            bool isSelected = animator.GetBool(isSelectedHash);

            //Walking Controls-------------------------------
            if (isMovementPressed && !isWalking)
            {
                animator.SetBool(isWalkingHash, true);
                //Debug.Log("WalkingAnimatorPressed");

            }


            else if (!isMovementPressed && isWalking)
            {
                animator.SetBool(isWalkingHash, false);
                // Debug.Log("WalkingAnimatorPressedoff");

            }

            //Running Controls-------------------------------
            if ((isMovementPressed && isRunPressed) && !isRunning)
            {
                animator.SetBool(isRunningHash, true);
                //Debug.Log("RunningAnimatorPressed");
            }
            else if ((!isMovementPressed || !isRunPressed) && isRunning)
            {
                animator.SetBool(isRunningHash, false);
                //  Debug.Log("RunningAnimatorPressedoff");
            }


            // ! means not


            if ((isSelectPressed) && !isSelected)
            {

                animator.SetBool(isSelectedHash, true);

                Debug.Log("E ANIMATED");

            }

            else if ((!isSelectPressed) && isSelected || isMovementPressed || isMovementPressed || isRunPressed || isCrouchPressed)
            {
               Debug.Log("E NOMORE");
               animator.SetBool(isSelectedHash, false);
            }

           
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            isCrouchPressed = context.ReadValueAsButton();
        }



        public void Enabled()
        {

            CanMove = true;

        }

        public void TurnOffMovement()
        {
            CanMove = false;
        
        }

        private void StartCrouch()
        {
            animator.SetBool(isCrouchingHash, true);
            Debug.Log("Crouching");

            // Debug.Log("CrouchAnimator");

            playersCapsuleCollider.height = playersCapsuleCollider.height; // settng the speed he can go whilst crouching
                                             //+ speed - crouchSpeed * Time.deltaTime;

            speed = crouchSpeed;
            // Debug.Log("Crouched");

            playersCapsuleCollider.height = crouchHeight;
            //isCrouching = true;
        }

        private void handleCrouch()
        {
            if (isCrouchPressed)
            {
                isCrouching = !isCrouching;
                Debug.Log("Swapping crouch");
                if (isCrouching)
                {
                    StartCrouch();
                }
                else
                {
                    StopCrouch();
                }

                isCrouchPressed = false;
            }

            if (isCrouching)
            {
                if (isMovementPressed)
                {
                    // Debug.LogWarning("pepeppeepp");
                    animator.SetFloat("crouchSpeed", 1);
                }
                else
                {
                    animator.SetFloat("crouchSpeed", 0);
                }
            }
        }
        private void StopCrouch()
        {
            animator.SetBool(isCrouchingHash, false);
            Debug.Log("Stop Crouching");
            playersCapsuleCollider.height = playersCapsuleCollider.height;
            //+ speed + crouchSpeed + speed * Time.deltaTime;
            speed = 2;
            playersCapsuleCollider.height = normalHeight;
            //isCrouching = false;
        }

        public void SetGrounded(bool state)
        {
            grounded = state;
        }


    






    }

   

}

