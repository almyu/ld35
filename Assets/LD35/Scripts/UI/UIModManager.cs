using System;
using JamSuite.Generative;
using UnityEngine;
using UnityEngine.UI;

namespace LD35 {

    public class UIModManager : HierarchyBuilder<Mod> {
        protected override void Build() {
            foreach (var mod in Mods.modList) {
                Spawn(mod);
            }
        }

        protected override void Tune(Transform spawn, Mod mod) {
            var unlockableText = spawn.FindChild("Label").GetComponent<Text>();
            unlockableText.text = mod.name;

            var toogle = spawn.GetComponent<Toggle>();
            toogle.onValueChanged.AddListener((value) => mod.active = value);

            toogle.isOn = mod.active;
        }
    }
}
