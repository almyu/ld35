using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : Scare
    {
        private Camera _camera;
        //private int leftMouseBtn = 0;
        private int rightMouseBtn = 1;
        private LayerMask island;
        private Vector3 _target;

        private float runSpeed;
        
        private Shepherd _shepherd;

        protected void Awake()
        {
            _camera = Camera.main;
            island = LayerMask.GetMask("Island");
            
            runSpeed = Balance.instance.DogMoveSpeed;
            _target = transform.position;

            _shepherd = Shepherd.instance;
        }

        protected void Update()
        {
            if(_shepherd.isWolf)
            {
                _target = _shepherd.transform.position;
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
        
        private void RefreshTarget()
        {
            if (_shepherd.isWolf)
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

            if (Vector3.Distance(start, _target) == 0)
                return;
            
            transform.position = Vector3.MoveTowards(start, _target, Time.deltaTime * runSpeed);

            var dir = _target - start;  
            if (dir.sqrMagnitude > 0.015f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * runSpeed);
        }
    }
}