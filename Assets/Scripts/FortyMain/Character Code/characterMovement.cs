using Microsoft.Win32.SafeHandles;
using Platformer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class characterMovement : MonoBehaviour
{
    public Transform cam;

    public Animator animator;
    public CharacterController characterController;
    CustomInputs input; // calls to the input manager
   
  

    int isWalkingHash;
    int isRunningHash;  
    int isCrouchingHash;
    int isPullingHash;
  

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isCrouchPressed = false;
    bool isPullPressed = false;
   
    //change these to is 

    //[SerializeField] float speed = 5; // might've lost this dont forget to add back

    //Constants
    public float rotationFactorPerFrame = 15.0f;
    public float runMultiplier = 3.0f;
    int zero = 0;
    //fancy jump
    //9.8
    public float gravity = 9.8F;
    public float groundedGravity = -.05f;

    //Jumping Varibles

    bool isJumpPressed = false;
    public float initialJumpVelocity;
    public float maxJumpHeight = 4.0f;
    public float maxJumpTime = 0.75f;
    bool isJumping = false;
    int isJumpingHash;
    bool isJumpAnimating = false;
   

    /// <summary>
    // For Couch Up and Down

    public float crouchSpeed = 2;
    public float normalHeight = 2;
    public float crouchHeight = 0.5f;
    public Vector3 offset;

    //Selected

    public bool isSelectPressed;
    //int isSelectedHash;
    bool selectPressed;


    public Transform player;

    /// </summary>


    void Awake()
    {
        input = new CustomInputs();

        characterController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isCrouchingHash = Animator.StringToHash("isCrouching");
        isPullingHash = Animator.StringToHash("isPulling");
        //isSelectedHash = Animator.StringToHash("isSelected");
       

        input.CharacterControls.Movement.started += OnMovementInput;

        input.CharacterControls.Movement.performed += OnMovementInput;

        input.CharacterControls.Movement.canceled += OnMovementInput;


        input.CharacterControls.Run.started += onRun;
        input.CharacterControls.Run.performed += onRun;
        input.CharacterControls.Run.canceled += onRun;


        input.CharacterControls.Jump.started += onJump;
        input.CharacterControls.Jump.performed += onJump;
        input.CharacterControls.Jump.canceled += onJump;

        input.CharacterControls.Crouch.started += onCrouch;
        input.CharacterControls.Crouch.performed += onCrouch;
        input.CharacterControls.Crouch.canceled += onCrouch;


        input.CharacterControls.Pull.started += onPull;
        input.CharacterControls.Pull.performed += onPull;
        input.CharacterControls.Pull.canceled += onPull;

        input.CharacterControls.Select.started += onSelect;
        input.CharacterControls.Select.performed += onSelect;
        input.CharacterControls.Select.canceled += onSelect;

        input.Enable();

        setupJumpVaribles();
    }
    //Setting Up Jumping Physics 
    void setupJumpVaribles()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }


    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity * .9f;
            currentRunMovement.y = initialJumpVelocity * .5f;
            //Debug.Log("JumpAnimationPlayed");
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
            //animator.SetBool(isJumpingHash, false);
        }
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
        //Debug.Log("RunPressed");
    }

    void onPull(InputAction.CallbackContext context)
    {
        isPullPressed = context.ReadValueAsButton();
        ///Debug.Log("PullPressed");
    }

     void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        //Debug.Log("JumpPressed");
    }

    void onCrouch(InputAction.CallbackContext context)
    {
        isCrouchPressed = context.ReadValueAsButton();
        //Debug.Log("CrouchedPressed");
    }

    public void onSelect(InputAction.CallbackContext context)
    {
        isSelectPressed = context.ReadValueAsButton();

        Debug.Log("SelectPressed");

    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = zero;
        positionToLookAt.z = currentMovement.z;

        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //creates a new rotation based on where player is looking
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            //Debug.Log("MovementHasBeenPressed");
            //rotate the character to face the positionToLookAt

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }



    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        float avoidFloorDistance = 0.1f; //so the climb doesnt even hit the ground 
        float ladderGrabDistance = .4f;


        if (Physics.Raycast(transform.position + Vector3.up * avoidFloorDistance, currentMovementInput, out RaycastHit raycastHit, ladderGrabDistance ))  // local position is the position the player is facing 
        {
             if(raycastHit.transform.TryGetComponent(out Ladder ladder))
              {
                currentMovement.x = 0f; // stops him from sliding left and right can remove if wanted (design pref)
                currentMovement.y = currentMovement.z;
                currentMovement.z = 0f;
                characterController.isGrounded = true;
            }
        
        }
          
        currentMovementInput = context.ReadValue<Vector2>();
        Vector3 moveDirection = CameraForward() + CameraRight();

        currentMovement.x = moveDirection.x * currentMovementInput.x;
        currentMovement.z = moveDirection.z * currentMovementInput.y;
        //currentMovement.x = currentMovementInput.x;
        //currentMovement.z = currentMovementInput.y;

        currentRunMovement.x = moveDirection.x * currentMovementInput.x * runMultiplier;
        currentRunMovement.z = moveDirection.z * currentMovementInput.y * runMultiplier;
        //currentRunMovement.x = currentMovementInput.x * runMultiplier;
        //currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;

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

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isPulling = animator.GetBool(isPullingHash);

       


    
        //Walking Controls-------------------------------
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
            Debug.Log("WalkingAnimatorPressed");

        }

       
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
            Debug.Log("WalkingAnimatorPressedoff");

        }

        //Running Controls-------------------------------
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
            Debug.Log("RunningAnimatorPressed");
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
            Debug.Log("RunningAnimatorPressedoff");
        }


        // ! means not
  
    

       
        //Pulling Controls-------------------------------
        if ((isPullPressed) && !isPulling)
        {
            animator.SetBool(isPullingHash, true);
            Debug.Log("Pull animator on");
        }
        else if ((!isPullPressed) && isPulling)
        {
            animator.SetBool(isPullingHash, false);
            Debug.Log("Pull Animator off");
        }
    }


    void handleGravity()
    {

        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;

        //larger the multiplier the steeper the fall 
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {

            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
               // Debug.Log("Jumpfalse");
            }

            animator.SetBool("isJumping", false);
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;

        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f); //adding a max stops the character from fall too fast from high distances
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVlecocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVlecocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }

    }

    //Crouch Controller
    
    void handleCrouch()
    {
        bool isCrouching = animator.GetBool(isCrouchingHash);

        if (currentMovement.x != 0 || currentMovement.z != 0)
        {
            animator.SetFloat("crouchSpeed", 1);
        }
        else
        {
            animator.SetFloat("crouchSpeed", 0);
        }

        //Crouching Controls-------------------------------
        if ((isCrouchPressed) && !isCrouching)
        {
            animator.SetBool(isCrouchingHash, true);
   
           // Debug.Log("CrouchAnimator");

            
            characterController.height = characterController.height - crouchSpeed * Time.deltaTime; // settng the speed he can go whilst crouching 
                
           // Debug.Log("Crouched");

            characterController.height = crouchHeight;
         
        }
        
        if ((!isCrouchPressed) && isCrouching)
        {
            animator.SetBool(isCrouchingHash, false);
            // Debug.Log("CrouchAnimatoroff");


            characterController.height = characterController.height + crouchSpeed * Time.deltaTime;


            characterController.height = normalHeight;
        }



        if (characterController.height < normalHeight) // safe guard to keep the player from clipping into the ground 
        {

            player.localPosition = offset;
        }
        else
        {
            player.localPosition = Vector3.zero;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set Animator Reference
        
        Debug.Log("start");
        //Set ID References
       
    }

   
    // Update is called once per frame
    void Update()
    {
        
        handleRotation();
        handleAnimation();
      

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }

        handleGravity();
        handleJump();
        handleCrouch();

    }


    /// <summary>
    /// Truning on and off players movement ability 
    /// 
    public void Enabled()
    {
        input.Enable();
    }

    public void TurnOffMovement()
    {
        input.Disable();
    }
/// </summary>

 



}
