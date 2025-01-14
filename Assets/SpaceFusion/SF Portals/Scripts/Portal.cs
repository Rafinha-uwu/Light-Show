using System.Collections.Generic;
using UnityEngine;

namespace SpaceFusion.SF_Portals.Scripts {
    public class Portal : Teleporter {

        public Camera playerCamera { get; set; }

        private Camera _portalCamera;
        
        /// <summary>
        /// initializes the portal Screens
        /// </summary>
        private void Awake() {
            _portalCamera = GetComponentInChildren<Camera>();
            _trackedPortables = new List<Portable>();
            
            var portalView = new RenderTexture(Screen.width, Screen.height, 24);
            _portalCamera.targetTexture = portalView;
            linkedPortal.GetScreen().material.SetTexture(MainTexture, portalView);
        }

        /// <summary>
        /// called by the main camera 
        /// </summary>
        public void Render() {
            // calculate the relative position and rotation of the portal camera to its portal
            // the camera should behave exactly like the main camera relative to the portal we are looking at
            var localToWorldMatrix = playerCamera.transform.localToWorldMatrix;
            localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix *
                                 localToWorldMatrix;
            // properly set the transform of the portal camera
            _portalCamera.transform.SetPositionAndRotation(localToWorldMatrix.GetColumn(3),
                localToWorldMatrix.rotation);
            UpdatePortalCameraProjectionMatrix();
            ProtectScreenFromClipping(playerCamera.transform.position);
        }


        /// <summary>
        /// adjusting the thickness and position of the portalView ensure a seamless transition through the portal
        /// </summary>
        private void ProtectScreenFromClipping(Vector3 viewPoint) {
            var height = playerCamera.nearClipPlane * Mathf.Tan(playerCamera.fieldOfView * Mathf.Deg2Rad * 0.5f);
            var width = height * playerCamera.aspect;
            var nearClipDist = new Vector3(width, height, playerCamera.nearClipPlane).magnitude;
            var screenT = screen.transform;
            screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, nearClipDist);
            screenT.localPosition = Vector3.forward * nearClipDist * GetPortalSide(viewPoint) * -0.5f;
        }
        
        /// <summary>
        /// performs a oblique projection and updates the portal camera projection matrix so that all the objects that are between the camera and the portal are clipped out
        /// With this we avoid rendering stuff in the portal view that simply does not belong there
        /// </summary>
        private void UpdatePortalCameraProjectionMatrix() {
            var camSpacePos = _portalCamera.worldToCameraMatrix.MultiplyPoint(transform.position);
            var camSpaceNormal = _portalCamera.worldToCameraMatrix.MultiplyVector(transform.forward) *
                                 -GetPortalSide(_portalCamera.transform.position);
            var camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + 0.05f;
            // If too close to portal we get weird graphical issues, so we add a limit of 0.5 distance for the oblique projection 
            if (Mathf.Abs(camSpaceDst) > 1f) {
                // calculate based on playerCamera so the camera settings are copied over to portal camera
                _portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(new Vector4(camSpaceNormal.x,
                    camSpaceNormal.y,
                    camSpaceNormal.z,
                    camSpaceDst));
            } else {
                _portalCamera.projectionMatrix = playerCamera.projectionMatrix;
            }
        }

    }
}