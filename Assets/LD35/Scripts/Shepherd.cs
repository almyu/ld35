using JamSuite.Audio;
using System.Collections;
using UnityEngine;

namespace LD35 {

    public class Shepherd : Scare {

        public static Shepherd instance {
            get { return JamSuite.SingletonHelper<Shepherd>.instance; }
        }

        public float manSpeed = 4.5f, manScareRadius = 3f, manScariness = 0.7f, manAnimationSpeed = 1f; 
        public float wolfSpeed = 4.5f, wolfScareRadius = 6f, wolfScariness = 1f, wolfAnimationSpeed = 1f, sheepKilledScareFactor = 1.5f;
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

            if (ModID.Faster.IsModActive()) {
                speed *= Mods.Faster.factor;
            }
        }

        public bool isWolf {
            get { return _isWolf; }
            set {
                _isWolf = value;

                speed = value ? wolfSpeed : manSpeed;
                radius = value ? wolfScareRadius : manScareRadius;
                power = value ? wolfScariness : manScariness;

                if (ModID.Faster.IsModActive()) {
                    speed *= Mods.Faster.factor;
                }

                UIManager.instance.RefreshPortrait();

                ShiftShape();
            }
        }
        private bool _isWolf;

        private float elapsed = 0f;
        private void Update() {
            if (isDead) {
                
                elapsed += Time.unscaledDeltaTime;
                if (elapsed >= PauseAfterDeadInSec) {
                    enabled = false;
                    //this.gameObject.SetActive(false);
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
                
                transform.position = World.Clamp(transform.position + speed * Time.unscaledDeltaTime * dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.unscaledDeltaTime * 7f);
            }

            if (isWolf)
                werewolfAnimator.SetFloat("Speed", wolfAnimationSpeed * axes.magnitude);
            else
                shepherdAnimator.SetFloat("Speed", manAnimationSpeed * axes.magnitude);
        }

        private void ShiftShape() {
            if (isDead) return;

            Sfx.Play("ShapeShiftRoar");

            shepherdGO.SetActive(!isWolf);
            werewolfGO.SetActive(isWolf);

            WolfVisionMaterials.SwapAll();
            UIFlash.Activate();
        }

        public bool AttackClosestSheep() {
            werewolfAnimator.SetTrigger("Punch");
            Sfx.Play("WolfPunches");

            if (!attackArea.victim) return false;

            var dir = attackArea.victim.planarPosition - planarPosition;
            if (!attackArea.Attack()) return false;

            transform.rotation = Quaternion.LookRotation(dir);

            Screenshake.Activate();

            //dirty hack to increase scary on eating
            power *= sheepKilledScareFactor;

            return true;
        }

        public void Die() {
            isDead = true;
            Sfx.Play("ShepherdDied");
            werewolfAnimator.SetTrigger("IsDead");
        }
    }
}
