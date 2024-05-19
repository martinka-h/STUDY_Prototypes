using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineTransposer;

namespace CameraSystem {
    public class VirtualCamera : MonoBehaviour {
        private Vector3 followOffset = new Vector3(0, 40, -26);
        private CinemachineTransposer transposer;

        private void Start()
        {
            Debug.Log(followOffset);
            CinemachineVirtualCamera camera = new GameObject("CinemachineVirtualCamera").AddComponent<Cinemachine.CinemachineVirtualCamera>();
            Transform cameraSystem = new GameObject("CameraSystem").transform;

            camera.Follow = cameraSystem;
            camera.LookAt = cameraSystem;

            camera.transform.position = new Vector3(0, 0, -10);

            // Add and set up Cinemachine Transposer
            transposer = camera.GetCinemachineComponent<CinemachineTransposer>();

            if (transposer == null) {
                camera.AddCinemachineComponent<CinemachineTransposer>();
            }

            transposer = camera.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = BindingMode.LockToTargetWithWorldUp;
            transposer.m_FollowOffset = followOffset;

            // Add Cinemachine Composer
            CinemachineComposer composer = camera.GetCinemachineComponent<CinemachineComposer>();

            if (composer == null) {
                camera.AddCinemachineComponent<CinemachineComposer>();
            }
        }
    }
}