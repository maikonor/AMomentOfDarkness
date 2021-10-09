using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateControllerTest : MonoBehaviour
{
    public Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        Debug.Log(animator);
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        bool forwardPress = Input.GetKey("w");
        bool backwardPress = Input.GetKey("s");
        bool leftPress = Input.GetKey("a");
        bool rightPress = Input.GetKey("d");
        bool runPress = Input.GetKey("left shift");
        bool jumpPress = Input.GetKey("space");

        //walking animation
        if(!isWalking && (forwardPress || backwardPress || leftPress || rightPress))
        {
            animator.SetBool(isWalkingHash, true);            
        }
        if((isWalking || isRunning) && !forwardPress && !backwardPress && !leftPress && !rightPress)
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
        }
        
        //running animation
        if(!isRunning && runPress)
        {
            animator.SetBool(isRunningHash, true);
        }
        if(isRunning && !runPress)
        {
            animator.SetBool(isRunningHash, false);
        }

        //jump animation
        if(!isJumping && jumpPress)
        {
            animator.SetBool(isJumpingHash, true);
        }
        if(isJumping && !jumpPress)
        {
            animator.SetBool(isJumpingHash, false);
        }
    }
}