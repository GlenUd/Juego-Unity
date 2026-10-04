using UnityEngine;
using UnityEngine.UI;
using AKIXA.Combat;

namespace AKIXA.UI
{
    public class EnemyHealthUI : MonoBehaviour
    {
        [SerializeField]
        private Damageable damageable;

        [SerializeField]
        private Slider healthBar;

        [SerializeField]
        private Transform cameraTransform;

        private void Start()
        {
            if (damageable != null)
            {
                healthBar.maxValue =
                    damageable.MaxHealth;

                healthBar.value =
                    damageable.CurrentHealth;

                damageable.HealthChanged +=
                    UpdateHealth;

                damageable.Died +=
                    HandleDeath;
            }

            if (cameraTransform == null &&
                Camera.main != null)
            {
                cameraTransform =
                    Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (cameraTransform == null)
                return;

            transform.rotation =
                Quaternion.LookRotation(
                    transform.position -
                    cameraTransform.position
                );
        }

        private void UpdateHealth(
            float current,
            float max)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (damageable == null)
                return;

            damageable.HealthChanged -=
                UpdateHealth;

            damageable.Died -=
                HandleDeath;
        }
    }
}