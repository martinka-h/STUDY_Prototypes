using UnityEngine;
using Cinemachine;
using static Cinemachine.CinemachineTransposer;

// Make sure that the Cinemachine camera prefab has "Save During Play" enabled.
// The changes transposer will be made only if transposer is already present.

namespace CameraSystem {
    public class VirtualCamera : MonoBehaviour {
        private void Start()
        {
            CinemachineVirtualCamera camera = GetComponent<CinemachineVirtualCamera>();

            // Set camera follow and lookat if empty
            if (camera.Follow == null) {
                Transform cameraSystem = new GameObject("CameraSystem").transform;
                camera.Follow = cameraSystem;
                camera.LookAt = cameraSystem;
            }    

            // Add and set up Cinemachine Transposer if not present
            CinemachineTransposer transposer = camera.GetCinemachineComponent<CinemachineTransposer>();

            if (transposer == null) {
                camera.AddCinemachineComponent<CinemachineTransposer>();
                transposer = camera.AddCinemachineComponent<CinemachineTransposer>();
                transposer.m_BindingMode = BindingMode.LockToTargetWithWorldUp;
                transposer.m_FollowOffset = new Vector3(0, 40, -26);
            }

            // Add Cinemachine Composer if not present
            CinemachineComposer composer = camera.GetCinemachineComponent<CinemachineComposer>();
            if (composer == null) {
                camera.AddCinemachineComponent<CinemachineComposer>();
            }
        }
    }
}