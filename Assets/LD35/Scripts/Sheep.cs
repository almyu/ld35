using UnityEngine;
using System.Collections.Generic;

namespace LD35 {

    public class Sheep : Scare {

        public static List<Sheep> sheepList = new List<Sheep>(16);

        public static float groundEpsilon = 0.02f;
        public static float bounceSpeed = 5f;

        public static float chilloutThreshold = 0.1f, chilloutTime = 2f;
        public static float wanderSpeedup = 0.25f, wanderRadius = 3f, minWanderInterval = 0.5f, maxWanderInterval = 5f;

        public Vector3 planarPosition { get { return transform.position.WithY(0f); } }
        public bool grounded { get { return transform.position.y <= groundEpsilon; } }
        public float speed = 5f;

        private float bounce, chilloutTimer;
        private Vector3 waypoint;

        public static void JumpAll(float minSpeed, float maxSpeed) {
            foreach (var sheep in sheepList)
                sheep.Jump(Random.Range(minSpeed, maxSpeed));
        }

        public void Jump(float speed) {
            if (bounce < speed)
                bounce = speed;
        }

        private void FixedUpdate() {
            var vel = Scare.GetEscapeVector(transform.position);

            var worry = vel.magnitude;
            if (worry > chilloutThreshold) chilloutTimer = chilloutTime;

            chilloutTimer -= Time.fixedDeltaTime;

            // Wander
            if (chilloutTimer < 0f) {
                var toWp = waypoint - planarPosition;
                var wpDistSq = toWp.sqrMagnitude;

                if (wpDistSq < 0.2f || wpDistSq > wanderRadius * wanderRadius + 1f) {
                    waypoint = planarPosition + wanderRadius * Random.onUnitSphere.WithY(0f);
                    chilloutTimer = Random.Range(minWanderInterval, maxWanderInterval);
                }
                else vel = wanderSpeedup / Mathf.Sqrt(wpDistSq) * toWp;
            }

            if (vel != Vector3.zero) {
                transform.position += speed * Time.fixedDeltaTime * vel.WithY(0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime * 7f);

                if (grounded) Jump(bounceSpeed * worry);
            }

            var y = Mathf.Max(0f, transform.position.y + bounce * Time.fixedDeltaTime);
            bounce += Physics.gravity.y;

            transform.position = transform.position.WithY(y);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Scare.GetEscapeVector(transform.position));
        }

        protected override void OnEnable() {
            base.OnEnable();
            sheepList.Add(this);
        }

        protected override void OnDisable() {
            base.OnDisable();
            sheepList.SwapRemove(this);
        }
    }
}
