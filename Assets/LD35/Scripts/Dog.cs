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

        protected void Awake()
        {
            _camera = Camera.main;
        }

        protected void Update()
        {
            if (Input.GetMouseButtonUp(rightMouseBtn))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
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
            var elapsed = 0f;

            var distance = Vector3.Distance(start, target);
            var currentSpeed = distance / runSpeed;

            while (Vector3.Distance(start, target) > 0)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, target, elapsed / currentSpeed);
                yield return new WaitForEndOfFrame();
            }

            transform.position = target;

            if (onComplete != null)
                onComplete();
        }
    }
}