using System.Linq;
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
        public GameObject sheepDeathPrefab;

        public Material RedSheepMaterial;
        public Material BlackSheepMaterial;
        public Material YellowSheepMaterial;

        [HideInInspector]
        public Sheep RedSheep;
        [HideInInspector]
        public bool alwaysKilledRedSheep = true;
        [HideInInspector]
        public Sheep BlackSheep;
        [HideInInspector]
        public Sheep YellowSheep;

        private void Awake() {
            for (int i = 0; i < numSheep; ++i) {
                var spawnedSheep = SpawnSheep();

                if(i == 0 && ModID.RedSheep.IsModActive()) {
                    RedSheep = PaintSheep(spawnedSheep, RedSheepMaterial);
                }

                if (i == numSheep / 2 && ModID.YellowSheep.IsModActive()) {
                    YellowSheep = PaintSheep(spawnedSheep, YellowSheepMaterial);
                }

                if (i == numSheep-1 && ModID.BlackSheep.IsModActive()) {
                    BlackSheep = PaintSheep(spawnedSheep, BlackSheepMaterial);
                }
            }
        }

        private Sheep PaintSheep(Sheep spawnedSheep, Material material) {
            var sheepModel = spawnedSheep.GetComponentInChildren<SheepModel>();
            sheepModel.PaintSheep(material);

            return spawnedSheep;
        }

        public Sheep SpawnSheep() {
            var pos = transform.position + Random.insideUnitSphere.WithY(0f) * spawnRadius;
            return Instantiate(sheepPrefab, pos, Quaternion.AngleAxis(Random.value * 360f, Vector3.up)) as Sheep;
        }
        
        public void OnSheepKilled(Sheep sheep) {
            if (sheep == RedSheep && (SheepCounter.instance.eatenSheep <= numSheep - 1)) {
                var simpleSheep = Sheep.sheepList.FirstOrDefault(s => s != BlackSheep && s != YellowSheep && s != RedSheep);

                if (simpleSheep != null) {
                    RedSheep = PaintSheep(simpleSheep, RedSheepMaterial);
                }
            }
            else {
                alwaysKilledRedSheep = false;
            }

            if (sheep == YellowSheep) {
                Sheep.JumpAll(5f, 10f);
            }

            if (sheep == BlackSheep) {
                var scareEmAll = new GameObject();
                scareEmAll.transform.position = Vector3.zero;
                var scare = scareEmAll.AddComponent<Scare>();
                scare.power = 3f;
                scare.radius = 100f;
            }
        }
    }
}
