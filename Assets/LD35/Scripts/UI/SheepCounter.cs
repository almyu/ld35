using UnityEngine;

namespace LD35 {

    public class SheepCounter : MonoSingleton<SheepCounter> {

        public void AddEatenSheep(int value)
        {
            EatenSheep += value;
            UpdateStats();
        }

        public void AddLostSheep(int value)
        {
            LostSheep += value;
            UpdateStats();
        }

        public int EatenSheep, LostSheep;

        public void UpdateStats()
        {
            UIManager.SetSheepStats(EatenSheep, LostSheep, Herd.instance.numSheep);
        }
    }
}
