using System;
using JamSuite.Generative;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace LD35 {

    public class UIModManager : HierarchyBuilder<Mod> {

        public Button startButton;
        public ScrollRect scrollRect;

        public Toggle selectAll;

        protected void Start() {
            Mods.Load();

            startButton.onClick.AddListener(OnStartClicked);
            selectAll.onValueChanged.AddListener((value) => OnSelectAll(value));
            LoadSelectedMods();
            //scrollRect.verticalScrollbar.value = 1f; //scroll on top
        }

        private void OnSelectAll(bool value) {
            for (int i = transform.childCount; i-- > 0;) {
                if (!Mods.modList[i].unlocked)
                    continue;

                var child = transform.GetChild(i);
                if (child != null) {
                    child.GetComponent<Toggle>().isOn = value;
                }
            }
        }

        private void LoadSelectedMods() {
            for (int i = transform.childCount; i-- > 0;) {
                var child = transform.GetChild(i);
                if (child != null) {
                    child.GetComponent<Toggle>().isOn = Mods.modList[i].active;
                }
            }
        }

        private void OnStartClicked() {
            for (int i = transform.childCount; i-- > 0;) {
                var child = transform.GetChild(i);
                if (child != null) {
                    Mods.modList[i].active = child.GetComponent<Toggle>().isOn;
                }
            }

            Mods.Save();
            SceneManager.LoadScene("Test");
        }

        protected override void Build() {
            foreach (var mod in Mods.modList) {
                Spawn(mod);
            }

            //LoadSelectedMods();
        }

        protected override void Tune(Transform spawn, Mod mod) {
            var unlockableText = spawn.FindChild("Label").GetComponent<Text>();
            var text = mod.name.ToUpperInvariant();
            if (!string.IsNullOrEmpty(mod.desc))
                text += " - " + mod.desc;

            unlockableText.text = text;

            if (mod.completed) {
                unlockableText.color = Color.white;
                unlockableText.fontStyle = FontStyle.Bold;
            }

            var toggle = spawn.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((value) => mod.active = value);

            toggle.isOn = mod.active;
            
//#if UNITY_EDITOR
//            toogle.interactable = true;
//#else
            toggle.interactable = mod.unlocked;
//#endif
        }

        protected override void Update() {
            base.Update();

            if (Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftControl))
                PlayerPrefs.DeleteAll();

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}
