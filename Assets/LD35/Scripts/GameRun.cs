using UnityEngine;

namespace LD35 {

    public class ModTrial {
        public Mod mod;
        public ModStatus status;
        public int current;

        public int max { get { return mod.num; } }
        public SheepEvent evt { get { return mod.evt; } }
        public SheepType type { get { return mod.type; } }
    }

    public class GameRun : MonoSingleton<GameRun> {

        public ModTrial[] trials;
        public int total, eaten, lost;

        private void Start() {
            total = Herd.instance.numSheep;
            trials = new ModTrial[ModID.MaxID];

            for (int i = 0; i < ModID.MaxID; ++i) {
                var mod = Mods.modList[i];
                var trial = trials[i] = new ModTrial();

                trial.mod = mod;
                trial.status = mod.active ? ModStatus.Active : ModStatus.Inactive;
            }
        }

        public static void OnEaten(SheepType type) {
            if (instance) instance.Poke(SheepEvent.Eaten, type);
        }

        public static void OnLost(SheepType type) {
            if (instance) instance.Poke(SheepEvent.Lost, type);
        }

        public void Poke(SheepEvent evt, SheepType type) {
            if (evt == SheepEvent.Lost) ++lost;
            else ++eaten;
            --total;

            /*Debug.LogFormat("{0} {1}: {2} eaten, {3} lost, {4} left",
                evt, type, eaten, lost, total);*/

            foreach (var trial in trials) {
                if (trial.status != ModStatus.Active) continue;

                if (trial.type == SheepType.Black && type == SheepType.Black && total > 0) {
                    Fail(trial);
                    continue;
                }
                if (trial.type == SheepType.Red && (trial.evt != evt || trial.type != type)) {
                    Fail(trial);
                    continue;
                }
                if (trial.evt == evt && trial.type == SheepType.Any || trial.type == type)
                    ++trial.current;

                if (trial.current >= trial.max) {
                    Complete(trial);
                    continue;
                }
                if (total < trial.max - trial.current) {
                    Fail(trial);
                    continue;
                }
            }
        }

        public void Complete(ModTrial trial) {
            trial.status = ModStatus.Completed;

            Notify("Completed: {0}", trial.mod.name);

            var next = Mods.UnlockNext();
            if (next != null) Notify("Unlocked: {1}", next.name);
        }

        public void Fail(ModTrial trial) {
            trial.status = ModStatus.Failed;
            Notify("Failed: {0}", trial.mod.name);
        }

        public void Notify(string format, params object[] args) {
            Debug.LogFormat(format, args);
        }
    }
}
