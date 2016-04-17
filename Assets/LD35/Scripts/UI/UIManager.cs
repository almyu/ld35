using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace LD35 {

    public class UIManager : MonoSingleton<UIManager> {

        public RectTransform stomachFill;
        public UICircle timer;
        public GameObject portrait;
        public Text eatenSheepText, lostSheepText, totalSheepText;

        public static void SetStomach(float stomach) {
            if (instance && instance.stomachFill)
                instance.stomachFill.anchorMax = instance.stomachFill.anchorMax.WithX(stomach);
        }

        public static void SetNormalizedTime(float t) {
            if (instance && instance.timer) {
                instance.timer.fillPercent = Mathf.RoundToInt((1f - t) * 100f);
                instance.timer.SetVerticesDirty();
            }
        }

        public static void SetShapeshiftAvailable(bool really) {
            if (instance && instance.portrait)
                instance.portrait.SetActive(really);
        }

        public static void SetSheepStats(int eaten, int lost, int total) {
            if (!instance) return;
            if (instance.eatenSheepText) instance.eatenSheepText.text = eaten.ToString();
            if (instance.lostSheepText) instance.lostSheepText.text = lost.ToString();
            if (instance.totalSheepText) instance.totalSheepText.text = total.ToString();
        }
    }
}
