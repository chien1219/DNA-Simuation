using UnityEngine;
using System.Collections;
/// <summary>
/// Holds a NarrationTrigger to be enabled, as well as a condition as to when it should be enabled,
/// relative to the NarrationTrigger that comes in the chain before it.
/// </summary>
namespace Narrate {
    [System.Serializable]
    public class ChainLink {
        public enum UnlockOn {
            Destroy,
            Disable
        }
        public UnlockOn unlockOn = UnlockOn.Disable;
        public NarrationTrigger toUnlock;
    }
}
