using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LD35 {

    public class PauseMenu : MonoSingleton<PauseMenu> {

        public enum Mode {
            Pause,
            Death,
            GameOver
        }
        public static Mode mode;

        private static bool Load() {
            if (instance) return false;

            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
            return true;
        }

        public static void Unload() {
            SceneManager.UnloadScene("PauseMenu");
        }

        public static void Pause() {
            mode = Mode.Pause;
            if (!Load()) Unload();
        }

        public static void Die() {
            mode = Mode.Death;
            Load();
        }

        public static void GameOver() {
            mode = Mode.GameOver;
            Load();
        }

        public Text labelPaused, labelDied, labelGameOver;
        public Button buttonResume;

        private void OnEnable() {
            Time.timeScale = 0f;

            if (labelPaused) labelPaused.gameObject.SetActive(mode == Mode.Pause);
            if (labelDied) labelDied.gameObject.SetActive(mode == Mode.Death);
            if (labelGameOver) labelGameOver.gameObject.SetActive(mode == Mode.GameOver);

            if (buttonResume) buttonResume.interactable = mode == Mode.Pause;
        }

        private void Update() {
            Time.timeScale = 0f;
        }

        private void OnDisable() {
            Time.timeScale = 1f;
        }

        public void GoTo(string name) {
            SceneManager.LoadScene(name);
        }

        public void Resume() {
            Unload();
        }
    }
}
