using UnityEngine;

namespace AKIXA.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterVerticalMotion : MonoBehaviour
    {
        [Header("Gravity Settings")]
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float groundedForce = -2f;

        [Header("Jump Assistance")]
        [SerializeField] private float coyoteTime = 0.12f;
        [SerializeField] private float jumpBufferTime = 0.12f;

        private CharacterController characterController;

        private float verticalVelocity;
        private float coyoteCounter;
        private float jumpBufferCounter;
        private float pendingJumpHeight = 1.5f;

        public float VerticalVelocity => verticalVelocity;

        public bool IsGrounded =>
            characterController != null &&
            characterController.isGrounded;

        private void Awake()
        {
            characterController =
                GetComponent<CharacterController>();
        }

        private void Update()
        {
            UpdateGroundState();
            UpdateJumpBuffer();
            ApplyGravity();
        }

        private void UpdateGroundState()
        {
            if (IsGrounded)
            {
                coyoteCounter = coyoteTime;

                if (verticalVelocity < 0f)
                {
                    verticalVelocity = groundedForce;
                }
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }

        private void UpdateJumpBuffer()
        {
            if (jumpBufferCounter > 0f)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter > 0f &&
                coyoteCounter > 0f)
            {
                ExecuteJump();
            }
        }

        private void ApplyGravity()
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        public void RequestJump(float jumpHeight)
        {
            pendingJumpHeight =
                Mathf.Max(0.1f, jumpHeight);

            jumpBufferCounter = jumpBufferTime;
        }

        private void ExecuteJump()
        {
            verticalVelocity = Mathf.Sqrt(
                2f *
                Mathf.Abs(gravity) *
                pendingJumpHeight
            );

            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }
}