using Cinemachine;
using UnityEngine;

namespace CameraSystem {
    public class CameraSystem : MonoBehaviour {

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float rotateSpeed = 50f;
        [SerializeField] private int edgeScrollSize = 10;
        [SerializeField] private float dragPanSpeed = 0.2f;
        [SerializeField] private float zoomSpeed = 5f;

        [SerializeField] private bool useEdgeScrolling;
        [SerializeField] private bool allowRotation;
        [SerializeField] private bool useDragPan;
        [SerializeField] private bool allowZoom;

        private bool dragPanActive = false;
        private Vector2 lastMousePosition;

        [SerializeField] private int fovMax = 50;
        [SerializeField] private int fovMin = 10;
        private float targetFieldOfView = 30;

        private void Start()
        {
            if (virtualCamera == null) {
                Debug.Log("The reference for virtualCamera is empty. Remember to assign it in the inspector.\nSearching for CinemacineVirtualCamera in scene.");
                virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            }

            if (virtualCamera == null) {
                Debug.Log("CinemachineVirtualCamera not found.");
            }
        }

        private void Update()
        {
            HandleCameraMovement();

            if (useDragPan) {
                HandleDragPan();
            }

            if (useEdgeScrolling) {
                HandleEdgeScrolling();
            }

            if (allowRotation) {
                HandleCameraRotation();
            }

            if (allowZoom) {
                HandleCameraZoom_FieldOfView();
            }
        }

        private void HandleCameraMovement()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraRotation()
        {
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
            if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
        }

        private void HandleEdgeScrolling()
        {
            if (dragPanActive) return;

            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
            if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z += +1f;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleDragPan()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.GetMouseButtonDown(1)) {
                dragPanActive = true;
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1)) {
                dragPanActive = false;
            }

            if (dragPanActive) {
                Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

                inputDir.x = mouseMovementDelta.x * dragPanSpeed;
                inputDir.z = mouseMovementDelta.y * dragPanSpeed;

                lastMousePosition = Input.mousePosition;
            }

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraZoom_FieldOfView()
        {
            if (Input.mouseScrollDelta.y > 0) {
                targetFieldOfView += 5;
            }

            if (Input.mouseScrollDelta.y < 0) {
                targetFieldOfView -= 5;
            }

            targetFieldOfView = Mathf.Clamp(targetFieldOfView, fovMin, fovMax);

            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        }
    }
}