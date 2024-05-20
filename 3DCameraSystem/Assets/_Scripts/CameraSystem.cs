using Cinemachine;
using UnityEngine;

namespace CameraSystem {
    public class CameraSystem : MonoBehaviour {

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineTransposer transposer;

        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float rotateSpeed = 50f;
        [SerializeField] private int edgeScrollSize = 10;
        [SerializeField] private float dragPanSpeed = 0.2f;
        [SerializeField] private float zoomSpeed = 5f;

        [SerializeField] private bool useEdgeScrolling;
        [SerializeField] private bool allowRotation;
        [SerializeField] private bool useDragPan;
        [SerializeField] ZoomOptions zoom;


        private bool dragPanActive = false;
        private Vector2 lastMousePosition;

        [SerializeField] private int fovMin = 10;
        [SerializeField] private int fovMax = 50;
        private float targetFieldOfView = 30;

        [SerializeField] private float followOffsetMin = 10f;
        [SerializeField] private float followOffsetMax = 50f;
        [SerializeField] private float zoomAmout = 3f;
        private Vector3 followOffset;

        private void Start()
        {
            if (virtualCamera == null) {
                Debug.Log("The reference for virtualCamera is empty. Remember to assign it in the inspector.\nSearching for CinemacineVirtualCamera in scene.");
                virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
            }

            if (virtualCamera == null) {
                Debug.Log("CinemachineVirtualCamera not found.");
            }

            if (zoom == ZoomOptions.FieldOfViewZoom || zoom == ZoomOptions.MoveForwardZoom) {
                transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
                followOffset = transposer.m_FollowOffset;

                if (followOffset.y < followOffsetMin) {
                    Debug.Log("Your minimum follow offset is larger than default follow offset.");
                }

                if (followOffset.y > followOffsetMax) {
                    Debug.Log("Your maximum follow offset is smaller than default follow offset");
                }
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

            switch (zoom) {
                case ZoomOptions.NoZoom: return;
                case ZoomOptions.FieldOfViewZoom: HandleCameraZoom_FieldOfView(); break;
                case ZoomOptions.MoveForwardZoom: HandleCameraZoom_MoveForward(); break;
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

        private void HandleCameraZoom_MoveForward()
        {
            Vector3 zoomDir = followOffset.normalized;

            if (Input.mouseScrollDelta.y > 0) {
                followOffset -= zoomDir * zoomAmout;
            } else if (Input.mouseScrollDelta.y < 0) {
                followOffset += zoomDir * zoomAmout;
            }

            if (followOffset.magnitude < followOffsetMin) {
                followOffset = zoomDir * followOffsetMin;
            } else if (followOffset.magnitude > followOffsetMax) {
                followOffset = zoomDir * followOffsetMax;
            }

            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
        }
    }

    public enum ZoomOptions {
        NoZoom,
        FieldOfViewZoom,
        MoveForwardZoom
    }
}