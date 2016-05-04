using JamSuite.Generative;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LD35 {

    public class UIModManager : HierarchyBuilder<Mod> {

        public string gameplayScene = "Gameplay";
        public Button startButton;
        public ScrollRect scrollRect;

        public Toggle selectAll;

        protected void Start() {
            Mods.Load();
            LoadSelectedMods();

            selectAll.isOn = Mods.modList.All(mod => mod.active);
            selectAll.onValueChanged.AddListener((value) => OnSelectAll(value));

            startButton.onClick.AddListener(OnStartClicked);
            //scrollRect.verticalScrollbar.value = 1f; //scroll on top
        }

        private void OnSelectAll(bool value) {
            for (int i = transform.childCount; i-- > 0; ) {
                if (!Mods.modList[i].unlocked) continue;

                var toggle = transform.GetChild(i).GetComponent<Toggle>();
                if (toggle) toggle.isOn = value;
            }
        }

        private void LoadSelectedMods() {
            for (int i = transform.childCount; i-- > 0; ) {
                var toggle = transform.GetChild(i).GetComponent<Toggle>();
                if (toggle) toggle.isOn = Mods.modList[i].active;
            }
        }

        private void OnStartClicked() {
            for (int i = transform.childCount; i-- > 0; ) {
                var toggle = transform.GetChild(i).GetComponent<Toggle>();
                if (toggle) Mods.modList[i].active = toggle.isOn;
            }

            Mods.Save();
            SceneManager.LoadScene(gameplayScene);
        }

        protected override void Build() {
            foreach (var mod in Mods.modList)
                Spawn(mod);

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
            toggle.isOn = mod.active;
            toggle.onValueChanged.AddListener((value) => mod.active = value);

            toggle.interactable = mod.unlocked || Application.isEditor;
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
