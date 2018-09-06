using UnityEngine;
using System.Collections;

/// <summary>
/// Maintains a count of something.  CountTriggers using this element will be 
/// alerted whenever the count changes.
/// </summary>
namespace Narrate {
    public delegate void CountEventHandler(object sender, CountEventArgs e);
    public class NarrationCountElement : MonoBehaviour {
        public event CountEventHandler CountChanged;
        public float initialCount;
        private float count;

        // Use this for initialization
        void Start() {
            count = initialCount;
        }

        public void Increment(float by) {
            count += by;
            CountChanged(this, new CountEventArgs(count));
        }

        public void Decrement(float by) {
            count -= by;
            CountChanged(this, new CountEventArgs(count));
        }
    }
}
