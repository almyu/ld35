using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    public class Sheep : Scare {

        public static List<Sheep> sheepList = new List<Sheep>(16);

        public static float groundEpsilon = 0.02f;

        public bool grounded { get { return transform.position.y <= groundEpsilon; } }
        public float speed = 5f;

        public float bounceSpeed = 1f, bounceGravity = 2f;

        public float chilloutThreshold = 0.1f, chilloutTime = 2f;
        public float wanderSpeedup = 0.25f, wanderRadius = 3f;
        public Vector2 wanderIntervalRange = new Vector2(0.5f, 5f);

        private float bounce, chilloutTimer;
        private Vector3 waypoint;

        public static Sheep GetAnyInRange(Vector3 point, float range) {
            foreach (var sheep in Sheep.sheepList)
                if (Vector3.Distance(sheep.planarPosition, point) <= range)
                    return sheep;

            return null;
        }

        public static void JumpAll(float minSpeed, float maxSpeed) {
            foreach (var sheep in sheepList)
                sheep.Jump(Random.Range(minSpeed, maxSpeed));
        }

        public void Jump(float speed) {
            if (bounce < speed)
                bounce = speed;
        }

        public void Eat() {
            SheepCounter.instance.EatSheep();
            Destroy(gameObject);
        }

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
                }
                else vel = wanderSpeedup / Mathf.Sqrt(wpDistSq) * toWp;
            }

            if (ModID.Wind.IsModActive())
                vel += Mods.Wind.windSpeed;

            if (vel != Vector3.zero) {
                transform.position += speed * Time.deltaTime * vel;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime * 7f);

                if (grounded) Jump(bounceSpeed * worry);
            }

            transform.position = transform.position.WithY(Mathf.Max(0f, transform.position.y + bounce * Time.deltaTime));
        }

        private void FixedUpdate() {
            bounce += Physics.gravity.y * bounceGravity * Time.fixedDeltaTime;
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
