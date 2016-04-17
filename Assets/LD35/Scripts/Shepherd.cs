using UnityEngine;

namespace LD35 {

    public class Shepherd : Scare {

        public static Shepherd instance {
            get { return JamSuite.SingletonHelper<Shepherd>.instance; }
        }

        public float manSpeed = 4.5f, manScareRadius = 3f, manScariness = 0.7f;
        public float wolfSpeed = 4.5f, wolfScareRadius = 6f, wolfScariness = 1f;
        
        public AttackArea attackArea;

        public GameObject shepherdGO;
        public GameObject werewolfGO;

        [HideInInspector]
        public float speed = 4f;
        
        private Animator shepherdAnimator;
        private Animator werewolfAnimator;

        protected void Awake() {
            werewolfGO.SetActive(false);
            //shepherdGO.SetActive(false);

            shepherdAnimator = shepherdGO.GetComponent<Animator>();
            werewolfAnimator = werewolfGO.GetComponent<Animator>();
        }

        public bool isWolf {
            get { return _isWolf; }
            set {
                _isWolf = value;

                speed = value ? wolfSpeed : manSpeed;
                radius = value ? wolfScareRadius : manScareRadius;
                power = value ? wolfScariness : manScariness;

                ShiftShape();
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
                
                transform.position += speed * Time.unscaledDeltaTime * dir;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.unscaledDeltaTime * 7f);
            }

            if(isWolf)
                werewolfAnimator.SetFloat("Speed", speed * axes.magnitude);
            else
                shepherdAnimator.SetFloat("Speed", speed * axes.magnitude);
        }

        private void ShiftShape() {
            if (isWolf) {
                shepherdGO.SetActive(false);
                werewolfGO.SetActive(true);
            }
            else {
                shepherdGO.SetActive(true);
                werewolfGO.SetActive(false);
            }
        }

        public bool AttackClosestSheep() {
            return attackArea.Attack();
        }
    }
}
