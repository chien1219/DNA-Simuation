using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Narrate {
    /// <summary>
    /// A simple script that unlocks/enables a series of NarrationTriggers. The next one in the chain is enabled when 
    /// the previous is disabled or destroyed
    /// </summary>
    public class NarrationChain : MonoBehaviour {
        public NarrationTrigger head;
        public List<ChainLink> links;
        private DisableDestroyEventHandler subscription;

        void Awake() {
            if (head == null || links == null || links.Count < 1) {
                Debug.LogWarning("NarrationChain attached to " + this.gameObject.name + " has a null trigger reference or empty list. Disabling script");
                this.enabled = false;
            } else {
                for (int i = 0; i < links.Count; i++) {
                    links[i].toUnlock.enabled = false;
                }
            }
        }

        void Start() {
            subscription = new DisableDestroyEventHandler(Unlock);
            Subscribe();
        }


        void Unlock(object sender, System.EventArgs e) {
            links[0].toUnlock.enabled = true;
            if (links[0].unlockOn == ChainLink.UnlockOn.Disable)
                head.DisableEvent -= subscription;
            if (links.Count > 1) {
                head = links[0].toUnlock;
                links.RemoveAt(0);
                Subscribe();
            } else
                Destroy(this);
        }

        void Subscribe() {
            if (links[0].unlockOn == ChainLink.UnlockOn.Destroy)
                head.DestroyEvent += subscription;
            else
                head.DisableEvent += subscription;
        }
    }
}
