using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    [RequireComponent(typeof(MeshRenderer))]
    public class WolfVisionMaterials : MonoBehaviour {

        public static List<WolfVisionMaterials> list = new List<WolfVisionMaterials>();

        public static void SwapAll() {
            foreach (var item in list)
                item.Swap();
        }

        public Material[] materials;

        [ContextMenu("Swap Materials")]
        public void Swap() {
            var ren = GetComponent<MeshRenderer>();
            var tmp = ren.sharedMaterials;
            ren.sharedMaterials = materials;
            materials = tmp;
        }

        private void Reset() {
            materials = (Material[]) GetComponent<MeshRenderer>().sharedMaterials.Clone();
        }

        private void OnEnable() {
            list.Add(this);
        }

        private void OnDisable() {
            list.SwapRemove(this);
        }
    }
}
