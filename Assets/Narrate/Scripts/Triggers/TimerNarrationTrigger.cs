using UnityEngine;
using System.Collections;

/// <summary>
///Begins a countdown when enabled.  When countdown ends, will play the Narration
/// </summary>
namespace Narrate {
    public class TimerNarrationTrigger : NarrationTrigger {
        public float time;
        private IEnumerator ienum;

        // Use this for initialization
        void OnEnable() {
            if (ienum != null)
                StopCoroutine(ienum);
            ienum = CountDown(time);
            StartCoroutine(ienum);
        }

        IEnumerator CountDown(float time) {
            yield return new WaitForSeconds(time);
            Trigger();
        }

        public override void Reset() {
            if (time <= 0) {
                Trigger();
            } else {
                ienum = CountDown(time);
                StartCoroutine(ienum);
            }
        }

        public void ResetClock() {
            if (ienum != null)
                StopCoroutine(ienum);
            ienum = CountDown(time);
            StartCoroutine(ienum);
        }

        void OnDisable() {
            if (ienum != null)
                StopCoroutine(ienum);
            Disabled();
        }
    }
}
