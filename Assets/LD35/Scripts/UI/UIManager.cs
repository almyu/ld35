using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace LD35 {

    public class UIManager : MonoSingleton<UIManager> {

        public RectTransform stomachFill;
        public UICircle timer;

        public void SetStomach(float stomach) {
            stomachFill.anchorMax = stomachFill.anchorMax.WithX(stomach);
        }

        public void SetNormalizedTime(float t) {
            timer.fillPercent = Mathf.RoundToInt((1f - t) * 100f);
        }
    }
}
