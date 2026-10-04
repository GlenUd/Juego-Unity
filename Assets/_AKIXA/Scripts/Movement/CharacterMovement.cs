using UnityEngine;
using AKIXA.GameplayInput;

namespace AKIXA.Movement
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private Transform movementOrientation;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float rotationSpeed = 12f;

        private float speedMultiplier = 1f;
        private bool movementEnabled = true;

        public Vector3 HorizontalVelocity { get; private set; }

        public bool IsMoving =>
            movementEnabled &&
            inputReader != null &&
            inputReader.MoveInput.sqrMagnitude > 0.01f;

        private void Update()
        {
            CalculateMovement();
        }

        private void CalculateMovement()
        {
            if (!movementEnabled ||
                inputReader == null ||
                movementOrientation == null)
            {
                HorizontalVelocity = Vector3.zero;
                return;
            }

            Vector2 input = inputReader.MoveInput;

            Vector3 forward = movementOrientation.forward;
            Vector3 right = movementOrientation.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 direction =
                forward * input.y +
                right * input.x;

            direction =
                Vector3.ClampMagnitude(direction, 1f);

            if (direction.sqrMagnitude > 0.01f)
            {
                RotateCharacter(direction);
            }

            HorizontalVelocity =
                direction *
                moveSpeed *
                speedMultiplier;
        }

        private void RotateCharacter(Vector3 direction)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            speedMultiplier = Mathf.Max(0f, multiplier);
        }

        public void SetMovementEnabled(bool enabled)
        {
            movementEnabled = enabled;

            if (!movementEnabled)
            {
                HorizontalVelocity = Vector3.zero;
            }
        }
    }
}