using System.Collections;
using UnityEngine;

namespace LD35 {

    public class Shepherd : Scare {

        public static Shepherd instance {
            get { return JamSuite.SingletonHelper<Shepherd>.instance; }
        }

        public float manSpeed = 4.5f, manScareRadius = 3f, manScariness = 0.7f, manAnimationSpeed = 1f; 
        public float wolfSpeed = 4.5f, wolfScareRadius = 6f, wolfScariness = 1f, wolfAnimationSpeed = 1f;
        public float PauseAfterDeadInSec = 5f;

        public AttackArea attackArea;

        public GameObject shepherdGO;
        public GameObject werewolfGO;

        [HideInInspector]
        public float speed = 4f;
        
        private Animator shepherdAnimator;
        private Animator werewolfAnimator;

        private bool isDead;

        protected void Awake() {
            werewolfGO.SetActive(false);

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

        private float elapsed = 0f;
        private void Update() {
            if (isDead) {
                
                elapsed += Time.unscaledDeltaTime;
                if (elapsed >= PauseAfterDeadInSec) {
                    this.gameObject.SetActive(false);
                }
                return;
            }

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

            if (isWolf)
                werewolfAnimator.SetFloat("Speed", wolfAnimationSpeed * axes.magnitude);
            else
                shepherdAnimator.SetFloat("Speed", manAnimationSpeed * axes.magnitude);
        }

        private void ShiftShape() {
            if (isDead) {
                return;
            }

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
            var hit = attackArea.Attack();
            if (hit) {
                werewolfAnimator.SetTrigger("Punch");
            }
            return hit;
        }

        public void Die() {
            isDead = true;
            werewolfAnimator.SetTrigger("IsDead");
        }
    }
}
