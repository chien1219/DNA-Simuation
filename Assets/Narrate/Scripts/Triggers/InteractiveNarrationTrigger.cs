using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// When the "triggeredBy" gets in range and the button referenced in NarrationManager has been pressed,
/// the Trigger will be activated.
/// </summary>
namespace Narrate {
    public class InteractiveNarrationTrigger : NarrationTrigger {
        public bool is2D = false;
        public Transform triggeredBy;//the Transforms of GameObjects that can trigger this
        public float proximity; //Triggers when objects are within this distance
        private bool timeOut;

        private InteractionEventHandler handler;
        void Awake() {
            if (triggeredBy == null) {
                Debug.LogWarning("InteractiveNarrationTrigger Warning: " + this.gameObject.name +
                     " has no TriggeredBy object and will never fire unless one is assigned. Disabling script");
                this.enabled = false;
            }
        }

        void Start() {
            handler = new InteractionEventHandler(CheckTrigger);
            NarrationManager.instance.InteractPressed += handler;
        }

        void OnEnable() {
            if (NarrationManager.instance != null && handler !=null)
            NarrationManager.instance.InteractPressed += handler;
        }

        void CheckTrigger(object sender, System.EventArgs e) {
            if (triggeredBy == null) return; //can't be triggered, so don't even try
            Vector3 distance = triggeredBy.position - this.transform.position;
            if (is2D)
                distance.z = 0;
            if ((distance.magnitude <= proximity)) {
                Trigger();
            }
        }

        void OnDisable() {
            NarrationManager.instance.InteractPressed -= handler;
            base.Disabled();
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
