using UnityEngine;
using System.Collections;
using Narrate;
namespace Narrate {
    public class TheEnd : MonoBehaviour {
        public Elevator finalLift;
        public NarrationTrigger finalNarration;
        public GameObject finalLiftBlockers;
        public GameObject lobster;

        void Update() {
            if (finalNarration == null) {
                lobster.transform.SetParent(finalLift.gameObject.transform);
                finalLift.enabled = true;
                finalLiftBlockers.SetActive(true);
                this.enabled = false;
            }
        }
    }
}
