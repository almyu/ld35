using UnityEngine;

namespace LD35 {

    public class Herd : MonoSingleton<Herd> {

        public static Vector3 GetCenterOfMass() {
            var com = Vector3.zero;

            if (Sheep.sheepList.Count > 0) {
                foreach (var sheep in Sheep.sheepList)
                    com += sheep.planarPosition;

                com *= 1f / Sheep.sheepList.Count;
            }
            return com;
        }

        public static Sheep GetLostSheep() {
            var center = GetCenterOfMass();
            var maxDistSq = 0f;
            var farthestSheep = default(Sheep);

            foreach (var sheep in Sheep.sheepList) {
                var distSq = (sheep.planarPosition - center).sqrMagnitude;
                if (distSq < maxDistSq) continue;

                maxDistSq = distSq;
                farthestSheep = sheep;
            }
            return farthestSheep;
        }

        public int numSheep = 10;
        public float spawnRadius = 5f;
        public Sheep sheepPrefab;

        private void Awake() {
            for (int i = 0; i < numSheep; ++i)
                SpawnSheep();
        }

        public void SpawnSheep() {
            var pos = transform.position + Random.insideUnitSphere.WithY(0f) * spawnRadius;
            Instantiate(sheepPrefab, pos, Quaternion.AngleAxis(Random.value * 360f, Vector3.up));
        }
    }
}
