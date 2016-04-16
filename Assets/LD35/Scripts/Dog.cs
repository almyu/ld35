using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : MonoBehaviour
    {
        private Camera _camera;
        private int leftMouseBtn = 0;

        protected void Awake()
        {
            _camera = Camera.main;
        }

        protected void Update()
        {
            if (Input.GetMouseButtonUp(leftMouseBtn))
            {
                var target = _camera.ScreenToWorldPoint(Input.mousePosition);
                StopCoroutine("Run");
                StartCoroutine(Run(target));
            }
        }

        private IEnumerator Run(Vector3 target, Action onComplete = null)
        {
            var start = transform.position;
            var runSpeed = Balance.instance.DogMoveSpeed;
            var elapsed = 0f;

            while (Vector3.Distance(start, target) > 0)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, target, elapsed / runSpeed);
                yield return new WaitForEndOfFrame();
            }

            transform.position = target;

            if (onComplete != null)
                onComplete();
        }
    }
}