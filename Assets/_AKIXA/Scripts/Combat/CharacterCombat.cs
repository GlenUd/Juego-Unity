using System.Collections;
using UnityEngine;
using AKIXA.GameplayInput;
using AKIXA.Equipment;
using AKIXA.Stats;
using AKIXA.Weapons;
using AKIXA.Animation;

namespace AKIXA.Combat
{
    public class CharacterCombat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PlayerInputReader inputReader;

        [SerializeField]
        private EquipmentManager equipment;

        [SerializeField]
        private CharacterResources resources;

        [SerializeField]
        private CombatAnimationDriver animationDriver;

        [SerializeField]
        private CharacterAnimatorBridge animatorBridge;

        [Header("Combo")]
        [SerializeField]
        private float comboResetTime = 1.2f;

        [SerializeField]
        private float inputBufferTime = 0.5f;

        private bool isAttacking;
        private bool attackBuffered;

        private int comboIndex;

        private float comboTimer;
        private float bufferTimer;

        public bool IsAttacking => isAttacking;

        private void OnEnable()
        {
            if (inputReader != null)
            {
                inputReader.LightAttackPressed +=
                    HandleAttackInput;
            }
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.LightAttackPressed -=
                    HandleAttackInput;
            }

            StopAllCoroutines();

            isAttacking = false;
            attackBuffered = false;
            comboIndex = 0;

            if (animationDriver != null)
            {
                animationDriver.ResetWeapon();
            }
        }

        private void Update()
        {
            HandleComboReset();
            HandleInputBuffer();
        }

        private void HandleAttackInput()
        {
            // Si ya estamos atacando,
            // guardamos el siguiente clic.
            if (isAttacking)
            {
                attackBuffered = true;
                bufferTimer = inputBufferTime;

                return;
            }

            TryStartAttack();
        }

        private void TryStartAttack()
        {
            if (equipment == null ||
                resources == null)
            {
                return;
            }

            WeaponDefinition weapon =
                equipment.EquippedWeapon;

            WeaponRuntime runtime =
                equipment.CurrentWeaponRuntime;

            if (weapon == null ||
                runtime == null)
            {
                return;
            }

            AttackProfile[] combo =
                weapon.LightAttackCombo;

            if (combo == null ||
                combo.Length == 0)
            {
                return;
            }

            if (comboIndex >= combo.Length)
            {
                comboIndex = 0;
            }

            AttackProfile profile =
                combo[comboIndex];

            if (profile == null)
                return;

            if (!resources.TrySpendStamina(
                weapon.StaminaCost))
            {
                return;
            }

            // Guardamos cuál golpe del combo estamos ejecutando.
            int attackIndex = comboIndex;

            StartCoroutine(
                AttackRoutine(
                    weapon,
                    runtime,
                    profile,
                    attackIndex
                )
            );
        }

        private IEnumerator AttackRoutine(
            WeaponDefinition weapon,
            WeaponRuntime runtime,
            AttackProfile profile,
            int attackIndex)
        {
            isAttacking = true;

            attackBuffered = false;

            // Animación corporal real del Animator.
            if (animatorBridge != null)
            {
                animatorBridge.PlayAttack(
                    attackIndex
                );
            }

            // Animación procedural provisional de la espada.
            if (animationDriver != null)
            {
                animationDriver.PlayAttack(
                    profile
                );
            }

            float speed =
                Mathf.Max(
                    weapon.AttackSpeed,
                    0.01f
                );

            float hitStart =
                profile.hitboxStart /
                speed;

            float hitDuration =
                profile.hitboxDuration /
                speed;

            float totalDuration =
                profile.duration /
                speed;

            // Esperar hasta la ventana de impacto.
            yield return new WaitForSeconds(
                hitStart
            );

            runtime.EnableHitbox();

            yield return new WaitForSeconds(
                hitDuration
            );

            runtime.DisableHitbox();

            float remaining =
                totalDuration -
                hitStart -
                hitDuration;

            if (remaining > 0f)
            {
                yield return new WaitForSeconds(
                    remaining
                );
            }

            isAttacking = false;

            // El ataque terminó correctamente.
            comboIndex++;

            AttackProfile[] combo =
                weapon.LightAttackCombo;

            if (comboIndex >= combo.Length)
            {
                comboIndex = 0;
            }

            comboTimer =
                comboResetTime;

            // ¿El jugador ya pidió el siguiente golpe?
            if (attackBuffered &&
                bufferTimer > 0f)
            {
                attackBuffered = false;

                yield return null;

                TryStartAttack();
            }
        }

        private void HandleComboReset()
        {
            if (isAttacking)
                return;

            if (comboTimer > 0f)
            {
                comboTimer -=
                    Time.deltaTime;

                if (comboTimer <= 0f)
                {
                    comboIndex = 0;
                }
            }
        }

        private void HandleInputBuffer()
        {
            if (!attackBuffered)
                return;

            bufferTimer -=
                Time.deltaTime;

            if (bufferTimer <= 0f)
            {
                attackBuffered = false;
            }
        }
    }
}