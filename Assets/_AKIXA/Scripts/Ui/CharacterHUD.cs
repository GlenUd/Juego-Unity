using UnityEngine;
using UnityEngine.UI;
using AKIXA.Stats;

namespace AKIXA.UI
{
    public class CharacterHUD : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterResources resources;

        [Header("Bars")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider manaBar;

        private void Start()
        {
            if (resources == null)
                return;

            SetupBars();

            resources.HealthChanged += UpdateHealth;
            resources.StaminaChanged += UpdateStamina;
            resources.ManaChanged += UpdateMana;
        }

        private void OnDestroy()
        {
            if (resources == null)
                return;

            resources.HealthChanged -= UpdateHealth;
            resources.StaminaChanged -= UpdateStamina;
            resources.ManaChanged -= UpdateMana;
        }

        private void SetupBars()
        {
            healthBar.minValue = 0f;
            healthBar.maxValue = resources.MaxHealth;
            healthBar.value = resources.CurrentHealth;

            staminaBar.minValue = 0f;
            staminaBar.maxValue = resources.MaxStamina;
            staminaBar.value = resources.CurrentStamina;

            manaBar.minValue = 0f;
            manaBar.maxValue = resources.MaxMana;
            manaBar.value = resources.CurrentMana;
        }

        private void UpdateHealth(float current, float max)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }

        private void UpdateStamina(float current, float max)
        {
            staminaBar.maxValue = max;
            staminaBar.value = current;
        }

        private void UpdateMana(float current, float max)
        {
            manaBar.maxValue = max;
            manaBar.value = current;
        }
    }
}