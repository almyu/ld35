using UnityEngine;

namespace LD35 {

    public class BulletTime : MonoSingleton<BulletTime> {

        public float scale = 0.1f;

        public static bool active {
            get { return _active; }
            set {
                _active = value;
                if (value) TimescaleStack.Push(instance.scale);
                else TimescaleStack.Pop();
            }
        }
        private static bool _active;
    }
}
