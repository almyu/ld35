using UnityEngine;
using System.Collections.Generic;

namespace JamSuite.Audio {

    [CreateAssetMenu(order = 220)]
    public class SfxList : ScriptableObject {

        [System.Serializable]
        public class ClipBinding {
            public string name;

            [Range(0f, 1f)]
            public float volumeScale = 1f;

            public AudioClip clip;
            public AudioClip[] extraClips;
        }

        public bool reserveMissing = true;
        public List<ClipBinding> clips;


        public AudioClip RollClip(ClipBinding binding) {
            if (binding.extraClips == null || binding.extraClips.Length == 0)
                return binding.clip;

            var index = Random.Range(-1, binding.extraClips.Length);
            return index < 0 ? binding.clip : binding.extraClips[index];
        }

        public AudioClip LookupClip(string name, ref float volumeScale) {
            foreach (var binding in clips) {
                if (binding.name != name) continue;

                volumeScale *= binding.volumeScale;
                return RollClip(binding);
            }

            if (reserveMissing)
                clips.Add(new ClipBinding { name = name });

            return null;
        }
    }
}
