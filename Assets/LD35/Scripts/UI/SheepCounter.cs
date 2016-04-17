using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace LD35 {

    public class SheepCounter : MonoSingleton<SheepCounter> {

        [FormerlySerializedAs("LostSheepsTxt")]
        public Text DebugText;


        public void AddEatenSheep(int value)
        {
            EatenSheep += value;
        }

        public void AddLostSheep(int value)
        {
            LostSheep += value;
        }

        public int EatenSheep, LostSheep;

        private void Update()
        {
            DebugText.text = string.Format("Eaten: {0:d2}\nLost: {1:d2}\nStomach: {2:p}\n{3}",
                EatenSheep, LostSheep, GameManager.instance.stomach, GameManager.instance.canShapeshift ? "Can shapeshift" : "");
        }
    }
}
