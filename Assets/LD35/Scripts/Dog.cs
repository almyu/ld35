﻿using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : Scare
    {
        public float runSpeed = 6f;

        private Camera _camera;
        private Vector3 _target;
        
        private Shepherd _shepherd;
        private bool _gameOver = false; //Remove later

        private static Plane _xz = new Plane(Vector3.up, Vector3.zero);

        protected void Awake()
        {
            runSpeed = Balance.instance.DogMoveSpeed;

            _target = transform.position;
            _shepherd = Shepherd.instance;
        }

        protected void Update()
        {
            if(_shepherd.isWolf)
            {
                if (Vector3.Distance(transform.position, _shepherd.transform.position) <= Balance.instance.DogAttackRange
                    && _gameOver == false)
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
            yield return new WaitForSeconds(Balance.instance.DogWaitBeforeAttackShepherd);
            _target = _shepherd.transform.position;
        }
        
        private void RefreshTarget()
        {
            if (_shepherd.isWolf)
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var dist = 0f;
            if (_xz.Raycast(ray, out dist))
            {
                _target = ray.origin + ray.direction * dist;
            }
        }

        private float angle = 45f;
        private void HelpFollowLostSheep()
        {
            angle *= -1;

            var planarPos = transform.position.WithY(0f);
            var radius = planarPos.magnitude;

            _target = Quaternion.AngleAxis(angle * Mathf.PI * 2f / radius, Vector3.up) * planarPos;
        }

        private void Run()
        {
            var start = transform.position;

            if (Vector3.Distance(start, _target) == 0)
            {
                HelpFollowLostSheep();
                return;
            }

            transform.position = Vector3.MoveTowards(start, _target, Time.deltaTime * runSpeed);

            var dir = _target - start;  
            if (dir.sqrMagnitude > 0.015f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * runSpeed);
        }
    }
}
