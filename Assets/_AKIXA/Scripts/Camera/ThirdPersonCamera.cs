using UnityEngine;
using UnityEngine.InputSystem;

namespace AKIXA.CameraSystem
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;

        [Header("Camera Settings")]
        [SerializeField] private float distance = 4f;
        [SerializeField] private float mouseSensitivity = 0.12f;

        [Header("Vertical Limits")]
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 65f;

        private float yaw;
        private float pitch = 15f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            ReadMouse();
            UpdateCamera();
        }

        private void ReadMouse()
        {
            if (Mouse.current == null)
                return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * mouseSensitivity;
            pitch -= mouseDelta.y * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        private void UpdateCamera()
        {
            if (target == null)
                return;

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

            Vector3 cameraPosition =
                target.position - rotation * Vector3.forward * distance;

            transform.position = cameraPosition;
            transform.rotation = rotation;
        }
    }
}