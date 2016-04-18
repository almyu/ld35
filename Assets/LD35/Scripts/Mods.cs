using UnityEngine;

namespace LD35 {

    public enum ModStatus {
        Locked = 'L',
        Inactive = 'I',
        Active = 'A',
        Failed = 'F',
        Completed = 'C'
    }

    public enum SheepEvent {
        Eaten,
        Lost
    }

    public enum SheepType {
        Any,
        Regular,
        Red,
        Black,
        Yellow
    }

    public class Mod {
        public bool unlocked, active;
        public string name, desc;
        public SheepEvent evt;
        public SheepType type;
        public int num;
    }

    public static class ModID {
        public const int
            None = -1,
            EatAll = 0,
            LoseAll = 1,
            RedSheep = 2,
            BlackSheep = 3,
            YellowSheep = 4,
            Wind = 5,
            Faster = 6,
            Scarier = 7,
            Diet = 8,
            UndertrainedDog = 9,
            MaxID = 10;

        public static bool IsModActive(this int modID) {
            return modID >= 0 && modID < MaxID && Mods.modList[modID].active;
        }
    }


    public class Mods {

        public static int numMods {
            get { return ModID.MaxID; }
        }

        public static readonly Mod[] modList = new Mod[ModID.MaxID];

        static Mods() {
            modList[ModID.EatAll] = new Mod { unlocked = true, name = "Eat All", num = 30 };
            modList[ModID.LoseAll] = new Mod { unlocked = true, name = "Lose All", evt = SheepEvent.Lost, num = 30 };
            modList[ModID.RedSheep] = new Mod { unlocked = true, name = "Red Sheep", desc = "Eat 10 red sheep", type = SheepType.Red, num = 10 };
            modList[ModID.BlackSheep] = new Mod { name = "Black Sheep", desc = "Eat the black sheep last", type = SheepType.Black, num = 1 };
            modList[ModID.YellowSheep] = new Mod { name = "Yellow Sheep", desc = "Eat the yellow sheep", type = SheepType.Yellow, num = 1 };
            modList[ModID.Wind] = new Mod { name = "Wind", desc = "Eat 20 sheep", num = 20 };
            modList[ModID.Faster] = new Mod { name = "Faster!", desc = "Eat 20 sheep", num = 20 };
            modList[ModID.Scarier] = new Mod { name = "Scarier!", desc = "Eat 20 sheep", num = 20 };
            modList[ModID.Diet] = new Mod { name = "Diet", desc = "Eat 20 sheep", num = 20 };
            modList[ModID.UndertrainedDog] = new Mod { name = "Undertrained Dog", desc = "Eat 15 sheep", num = 15 };

            Load();
        }

        public static void Save() {
            var bits = new string(System.Array.ConvertAll(modList, mod =>
                mod.unlocked ? mod.active ? 'a' : 'u' : '-'));

            PlayerPrefs.SetString("Mods", bits);
        }

        public static void Load() {
            var bits = PlayerPrefs.GetString("Mods", "");

            for (int i = 0, n = Mathf.Min(bits.Length, ModID.MaxID); i < n; ++i) {
                modList[i].active = bits[i] == 'a';
                modList[i].unlocked = bits[i] == 'u' || modList[i].active;
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


        public class Wind : Mod {

            public static float speed = 0.1f;
            public static bool left;

            public static Vector3 GetWindVelocity() {
                return new Vector3(left ? -speed : speed, 0f, 0f);
            }

            public static void Init() {
                left = Random.value < 0.5f;
            }
        }

        public class Diet : Mod {

            public static float dietValue = 0.5f;
        }

        public class Scarier : Mod {
            public static float factor = 2f;
        }

        public class Faster : Mod {
            public static float factor = 2f;
        }
    }
}
