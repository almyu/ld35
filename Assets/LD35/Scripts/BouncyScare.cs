using UnityEngine;

namespace LD35 {

    public class BouncyScare : Scare {

        public static float groundEpsilon = 0.02f;
        public bool grounded { get { return y <= groundEpsilon; } }

        public float bounciness = 5f, bounceGravity = 5f;

        protected float y, bounce;

        public bool Jump(float speed, bool groundcheck = true) {
            if (groundcheck && !grounded) return false;

            bounce = Mathf.Max(bounce, speed * bounciness);
            return true;
        }

        protected virtual void FixedUpdate() {
            bounce += Physics.gravity.y * bounceGravity * Time.fixedDeltaTime;
        }

        protected virtual void LateUpdate() {
            y = Mathf.Max(0f, y + bounce * Time.deltaTime);
            transform.position = transform.position.WithY(y);
        }
    }
}
