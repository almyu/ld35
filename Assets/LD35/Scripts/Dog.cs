using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : Scare
    {
        public float runSpeed = 6f;
        public float attackDelay = 1f;
        public float attackRange = 0.5f;

        public Vector3 target {
            get { return _target; }
            set { _target = World.Clamp(value); }
        }
        private Vector3 _target;

        private Camera _camera;
        
        private Shepherd _shepherd;
        private bool _gameOver = false; //Remove later

        private static Plane _xz = new Plane(Vector3.up, Vector3.zero);

        protected void Awake()
        {
            target = transform.position;
            _shepherd = Shepherd.instance;
        }

        protected void Update()
        {
            if(_shepherd.isWolf)
            {
                if (Vector3.Distance(transform.position, _shepherd.transform.position) <= attackRange
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
            yield return new WaitForSeconds(attackDelay);
            target = _shepherd.transform.position;
        }
        
        private void RefreshTarget()
        {
            if (_shepherd.isWolf)
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var dist = 0f;
            if (_xz.Raycast(ray, out dist))
                target = ray.origin + ray.direction * dist;
        }

        private float angle = 45f;
        private void HelpFollowLostSheep()
        {
            angle *= -1;

            var radius = planarPosition.magnitude;
            target = Quaternion.AngleAxis(angle * Mathf.PI * 2f / radius, Vector3.up) * planarPosition;
        }

        private void Run()
        {
            var start = transform.position;

            if (start == target)
            {
                HelpFollowLostSheep();
                return;
            }

            transform.position = Vector3.MoveTowards(start, target, Time.deltaTime * runSpeed);

            var dir = target - start;  
            if (dir.sqrMagnitude > 0.015f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * runSpeed);
        }
    }
}
