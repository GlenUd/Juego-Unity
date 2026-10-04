using UnityEngine;

namespace AKIXA.Stats
{
    public class StaminaRegeneration : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CharacterResources resources;

        [Header("Regeneration")]
        [SerializeField]
        private float regenerationPerSecond = 20f;

        [SerializeField]
        private float regenerationDelay = 1f;

        private float regenerationTimer;

        private void OnEnable()
        {
            if (resources != null)
            {
                resources.StaminaSpent += HandleStaminaSpent;
            }
        }

        private void OnDisable()
        {
            if (resources != null)
            {
                resources.StaminaSpent -= HandleStaminaSpent;
            }
        }

        private void Update()
        {
            if (resources == null)
                return;

            if (regenerationTimer > 0f)
            {
                regenerationTimer -= Time.deltaTime;
                return;
            }

            if (resources.CurrentStamina <
                resources.MaxStamina)
            {
                resources.RestoreStamina(
                    regenerationPerSecond *
                    Time.deltaTime
                );
            }
        }

        private void HandleStaminaSpent()
        {
            regenerationTimer = regenerationDelay;
        }
    }
}