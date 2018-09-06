using UnityEngine;
using System.Collections;
namespace Narrate {
    public class KeyChecker : MonoBehaviour {
        private Transform player;
        public BoxCounter counter;
        public float prox;
        public MonoBehaviour[] enableOnUnlock;
        public NarrationTrigger bored;
        // Use this for initialization
        void Start() {
            player = GameObject.Find("Player").transform;
        }

        // Update is called once per frame
        void Update() {
            if (((this.transform.position - player.transform.position).magnitude <= prox) && (counter.count >= counter.totalBoxes)) {
                for (int i = 0; i < enableOnUnlock.Length; i++) {
                    enableOnUnlock[i].enabled = true;
                }
                bored.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
}
