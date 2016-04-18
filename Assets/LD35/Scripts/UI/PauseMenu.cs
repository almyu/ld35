using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD35 {

    public class PauseMenu : MonoSingleton<PauseMenu> {

        public static void Toggle() {
            if (instance) SceneManager.UnloadScene("PauseMenu");
            else SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }
}
