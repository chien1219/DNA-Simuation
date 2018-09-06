using UnityEngine;
using System.Collections;
using Narrate;
namespace Narrate {
    public class GeneratorManager : MonoBehaviour {
        public int activeGenerators = 0;
        public int totalGenerators = 3;
        public NarrationTrigger ending;
        public NarrationTrigger toDisable;
        public Transform lobster;
        public Vector3 lobsterFinalPos;

        // Use this for initialization
        void Start() {
            activeGenerators = 0;
            if (activeGenerators >= totalGenerators)
                ThresholdReached();
        }

        public void AddGenerator() {
            activeGenerators++;
            if (activeGenerators >= totalGenerators)
                ThresholdReached();
        }

        //whatever should happen when we have as many generators as necessary
        public void ThresholdReached() {
            ending.enabled = true;
            toDisable.enabled = false;
            lobster.position = lobsterFinalPos;
        }
    }
}
