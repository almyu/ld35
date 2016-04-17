using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LD35 {

    public class GameManager : MonoSingleton<GameManager> {

        public static Shepherd shepherd { get { return Shepherd.instance; } }

        public float stomach = 1f, hungerTime= 30f;
        public float manualShapeshiftThreshold = 0.5f;
        public bool canShapeshift { get { return stomach <= manualShapeshiftThreshold; } }
        public float bulletTime = 2f, hellTime = 3f;

        private void Start() {
            UIManager.SetupSheep(Herd.instance.numSheep);
        }

        private void Update() {
            if(!shepherd.gameObject.activeInHierarchy) {
                GameOver();
            }

            if (!shepherd.isWolf) {
                stomach = Mathf.Clamp01(stomach - Time.deltaTime / hungerTime);
                UIManager.SetStomach(stomach);
                UIManager.SetShapeshiftAvailable(canShapeshift);
                UIManager.SetNormalizedTime(1f);

                if (stomach == 0f || (canShapeshift && Input.GetButtonDown("Jump")))
                    StartCoroutine(DoShapeshift());
            }
            else if (Input.GetButtonDown("Fire1")) {
                if (shepherd.AttackClosestSheep()) {
                    stomach = 1f;
                    UIManager.SetStomach(stomach);
                    UIManager.SetShapeshiftAvailable(canShapeshift);
                }
            }
        }

        private IEnumerator DoShapeshift() {
            BulletTime.active = true;
            shepherd.isWolf = true;

            UIManager.SetShapeshiftAvailable(false);

            var lastEatenSheep = SheepCounter.instance.eatenSheep;

            for (var t = 0f; t <= bulletTime; t += Time.unscaledDeltaTime) {
                UIManager.SetNormalizedTime(t / bulletTime);
                yield return null;
                if (SheepCounter.instance.eatenSheep != lastEatenSheep) break;
            }
            BulletTime.active = false;

            for (var t = 0f; t <= hellTime; t += Time.unscaledDeltaTime) {
                UIManager.SetNormalizedTime(t / hellTime);
                yield return null;
                // break if dead
            }
            UIManager.SetNormalizedTime(1f);

            shepherd.isWolf = false;
        }

        private void GameOver() {
            //Show 'restart' button here
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
