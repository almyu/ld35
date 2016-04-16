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

        protected void Awake()
        {
            _camera = Camera.main;
            island = LayerMask.GetMask("Island");
        }

        protected void Update()
        {
            if (Input.GetMouseButtonDown(rightMouseBtn))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, island))
                {
                    StopCoroutine("Run");
                    StartCoroutine(Run(hit.point));
                }

            }
        }

        private IEnumerator Run(Vector3 target, Action onComplete = null)
        {
            var start = transform.position;
            var runSpeed = Balance.instance.DogMoveSpeed;
            var runCurve = Balance.instance.DogMoveCurve;
            var elapsed = 0f;

            var distance = Vector3.Distance(start, target);
            var currentSpeed = distance / runSpeed;
            var dir = target - start;

            while (Vector3.Distance(start, target) > 0)
            {
                elapsed += Time.deltaTime;
                var progress = elapsed / currentSpeed;

                transform.position = Vector3.Lerp(start, target, runCurve.Evaluate(progress));

                
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir.normalized), progress * 7);
                yield return new WaitForEndOfFrame();
            }

            transform.position = target;

            if (onComplete != null)
                onComplete();
        }
    }
}