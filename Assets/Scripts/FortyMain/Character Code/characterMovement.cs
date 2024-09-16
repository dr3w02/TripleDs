using Microsoft.Win32.SafeHandles;
using Platformer;
using System;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;




public enum CharacterState { Idle, Walking, Sprinting, Climbing, Pushing, Falling }
public class characterMovement : MonoBehaviour
{
    public CharacterState currentState;
    public Transform cam;

    public Animator animator;

    CustomInputs input; // calls to the input manager
    InputManager inputManager;
    public Transform orientation;

    Rigidbody rb;

    
    public Vector3 collision = Vector3.zero;

    [Header("Controls")]

    int isWalkingHash;
    int isRunningHash;
    int isCrouchingHash;
    int isPullingHash;
    int isRopeHash;
    int isLadderHash;

   
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
    public bool isClimbingLadder;
    public CharacterController characterController;
    public LayerMask climbableLayer;


    Transform currentSwingable;
    ConstantForce myConstantForce;
    public Vector3 vineVelocityWhenGrabbed;

  

    public float climbSpeed = 2f;

    public float dropSpeed = 9.8f;

    private bool isClimbing = false;

    public Vector3 targetDirection;
    //bool swinging = false;

    Vector3 raycastOffset = new Vector3(0, 0.5f, 0);


    public Vector3 ClimbOffset = new Vector3(0, 0, -0.8f);

    private Vector3 lastGrabLadderDirection;

    //change these to is

    //[SerializeField] float speed = 5; // might've lost this dont forget to add back

    //Constants
    public float rotationFactorPerFrame = 15.0f;

    public float runMultiplier = 3.0f;
    int zero = 0;
    //fancy jump
    //9.8


    [Header("Gravity")]
    public float gravity = 9.8F;
    public float groundedGravity = -.05f;


    //Jumping Varibles

    [Header("Jump")]
    bool isJumpPressed = false;
    public float initialJumpVelocity;
    public float maxJumpHeight = 4.0f;
    public float maxJumpTime = 0.75f;
    bool isJumping = false;
    int isJumpingHash;
    bool isJumpAnimating = false;

 

    /// <summary>



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


    [Header("SlopeHandling")]

    // public float maxSlopeAngle;

    //public float playerHeight = 0.3057787f;


    // private RaycastHit slopeHit;




    public bool gravityEnabled = true;


    [Header("Crouched")]
    public bool crouched;
    // For Couch Up and Down

    public float crouchSpeed = 2;
    public float normalHeight = 2;
    public float crouchHeight = 0.5f;
    public Vector3 offset;

    [Header("Climbing")]

   
    public bool ClimableFound;
    
    public GameObject RopeBottom;

    public Vector3 climbRayOffset = new Vector3(0, 3f, 0);

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

     

        //CheckPoint saver
        Collider collider = GetComponent<Collider>();
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

        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

        PlayerPrefs.SetFloat("PlayerXR", player.transform.rotation.x);
        PlayerPrefs.SetFloat("PlayerYR", player.transform.rotation.y);
        PlayerPrefs.SetFloat("PlayerZR", player.transform.rotation.z);

    }


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
        isRopeHash = Animator.StringToHash("isRope");
        isLadderHash = Animator.StringToHash("isLadder");


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

        // this is to charge up the music box might change my interacables to match this control?
        input.CharacterControls.Hold.performed += onHolding;
        input.CharacterControls.Hold.canceled += onNotHolding;

        input.Enable();

        setupJumpVaribles();
    }



    private void onNotHolding(InputAction.CallbackContext context)
    {

        InputManager.Instance.charging = false;
    }

    private void onHolding(InputAction.CallbackContext context)
    {
        InputManager.Instance.charging = true;
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
        if (context.performed)
        {
            if (!isRightMouseButtonPressed)
            {
                isCrouchPressed = context.ReadValueAsButton();


            }
        }
       
        isCrouchPressed = context.ReadValueAsButton();
        Debug.Log("CrouchedPressed");
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
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovement.z;

        
        Quaternion currentRotation = transform.rotation;


        

      

        if (isMovementPressed)
        {

           Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);


           transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        }




       // else if (ClimableFound == true)
        //{

            //Quaternion climbingRotation = transform.rotation;

            //climbingRotation.x = 0; 

            //transform.rotation = climbingRotation;

           // return;
       // }


    }


    private void CheckForLadder()
    {
  
            if (lastHit != null || ClimableFound == true)
            {
          
                if (isPullPressed)
                {

                    gravityEnabled = false;
                    isGrounded = true;


                    if (!isClimbingLadder)
                    {

                        AlignToLadder();
                    
                        isClimbingLadder = true;

                        HandleClimbingMovement();
                    }

                 
                }
                
            }



      


       // else if (isClimbingLadder || ClimableFound == false)
        //{
         

          //  isClimbingLadder = false;
          //  DropLadder();
      //  }
    }

    void AlignToLadder()
    {
        if (lastHit != null)
        {
            Vector3 ladderForward = lastHit.transform.forward;

            //position


            Vector3 lastHitPosition = lastHit.transform.position;
            Vector3 newPosition = lastHitPosition + ClimbOffset;
            //player.transform.position = newPosition;
            this.transform.position = newPosition;


            //Rotation



            Vector3 ladderRotation = lastHit.transform.rotation.eulerAngles.normalized;



            float ladderXRotation = -ladderRotation.x;
            float ladderZRotation = 0f;
            float ladderYRotation = -ladderRotation.y;


            Vector3 currentRotation = player.transform.rotation.eulerAngles;
            Vector3 currentRotationThis = this.transform.rotation.eulerAngles;


            this.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotationThis.y, ladderZRotation).normalized;

            player.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotation.y, ladderZRotation).normalized;
            //player.transform.rotation = Quaternion.Euler(ladderXRotation, currentRotation.y, ladderZRotation);


            Debug.LogWarning(lastHit.transform.position);
            Debug.Log(player.transform.position);
            // player.transform.position = lasthit.point;


            //this.transform.rotation = Quaternion.Euler(0f, ladderYRotation, 0f);

           
        }
        else
        {
            Debug.LogError("No valid lastHitTransform! Cannot align to ladder.");
        }
    }



    private void HandleClimbingMovement()
    {
        if (lastHit != null || ClimableFound == true)
        {

           
            // Move up if input is -1 climbing up
            if (currentMovementInput.y == -1)
            {
                if (isMovementPressed)
                {
                    Vector3 climbDirection = Vector3.ProjectOnPlane(Vector3.up * climbSpeed, lastHitNormal);


                    characterController.Move(climbDirection * Time.deltaTime);
                    // rb.MovePosition(player.transform.position + orientation.up * climbSpeed * Time.deltaTime);
                    //rb.AddForce(Vector3.up, ForceMode.Impulse);
                    isClimbing = true;
                    Debug.DrawRay(player.transform.position, Vector3.up * 100f, Color.yellow);
                    Debug.LogWarning("CLIMBING UP");


                }
               


            }
            // Move down if input is 1 climbing down


            else if (currentMovementInput.y == 1)
            {
                if (isMovementPressed)
                {
                    Vector3 climbDirection = Vector3.ProjectOnPlane(Vector3.down * climbSpeed, lastHitNormal);

                    isClimbing = true;
                    characterController.Move(climbDirection * Time.deltaTime);

                    // characterController.enabled = false;
                    // rb.freezeRotation = true;

                    //rb.MovePosition(Vector3.down * climbSpeed * Time.deltaTime);



                    //characterController.transform.position += Vector3.up / climbSpeed;
                    Debug.LogWarning("CLIMBING DOWN");
                }
               
            }

            else
            {
                if (!isMovementPressed)
                {
                    Vector3 climbDirection = Vector3.ProjectOnPlane(Vector3.up * 0f, lastHitNormal);
                }
                if (isClimbing)
                {
                    DropLadder();
                }

                if (!isPullPressed)
                {
                    DropLadder();
                }
            }



                if (gameObject.tag == "Vine")
            {
                RopeBottom.GetComponent<Rigidbody>().AddForce(orientation.forward * currentMovement.y, ForceMode.Acceleration);
                RopeBottom.GetComponent<Rigidbody>().AddForce(orientation.right * currentMovement.x, ForceMode.Acceleration);
            }

        }


    }

    private void DropLadder()
    {
        // Stop climbing and reset the climb state
        isClimbing = false;

        // Apply downward movement or gravity to simulate dropping
        Vector3 dropDirection = Vector3.down * dropSpeed;

        // Move the character down when dropping
        characterController.Move(dropDirection * Time.deltaTime);

        Debug.Log("Dropped from climbing.");

        float playerXRotation = PlayerPrefs.GetFloat("PlayerXR");
        float playerYRotation = PlayerPrefs.GetFloat("PlayerYR");
        float playerZRotation = PlayerPrefs.GetFloat("PlayerZR");

      
        this.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));

        Quaternion playerRotation = Quaternion.Euler(playerXRotation, playerYRotation, playerZRotation);

        //player.transform.rotation = playerRotation;


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


        /*
        if (OnSlope())
        {
            // Vector3 slopeMovement = GetSlopeMoveDirection() * speed;
            //characterController.Move(slopeMovement * Time.deltaTime);


        
            rb.AddForce(GetSlopeMoveDirection() * speed * 20f, ForceMode.Force);
        }

        */


    }




    //CheckPonts
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

        if ((isCrouchPressed) && !isCrouching)
        {
            crouched = true;
        }
        else if ((isCrouchPressed) && isCrouching)
        {
            crouched = false;
            Debug.Log("CROUCHEDAGAIN");
        }
        

        if (crouched == true)
        {

            if (currentMovement.x != 0 || currentMovement.z != 0 )
            {
                animator.SetFloat("crouchSpeed", 1);
            }
            else
            {
                animator.SetFloat("crouchSpeed", 0);
            }

            
                animator.SetBool(isCrouchingHash, true);

                // Debug.Log("CrouchAnimator");


                characterController.height = characterController.height - crouchSpeed * Time.deltaTime; // settng the speed he can go whilst crouching

                // Debug.Log("Crouched");

                characterController.height = crouchHeight;
            
            

            
        }

       

        if (crouched == false)
        {
           
                animator.SetBool(isCrouchingHash, false);
                // Debug.Log("CrouchAnimatoroff");

                crouched = false;
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


    /*
    private bool OnSlope()
    {
       
        if (Physics.Raycast(player.transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
           float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
           return angle < maxSlopeAngle && angle != 0;

        }

        if (Physics.Raycast(this.transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }


        return false;
    }
    

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(currentMovement, slopeHit.normal).normalized;
    }

*/
    public RaycastHit lastHitTemp;  
    public Vector3 lastHitNormal;
    Transform lastHit;


    public float rayDistance = 3;
   
    void DetectClimbable()
    {
        RaycastHit hit;

        // Perform the raycast and check if it hits an object
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, climbableLayer))
        {

            Debug.Log("Climbable object detected!");

            // Store the hit information
            lastHitNormal = hit.normal;
            lastHit = hit.transform;

            CheckForLadder();
            ClimableFound = true;
        }


        else
        {
            Debug.LogWarning("Raycast did not hit anything.");


            DropLadder();
            lastHitNormal = Vector3.zero;
            lastHit = null;
        }
    }

        private void FixedUpdate()
    {
        DetectClimbable();
        //switch (currentState)
        //{
        //   case CharacterState.Idle:


        //      break;
        //  case CharacterState.Walking:


        //      break;
        //   case CharacterState.Sprinting:


        //    break;
        //    case CharacterState.Climbing:


        //   break;
        //  case CharacterState.Pushing:


        //    break;
        //  case CharacterState.Falling:


        //   break;
        // }

        //float ladderGrabDistance = 10f;
        //float sphereRadius = 0.5f;


        handleRotation();

        handleAnimation();
        
     

        loadCheckPoints();

        /*
        var ray = new Ray(this.transform.position, this.transform.forward);

        RaycastHit hitClimable;

        Debug.DrawRay(this.transform.position, this.transform.forward * ladderGrabDistance, Color.red);

        Debug.Log("Ray Origin: " + this.transform.position + ", Ray Direction: " + this.transform.forward * ladderGrabDistance);

        if (Physics.Raycast(ray, out hitClimable, ladderGrabDistance))
        {
            if (hitClimable.collider.CompareTag("Ladder"))
            {
                lastHit = hitClimable.transform.gameObject;
                collision = hitClimable.point;
                ClimableFound = true;
                Debug.Log("Hit object: " + hitClimable.transform.gameObject.name);

            }

            
        }
        else
        {
            if (lastHit == null)
            {
                Debug.LogWarning("Raycast did not hit anything and LastHit is null.");
                ClimableFound = false;
            }
            else
            {
                ClimableFound = false;
                Debug.LogWarning("Raycast did not hit anything but LastHit is not null.");
            }
        }

        */

        if (isRunPressed)
        {
                if(characterController != null)
                    characterController.Move(currentRunMovement * Time.deltaTime);
            }
            else
            {
                if(characterController != null)
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

 

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, 3);


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



