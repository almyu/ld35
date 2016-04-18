using UnityEngine;

namespace LD35 {

    public class Mod {
        public bool active;
        public string name, desc;
    }


    public class Mods {

        public static int numMods {
            get { return modList.Length; }
        }

        public static readonly Mod[] modList = new[] {
            new Mod { name = "Test 0", desc = "Do stuff" },
            new Mod { name = "Test 1", desc = "Do moar stuff" },
            new Mod { name = "Test 2", desc = "Do some crazy shit" }
        };
    }
}
