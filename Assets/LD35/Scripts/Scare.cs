using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    public class Scare : MonoBehaviour {

        public static List<Scare> scareList = new List<Scare>(64);

        public static Vector3 GetEscapeVector(Vector3 position) {
            var escape = Vector3.zero;

            foreach (var scare in scareList) {
                var dir = (position - scare.transform.position).WithY(0f);
                var distSq = dir.sqrMagnitude;
                if (Mathf.Approximately(distSq, 0f)) continue;

                dir /= Mathf.Sqrt(distSq);
                escape += (1f - Mathf.Clamp01(distSq / scare.sqrRadius)) * scare.power * dir;
            }
            return escape;
        }

        public float radius = 2f, power = 1f;
        public float sqrRadius { get { return radius * radius; } }

        public float bounceRadius = 1f, bouncePower = 10f;
        public float sqrBounceRadius { get { return bounceRadius * bounceRadius; } }

        protected virtual void OnEnable() {
            scareList.Add(this);
        }

        protected virtual void OnDisable() {
            scareList.SwapRemove(this);
        }
    }
}
