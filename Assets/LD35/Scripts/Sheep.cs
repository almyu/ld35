using UnityEngine;
using System.Collections.Generic;
using JamSuite.Audio;

namespace LD35 {

    public class Sheep : BouncyScare {

        public static List<Sheep> sheepList = new List<Sheep>(16);

        public float speed = 5f;

        public float chilloutThreshold = 0.1f, chilloutTime = 2f;
        public float wanderSpeedup = 0.25f, wanderRadius = 3f;
        public Vector2 wanderIntervalRange = new Vector2(0.5f, 5f);

        private float chilloutTimer;
        private Vector3 waypoint;

        public static Sheep GetAnyInRange(Vector3 point, float range) {
            var closest = default(Sheep);
            var minDistSq = range * range;

            foreach (var sheep in Sheep.sheepList) {
                var distSq = (sheep.planarPosition - point).sqrMagnitude;
                if (distSq > minDistSq) continue;

                minDistSq = distSq;
                closest = sheep;
            }
            return closest;
        }

        public static void JumpAll(float minSpeed, float maxSpeed) {
            foreach (var sheep in sheepList)
                sheep.Jump(Random.Range(minSpeed, maxSpeed));
        }

        public void Eat() {
            SheepCounter.instance.EatSheep();
            Herd.instance.OnSheepKilled(this);
            Destroy(gameObject);
            Sfx.Play("SheepGetsEaten");
        }

        private static readonly string[] baas = new[] { "baa1", "baa2", "baa3"  };
        private void Update() {
            var vel = Scare.GetEscapeVector(transform.position);

            var worry = vel.magnitude;
            if (worry > chilloutThreshold) chilloutTimer = chilloutTime;

            chilloutTimer -= Time.deltaTime;

            // Wander
            if (chilloutTimer < 0f) {
                var toWp = waypoint - planarPosition;
                var wpDistSq = toWp.sqrMagnitude;

                if (wpDistSq < 0.2f || wpDistSq > wanderRadius * wanderRadius + 1f) {
                    waypoint = planarPosition + wanderRadius * Random.onUnitSphere.WithY(0f);
                    chilloutTimer = Random.Range(wanderIntervalRange.x, wanderIntervalRange.y);

                    var currentBaa = baas[Random.Range(0, baas.Length)];
                    Sfx.Play(currentBaa);
                }
                else vel = wanderSpeedup / Mathf.Sqrt(wpDistSq) * toWp;
            }

            if (ModID.Wind.IsModActive())
                vel += Mods.Wind.windSpeed;

            if (vel != Vector3.zero) {
                transform.position += speed * Time.deltaTime * vel;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime * 7f);

                Jump(worry);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Scare.GetEscapeVector(transform.position));
        }

        protected override void OnEnable() {
            if (ModID.Faster.IsModActive()) {
                speed *= Mods.Faster.factor;
            }

            base.OnEnable();
            sheepList.Add(this);
        }

        protected override void OnDisable() {
            base.OnDisable();
            sheepList.SwapRemove(this);
        }
    }
}
