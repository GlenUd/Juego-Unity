using UnityEngine;
using AKIXA.GameplayInput;
using AKIXA.Stats;

namespace AKIXA.Movement
{
    public class CharacterJump : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PlayerInputReader inputReader;

        [SerializeField]
        private CharacterVerticalMotion verticalMotion;

        [SerializeField]
        private CharacterResources resources;

        [Header("Jump Settings")]
        [SerializeField]
        private float jumpHeight = 1.5f;

        [SerializeField]
        private float staminaCost = 10f;

        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.JumpPressed += HandleJump;
            }
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.JumpPressed -= HandleJump;
            }
        }

        private void HandleJump()
        {
            if (verticalMotion == null ||
                resources == null)
            {
                return;
            }

            if (!verticalMotion.IsGrounded)
                return;

            if (!resources.TrySpendStamina(staminaCost))
                return;

            verticalMotion.RequestJump(jumpHeight);
        }
    }
}