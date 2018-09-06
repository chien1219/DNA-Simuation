using UnityEngine;
using System.Collections;
namespace Narrate {
    public class Generator : MonoBehaviour {
        public GameObject onLight;
        public GameObject offLight;
        private InteractiveNarrationTrigger trig;
        public GeneratorManager manager;
        private bool on;
        // Use this for initialization
        void Start() {
            trig = this.GetComponent<InteractiveNarrationTrigger>();
            on = false;
            SetLights();
        }

        void Update() {
            //if the associated narrationTrigger has been fired, activate the generator.
            if (trig == null || !trig.enabled) {
                on = true;
                SetLights();
                if (manager != null)
                    manager.AddGenerator();
                this.enabled = false;
            }
        }

        void SetLights() {
            if (on) {
                onLight.SetActive(true);
                offLight.SetActive(false);
            } else {
                onLight.SetActive(false);
                offLight.SetActive(true);
            }
        }
    }
}
