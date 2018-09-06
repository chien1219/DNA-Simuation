using UnityEngine;
using System.Collections;

/// <summary>
/// Trigger will immediately go off when enabled.
/// </summary>
namespace Narrate {
    public class OnEnableNarrationTrigger : NarrationTrigger {
        void OnEnable() {
            Trigger();
        }
    }
}
