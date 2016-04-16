using UnityEngine;

namespace LD35 {

    public class Shepherd : Scare {

        public static Shepherd instance {
            get { return JamSuite.SingletonHelper<Shepherd>.instance; }
        }

        public float speed = 4f;

        public bool isWolf {
            get { return _isWolf; }
            set {
                _isWolf = value;

                var balance = Balance.instance;
                speed = value ? balance.WolfSpeed : balance.ManSpeed;
                radius = value ? balance.WolfScareRadius : balance.ManScareRadius;
                power = value ? balance.WolfScariness : balance.ManScariness;
            }
        }
        private bool _isWolf;

        private void Update() {
            var axes = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            axes.Normalize();

            if (axes != Vector3.zero) {
                var camXf = Camera.main.transform;
                var dir =
                    camXf.right.WithY(0f).normalized * axes.x +
                    camXf.forward.WithY(0f).normalized * axes.z;

                transform.position += speed * Time.deltaTime * dir;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 7f);
            }

            if (Input.GetButtonDown("Jump"))
                isWolf = !isWolf;
        }
    }
}
