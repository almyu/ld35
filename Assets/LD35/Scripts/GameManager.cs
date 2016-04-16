using UnityEngine;
using System.Collections;

namespace LD35 {

    public class GameManager : MonoSingleton<GameManager> {

        public static Shepherd shepherd {
            get { return Shepherd.instance; }
        }

        public float bulletTime = 2f, hellTime = 3f;

        private void Update() {
            if (Input.GetButtonDown("Jump") && !shepherd.isWolf)
                StartCoroutine(DoShapeshift());
        }

        private IEnumerator DoShapeshift() {
            BulletTime.active = true;
            shepherd.isWolf = true;

            for (var t = 0f; t <= bulletTime; t += Time.unscaledDeltaTime) {
                yield return null;
                // check count, break if sheep eaten
            }
            BulletTime.active = false;

            for (var t = 0f; t < hellTime; t += Time.unscaledDeltaTime) {
                yield return null;
                // break if dead
            }
            shepherd.isWolf = false;
        }
    }
}
