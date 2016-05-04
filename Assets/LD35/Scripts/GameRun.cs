using JamSuite.Logic;
using UnityEngine;

namespace LD35 {

    public class ModTrial {
        public Mod mod;
        public ModStatus status;
        public int progress;

        public string name { get { return mod != null ? mod.name : "<missing>"; } }
    }

    public class GameRun : MonoSingleton<GameRun> {

        public ModTrial[] trials = System.Array.ConvertAll(Mods.modList, mod => new ModTrial {
            mod = mod,
            status = mod.active ? ModStatus.Active : ModStatus.Inactive
        });

        private void Start() {
            Counters.ResetAll();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                var str = new System.Text.StringBuilder();

                foreach (var pair in Counters.All)
                    str.AppendFormat("{0}:{1} ", pair.Key, pair.Value.Value);

                Debug.Log(str);
            }
        }

        public static void OnEaten(SheepType type) {
            if (!instance) return;

            if (type != SheepType.Red) Counters.Increase("RedFailed");

            Counters.Increase(type + "Eaten");
            Counters.Increase(type + "Gone");
            Counters.Increase("Eaten");
            Counters.Increase("Gone");
            instance.Check();
        }

        public static void OnLost(SheepType type) {
            if (!instance) return;

            if (type == SheepType.Red) Counters.Increase("RedFailed");

            Counters.Increase(type + "Lost");
            Counters.Increase(type + "Gone");
            Counters.Increase("Lost");
            Counters.Increase("Gone");
            instance.Check();
        }

        public void Check() {
            foreach (var trial in trials) {
                if (trial.status != ModStatus.Active) continue;

                trial.progress = Counters.Get(trial.mod.winCounter);

                if (trial.progress >= trial.mod.winCount) Complete(trial);
                else if (Counters.Get(trial.mod.failCounter) >= trial.mod.failCount) Fail(trial);
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
