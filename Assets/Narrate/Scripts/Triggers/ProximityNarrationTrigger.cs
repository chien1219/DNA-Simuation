using UnityEngine;
using System.Collections;
using System.Collections.Generic;

# if UNITY_EDITOR
using UnityEditor;
#endif

/////////////////////////////////80-chars/////////////////////////////////////
/// <summary>
/// A WithinRadiusNarration Trigger is a NarrationTrigger that plays a Narration 
/// when one of the object(s) capable of triggering it is within <radius> distance
/// of it.
/// </summary>
//////////////////////////////////////////////////////////////////////////////
namespace Narrate {
    public class ProximityNarrationTrigger : NarrationTrigger {
        /// <summary>
        /// Ignore z-coordinate when computing proximty. Meant for 2D games
        /// </summary>
        public bool is2D = false;
        public Transform triggeredBy;//the Transforms of GameObjects that can trigger this
        public float proximity; //Triggers when objects are within this distance
        public float timeInProximityToTrigger = 0;//ADDED
        public bool timeMustBeConsecutive = false;
        private float delayTimer;//tracks how much time has been spent inside the area ADDED
                                 /// <summary>
                                 /// Logs a warning if there are no triggered-by objects and disables this
                                 /// </summary>
        void Awake() {
            if (triggeredBy == null) {
                Debug.LogWarning("ProximityNarrationTrigger Warning: " + this.gameObject.name +
                    " has no TriggeredBy object and will never fire unless one is assigned. Disabling script.");
                this.enabled = false;
            }
        }
        void OnEnable() {
            StartCoroutine(DistanceWatch());
        }

        IEnumerator DistanceWatch() {
            float timeToTrig = timeInProximityToTrigger;
            if (timeToTrig <= 0)
                timeToTrig = 0.01f;

            delayTimer = 0;
            while (delayTimer < timeToTrig) {
                if (triggeredBy == null) yield break; //can't be triggered, so don't even try
                Vector3 distance = triggeredBy.position - this.transform.position;
                if (is2D)
                    distance.z = 0;
                if (distance.magnitude <= proximity)
                    delayTimer += 0.33f;
                else if (timeMustBeConsecutive)
                    delayTimer = 0;
                yield return new WaitForSeconds(.33f);
            }
            Trigger();
        }

        /// <summary>
        ///Reset the timer to prepare for a new countdown. This is called by the 
        /// parent class at the end of a reset.
        /// </summary>
        public override void Reset() {
            StartCoroutine(DistanceWatch());
        }

        public void OnDisable() {
            StopAllCoroutines();
            Disabled();
        }

#if UNITY_EDITOR
        void OnDrawGizmos() {
            Color c = Color.cyan;
            c.a = .3f;
            if (is2D) {
                Handles.color = c;
                Handles.DrawSolidDisc(transform.position, new Vector3(0, 0, 1), proximity);
                //draw some lines so if used in 3d space it makes more sense
                Vector3 p1 = transform.position;
                Handles.DrawLine(new Vector3(p1.x, p1.y, p1.z + 100), new Vector3(p1.x, p1.y, p1.z - 100));
                Handles.DrawLine(new Vector3(p1.x + proximity, p1.y, p1.z + 100), new Vector3(p1.x + proximity, p1.y, p1.z - 100));
                Handles.DrawLine(new Vector3(p1.x - proximity, p1.y, p1.z + 100), new Vector3(p1.x - proximity, p1.y, p1.z - 100));
                Handles.DrawLine(new Vector3(p1.x, p1.y + proximity, p1.z + 100), new Vector3(p1.x, p1.y + proximity, p1.z - 100));
                Handles.DrawLine(new Vector3(p1.x, p1.y - proximity, p1.z + 100), new Vector3(p1.x, p1.y - proximity, p1.z - 100));
            } else {
                Gizmos.color = c;
                Gizmos.DrawSphere(this.transform.position, proximity);
            }
        }
#endif
    }
}
