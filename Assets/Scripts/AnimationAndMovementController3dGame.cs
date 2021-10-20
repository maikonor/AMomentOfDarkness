using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class AnimationAndMovementController3dGame : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // animation hashes
    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 velocity;

    // input bools
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;
    bool isGrounded;
    bool isUsePressed;


    float targetAngle;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public float runMultiplier = 3.0f;
    public float gravity = -9.81f;
    public Transform cam;
    public float jumpHeight = 2;


    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        playerInput.CharacterControls.Use.started += onUse;
        playerInput.CharacterControls.Use.canceled += onUse;
    }


    // scripts for initializing inputs and controls
    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onUse(InputAction.CallbackContext context)
    {
        isUsePressed = context.ReadValueAsButton();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }


    // handle functions for movement/animation/mechanics and physics
    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking){
            animator.SetBool(isWalkingHash,true);
        }
        else if(!isMovementPressed && isWalking){
            animator.SetBool(isWalkingHash,false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning){
            animator.SetBool(isRunningHash, true);
        }
        else if((!isMovementPressed || !isRunPressed) && isRunning){
            animator.SetBool(isRunningHash,false);
        }
    }

    void handleRotation()
    {
        
        // Vector3 positionToLookAt;
        // Quaternion currentRotation = transform.rotation;
        
        // positionToLookAt.x = currentMovement.x;
        // positionToLookAt.y = 0.0f;
        // positionToLookAt.z = currentMovement.z;

        // if(isMovementPressed){
        //     Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        //     transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        // }
        targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f,angle,0f);
    }

    // Handling the character movement
    void handleMovement()
    {
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        if(isRunPressed) {
            characterController.Move(moveDir.normalized * currentRunMovement.magnitude * Time.deltaTime * 2);
        }
        else{
            characterController.Move(moveDir.normalized * currentMovement.magnitude * Time.deltaTime * 2);
        }
    }

    void handleJump()
    {
        if(isJumpPressed && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    void handleMechanics()
    {

    }

    void handleUI()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        handleRotation();
        handleAnimation();
        handleMovement();
        handleJump();
        handleMechanics();
        handleUI();
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}