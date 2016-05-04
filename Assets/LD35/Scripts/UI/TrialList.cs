using JamSuite.Generative;
using JamSuite.Logic;
using UnityEngine;
using UnityEngine.UI;

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
                else if (text.name == "Counter") text.text = string.Format("{0}/{1}", data.progress, data.mod.winCount);

                if (data.mod.completed) text.color = completedColor;
                else if (!data.mod.unlocked) text.color = inactiveColor;
                else text.color = activeColor;
            }
        }
    }
}
