using UnityEngine;

namespace LD35 {

    public class Mod {
        public bool active;
        public string name, desc;
    }

    public class ModID {
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
    }


    public class Mods {

        public static int numMods {
            get { return ModID.MaxID; }
        }

        public static readonly Mod[] modList = new Mod[ModID.MaxID];

        static Mods() {
            modList[ModID.EatAll] = new Mod { name = "Eat All" };
            modList[ModID.LoseAll] = new Mod { name = "Lose All" };
            modList[ModID.RedSheep] = new Mod { name = "Red Sheep", desc = "Eat 10 red sheep" };
            modList[ModID.BlackSheep] = new Mod { name = "Black Sheep", desc = "Eat the black sheep last" };
            modList[ModID.YellowSheep] = new Mod { name = "Yellow Sheep", desc = "Eat the yellow sheep" };
            modList[ModID.Wind] = new Mod { name = "Wind", desc = "Eat 20 sheep" };
            modList[ModID.Faster] = new Mod { name = "Faster!", desc = "Eat 20 sheep" };
            modList[ModID.Scarier] = new Mod { name = "Scarier!", desc = "Eat 20 sheep" };
            modList[ModID.Diet] = new Mod { name = "Diet", desc = "Eat 20 sheep" };
            modList[ModID.UndertrainedDog] = new Mod { name = "Undertrained Dog", desc = "Eat 15 sheep" };
        }
    }
}
