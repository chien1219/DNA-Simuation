using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Narrate {
    [CustomEditor(typeof(TimerNarrationTrigger))]
    public class TimerTriggerInspector : NarrationTriggerInspector {

        public override void OnInspectorGUI() {
            TimerNarrationTrigger t = (TimerNarrationTrigger)target;
            t.time = EditorGUILayout.FloatField(new GUIContent("Timer:", "How long before the trigger goes off."), t.time);
            if (t.time < 0) t.time = 0;

            base.OnInspectorGUI();
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
