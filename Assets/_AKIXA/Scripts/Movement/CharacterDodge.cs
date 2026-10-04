using UnityEngine;
using AKIXA.GameplayInput;
using AKIXA.Stats;

namespace AKIXA.Movement
{
    public class CharacterDodge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterMotor motor;
        [SerializeField] private CharacterVerticalMotion verticalMotion;
        [SerializeField] private CharacterResources resources;

        [Header("Dodge Settings")]
        [SerializeField] private float dodgeSpeed = 9f;
        [SerializeField] private float dodgeDuration = 0.30f;
        [SerializeField] private float staminaCost = 20f;
        [SerializeField] private float cooldown = 0.45f;

        private bool isDodging;
        private float dodgeTimer;
        private float cooldownTimer;

        private Vector3 dodgeDirection;

        public bool IsDodging => isDodging;

        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.DodgePressed += TryDodge;
            }
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.DodgePressed -= TryDodge;
            }

            StopDodge();
        }

        private void Update()
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }

            if (!isDodging)
                return;

            dodgeTimer -= Time.deltaTime;

            motor.SetExternalHorizontalVelocity(
                dodgeDirection * dodgeSpeed
            );

            if (dodgeTimer <= 0f)
            {
                StopDodge();
            }
        }

        private void TryDodge()
        {
            if (isDodging ||
                cooldownTimer > 0f ||
                inputReader == null ||
                movement == null ||
                motor == null ||
                verticalMotion == null ||
                resources == null)
            {
                return;
            }

            if (!verticalMotion.IsGrounded)
                return;

            if (!resources.TrySpendStamina(staminaCost))
                return;

            StartDodge();
        }

        private void StartDodge()
        {
            Vector3 movementDirection =
                movement.HorizontalVelocity;

            if (movementDirection.sqrMagnitude > 0.01f)
            {
                dodgeDirection =
                    movementDirection.normalized;
            }
            else
            {
                dodgeDirection = transform.forward;
            }

            transform.rotation =
                Quaternion.LookRotation(dodgeDirection);

            isDodging = true;
            dodgeTimer = dodgeDuration;
            cooldownTimer = cooldown;

            movement.SetMovementEnabled(false);

            motor.SetExternalHorizontalVelocity(
                dodgeDirection * dodgeSpeed
            );
        }

        private void StopDodge()
        {
            if (!isDodging)
                return;

            isDodging = false;

            if (motor != null)
            {
                motor.ClearExternalHorizontalVelocity();
            }

            if (movement != null)
            {
                movement.SetMovementEnabled(true);
            }
        }
    }
}