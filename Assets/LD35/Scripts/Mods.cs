using UnityEngine;

namespace LD35 {

    public class Mod {
        public bool unlocked, active;
        public string name, desc;
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
            modList[ModID.EatAll] = new Mod { unlocked = true, name = "Eat All" };
            modList[ModID.LoseAll] = new Mod { unlocked = true, name = "Lose All" };
            modList[ModID.RedSheep] = new Mod { unlocked = true, name = "Red Sheep", desc = "Eat 10 red sheep" };
            modList[ModID.BlackSheep] = new Mod { name = "Black Sheep", desc = "Eat the black sheep last" };
            modList[ModID.YellowSheep] = new Mod { name = "Yellow Sheep", desc = "Eat the yellow sheep" };
            modList[ModID.Wind] = new Mod { name = "Wind", desc = "Eat 20 sheep" };
            modList[ModID.Faster] = new Mod { name = "Faster!", desc = "Eat 20 sheep" };
            modList[ModID.Scarier] = new Mod { name = "Scarier!", desc = "Eat 20 sheep" };
            modList[ModID.Diet] = new Mod { name = "Diet", desc = "Eat 20 sheep" };
            modList[ModID.UndertrainedDog] = new Mod { name = "Undertrained Dog", desc = "Eat 15 sheep" };

            Load();
        }

        public static void Save() {
            var bits = new string(System.Array.ConvertAll(modList, mod =>
                mod.unlocked ? mod.active ? '2' : '1' : '0'));

            PlayerPrefs.SetString("Mods", bits);
        }

        public static void Load() {
            var bits = PlayerPrefs.GetString("Mods", "");

            for (int i = 0, n = Mathf.Min(bits.Length, ModID.MaxID); i < n; ++i) {
                modList[i].unlocked = bits[i] > '0';
                modList[i].active = bits[i] > '1';
            }
        }


        public class Wind : Mod {

            public static Vector3 windSpeed;

            public static void Init() {
                windSpeed = Random.onUnitSphere.WithY(0f).normalized * 0.1f;
            }
        }

        public class Diet : Mod {

            public static float dietValue = 0.5f;
        }
    }
}
