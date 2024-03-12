using Unity.VisualScripting;
using UnityEngine;

namespace Movement {
    public class FirstPersonController : MonoBehaviour {

        [Header("Movement speed")]
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float sprintMultiplier= 2.0f;

        [Header("Jump parameters")]
        [SerializeField] private float jumpForce = 5.0f;
        [SerializeField] private float gravity = 9.81f;

        [Header("Look sensitivity")]
        [SerializeField] private float mouseSensitivity = 2.0f;
        [SerializeField] private float upDownRange = 80.0f;

        [Header("Inputs Customisation")]
        private string horizontalMoveInput = "Horizontal";
        private string verticalMoveInput = "Vertical";
        private string mouseXInput = "Mouse X";
        private string mouseYInput = "Mouse Y";
        private KeyCode sprintKey = KeyCode.LeftShift;
        private KeyCode jumpKey = KeyCode.Space;

        private float verticalRotation;
        private Vector3 currentMovement = Vector3.zero;

        private Camera mainCamera;
        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        #region Movement
        private void HandleMovement()
        {
            float speedMultiplier = Input.GetKey(sprintKey) ? sprintMultiplier : 1f;
            float verticalSpeed = Input.GetAxis(verticalMoveInput) * walkSpeed * speedMultiplier;
            float horizontalSpeed = Input.GetAxis(horizontalMoveInput) * walkSpeed * speedMultiplier;

            Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
            horizontalMovement = transform.rotation * horizontalMovement;

            HandleGravityAndJumping();

            currentMovement.x = horizontalMovement.x;
            currentMovement.z = horizontalMovement.z;

            characterController.Move(currentMovement * Time.deltaTime);
        }

        #region Jump
        private void HandleGravityAndJumping()
        {
            if (characterController.isGrounded) {
                currentMovement.y = -0.5f;

                if (Input.GetKeyDown(jumpKey)) {
                    currentMovement.y = jumpForce;
                }
            } else {
                currentMovement.y -= gravity * Time.deltaTime;
            }
        }
        #endregion

        #endregion
        #region Camera rotation
        private void HandleRotation()
        {
            float mouseXRotation = Input.GetAxis(mouseXInput) * mouseSensitivity;
            transform.Rotate(0, mouseXRotation, 0);

            verticalRotation -= Input.GetAxis(mouseYInput) * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

            mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
        #endregion
    }
}
