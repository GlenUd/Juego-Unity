using System;
using UnityEngine;

namespace AKIXA.Combat
{
    public class Damageable : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;

        [Header("Death")]
        [SerializeField] private bool destroyOnDeath = false;
        [SerializeField] private float destroyDelay = 1f;

        public float CurrentHealth { get; private set; }
        public float MaxHealth => maxHealth;
        public bool IsDead { get; private set; }

        public event Action<float, float> HealthChanged;
        public event Action Died;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead || damage <= 0f)
                return;

            CurrentHealth = Mathf.Max(
                CurrentHealth - damage,
                0f
            );

            HealthChanged?.Invoke(
                CurrentHealth,
                maxHealth
            );

            Debug.Log(
                $"{name} recibió {damage} de daño. " +
                $"Vida: {CurrentHealth}/{maxHealth}"
            );

            if (CurrentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead)
                return;

            IsDead = true;

            Debug.Log($"{name} murió.");

            Died?.Invoke();

            if (destroyOnDeath)
            {
                Destroy(gameObject, destroyDelay);
            }
        }
    }
}