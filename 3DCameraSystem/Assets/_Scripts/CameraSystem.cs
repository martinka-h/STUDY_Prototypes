using UnityEngine;

namespace CameraSystem {
    public class CameraSystem : MonoBehaviour {

        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float rotateSpeed = 50f;
        [SerializeField] int edgeScrollSize = 10;

        [SerializeField] private bool useEdgeScrolling = true;

        private void Update()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            // edge scrolling
            if (useEdgeScrolling) {
                if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
                if (Input.mousePosition.y < edgeScrollSize) inputDir.z = -1f;
                if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f;
                if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.z += +1f;
            }

            // move
            if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
            if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
            if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
            if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            // rotate
            float rotateDir = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
            if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

            transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);


        }
    }
}