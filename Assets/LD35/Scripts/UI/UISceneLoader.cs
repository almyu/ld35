using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD35 {

    public class UISceneLoader : MonoBehaviour {

        private void Awake() {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }
}
