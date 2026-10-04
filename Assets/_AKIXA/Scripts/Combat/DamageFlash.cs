using System.Collections;
using UnityEngine;

namespace AKIXA.Combat
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField]
        private Damageable damageable;

        [SerializeField]
        private Renderer targetRenderer;

        [SerializeField]
        private float flashDuration = 0.08f;

        private Color originalColor;

        private void Awake()
        {
            if (targetRenderer != null)
            {
                originalColor =
                    targetRenderer.material.color;
            }
        }

        private void OnEnable()
        {
            if (damageable != null)
            {
                damageable.HealthChanged +=
                    HandleDamage;
            }
        }

        private void OnDisable()
        {
            if (damageable != null)
            {
                damageable.HealthChanged -=
                    HandleDamage;
            }
        }

        private void HandleDamage(
            float current,
            float max)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            if (targetRenderer == null)
                yield break;

            targetRenderer.material.color =
                Color.white;

            yield return new WaitForSeconds(
                flashDuration
            );

            targetRenderer.material.color =
                originalColor;
        }
    }
}