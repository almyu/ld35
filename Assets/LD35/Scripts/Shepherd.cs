﻿using UnityEngine;

namespace LD35 {

    public class Shepherd : Scare {

        public static Shepherd instance {
            get { return JamSuite.SingletonHelper<Shepherd>.instance; }
        }

        public float manSpeed = 4.5f, manScareRadius = 3f, manScariness = 0.7f;
        public float wolfSpeed = 4.5f, wolfScareRadius = 6f, wolfScariness = 1f, wolfAttackRadius = 3f;

        [HideInInspector]
        public float speed = 4f;

        private GameObject shepherdGO;
        private Animator shepherdAnimator;
        private GameObject wolfGO;

        private float directionDampTime = .25f;

        protected void Awake() {
            shepherdGO = transform.FindChild("Shepherd_Model").gameObject;
            shepherdAnimator = shepherdGO.GetComponent<Animator>();
        }

        public bool isWolf {
            get { return _isWolf; }
            set {
                _isWolf = value;

                speed = value ? wolfSpeed : manSpeed;
                radius = value ? wolfScareRadius : manScareRadius;
                power = value ? wolfScariness : manScariness;
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

            shepherdAnimator.SetFloat("Speed", speed * axes.magnitude);
        }

        public bool AttackClosestSheep() {
            var target = Sheep.GetAnyInRange(planarPosition, wolfAttackRadius);
            if (!target) return false;

            target.Eat();
            transform.position = target.planarPosition.WithY(transform.position.y);
            return true;
        }
    }
}
