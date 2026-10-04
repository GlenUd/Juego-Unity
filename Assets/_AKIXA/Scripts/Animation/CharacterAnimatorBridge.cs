using UnityEngine;
using AKIXA.Movement;

namespace AKIXA.Animation
{
    public class CharacterAnimatorBridge : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterVerticalMotion verticalMotion;

        private static readonly int SpeedHash =
            Animator.StringToHash("Speed");

        private static readonly int GroundedHash =
            Animator.StringToHash("Grounded");

        private static readonly int VerticalSpeedHash =
            Animator.StringToHash("VerticalSpeed");

        private static readonly int Attack1Hash =
            Animator.StringToHash("Attack1");

        private static readonly int Attack2Hash =
            Animator.StringToHash("Attack2");

        private static readonly int Attack3Hash =
            Animator.StringToHash("Attack3");

        private void Update()
        {
            if (animator == null || movement == null)
                return;

            float normalizedSpeed =
                Mathf.InverseLerp(
                    0f,
                    7f,
                    movement.HorizontalVelocity.magnitude
                );

            animator.SetFloat(
                SpeedHash,
                normalizedSpeed,
                0.1f,
                Time.deltaTime
            );

            if (verticalMotion != null)
            {
                animator.SetBool(
                    GroundedHash,
                    verticalMotion.IsGrounded
                );

                animator.SetFloat(
                    VerticalSpeedHash,
                    verticalMotion.VerticalVelocity
                );
            }
        }

        public void PlayAttack(int attackIndex)
        {
            if (animator == null)
                return;

            switch (attackIndex)
            {
                case 0:
                    animator.SetTrigger(Attack1Hash);
                    break;

                case 1:
                    animator.SetTrigger(Attack2Hash);
                    break;

                case 2:
                    animator.SetTrigger(Attack3Hash);
                    break;
            }
        }
    }
}