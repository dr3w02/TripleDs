using Microsoft.Win32.SafeHandles;
using Platformer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Tilemaps.Tilemap;



public class characterMovement : MonoBehaviour
{
    public GameObject RopeBottom;
    public Transform cam;

    public Animator animator;

    CustomInputs input; // calls to the input manager

    public Transform orientation;

    Rigidbody rb;


    int isWalkingHash;
    int isRunningHash;
    int isCrouchingHash;
    int isPullingHash;


    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    public float speed;
    bool isMovementPressed;
    bool isRunPressed;
    bool isCrouchPressed = false;
    bool isPullPressed = false;

    //Things for ground check
    public float distToGround = 1f;

    //things for climb and swing
    private bool isClimbingLadder;
    public CharacterController characterController;


    Transform currentSwingable;
    ConstantForce myConstantForce;
    public Vector3 vineVelocityWhenGrabbed;
    public float climbSpeed = 2f;

    public Vector3 targetDirection;
    //bool swinging = false;

    Vector3 raycastOffset = new Vector3(0, 0.5f, 0);

    private Vector3 lastGrabLadderDirection;

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

    //roation bool
    public bool isRotating;

    public Transform player;

    /// </summary>
    // For CheckPoints

    [SerializeField] private GameObject _checkpointsParents;
    public GameObject[] _checkPointsArray;

    private Vector3 _startingPoint;

    private const string SAVE_CHECKPOINT_INDEX = "Last_checkpoint_index";

    //Dylan
    private bool isRightMouseButtonPressed;
    //Dylan




    void Awake()
    {
        input = new CustomInputs();



       

        //characterController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();

        myConstantForce = GetComponent<ConstantForce>();

        rb = GetComponent<Rigidbody>();

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

        //Dylan
        input.CharacterControls.Pull.started += OnRightMouseButtonDown;
        input.CharacterControls.Pull.canceled += OnRightMouseButtonUp;
        //Dylan

        input.CharacterControls.Select.started += onSelect;
        input.CharacterControls.Select.performed += onSelect;
        input.CharacterControls.Select.canceled += onSelect;



        input.Enable();

        setupJumpVaribles();
    }
    

    //Dylan
    private void OnRightMouseButtonDown(InputAction.CallbackContext context)
    {
        isRightMouseButtonPressed = true;
        // Disable run, jump, and crouch controls
        input.CharacterControls.Run.Disable();
        input.CharacterControls.Jump.Disable();
        input.CharacterControls.Crouch.Disable();
    }

    private void OnRightMouseButtonUp(InputAction.CallbackContext context)
    {
        isRightMouseButtonPressed = false;
        // Re-enable run, jump, and crouch controls
        input.CharacterControls.Run.Enable();
        input.CharacterControls.Jump.Enable();
        input.CharacterControls.Crouch.Enable();
    }
    //Dylan
    
    public void RespawnPlayer()
    {
        gameObject.transform.position = _startingPoint;
        //TurnOffMovement();
        //currentMovement = Vector3.zero;

        //Debug.Log("WorkingReset");



    }

    private void loadCheckPoints()
    {
        _checkPointsArray = new GameObject[_checkpointsParents.transform.childCount];

        int index = 0;

        foreach (Transform singleCheckpoint in _checkpointsParents.transform)
        {
            _checkPointsArray[index] = singleCheckpoint.gameObject;
            index++;

        }
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
        if (!isJumping && characterController.isGrounded && isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);

            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity * .9f;
            currentRunMovement.y = initialJumpVelocity * .5f;
            //Debug.Log("JumpAnimationPlayed");

            
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded && isGrounded)
        {
            isJumping = false;

            animator.SetBool(isJumpingHash, false);
        }

        else
        {
            animator.SetBool(isJumpingHash, false);
        }


    }

    //Dylan edits - when pulling/pushing disable other controls
    void onRun(InputAction.CallbackContext context)
    {
        if (!isRightMouseButtonPressed)
        {
            isRunPressed = context.ReadValueAsButton();
        }

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
        if (!isRightMouseButtonPressed)
        {
            isJumpPressed = context.ReadValueAsButton();
        }
        isJumpPressed = context.ReadValueAsButton();
        //Debug.Log("JumpPressed");
    }
   
    void onCrouch(InputAction.CallbackContext context)
    {
        if (!isRightMouseButtonPressed)
        {
            isCrouchPressed = context.ReadValueAsButton();
        }
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
    
    private void Climables()
    {


        Debug.DrawRay(orientation.position + raycastOffset, orientation.forward * 1f, Color.magenta);

        //not climbing the ladder


        if (isClimbingLadder)
        {

            characterController.enabled = false;
            isClimbingLadder = true;

            // Lock the players position to the X and Z coordinates of the ladder
            //Vector3 currentPosition = transform.position;
            //currentPosition.x = currentSwingable.position.x; // Lock X to the ladders X
            //currentPosition.z = currentSwingable.position.z; // Lock Z to the ladders Z
            //transform.position = currentPosition;


            // Adjust the upward movement to match the ladder's "up" direction
            //Vector3 ladderUpDirection = lastGrabLadderDirection;



            if (currentMovementInput.y == -1)
            {

                ///myConstantForce.enabled = false;
                //transform.position = currentSwingable.position;
                currentMovementInput.x = 0;

                Debug.Log("uppies!");
                characterController.enabled = false;
                transform.Translate(Vector3.up * Time.deltaTime * 2f);


            }

            else if (currentMovementInput.y == 0)
            {
                rb.velocity = Vector3.zero;

            }



            isGrounded = true;


            if (currentMovementInput.y == 1)
            {

                Debug.Log("uppies!");
                currentMovementInput.x = 0;
                characterController.enabled = false;

                rb.MovePosition(transform.position + Vector3.down * 2 * Time.deltaTime);

            }

            else if (currentMovementInput.y == 0)
            {
                rb.velocity = Vector3.zero;

            }


            isGrounded = true;


        }



    }
    


    private void CheckForLadder()
    {

        float ladderGrabDistance = .1f;

        int climbableLayer = LayerMask.GetMask("Climbable");


        Debug.DrawRay(orientation.position + raycastOffset, orientation.forward * ladderGrabDistance, Color.magenta);

        if (Physics.Raycast(orientation.position + raycastOffset, orientation.forward, out RaycastHit hit, ladderGrabDistance, climbableLayer))
        {

            if (!isClimbingLadder && isPullPressed)
            {
                Debug.Log("Raycast hit " + hit.collider.name);

  

                GrabLadder(hit.normal);

            }
            else if (isClimbingLadder && (!isPullPressed || hit.collider == null))
            {
                DropLadder();
            }
        }
        else if (isClimbingLadder)
        {

            DropLadder();

        }



    }

    

    private void OnTriggerEnter(Collider other)
    {
        int checkPointIndex = -1;
        checkPointIndex = Array.FindIndex(_checkPointsArray, match => match == other.gameObject);
        if (checkPointIndex != -1)
        {
            PlayerPrefs.SetInt(SAVE_CHECKPOINT_INDEX, checkPointIndex);
            _startingPoint = other.gameObject.transform.position;
            other.gameObject.SetActive(false);
        }

    }

    
    private void GrabLadder(Vector3 lastGrabLadderDirection)
    {

        //rb.isKinematic = false;
        isClimbingLadder = true;
        this.lastGrabLadderDirection = lastGrabLadderDirection.normalized;

        //currentMovement.x = 0f;
        //currentMovement.y = climbSpeed;
        //currentMovement.z = 0f;

        //Adding the sway when you grab
        if (gameObject.tag == "Vine")
        {
            RopeBottom.GetComponent<Rigidbody>().AddForce(orientation.forward * currentMovement.y, ForceMode.Acceleration);
            RopeBottom.GetComponent<Rigidbody>().AddForce(orientation.right * currentMovement.x, ForceMode.Acceleration);

        }

        

    }

    

    private void DropLadder()
    {
        isClimbingLadder = false;
        //rb.isKinematic = true;
        Debug.Log("Falling off");
        characterController.enabled = true;
        rb.velocity = Vector3.zero;
        handleGravity();
        Debug.Log("DROPlADDER");

      
    }


    void OnMovementInput(InputAction.CallbackContext context)
    {

        currentMovementInput = context.ReadValue<Vector2>();
        Vector3 moveDirection = CameraForward() + CameraRight();

        currentMovement.x = moveDirection.x * currentMovementInput.x * speed;
        currentMovement.z = moveDirection.z * currentMovementInput.y * speed;
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

        if (characterController.isGrounded && isGrounded)
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

            animator.SetBool(isJumpingHash, false);


            isGrounded = true;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVlecocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVlecocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;

            animator.SetBool(isJumpingHash, false);
            isGrounded = true;


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


    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        //CheckPoint saver

        int savedCheckPointIndex = -1;
        savedCheckPointIndex = PlayerPrefs.GetInt(SAVE_CHECKPOINT_INDEX, -1);

        if (savedCheckPointIndex != -1)
        {
            _startingPoint = _checkPointsArray[savedCheckPointIndex].transform.position; //creates the new starting point for everey checkpoint walked through spawns player in right direction
            RespawnPlayer();
        }
        else
        {
            _startingPoint = gameObject.transform.position; //no checkpoint current position of player
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        handleRotation();
        handleAnimation();
        Climables();
        CheckForLadder();

        loadCheckPoints();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }

        GroundCheck();
        handleGravity();
        handleJump();
        handleCrouch();

        // Incase player falls through ground

        //if (transform.position.y <= .10f) // checking players Y axsis
        //{
          //  RespawnPlayer();
        //
        //}

        //add unstuck button here?




    }



    public bool isGrounded = false;

    void GroundCheck()
    {
        float groundCheckDistance = distToGround + 0.2f;
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hit, groundCheckDistance))
            {
            //Debug.Log("Grounded");
            isGrounded = true;
        }

        else
        {
            //Debug.Log("NotGrounded");
            isGrounded = false;
        }

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



