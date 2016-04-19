using UnityEngine;
using UnityEngine.UI;
using JamSuite.Generative;

namespace LD35 {

    public class TrialList : HierarchyBuilder<ModTrial> {

        public Color
            activeColor = new Color32(137, 30, 46, 255),
            completedColor = Color.white,
            inactiveColor = Color.grey;

        protected override void Build() {
            if (!GameRun.instance) return;

            foreach (var trial in GameRun.instance.trials)
                Spawn(trial);
        }

        protected override void Tune(Transform spawn, ModTrial data) {
            foreach (var img in spawn.GetComponentsInChildren<Image>(true))
                if (img.name.StartsWith("Checkbox"))
                    img.gameObject.SetActive(img.name == "Checkbox" + data.status);

            foreach (var text in spawn.GetComponentsInChildren<Text>()) {
                if (text.name == "Text") text.text = data.name;
                else if (text.name == "Counter") text.text = string.Format("{0}/{1}", data.current, data.max);

                switch (data.status) {
                case ModStatus.Active:
                case ModStatus.Failed:
                    text.color = activeColor;
                    break;

                case ModStatus.Completed:
                    text.color = completedColor;
                    break;

                default:
                    text.color = inactiveColor;
                    break;
                }
            }
        }

        private void OnEnable() {
            if (Application.isPlaying) Rebuild();
        }
    }
}
