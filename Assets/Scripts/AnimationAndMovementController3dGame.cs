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
    bool isDarknessPressed;

    bool isDark;

    public bool teleportTip = false;
    private bool teleporter = false;

    float targetAngle;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public float runMultiplier = 3.0f;
    public float gravity = -9.81f;
    public Transform cam;
    public float jumpHeight = 1;

    // Gem Collector

    public int gemCount = 0;
    public float darknessTotalTime;
    public TextMeshProUGUI countText;
    public GameObject teleportTipText;



    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        countText.gameObject.SetActive(false);

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
        playerInput.CharacterControls.Darkness.started += onDarkness;
        playerInput.CharacterControls.Darkness.canceled += onDarkness;
    }

    void start()
    {
        gemCount = PlayerPrefs.GetInt("gems");
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
    void onDarkness(InputAction.CallbackContext context)
    {
        isDarknessPressed = context.ReadValueAsButton();
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
        // teleport back to the room
        if(teleporter && isUsePressed)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
        }

        // our Darkness™ mechanic XD
        if(isDarknessPressed && !isDark)
        {
            GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Hideable");
            Debug.Log("become dark");
            foreach(GameObject go in gameObjectArray)
            {
                go.GetComponent<Light>().enabled = false;
            }
            RenderSettings.ambientIntensity = 0f;
            isDark = true;
        }
        else if(!isDarknessPressed && isDark)
        {
            GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Hideable");
            Debug.Log("become bright");
            
            foreach(GameObject go in gameObjectArray)
            {
                go.GetComponent<Light>().enabled = true;
            }
            RenderSettings.ambientIntensity = 0.2f;
            isDark = false;
        }
    }

    void handleUI()
    {
        if(teleportTip)
        {
            teleportTipText.gameObject.SetActive(true);
        }
        if(!teleportTip)
        {
            teleportTipText.gameObject.SetActive(false);
        }
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Portal"))
        {
            teleporter = true;
            teleportTip = true;
        }
        if (other.gameObject.CompareTag("GemPickUp")) 
        {
            other.gameObject.SetActive(false);
            gemCount++;
            countText.gameObject.SetActive(true);
            TimeCalculator();
            SetCountText();
            PlayerPrefs.SetInt("new gems", gemCount);
            //PlayerPrefs.Save();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Portal"))
        {
            teleporter = false;
            teleportTip = false;
        }
    }

    void TimeCalculator()
    {
        darknessTotalTime = 15 * gemCount;
        darknessTotalTime = darknessTotalTime / 60;
        PlayerPrefs.SetFloat("new dark", darknessTotalTime);

    }

    void SetCountText()
	{
		countText.text = "Colected Gems: " + gemCount.ToString() + "\n" + "Collected Darkness time: " + darknessTotalTime.ToString();
	}
}
