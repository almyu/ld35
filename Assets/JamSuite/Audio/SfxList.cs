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


        public AudioClip LookupClip(string name) {
            foreach (var binding in clips)
                if (binding.name == name)
                    return binding.clip;

            if (reserveMissing)
                clips.Add(new ClipBinding { name = name });

            return null;
        }
    }
}
