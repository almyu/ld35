using UnityEngine;

namespace LD35 {

    public class ModTrial {
        public Mod mod;
        public ModStatus status;
        public int current;

        public string name { get { return mod != null ? mod.name : "<missing>"; } }
        public int max { get { return mod != null ? mod.num : 0; } }
        public SheepEvent evt { get { return mod.evt; } }
        public SheepType type { get { return mod.type; } }
    }

    public class GameRun : MonoSingleton<GameRun> {

        public ModTrial[] trials = System.Array.ConvertAll(Mods.modList, mod => new ModTrial {
            mod = mod,
            status = mod.active ? ModStatus.Active : ModStatus.Inactive
        });
        public int total = 30, eaten, lost;

        private void Start() {
            total = Herd.instance.numSheep;
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
                if (trial.type == SheepType.Red && (trial.evt == evt) != (trial.type == type)) {
                    Fail(trial);
                    continue;
                }
                if (trial.type == SheepType.Yellow && trial.evt != evt && trial.type == type) {
                    Fail(trial);
                    continue;
                }
                if (trial.evt == evt && (trial.type == SheepType.Any || trial.type == type))
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

            Notify("Completed: {0}", trial.name);

            if (!trial.mod.completed) {
                trial.mod.completed = true;

                var next = Mods.UnlockNext();
                if (next != null) Notify("Unlocked: {0}", next.name);
            }
            Mods.Save();
        }

        public void Fail(ModTrial trial) {
            trial.status = ModStatus.Failed;
            Notify("Failed: {0}", trial.name);
        }

        public void Notify(string format, params object[] args) {
            UIManager.instance.SpawnMessage(string.Format(format, args));
            Debug.LogFormat(format, args);
        }
    }
}
