using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AKIXA.GameplayInput
{
    public class PlayerInputReader : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public bool SprintHeld { get; private set; }

        public event Action JumpPressed;
        public event Action DodgePressed;
        public event Action InteractPressed;

        public event Action LightAttackPressed;
        public event Action HeavyAttackPressed;
        public event Action BlockPressed;
        public event Action BlockReleased;

        private bool blockWasHeld;

        private void Update()
        {
            ReadMovement();
            ReadSprint();

            ReadJump();
            ReadDodge();
            ReadInteract();

            ReadCombat();
        }

        private void ReadMovement()
        {
            Vector2 input = Vector2.zero;

            if (Keyboard.current == null)
                return;

            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;

            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;

            MoveInput =
                Vector2.ClampMagnitude(input, 1f);
        }

        private void ReadSprint()
        {
            if (Keyboard.current == null)
            {
                SprintHeld = false;
                return;
            }

            SprintHeld =
                Keyboard.current.leftShiftKey.isPressed ||
                Keyboard.current.rightShiftKey.isPressed;
        }

        private void ReadJump()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                JumpPressed?.Invoke();
            }
        }

        private void ReadDodge()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
            {
                DodgePressed?.Invoke();
            }
        }

        private void ReadInteract()
        {
            if (Keyboard.current == null)
                return;

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                InteractPressed?.Invoke();
            }
        }

        private void ReadCombat()
        {
            if (Mouse.current == null)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                LightAttackPressed?.Invoke();
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                HeavyAttackPressed?.Invoke();
            }

            bool blockHeld =
                Mouse.current.rightButton.isPressed;

            if (blockHeld && !blockWasHeld)
            {
                BlockPressed?.Invoke();
            }

            if (!blockHeld && blockWasHeld)
            {
                BlockReleased?.Invoke();
            }

            blockWasHeld = blockHeld;
        }
    }
}