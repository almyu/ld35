using UnityEngine;
using System.Collections;
using System;

namespace LD35
{
    public class Dog : Scare
    {
        public float runSpeed = 6f;
        public float herdingRadiusPrio = 1f;
        public float herdingDistPrio = 2f;
        public float herdingExtraRadius = 2f;
        public float attackDelay = 1f;
        public float attackRange = 0.5f;

        public Vector3 target {
            get { return _target; }
            set {
                _target = World.Clamp(value);
                _position = planarPosition;
            }
        }
        private Vector3 _target, _position;
                
        private bool _gameOver = false; //Remove later

        private static Plane _xz = new Plane(Vector3.up, Vector3.zero);

        protected void Awake()
        {
            target = planarPosition;

            if (ModID.Faster.IsModActive()) {
                runSpeed *= Mods.Faster.factor;
            }
        }

        protected void Update()
        {
            if (_gameOver)
                return;

            if (Shepherd.instance.isWolf)
            {
                if (WerewolfKilledByDog())
                {
                    Shepherd.instance.Die();
                    _gameOver = true;
                    return;
                }

                StartCoroutine(WaitAndAttackShepherd());
            }

            if (Input.GetButton("Fire2") || Input.GetButton("Fire1"))
            {
                if (!ModID.UndertrainedDog.IsModActive())
                    RefreshTarget();
            }

            Run();
        }

        private bool WerewolfKilledByDog() {
            return Vector3.Distance(planarPosition, Shepherd.instance.planarPosition) <= attackRange;
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
            if (_xz.Raycast(ray, out dist))
                target = ray.origin + ray.direction * dist;
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

            var polarSpeed = new Vector2(
                Mathf.Abs(herdingTarget.polarPosition.x + herdingExtraRadius - polarPosition.x),
                World.AngularDistance(polarPosition.y, herdingTarget.polarPosition.y));

            polarSpeed.y *= Mathf.Deg2Rad * polarPosition.x;

            if (polarSpeed.sqrMagnitude > 1.1f)
                polarSpeed.Normalize();

            polarSpeed.y *= Mathf.Rad2Deg / polarPosition.x;
            polarSpeed *= runSpeed * Time.deltaTime;

            polarPosition.x = Mathf.MoveTowards(polarPosition.x, herdingTarget.polarPosition.x + herdingExtraRadius, polarSpeed.x);
            polarPosition.y = Mathf.MoveTowardsAngle(polarPosition.y, herdingTarget.polarPosition.y, polarSpeed.y);

            var nextPlanarPosition = World.GetPlanar(polarPosition);
            var planarDirection = nextPlanarPosition - planarPosition;

            transform.position = nextPlanarPosition;

            if (planarDirection != Vector3.zero)
            {
                var desiredRotation = Quaternion.LookRotation(planarDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 7f);
            }
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
            var angularDiff = Mathf.DeltaAngle(polarPos.y, herdingTarget.polarPosition.y);

            var polarSpeed = new Vector2(
                Mathf.Abs(herdingTarget.polarPosition.x + herdingExtraRadius - polarPosition.x),
                World.AngularDistance(polarPosition.y, herdingTarget.polarPosition.y));

            polarSpeed.y *= Mathf.Deg2Rad * polarPosition.x;

            UnityEditor.Handles.Label(herdingTarget.sheep.planarPosition.WithY(2.5f), string.Format("{0:n0}°\n{1}", angularDiff, polarSpeed));
        }
#endif
    }
}
