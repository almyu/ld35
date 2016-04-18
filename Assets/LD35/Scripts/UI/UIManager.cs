using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace LD35 {

    public class UIManager : MonoSingleton<UIManager> {

        public RectTransform stomachFill;
        public bool stomachVertical;
        public UICircle timer;
        public GameObject portrait;
        public Text eatenSheepText, lostSheepText, totalSheepText;
        public Graphic sheepIconPrefab;
        public LayoutGroup eatenSheepIcons, lostSheepIcons;

        public GameObject gameOverWindow;
        public Button restartButton;

        public Color baseSheepColor = Color.white, eatenSheepColor = Color.red, lostSheepColor = Color.grey;

        private void Awake() {
            gameOverWindow.SetActive(false);
            restartButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        }

        public static void SetStomach(float stomach) {
            if (instance && instance.stomachFill)
                instance.stomachFill.anchorMax = instance.stomachVertical
                    ? instance.stomachFill.anchorMax.WithY(stomach)
                    : instance.stomachFill.anchorMax.WithX(stomach);
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

        public static void SetSheepStats(int eaten, int lost) {
            if (!instance) return;

            if (instance.eatenSheepText) instance.eatenSheepText.text = eaten.ToString();
            if (instance.lostSheepText) instance.lostSheepText.text = lost.ToString();
        }

        public static void SetupSheep(int numSheep) {
            if (!instance) return;

            if (instance.totalSheepText)
                instance.totalSheepText.text = numSheep.ToString();
            
            if (!instance.eatenSheepIcons || !instance.lostSheepIcons) return;

            var eatenXf = instance.eatenSheepIcons.transform;

            for (int i = numSheep; i-- > 0; ) {
                var child = Instantiate(instance.sheepIconPrefab);
                child.color = instance.baseSheepColor;

                var xf = child.transform;
                xf.SetParent(eatenXf, false);
            }
        }

        public static void EatSheep() {
            if (!instance || !instance.eatenSheepIcons) return;

            foreach (Transform child in instance.eatenSheepIcons.transform) {
                var graphic = child.GetComponent<Graphic>();
                if (graphic && graphic.color == instance.baseSheepColor) {
                    graphic.color = instance.eatenSheepColor;
                    return;
                }
            }
        }

        public static void LoseSheep() {
            if (!instance || !instance.eatenSheepIcons || !instance.lostSheepIcons) return;

            var srcGroupXf = instance.eatenSheepIcons.transform;
            var dstGroupXf = instance.lostSheepIcons.transform;

            if (srcGroupXf.childCount > 0) {
                var child = srcGroupXf.GetChild(srcGroupXf.childCount - 1);
                child.SetParent(dstGroupXf, false);

                var graphic = child.GetComponent<Graphic>();
                if (graphic) graphic.color = instance.lostSheepColor;
            }
        }

        public void ShowGameOverWindow() {
            gameOverWindow.SetActive(true);
        }
    }
}
