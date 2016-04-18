using UnityEngine;
using System.Collections;
using JamSuite.Audio;

namespace LD35 {

    public class FallingSheep : MonoBehaviour {

        private Vector3 velocity;

        private void Start() {
            var sheep = GetComponent<Sheep>();
            if (sheep)
            {
                SheepCounter.instance.LoseSheep();
                Sfx.Play("SheepFalls");
                Destroy(sheep);
            }
        }

        private void FixedUpdate() {
            velocity += Physics.gravity * Time.fixedDeltaTime;
        }

        private void Update() {
            transform.position += velocity * Time.deltaTime;
        }
    }
}
