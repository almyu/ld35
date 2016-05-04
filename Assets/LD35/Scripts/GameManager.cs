using JamSuite.Logic;
using System.Collections;
using UnityEngine;

namespace LD35 {

    public class GameManager : MonoSingleton<GameManager> {

        public static Shepherd shepherd { get { return Shepherd.instance; } }

        public float stomach = 1f, hungerTime= 30f;
        public float manualShapeshiftThreshold = 0.5f;
        public bool canShapeshift { get { return stomach <= manualShapeshiftThreshold; } }
        public float bulletTimeScale = 0.1f, bulletTime = 2f, hellTime = 3f;

        public GameObject zeroWindPrefab, leftWindPrefab, rightWindPrefab;

        private void Start() {
            UIManager.SetupSheep(Herd.instance.numSheep);

            if (ModID.Wind.IsModActive()) {
                Mods.Wind.Init();
                Instantiate(Mods.Wind.left ? leftWindPrefab : rightWindPrefab);
            }
            else Instantiate(zeroWindPrefab);
        }

        private void Update() {
            if (!shepherd.enabled) PauseMenu.Die();
            if (Counters.Get("Eaten") + Counters.Get("Lost") >= Herd.instance.numSheep) PauseMenu.GameOver();

            if (!shepherd.isWolf) {
                if (ModID.Diet.IsModActive()) {
                    stomach = Mods.Diet.dietValue;
                }
                else {
                    stomach = Mathf.Clamp01(stomach - Time.deltaTime / hungerTime);
                }

                UIManager.SetStomach(stomach);
                UIManager.SetShapeshiftAvailable(canShapeshift);
                UIManager.SetNormalizedTime(1f);

                if (stomach == 0f || (canShapeshift && Input.GetButtonDown("Jump")))
                    StartCoroutine(DoShapeshift());
            }
            else if (Input.GetButtonDown("Jump")) {
                if (shepherd.AttackClosestSheep()) {
                    stomach = 1f;
                    UIManager.SetStomach(stomach);
                    UIManager.SetShapeshiftAvailable(canShapeshift);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                PauseMenu.Pause();

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab))
                stomach = 1f - stomach;
#endif
        }

        private void OnDestroy() {
            Time.timeScale = 1f;
        }

        private IEnumerator DoShapeshift() {
            Time.timeScale = bulletTimeScale;
            shepherd.isWolf = true;

            UIManager.SetShapeshiftAvailable(false);

            var counter = Counters.Find("Eaten");
            var lastEatenSheep = counter.Value;

            for (var t = 0f; t <= bulletTime; t += Time.unscaledDeltaTime) {
                UIManager.SetNormalizedTime(t / bulletTime);
                yield return null;
                if (counter.Value != lastEatenSheep) break;
            }
            Time.timeScale = 1f;

            for (var t = 0f; t <= hellTime; t += Time.unscaledDeltaTime) {
                UIManager.SetNormalizedTime(t / hellTime);
                yield return null;
                // break if dead
            }
            UIManager.SetNormalizedTime(1f);

            shepherd.isWolf = false;
        }
    }
}
