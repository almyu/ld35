using UnityEngine;

namespace LD35 {

    public class World : MonoSingleton<World> {

        public static readonly Vector3 center = Vector3.zero;

        public static Vector3 Clamp(Vector3 pos) {
            var lenSq = pos.sqrMagnitude;
            var radiusSq = instance.radius * instance.radius;
            return lenSq > radiusSq ? pos * radiusSq / lenSq : pos;
        }


        public float radius = 20.5f;

        private void FixedUpdate() {
            var radiusSq = radius * radius;

            foreach (var sheep in Sheep.sheepList)
                if (sheep.planarPosition.sqrMagnitude > radiusSq)
                    sheep.gameObject.AddComponent<FallingSheep>();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, radius);
        }
    }
}
