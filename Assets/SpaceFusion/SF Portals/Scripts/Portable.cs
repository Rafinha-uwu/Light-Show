using System;
using System.Linq;
using UnityEngine;

namespace SpaceFusion.SF_Portals.Scripts {
    /// <summary>
    /// Every Object that should be able to teleport through the portal should have this or any inheritor of this script attached.
    /// </summary>
    public class Portable : MonoBehaviour {

        [Header("Object that should be teleported. Clone of it will be created on the second portal")]
        public GameObject portableObject;

        public GameObject clone { get; private set; }
        public Vector3 previousPortalOffset { get; set; }
        public Material[] originalMaterials { get; private set; }
        public Material[] cloneMaterials { get; private set; }


        public virtual void Teleport(Transform from, Transform to, Vector3 pos, Quaternion rot) {
            transform.position = pos;
            transform.rotation = rot;
            Physics.SyncTransforms();
        }

        /// <summary>
        /// If Portable enters portal area, a clone will be activated 
        /// </summary>
        public void EnterPortalArea() {
            if (clone != null) {
                clone.SetActive(true);
                return;
            }

            clone = Instantiate(portableObject);
            clone.transform.parent = portableObject.transform.parent;
            clone.transform.localScale = portableObject.transform.localScale;
            
            // only fetch materials if not already fetched on previous portal enter 
            originalMaterials ??= GetMaterials(portableObject);
            cloneMaterials ??= GetMaterials(clone);

        }

        /// <summary>
        /// portable exists portal area
        /// Since clipping of the SF Slice shader materials should only be activated when in portal area
        /// "_Clipable" property will be set to false for all the materials
        /// </summary>
        public void ExitPortalArea() {
            clone.SetActive(false);
            foreach (var mat in originalMaterials) {
                mat.SetFloat("_IsClipable", 0);
            }

            foreach (var mat in cloneMaterials) {
                mat.SetFloat("_IsClipable", 0);
            }
        }

        /// <summary>
        /// extracts all Materials of all the Children under the Portable GameObject
        /// </summary>
        private Material[] GetMaterials(GameObject g) {
            var renderers = g.GetComponentsInChildren<MeshRenderer>();
            return renderers.SelectMany(r => r.materials).ToArray();

        }

    }
}