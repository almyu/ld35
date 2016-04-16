using UnityEngine;

namespace LD35 {

    public class World : MonoBehaviour {

        public float radius = 20.5f;

        private void FixedUpdate() {
            var radiusSq = radius * radius;

            foreach (var sheep in Sheep.sheepList)
                if (sheep.planarPosition.sqrMagnitude > radiusSq)
                    sheep.gameObject.AddComponent<FallingSheep>();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Vector3.zero, radius);
        }
    }
}
