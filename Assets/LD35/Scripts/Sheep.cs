using UnityEngine;
using System.Collections;

namespace LD35 {

    public class Sheep : Scare {

        public float speed = 5f;
        public float bounceInertia = 0.95f;

        private Vector3 bounce;

        private void FixedUpdate() {
            Vector3 vel;
            Scare.GetEscapeVector(transform.position, out vel, out bounce);

            if (vel != Vector3.zero) {
                transform.position += speed * Time.fixedDeltaTime * vel.WithY(0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime * 7f);
            }

            transform.position += Time.deltaTime * bounce.WithY(0f);
            bounce *= bounceInertia;
        }

        private void OnDrawGizmos() {
            Vector3 vel, bnc;
            Scare.GetEscapeVector(transform.position, out vel, out bnc);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, vel);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, bnc);
        }
    }
}
