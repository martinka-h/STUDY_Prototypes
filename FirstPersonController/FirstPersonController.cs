using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement {
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour {

        [Header("Input actions")]
        [SerializeField] private InputActionAsset playerControls;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private Vector2 moveInput;
        private Vector2 lookInput;

        [Header("Movement speed")]
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float sprintMultiplier = 2.0f;

        [Header("Jump parameters")]
        [SerializeField] private float jumpForce = 5.0f;
        [SerializeField] private float gravity = 9.81f;

        [Header("Look sensitivity")]
        [SerializeField] private float mouseSensitivity = 2.0f;
        [SerializeField] private float upDownRange = 80.0f;

        [Header("Footstep sounds")]
        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private AudioClip[] footstepSounds;
        private float velocityThreshold = 2.0f;
        private float walkStepInterval = 0.5f;
        private float sprintStepInterval = 0.3f;

        private float nextStepTime;
        private float verticalRotation;
        private Vector3 currentMovement = Vector3.zero;
        private bool isMoving;

        private Camera mainCamera;
        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            mainCamera = Camera.main;

            moveAction = playerControls.FindActionMap("Player").FindAction("Move");
            lookAction = playerControls.FindActionMap("Player").FindAction("Look");
            jumpAction = playerControls.FindActionMap("Player").FindAction("Jump");
            sprintAction = playerControls.FindActionMap("Player").FindAction("Sprint");

            moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
            moveAction.canceled += contexy => moveInput = Vector2.zero;

            lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
            lookAction.canceled += context => lookInput = Vector2.zero;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            moveAction.Enable();
            lookAction.Enable();
            jumpAction.Enable();
            sprintAction.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
            lookAction.Disable();
            jumpAction.Disable();
            sprintAction.Disable();
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleFootsteps();
            HandleGravityAndJumping();
        }

        #region Movement
        private void HandleMovement()
        {
            float speedMultiplier = sprintAction.ReadValue<float>() > 0 ? sprintMultiplier : 1;

            float verticalSpeed = moveInput.y * walkSpeed * speedMultiplier;
            float horizontalSpeed = moveInput.x * walkSpeed * speedMultiplier;

            Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
            horizontalMovement = transform.rotation * horizontalMovement;

            currentMovement.x = horizontalMovement.x;
            currentMovement.z = horizontalMovement.z;

            characterController.Move(currentMovement * Time.deltaTime);
            isMoving = moveInput.y != 0 || moveInput.x != 0;
        }

        #region Jump
        private void HandleGravityAndJumping()
        {
            if (characterController.isGrounded) {
                currentMovement.y = -0.5f;
                if (jumpAction.triggered) {
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
            float mouseXRotation = lookInput.x * mouseSensitivity;
            transform.Rotate(0, mouseXRotation, 0);

            verticalRotation -= lookInput.y * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

            mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
        #endregion
        #region Footsteps
        private void HandleFootsteps()
        {
            float currentStepInterval = (sprintAction.ReadValue<float>() > 0 ? sprintStepInterval : walkStepInterval);

            if (isMoving && Time.time > nextStepTime && characterController.velocity.magnitude > velocityThreshold && characterController.isGrounded) {
                PlayFootstepSounds();
                nextStepTime = Time.time + currentStepInterval;
            }
        }

        private void PlayFootstepSounds()
        {
            int randomIndexx = footstepSounds.Length == 1 ? 0 : Random.Range(0, footstepSounds.Length - 1);

            footstepSource.clip = footstepSounds[randomIndexx];
            footstepSource.Play();
        }
        #endregion
    }
}
