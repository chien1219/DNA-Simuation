using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Narrate {
    [CustomEditor(typeof(CollisionNarrationTrigger))]
    public class CollisionNarrationTriggerInspector : NarrationTriggerInspector {
        public override void OnInspectorGUI() {
            SerializedObject cnO = new SerializedObject(target);
            base.OnInspectorGUI();
            SerializedProperty prop = cnO.FindProperty("ByNameOrTag");
            EditorGUILayout.PropertyField(prop, new GUIContent("By Name Or Tag:", "Should it check object's name or its tag?"), true);
            prop = cnO.FindProperty("TriggeredBy");
            EditorGUILayout.PropertyField(prop, new GUIContent("Triggered By", "List of strings that either are object tags or names that this can be triggered by"), true);
            cnO.ApplyModifiedProperties();
        }
    }
}
