using UnityEngine;
using UnityEngine.UI;

namespace LD35 {

    public class SheepCounter : MonoSingleton<SheepCounter> {
        public Text LostSheepsTxt;
        public Text EatenSheepsTxt;


        public void AddEatenSheep(int value)
        {
            EatenSheeps += value;
        }

        public void AddLostSheep(int value)
        {
            LostSheeps += value;
        }

        private int _eatenSheeps;
        private int EatenSheeps
        {
            get { return _eatenSheeps; }
            set
            {
                _eatenSheeps = value;
                RefreshEatenText();
            }
        }

        private int _lostSheeps;
        private int LostSheeps
        {
            get { return _lostSheeps; }
            set
            {
                _lostSheeps = value;
                RefreshLostText();
            }
        }

        private void RefreshEatenText()
        {
            EatenSheepsTxt.text = "Eaten: " + EatenSheeps.ToString("00");
        }

        private void RefreshLostText()
        {
            LostSheepsTxt.text = "Lost: " + LostSheeps.ToString("00");
        }
    }
}
