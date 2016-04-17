using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace LD35 {

    public class SheepCounter : MonoSingleton<SheepCounter> {

        [FormerlySerializedAs("LostSheepsTxt")] public Text LostSheepTxt;
        [FormerlySerializedAs("EatenSheepsTxt")] public Text EatenSheepTxt;


        public void AddEatenSheep(int value)
        {
            EatenSheep += value;
        }

        public void AddLostSheep(int value)
        {
            LostSheep += value;
        }

        private int _eatenSheep;
        public int EatenSheep
        {
            get { return _eatenSheep; }
            set
            {
                _eatenSheep = value;
                RefreshEatenText();
            }
        }

        private int _lostSheep;
        public int LostSheep
        {
            get { return _lostSheep; }
            set
            {
                _lostSheep = value;
                RefreshLostText();
            }
        }

        private void RefreshEatenText()
        {
            EatenSheepTxt.text = "Eaten: " + EatenSheep.ToString("00");
        }

        private void RefreshLostText()
        {
            LostSheepTxt.text = "Lost: " + LostSheep.ToString("00");
        }
    }
}
