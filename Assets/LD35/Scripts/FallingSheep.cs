using JamSuite.Audio;
using UnityEngine;

namespace LD35 {

    public class FallingSheep : MonoBehaviour {

        private Vector3 velocity;

        private void Start() {
            var sheep = GetComponent<Sheep>();
            if (sheep) {
                GameRun.OnLost(Herd.instance.InferType(sheep));
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
