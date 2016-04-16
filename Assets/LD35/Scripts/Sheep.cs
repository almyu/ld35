﻿using UnityEngine;
using System.Collections;

namespace LD35 {

    public class Sheep : Scare {

        public float speed = 5f;

        private void FixedUpdate() {
            var vel = Scare.GetEscapeVector(transform.position);
            transform.position += speed * Time.fixedDeltaTime * vel.WithY(0f);

            if (vel != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime * 7f);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Scare.GetEscapeVector(transform.position));
        }
    }
}
