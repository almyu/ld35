﻿using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : Scare
    {
        public float runSpeed = 6f;
        public float herdingRadiusPrio = 1f;
        public float herdingDistPrio = 2f;
        public float attackDelay = 1f;
        public float attackRange = 0.5f;

        public Vector3 target {
            get { return _target; }
            set { _target = World.Clamp(value); }
        }
        private Vector3 _target, _position;

        private Camera _camera;
        
        private Shepherd _shepherd;
        private bool _gameOver = false; //Remove later

        private static Plane _xz = new Plane(Vector3.up, Vector3.zero);

        protected void Awake()
        {
            target = _position = transform.position;
            _shepherd = Shepherd.instance;
        }

        protected void Update()
        {
            if(Shepherd.instance.isWolf)
            {
                if (Vector3.Distance(planarPosition, Shepherd.instance.planarPosition) <= attackRange && !_gameOver)
                {
                    _gameOver = true;
                    Debug.Log("GAME OVER: you dead, all your friends are dead, your family is dead, your cat is dead..etc");
                }

                StartCoroutine(WaitAndAttackShepherd());
            }

            if (Input.GetButton("Fire2"))
            {
                RefreshTarget();
            }

            Run();
        }

        private IEnumerator WaitAndAttackShepherd()
        {
            yield return new WaitForSeconds(attackDelay);
            target = Shepherd.instance.planarPosition;
        }
        
        private void RefreshTarget()
        {
            if (Shepherd.instance.isWolf) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var dist = 0f;
            if (_xz.Raycast(ray, out dist)) {
                _position = planarPosition;
                target = ray.origin + ray.direction * dist;
            }
        }

        private void Run()
        {
            if (_position == target)
            {
                Orbit();
                return;
            }

            transform.position = _position = Vector3.MoveTowards(_position, target, Time.deltaTime * runSpeed);

            var dir = target - _position;
            if (dir.sqrMagnitude > 0.015f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * runSpeed);
        }

        private void Orbit()
        {
            var polarPosition = World.GetPolar(planarPosition);
            var herdingTarget = HerdingTactics.FindTarget(polarPosition, herdingRadiusPrio, herdingDistPrio);

            polarPosition.x = Mathf.MoveTowards(polarPosition.x, herdingTarget.polarPosition.x += 2f, runSpeed * Time.deltaTime);
            polarPosition.y = Mathf.MoveTowardsAngle(polarPosition.y, herdingTarget.polarPosition.y, runSpeed / polarPosition.x * Mathf.Rad2Deg * Time.deltaTime);
            transform.position = World.GetPlanar(polarPosition);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var polarPosition = World.GetPolar(planarPosition);
            var herdingTarget = HerdingTactics.FindTarget(polarPosition, herdingRadiusPrio, herdingDistPrio);

            if (!herdingTarget.sheep) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(herdingTarget.sheep.planarPosition, 1f);

            var polarPos = World.GetPolar(planarPosition);
            var angularDiff = World.AngularDiff(polarPos.y, herdingTarget.polarPosition.y);
            UnityEditor.Handles.Label(herdingTarget.sheep.planarPosition.WithY(2.5f), string.Format("{0:n0}°", angularDiff));
        }
#endif
    }
}
