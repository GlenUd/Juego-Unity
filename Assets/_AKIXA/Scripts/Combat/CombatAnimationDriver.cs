using System.Collections;
using UnityEngine;

namespace AKIXA.Combat
{
    public class CombatAnimationDriver : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform weaponSocket;

        private Coroutine currentRoutine;

        private Vector3 restingPosition;
        private Quaternion restingRotation;

        private void Awake()
        {
            SaveRestingTransform();
        }

        public void SaveRestingTransform()
        {
            if (weaponSocket == null)
                return;

            restingPosition = weaponSocket.localPosition;
            restingRotation = weaponSocket.localRotation;
        }

        public void PlayAttack(AttackProfile profile)
        {
            if (profile == null || weaponSocket == null)
                return;

            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }

            currentRoutine =
                StartCoroutine(AttackRoutine(profile));
        }

        private IEnumerator AttackRoutine(
            AttackProfile profile)
        {
            Quaternion attackStart =
                restingRotation *
                Quaternion.Euler(profile.startEuler);

            Quaternion attackImpact =
                restingRotation *
                Quaternion.Euler(profile.impactEuler);

            Quaternion attackEnd =
                restingRotation *
                Quaternion.Euler(profile.endEuler);

            Vector3 startPosition =
                restingPosition +
                profile.startPosition;

            Vector3 impactPosition =
                restingPosition +
                profile.impactPosition;

            Vector3 endPosition =
                restingPosition +
                profile.endPosition;

            float firstHalf =
                Mathf.Max(
                    profile.duration * 0.5f,
                    0.01f
                );

            float timer = 0f;

            // PRIMERA MITAD
            while (timer < firstHalf)
            {
                timer += Time.deltaTime;

                float t =
                    Mathf.Clamp01(
                        timer / firstHalf
                    );

                float smoothT =
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        t
                    );

                weaponSocket.localRotation =
                    Quaternion.Slerp(
                        attackStart,
                        attackImpact,
                        smoothT
                    );

                weaponSocket.localPosition =
                    Vector3.Lerp(
                        startPosition,
                        impactPosition,
                        smoothT
                    );

                yield return null;
            }

            timer = 0f;

            // SEGUNDA MITAD
            while (timer < firstHalf)
            {
                timer += Time.deltaTime;

                float t =
                    Mathf.Clamp01(
                        timer / firstHalf
                    );

                float smoothT =
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        t
                    );

                weaponSocket.localRotation =
                    Quaternion.Slerp(
                        attackImpact,
                        attackEnd,
                        smoothT
                    );

                weaponSocket.localPosition =
                    Vector3.Lerp(
                        impactPosition,
                        endPosition,
                        smoothT
                    );

                yield return null;
            }

            // VOLVER SIEMPRE AL PUNTO REAL DE REPOSO
            weaponSocket.localPosition =
                restingPosition;

            weaponSocket.localRotation =
                restingRotation;

            currentRoutine = null;
        }

        public void ResetWeapon()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            if (weaponSocket == null)
                return;

            weaponSocket.localPosition =
                restingPosition;

            weaponSocket.localRotation =
                restingRotation;
        }
    }
}