﻿using UnityEngine;

namespace LD35 {

    public class World : MonoSingleton<World> {

        public static readonly Vector3 center = Vector3.zero;

        public static Vector3 Clamp(Vector3 pos) {
            var lenSq = pos.sqrMagnitude;
            var radiusSq = instance.radius * instance.radius;
            return lenSq > radiusSq ? pos * (instance.radius / Mathf.Sqrt(lenSq)) : pos;
        }

        public static Vector2 GetPolar(Vector3 planar) {
            planar = planar.WithY(0f);
            return new Vector2(planar.magnitude, Mathf.Atan2(planar.z, planar.x) * Mathf.Rad2Deg);
        }

        public static Vector3 GetPlanar(Vector2 polar) {
            polar.y *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(polar.y) * polar.x, 0f, Mathf.Sin(polar.y) * polar.x);
        }

        public static float AngularDistance(float a, float b) {
            return Mathf.Abs(Mathf.DeltaAngle(a, b));
        }


        public float radius = 14.5f, fallOffset = 1f;

        public float fallRadius {
            get { return radius + fallOffset; }
        }
        public float fallRadiusSq {
            get {
                var edge = fallRadius;
                return edge * edge;
            }
        }

        private void Update() {
            var edgeSq = fallRadiusSq;

            foreach (var sheep in Sheep.sheepList)
                if (sheep.planarPosition.sqrMagnitude > edgeSq)
                    sheep.gameObject.AddComponent<FallingSheep>();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, radius);
        }
    }
}
