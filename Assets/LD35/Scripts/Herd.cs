using UnityEngine;

namespace LD35 {

    public class Herd : MonoBehaviour {

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
