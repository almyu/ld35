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

            while (Vector3.Distance(start, target) > 0)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Slerp(start, target, runCurve.Evaluate(elapsed / currentSpeed));
                yield return new WaitForEndOfFrame();
            }

            transform.position = target;

            if (onComplete != null)
                onComplete();
        }
    }
}