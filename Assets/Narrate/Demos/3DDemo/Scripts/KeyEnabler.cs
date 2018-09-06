using UnityEngine;
using System.Collections;
namespace Narrate{
    public class KeyEnabler : MonoBehaviour {
        public NarrationTrigger waitingOn;
        public GameObject[] keysToEnable;
        
        void Update() {
            if(!waitingOn.enabled)
                for(int i  = 0; i < keysToEnable.Length; i++) {
                    keysToEnable[i].SetActive(true);
                }
        }
    }
}