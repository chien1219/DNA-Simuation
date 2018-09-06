using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/////////////////////////////////80-chars/////////////////////////////////////
/// <summary>
/// A CollisionNarrationTrigger is a NarrationTrigger that plays a Narration 
/// when one of the object(s) capable of triggering it collides with the any
/// physics collider(s) attached to this same gameobject or its children.
/// </summary>
//////////////////////////////////////////////////////////////////////////////
namespace Narrate {
    public class CollisionNarrationTrigger : NarrationTrigger {
        public bool onCollision = true;
        public bool onTrigger = true;
        public Style ByNameOrTag = Style.ByTag;
        /// <summary>
        /// The list of tags that can trigger this narration.  If empty, any object can trigger it.
        /// </summary>
        public List<string> TriggeredBy;

        public enum Style {
            ByTag,
            ByName
        }

        /// <summary>
        /// Logs a warning if there are no colliders, 2D or 3D, attached to this
        /// object. No colliders means this trigger can't fire
        /// </summary>
        void Awake() {
            if (this.GetComponent<Collider>() == null && this.GetComponent<Collider2D>() == null)
                Debug.LogWarning("CollisionNarrationTrigger Warning: " + this.gameObject.name +
                    " has no Collider or Collider2D.  Collider required to trigger.");
        }

        /// <summary>
        /// 2D triggering mechanism - for objects that have Collider2D components.
        /// Triggers when an object with a tag in TriggeredBy and a Collider2D
        /// touches this object's Collider2D.
        /// </summary>
        void OnTriggerEnter2D(Collider2D other) {
            if (!onTrigger)
                return;

            string s = other.gameObject.tag;
            if (ByNameOrTag == Style.ByName)
                s = other.gameObject.name;

            if (TriggeredBy == null || TriggeredBy.Count <= 0 || TriggeredBy.Contains(s))
                this.Trigger();
        }

        /// <summary>
        /// 3D triggering mechanism - for objects that have Collider components.
        /// Triggers when an object with a tag in TriggeredBy and a Collider
        /// touches this object's Collider.
        /// </summary>
        void OnTriggerEnter(Collider other) {
            if (!onTrigger)
                return;

            string s = other.gameObject.tag;
            if (ByNameOrTag == Style.ByName)
                s = other.gameObject.name;

            if (TriggeredBy == null || TriggeredBy.Count <= 0 || TriggeredBy.Contains(s))
                this.Trigger();
        }

        void OnCollisionEnter2D(Collision2D other) {
            if (!onCollision)
                return;

            string s = other.gameObject.tag;
            if (ByNameOrTag == Style.ByName)
                s = other.gameObject.name;

            if (TriggeredBy == null || TriggeredBy.Count <= 0 || TriggeredBy.Contains(s))
                this.Trigger();
        }

        void OnCollisionEnter(Collision other) {
            if (!onCollision)
                return;

            string s = other.gameObject.tag;
            if (ByNameOrTag == Style.ByName)
                s = other.gameObject.name;

            if (TriggeredBy == null || TriggeredBy.Count <= 0 || TriggeredBy.Contains(s))
                this.Trigger();
        }
    }
}
