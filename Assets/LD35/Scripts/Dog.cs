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
        private bool _gameOver = false; //Remove later

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
                if (Vector3.Distance(transform.position, _shepherd.transform.position) <= Balance.instance.DogAttackRange
                    && _gameOver == false)
                {
                    _gameOver = true;
                    Debug.Log("GAME OVER: you dead, all your friends are dead, your family is dead, your cat is dead..etc");
                }

                StartCoroutine(WaitAndAttackShepherd());
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

        private IEnumerator WaitAndAttackShepherd()
        {
            yield return new WaitForSeconds(Balance.instance.DogWaitBeforeAttackShepherd);
            _target = _shepherd.transform.position;
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

        private float angle = 45f;
        private void HelpFollowLostSheep()
        {
            angle *= -1;
            //var lostSheep = Herd.GetLostSheep();
            var centerOfMass = Herd.GetCenterOfMass();
            var quat = Quaternion.AngleAxis(angle, Vector3.up);
            var toDog = transform.position - centerOfMass;
            toDog = quat * toDog;
            toDog += centerOfMass;
            _target = toDog;
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