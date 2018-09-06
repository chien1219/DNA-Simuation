using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Narrate {
    [CustomEditor(typeof(InteractiveNarrationTrigger))]
    public class InteractiveTriggerInspector : NarrationTriggerInspector {
        bool proxExpanded = false;
        public override void OnInspectorGUI() {
            proxExpanded = EditorGUILayout.Foldout(proxExpanded, new GUIContent("Proximity Settings"));
            InteractiveNarrationTrigger trig = (InteractiveNarrationTrigger)target;
            if (proxExpanded) {
                EditorGUI.indentLevel++;
                trig.is2D = EditorGUILayout.Toggle(new GUIContent("Is 2D:", "Z axis will be ignored. Recommended for 2D scenes"), trig.is2D);
                trig.triggeredBy = (Transform)EditorGUILayout.ObjectField(new GUIContent("Player: ", "The Player object's transform to detect when it's close enough to interact"),
                    trig.triggeredBy, typeof(Transform), true);
                trig.proximity = EditorGUILayout.FloatField(new GUIContent("Proximity: ", "How close the player must be to interact"), trig.proximity);
                if (trig.proximity < 0) trig.proximity = 0;
                EditorGUI.indentLevel--;
            }
            base.OnInspectorGUI();
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
