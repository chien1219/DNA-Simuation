using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Narrate {
    [CustomEditor (typeof(CountingNarrationTrigger))]
    public class CountTriggerInspector : NarrationTriggerInspector {
        bool cExpanded = false;
        public override void OnInspectorGUI() {
            CountingNarrationTrigger ct = (CountingNarrationTrigger)target;
            cExpanded = EditorGUILayout.Foldout(cExpanded, new GUIContent("Count Settings"));
            if (cExpanded) {
                EditorGUI.indentLevel++;
                ct.counter = (NarrationCountElement)EditorGUILayout.ObjectField(new GUIContent("Count Element:","The NarrationCountElement this trigger will monitor"), ct.counter, typeof(NarrationCountElement), true);
                ct.relation = (CountingNarrationTrigger.Relation)EditorGUILayout.EnumPopup(new GUIContent("Relation:","Trigger when Count Element's value is _______ target value"), ct.relation);
                ct.targetValue = EditorGUILayout.FloatField(new GUIContent("Target value:", "The value this trigger is waiting for"), ct.targetValue);
                EditorGUI.indentLevel--;
            }
            base.OnInspectorGUI();
        }
    }
}
