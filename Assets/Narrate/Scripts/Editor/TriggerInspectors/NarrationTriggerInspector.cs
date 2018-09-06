using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Narrate {
    [CustomEditor(typeof(NarrationTrigger))]
    public class NarrationTriggerInspector : Editor {
        bool reactFoldout = false;
        public override void OnInspectorGUI() {
            NarrationTrigger nt = (NarrationTrigger)target;
            SerializedObject ntO = new SerializedObject(target);

            nt.UseNarrationList = EditorGUILayout.Toggle(new GUIContent("Use a NarrationList", "Plays a narration from a NarrationList instead of the built in Narration"),
                                                          nt.UseNarrationList);
            if (nt.UseNarrationList) {
                nt.narrationList = (NarrationList)EditorGUILayout.ObjectField(new GUIContent("NarrationList:", "The NarrationList that will be played from"),
                                                                               nt.narrationList, typeof(NarrationList), true);
            } else {
                SerializedProperty prop = ntO.FindProperty("theNarration");
                EditorGUILayout.PropertyField(prop, new GUIContent("Narration", "The Narration that will play"), true);
            }

            reactFoldout = EditorGUILayout.Foldout(reactFoldout, new GUIContent("Reactions"));
            if (reactFoldout) {
                nt.OnSuccess = (NarrationTrigger.Reaction)EditorGUILayout.EnumPopup(new GUIContent("On Play Success:", "Event that will happen if the Narration is successfully played by the NarrationManager")
                                                                                     , nt.OnSuccess);
                nt.OnFailure = (NarrationTrigger.Reaction)EditorGUILayout.EnumPopup(new GUIContent("On Play Failure:", "Event that will happen if the NarrationManager fails an attempt to play this Narration"),
                                                                                     nt.OnFailure);
                if (nt.OnSuccess == NarrationTrigger.Reaction.Reset || nt.OnFailure == NarrationTrigger.Reaction.Reset) {
                    nt.ResetCooldown = EditorGUILayout.FloatField(new GUIContent("Reset Wait:", "How long to wait before resetting the trigger"), nt.ResetCooldown);
                    if (nt.ResetCooldown < 0)
                        nt.ResetCooldown = 0;
                }
            }
           ntO.ApplyModifiedProperties();
            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
