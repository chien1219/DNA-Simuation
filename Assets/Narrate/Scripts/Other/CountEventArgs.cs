using UnityEngine;
using System.Collections;

/// <summary>
/// Used in the event issued by NarrationCountElement when it's value changes.
/// </summary>
namespace Narrate {
    public class CountEventArgs : System.EventArgs {
        private float currentCount;
        // Use this for initialization
        public CountEventArgs(float curCount) {
            this.currentCount = curCount;
        }
        public float CurrentCount {
            get {
                return currentCount;
            }
        }
    }
}
