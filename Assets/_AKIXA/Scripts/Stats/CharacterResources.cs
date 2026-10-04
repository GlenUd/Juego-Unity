using System;
using UnityEngine;

namespace AKIXA.Stats
{
    public class CharacterResources : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;

        [Header("Stamina")]
        [SerializeField] private float maxStamina = 100f;

        [Header("Mana")]
        [SerializeField] private float maxMana = 100f;

        public float CurrentHealth { get; private set; }
        public float CurrentStamina { get; private set; }
        public float CurrentMana { get; private set; }

        public float MaxHealth => maxHealth;
        public float MaxStamina => maxStamina;
        public float MaxMana => maxMana;

        public event Action<float, float> HealthChanged;
        public event Action<float, float> StaminaChanged;
        public event Action<float, float> ManaChanged;

        public event Action StaminaSpent;

        private void Awake()
        {
            CurrentHealth = maxHealth;
            CurrentStamina = maxStamina;
            CurrentMana = maxMana;
        }

        // HEALTH

        public void TakeDamage(float amount)
        {
            if (amount <= 0f)
                return;

            CurrentHealth = Mathf.Max(
                CurrentHealth - amount,
                0f
            );

            HealthChanged?.Invoke(
                CurrentHealth,
                maxHealth
            );
        }

        public void Heal(float amount)
        {
            if (amount <= 0f)
                return;

            CurrentHealth = Mathf.Min(
                CurrentHealth + amount,
                maxHealth
            );

            HealthChanged?.Invoke(
                CurrentHealth,
                maxHealth
            );
        }

        // STAMINA

        public bool TrySpendStamina(float amount)
        {
            if (amount <= 0f)
                return true;

            if (CurrentStamina < amount)
                return false;

            CurrentStamina -= amount;

            StaminaChanged?.Invoke(
                CurrentStamina,
                maxStamina
            );

            StaminaSpent?.Invoke();

            return true;
        }

        public void RestoreStamina(float amount)
        {
            if (amount <= 0f)
                return;

            CurrentStamina = Mathf.Min(
                CurrentStamina + amount,
                maxStamina
            );

            StaminaChanged?.Invoke(
                CurrentStamina,
                maxStamina
            );
        }

        // MANA

        public bool TrySpendMana(float amount)
        {
            if (amount <= 0f)
                return true;

            if (CurrentMana < amount)
                return false;

            CurrentMana -= amount;

            ManaChanged?.Invoke(
                CurrentMana,
                maxMana
            );

            return true;
        }

        public void RestoreMana(float amount)
        {
            if (amount <= 0f)
                return;

            CurrentMana = Mathf.Min(
                CurrentMana + amount,
                maxMana
            );

            ManaChanged?.Invoke(
                CurrentMana,
                maxMana
            );
        }
    }
}