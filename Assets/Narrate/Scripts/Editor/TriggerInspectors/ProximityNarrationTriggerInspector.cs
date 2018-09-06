using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Narrate {
    [CustomEditor(typeof(ProximityNarrationTrigger))]
    public class ProximityNarrationTriggerInspector : NarrationTriggerInspector {
        private bool foldout = false;
        public override void OnInspectorGUI() {
            ProximityNarrationTrigger pn = (ProximityNarrationTrigger)target;
            base.OnInspectorGUI();
            foldout = EditorGUILayout.Foldout(foldout, "ProximitySettings");
            if (foldout) {
                pn.triggeredBy = (Transform)EditorGUILayout.ObjectField(new GUIContent("Target:", "The object that triggers this Narration when it comes within proximity"),
                                                                     pn.triggeredBy, typeof(Transform), true);
                pn.is2D = EditorGUILayout.Toggle(new GUIContent("Is 2D:", "For 2D games: if checked, only the x and y axes are used when calculating proximity"), pn.is2D);

                pn.proximity = EditorGUILayout.FloatField(new GUIContent("Proximity:", "Trigger when Target is within this distance"),
                                                          pn.proximity);
                pn.timeInProximityToTrigger = EditorGUILayout.FloatField(new GUIContent("Time Inside to Trigger:", "How long the target must be in proximity range " +
                    "before the Narration will fire (set to zero to fire immediately)"),
                                                                         pn.timeInProximityToTrigger);
                if (pn.timeInProximityToTrigger > 0) {
                    pn.timeMustBeConsecutive = EditorGUILayout.Toggle(new GUIContent("Consecutive:", "If checked, the timer starts from zero each time the target comes in range." +
                        "If unchecked, every bit of time spent inside is counted and may eventually add up to trigger the narration"),
                                                                      pn.timeMustBeConsecutive);
                }
            }
            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
