using UnityEngine;
using System.Collections;
//Will be "collected" and added to count on disable
namespace Narrate {
    public class OnDisableBox : MonoBehaviour {

        void OnTriggerEnter(Collider other) {
            if (other.tag.Equals("Player"))
                this.gameObject.SetActive(false);
        }

        void OnDisable() {
            if (BoxCounter.instance != null)
                BoxCounter.instance.BoxCollected();
        }
    }
}
