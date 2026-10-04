using UnityEngine;
using AKIXA.GameplayInput;
using AKIXA.Stats;

namespace AKIXA.Movement
{
    public class CharacterSprint : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PlayerInputReader inputReader;

        [SerializeField]
        private CharacterMovement movement;

        [SerializeField]
        private CharacterResources resources;

        [Header("Sprint Settings")]
        [SerializeField]
        private float sprintMultiplier = 1.75f;

        [SerializeField]
        private float staminaCostPerSecond = 15f;

        public bool IsSprinting { get; private set; }

        private void Update()
        {
            HandleSprint();
        }

        private void HandleSprint()
        {
            if (inputReader == null ||
                movement == null ||
                resources == null)
            {
                return;
            }

            bool wantsToSprint =
                inputReader.SprintHeld &&
                movement.IsMoving;

            if (!wantsToSprint)
            {
                StopSprinting();
                return;
            }

            float staminaCost =
                staminaCostPerSecond *
                Time.deltaTime;

            if (resources.TrySpendStamina(staminaCost))
            {
                IsSprinting = true;

                movement.SetSpeedMultiplier(
                    sprintMultiplier
                );
            }
            else
            {
                StopSprinting();
            }
        }

        private void StopSprinting()
        {
            IsSprinting = false;

            if (movement != null)
            {
                movement.SetSpeedMultiplier(1f);
            }
        }

        private void OnDisable()
        {
            StopSprinting();
        }
    }
}