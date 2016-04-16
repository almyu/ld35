using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : Scare
    {
        private Camera _camera;
        private int leftMouseBtn = 0;
        private int rightMouseBtn = 1;
        private LayerMask island;
        private Vector3 _target;

        private float runSpeed;
        private AnimationCurve runCurve;

        private Transform _shepherd;
        private bool _isWerewolf = false;

        protected void Awake()
        {
            _camera = Camera.main;
            island = LayerMask.GetMask("Island");
            
            runSpeed = Balance.instance.DogMoveSpeed;
            runCurve = Balance.instance.DogMoveCurve;
            _target = transform.position;

            _shepherd = GameObject.Find("Shepherd").transform;
        }

        protected void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                OnShepherdShift();
            }

            if (Input.GetMouseButtonDown(rightMouseBtn))
            {
                RefreshTarget();
            }

            if (Input.GetMouseButton(rightMouseBtn))
            {
                RefreshTarget();
            }

            Run();
        }

        public void OnShepherdShift()
        {
            _isWerewolf = !_isWerewolf;
            _target = _shepherd.position;
        }

        private void RefreshTarget()
        {
            if (_isWerewolf)
                return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, island))
            {
                _target = hit.point;
            }
        }

        private void Run()
        {
            var start = transform.position;

            if (Vector3.Distance(start, _target) <= 0)
                return;
            
            var distance = Vector3.Distance(start, _target);
            var currentSpeed = distance / runSpeed;
            var dir = _target - start;
            
            var progress = Time.deltaTime / currentSpeed;

            transform.position = Vector3.Lerp(start, _target, runCurve.Evaluate(progress));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir.normalized), progress * 7);
        }
    }
}