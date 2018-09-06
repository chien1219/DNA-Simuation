using UnityEngine;
using System.Collections;

/// <summary>
/// When the value in the NarrationCountElement reaches the correct value,
/// the Narration will be played
/// </summary>
namespace Narrate {
    public class CountingNarrationTrigger : NarrationTrigger {
        public NarrationCountElement counter;
        public float targetValue;
        public Relation relation;
        public enum Relation {
            GreaterThan,
            GreaterThanOrEqualTo,
            Equal,
            LessThan,
            LessThanOrEqualTo
        }

        CountEventHandler subscription;
        void Awake() {
            if (counter == null) {
                Debug.Log("CountTrigger attached to " + gameObject.name + " has no CountElement.  Destroying.");
                Destroy(this);
            }
        }

        void OnEnable() {
            subscription = new CountEventHandler(CounterWatcher);
            if (counter != null)
                counter.CountChanged += subscription;
        }
        void OnDisable() {
            if (counter != null && subscription != null)
                counter.CountChanged -= subscription;
            base.Disabled();
        }

        void CounterWatcher(object sender, CountEventArgs e) {
            float c = e.CurrentCount;
            switch (relation) {
                case Relation.Equal:
                    if (c == targetValue)
                        Trigger();
                    break;
                case Relation.GreaterThan:
                    if (c > targetValue)
                        Trigger();
                    break;
                case Relation.GreaterThanOrEqualTo:
                    if (c >= targetValue)
                        Trigger();
                    break;
                case Relation.LessThan:
                    if (c < targetValue)
                        Trigger();
                    break;
                case Relation.LessThanOrEqualTo:
                    if (c <= targetValue)
                        Trigger();
                    break;
            }
        }

    }
}
