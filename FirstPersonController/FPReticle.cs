using UnityEngine;
using UnityEngine.UI;

namespace FirstPersonController {
    public class FPReticle : MonoBehaviour {

        private void Start()
        {
            GameObject firstPersonReticle = new GameObject("UI - First Person Reticle", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            Canvas canvas = firstPersonReticle.GetComponent<Canvas>();
            CanvasScaler canvasScaler = firstPersonReticle.GetComponent<CanvasScaler>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);

            GameObject reticleImage = new GameObject("UI Reticle", typeof(Image));
            reticleImage.transform.SetParent(firstPersonReticle.transform, false);
            reticleImage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(10,10);
            reticleImage.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        }
    }
}
