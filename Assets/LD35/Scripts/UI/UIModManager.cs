using System;
using JamSuite.Generative;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD35 {

    public class UIModManager : HierarchyBuilder<Mod> {

        public Button startButton;
        public ScrollRect scrollRect;

        protected void Start() {
            startButton.onClick.AddListener(OnStartClicked);
            scrollRect.verticalScrollbar.value = 1f; //scroll on top
        }

        private void OnStartClicked() {

            for (int i = transform.childCount; i-- > 0;) {
                var child = transform.GetChild(i);
                if (child != null) {
                    Mods.modList[i].active = child.GetComponent<Toggle>().isOn;
                }
            }

            SceneManager.LoadScene("Test");
        }

        protected override void Build() {
            foreach (var mod in Mods.modList) {
                Spawn(mod);
            }
        }

        protected override void Tune(Transform spawn, Mod mod) {
            var unlockableText = spawn.FindChild("Label").GetComponent<Text>();
            unlockableText.text = mod.name + "\n" + mod.desc;

            var toogle = spawn.GetComponent<Toggle>();
            toogle.onValueChanged.AddListener((value) => mod.active = value);

            toogle.isOn = mod.active;
            toogle.interactable = mod.unlocked;
#if UNITY_EDITOR
            toogle.interactable = true;
#endif
        }

        protected override void Update() {
            base.Update();

            if (Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftControl))
                PlayerPrefs.DeleteAll();
        }
    }
}
