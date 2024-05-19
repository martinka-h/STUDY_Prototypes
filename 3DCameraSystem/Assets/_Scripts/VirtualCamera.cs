using UnityEngine;
using Cinemachine;

namespace CameraSystem {
    public class VirtualCamera : MonoBehaviour {
        private void Start()
        {
            CinemachineVirtualCamera camera = new GameObject("CinemachineVirtualCamera").AddComponent<Cinemachine.CinemachineVirtualCamera>();

            Transform cameraSystem = new GameObject("CameraSystem").transform;

            camera.Follow = cameraSystem;
        }
    }
}