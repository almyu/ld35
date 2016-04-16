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

        public static Sheep GetClosestScare(Vector3 shepherdPosition, float distance)
        {
            foreach (var sheep in Sheep.sheepList)
            {
                if (Vector3.Distance(sheep.transform.position, shepherdPosition) <= distance) 
                    return sheep;
            }

            return default(Sheep);
        }

        public float radius = 2f, power = 1f;
        public float sqrRadius { get { return radius * radius; } }

        protected virtual void OnEnable() {
            scareList.Add(this);
        }

        protected virtual void OnDisable() {
            scareList.SwapRemove(this);
        }
    }
}
