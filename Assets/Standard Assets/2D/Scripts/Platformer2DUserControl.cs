using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.InputSystem;


namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private OldControls oldControls;



        private void Awake()
        {
            //m_Character = GetComponent<PlatformerCharacter2D>();
            oldControls = new OldControls();
        }

        void start()
        {
            oldControls.Land.Jump.started += onJump;
            oldControls.Land.Jump.canceled += onJump;
        }

        void onJump(InputAction.CallbackContext context)
        {
            m_Jump = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            oldControls.Enable();
        }

        private void OnDisable()
        {
            oldControls.Disable();
        }

        private void Update()
        {
            //if (!m_Jump)
            //{
                // Read the jump input in Update so button presses aren't missed.
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                //m_Jump = oldControls.Land.Jump.ReadValue<bool>();
            //}
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            //bool crouch = Input.GetKey(KeyCode.LeftControl);
            //bool crouch = oldControls.Land.Crouch.ReadValue<bool>();
            bool crouch = false;
            
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float h = oldControls.Land.Move.ReadValue<float>();
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
