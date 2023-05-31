using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MouseLook : MonoBehaviour
    {
        public InputActionReference horizontalLook;
        public InputActionReference verticalLook;
        public float lookSpeed = 1f;
        public Transform cameraTransform;

        private float pitch;
        private float yaw;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            horizontalLook.action.performed += HandleHorizontalLookChange;
            verticalLook.action.performed += HandleVerticalLookChange;
        }

        void HandleHorizontalLookChange(InputAction.CallbackContext obj)
        {
            yaw += obj.ReadValue<float>();
            transform.localRotation = Quaternion.AngleAxis(yaw * lookSpeed, Vector3.up);
        }

        void HandleVerticalLookChange(InputAction.CallbackContext obj)
        {
            pitch -= obj.ReadValue<float>();
            cameraTransform.localRotation = Quaternion.AngleAxis(pitch * lookSpeed, Vector3.right);
        }
    }
}