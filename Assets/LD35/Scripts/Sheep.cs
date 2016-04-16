using UnityEngine;
using System.Collections;

namespace LD35 {

    public class Sheep : MonoBehaviour {

        private void FixedUpdate() {
            //
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Scare.GetEscapeVector(transform.position, 10f));
        }
    }
}
