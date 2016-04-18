using UnityEngine;

namespace LD35 {

    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIFlash : MonoSingleton<UIFlash> {

        public float duration = 0.3f;
        public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        private CanvasRenderer cachedRenderer;
        private float timer;

        public static void Activate() {
            if (instance) instance.timer = instance.duration;
        }

        private void Awake() {
            cachedRenderer = GetComponent<CanvasRenderer>();
        }

        private void Update() {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F)) Activate();
#endif
            if (timer < 0f) return;

            cachedRenderer.SetAlpha(alphaCurve.Evaluate(1f - Mathf.Clamp01(timer / duration)));
            timer -= Time.unscaledDeltaTime;
        }
    }
}
