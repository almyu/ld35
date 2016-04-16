using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    public class Scare : MonoBehaviour {

        public static List<Scare> list = new List<Scare>(64);

        public static void Register(Scare scare) {
            list.Add(scare);
        }

        public static void Unregister(Scare scare) {
            var index = list.IndexOf(scare);
            if (index == -1) return;

            list[index] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }

        public static Vector3 GetEscapeVector(Vector3 position) {
            var escape = Vector3.zero;

            foreach (var scare in list) {
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

        private void OnEnable() {
            Register(this);
        }

        private void OnDisable() {
            Unregister(this);
        }
    }
}
