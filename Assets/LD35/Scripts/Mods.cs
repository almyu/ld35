using UnityEngine;

namespace LD35 {

    public enum ModStatus {
        Locked = 'L',
        Inactive = 'I',
        Active = 'A',
        Failed = 'F',
        Completed = 'C'
    }

    public enum SheepType {
        Any,
        Regular,
        Red,
        Black,
        Yellow
    }

    public class Mod {
        public bool unlocked, completed, active;
        public string name, desc;
        public string winCounter, failCounter;
        public int winCount, failCount;

        public Mod(string name, string desc, string winCounter, int winCount, string failCounter, int failCount) {
            this.name = name;
            this.desc = desc;
            this.winCounter = winCounter;
            this.winCount = winCount;
            this.failCounter = failCounter;
            this.failCount = failCount;
        }
    }

    public static class ModID {
        public const int
            YellowSheep = 0,
            UndertrainedDog = 1,
            LoseAll = 2,
            Diet = 3,
            EatAll = 4,
            BlackSheep = 5,
            RedSheep = 6,
            Faster = 7,
            Wind = 8,
            Scarier = 9,
            MaxID = 10;

        public static bool IsModActive(this int modID) {
            return modID >= 0 && modID < MaxID && Mods.modList[modID].active;
        }
    }


    public class Mods {

        public static readonly Mod[] modList = new Mod[ModID.MaxID];

        static Mods() {
            modList[ModID.YellowSheep]      = new Mod("Yellow Sheep",       "Eat the yellow sheep",     "YellowEaten", 1,   "YellowLost", 1);
            modList[ModID.UndertrainedDog]  = new Mod("Undertrained Dog",   "Eat 15 sheep",             "Eaten", 15,        "Lost", 16);
            modList[ModID.LoseAll]          = new Mod("Lose All",           "",                         "Lost", 30,         "Eaten", 1);
            modList[ModID.Diet]             = new Mod("Diet",               "Eat 20 sheep",             "Eaten", 20,        "Lost", 11);
            modList[ModID.EatAll]           = new Mod("Eat All",            "",                         "Eaten", 30,        "Lost", 1);
            modList[ModID.BlackSheep]       = new Mod("Black Sheep",        "Eat the black sheep last", "Gone", 30,         "BlackGone", 1);
            modList[ModID.RedSheep]         = new Mod("Red Sheep",          "Eat 10 red sheep",         "RedEaten", 10,     "RedFailed", 1);
            modList[ModID.Faster]           = new Mod("Faster",             "Eat 20 sheep",             "Eaten", 20,        "Lost", 11);
            modList[ModID.Wind]             = new Mod("Wind",               "Eat 20 sheep",             "Eaten", 20,        "Lost", 11);
            modList[ModID.Scarier]          = new Mod("Scarier",            "Eat 20 sheep",             "Eaten", 20,        "Lost", 11);

            try { Load(); }
            catch {}
        }

        public static void Save() {
            int unlocked = 0, completed = 0, activated = 0;

            for (int i = 0; i < ModID.MaxID; ++i) {
                var mod = modList[i];
                var bit = 1 << i;

                if (mod.unlocked) unlocked |= bit;
                if (mod.completed) completed |= bit;
                if (mod.active) activated |= bit;
            }
            PlayerPrefs.SetInt("Unlocked", unlocked);
            PlayerPrefs.SetInt("Completed", completed);
            PlayerPrefs.SetInt("Activated", activated);
        }

        public static void Load() {
            var unlocked = PlayerPrefs.GetInt("Unlocked", 7);
            var completed = PlayerPrefs.GetInt("Completed", 0);
            var activated = PlayerPrefs.GetInt("Activated", 0);

            for (int i = 0; i < ModID.MaxID; ++i) {
                var mod = modList[i];
                var bit = 1 << i;

                mod.unlocked = (unlocked & bit) != 0;
                mod.completed = (completed & bit) != 0;
                mod.active = (activated & bit) != 0;
            }
        }

        public static Mod UnlockNext() {
            foreach (var mod in modList) {
                if (mod.unlocked) continue;

                mod.unlocked = true;
                Save();
                return mod;
            }
            return null;
        }


        public class Wind {
            public static float speed = 0.1f;
            public static bool left;

            public static Vector3 GetWindVelocity() {
                return new Vector3(left ? -speed : speed, 0f, 0f);
            }

            public static void Init() {
                left = Random.value < 0.5f;
            }
        }

        public class Diet {
            public static float dietValue = 0.5f;
        }

        public class Scarier {
            public static float factor = 2f;
        }

        public class Faster {
            public static float factor = 2f;
        }
    }
}
