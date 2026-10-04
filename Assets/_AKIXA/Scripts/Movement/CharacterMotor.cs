using UnityEngine;

namespace AKIXA.Movement
{
    [DefaultExecutionOrder(100)]
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMotor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterVerticalMotion verticalMotion;

        private CharacterController characterController;

        private Vector3 externalHorizontalVelocity;

        private void Awake()
        {
            characterController =
                GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyMotion();
        }

        private void ApplyMotion()
        {
            if (movement == null ||
                verticalMotion == null)
            {
                return;
            }

            Vector3 horizontalVelocity =
                movement.HorizontalVelocity +
                externalHorizontalVelocity;

            Vector3 velocity =
                horizontalVelocity +
                Vector3.up *
                verticalMotion.VerticalVelocity;

            characterController.Move(
                velocity * Time.deltaTime
            );
        }

        public void SetExternalHorizontalVelocity(
            Vector3 velocity)
        {
            velocity.y = 0f;
            externalHorizontalVelocity = velocity;
        }

        public void ClearExternalHorizontalVelocity()
        {
            externalHorizontalVelocity = Vector3.zero;
        }
    }
}