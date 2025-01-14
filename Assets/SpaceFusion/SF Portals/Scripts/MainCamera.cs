using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceFusion.SF_Portals.Scripts {
    public class MainCamera : MonoBehaviour {

        private Portal[] portals;

        /// <summary>
        /// this actually fixes some weird flickering of the portal when the player goes through the portal sideways
        /// So instead of rendering the RenderTextures directly in the Update function of the Portal, we control it from the main camera for all the listed portals 
        /// </summary>
        private void Awake() {
            var cam = GetComponent<Camera>();
            portals = FindObjectsOfType<Portal>();
             foreach (var portal in portals) {
                 portal.playerCamera = cam;
             }
            RenderPipelineManager.beginCameraRendering += RenderPortal;
        }

        private void OnDestroy() {
            RenderPipelineManager.beginCameraRendering -= RenderPortal;
        }

        private void RenderPortal(ScriptableRenderContext context, Camera cam) {
            foreach (var portal in portals) {
                portal.Render();
            }
        }

    }
}