using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Narrate {
    public class BoxCounter : MonoBehaviour {
        public static BoxCounter instance;
        public Text display;
        public int totalBoxes;
        public int count;
        public NarrationCountElement countElem;
        public TimerNarrationTrigger boredTrig;
        void Awake() {
            if (instance == null)
                instance = this;
            else if (instance != this)
                this.enabled = false;
        }
        void Start() {
            count = 0;
            display.text = count + " of " + totalBoxes + " found";
        }

        public void BoxCollected() {
            if(boredTrig.enabled)
                boredTrig.ResetClock();
            count++;
            countElem.Increment(1);
            display.text = count + " of " + totalBoxes + " found";
        }
    }
}
