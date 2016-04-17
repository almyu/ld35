using UnityEngine;

namespace LD35 {

    public class SheepCounter : MonoSingleton<SheepCounter> {

        public int eatenSheep, lostSheep;

        public void EatSheep() {
            ++eatenSheep;
            UIManager.EatSheep();
        }

        public void LoseSheep() {
            ++lostSheep;
            UIManager.EatSheep();
        }
    }
}
