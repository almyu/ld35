using UnityEngine;
using UnityEngine.UI;

namespace LD35 {

    public class VolumeControl : MonoBehaviour {

        public Slider sfxSlider;
        public float defaultVolume = 0.5f;

        private const string volumeKey = "volume";

        protected void Awake() {
            AudioListener.volume = PlayerPrefs.GetFloat(volumeKey, defaultVolume);

            sfxSlider.value = AudioListener.volume;

            sfxSlider.onValueChanged.AddListener((value) => { AudioListener.volume = value; });
        }

        private void SaveSound() {
            PlayerPrefs.SetFloat(volumeKey, AudioListener.volume);
        }

        protected void OnDestroy() {
            SaveSound();
        }

        protected void OnApplicationQuit() {
            SaveSound();
        }
    }
}
