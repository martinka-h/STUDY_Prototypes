using UnityEngine;

namespace CameraSystem {
    public class CameraSystem : MonoBehaviour {

        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float rotateSpeed = 50f;
        [SerializeField] private int edgeScrollSize = 10;
        [SerializeField] private float dragPanSpeed = 0.2f;

        [SerializeField] private bool useEdgeScrolling = true;
        [SerializeField] private bool allowRotation = true;
        [SerializeField] private bool useDragPan = true;

        private bool dragPanActive = false;
        private Vector2 lastMousePosition;

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
    }
}