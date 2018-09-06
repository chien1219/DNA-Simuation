using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Narrate {
    [CustomEditor(typeof(OnEnableNarrationTrigger))]
    public class OnEnableTriggerInspector : NarrationTriggerInspector {
        public override void OnInspectorGUI() {

            base.OnInspectorGUI();
        }
    }
}
