using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerOld : MonoBehaviour
{

    [SerializeField] private float speed, sprintMultiplier;
    [SerializeField] private float jumpSpeed;
    //[SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask m_WhatIsGround; 
    private OldControls oldControls;
    private Transform groundCheck;
    private Rigidbody2D rigidbod;
    private bool m_Grounded;
    private Transform m_GroundCheck;
    const float k_GroundedRadius = .2f;

    private bool FacingRight = true;
    private Collider2D col;
    private Animator anim;

    

    private void Awake()
    {
        m_GroundCheck = transform.Find("GroundCheck");
        col = GetComponent<Collider2D>();
        oldControls = new OldControls();
        rigidbod = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        oldControls.Enable();
    }

    private void OnDisable()
    {
        oldControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        oldControls.Land.Jump.performed += _ => Jump();
    }


    void Jump()
    {
        if (m_Grounded)
        {
            rigidbod.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            Debug.Log("jump");
        }
        Debug.Log("space Pressed");
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = oldControls.Land.Move.ReadValue<float>();

        rigidbod.velocity = new Vector2(moveInput * speed, rigidbod.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        

        if (moveInput > 0 && !FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (moveInput < 0 && FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
    
    void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
        anim.SetBool("Ground", m_Grounded);

        anim.SetFloat("vSpeed", rigidbod.velocity.y);
    }

    private void Flip()
        {
            // Switch the way the player is labelled as facing.
            FacingRight = !FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
}
