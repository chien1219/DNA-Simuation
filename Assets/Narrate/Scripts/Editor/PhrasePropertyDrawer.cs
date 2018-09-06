using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Narrate {
    [CustomPropertyDrawer(typeof(Phrase))]
    public class PhrasePropertyDrawer : PropertyDrawer {
        private float rowHeight = 18;
        private float buttonWidth = 20;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            rowHeight = EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty prop;
            Rect pos = position;
            pos.height = rowHeight;
            pos.y += rowHeight;
            if (!label.text.Contains("noAudio")) {
                prop = property.FindPropertyRelative("audio");
                property.FindPropertyRelative("drawAudio").boolValue = true;
                EditorGUI.PropertyField(pos, prop, new GUIContent("Audio"));
                pos.y += rowHeight;
                if (prop != null) {
                    AudioClip c = ((AudioClip)prop.objectReferenceValue);
                    if (c != null) {
                        EditorGUI.indentLevel++;
                        EditorGUI.LabelField(pos, new GUIContent("Clip length: " + (Mathf.Round(c.length * 100.0f) * 0.01f) + " seconds"));
                        EditorGUI.indentLevel--;
                        pos.y += rowHeight;
                    }
                }
            } else
                property.FindPropertyRelative("drawAudio").boolValue = false;

            prop = property.FindPropertyRelative("text");
            EditorGUI.PropertyField(pos, prop, new GUIContent("Text"));
            pos.y += rowHeight;

            if (!label.text.Contains("smartSubs")) {
                prop = property.FindPropertyRelative("duration");
                EditorGUI.PropertyField(pos, prop, new GUIContent("Duration"));
                EditorGUI.EndProperty();
                pos.y += rowHeight;
            }
            //buttons
            float x = pos.x + pos.width - buttonWidth;
            prop = property.FindPropertyRelative("move");
            prop.enumValueIndex = (int)Phrase.Movement.None;
            if (GUI.Button(new Rect(x,pos.y,buttonWidth,pos.height), new GUIContent("X"))) {
                prop.enumValueIndex = (int)Phrase.Movement.Delete;
            }
            x = x - buttonWidth - 1;
            if (GUI.Button(new Rect(x, pos.y, buttonWidth, pos.height), new GUIContent("↓"))) {
                prop.enumValueIndex = (int)Phrase.Movement.Down;
            }
            x = x - buttonWidth - 1;
            if (GUI.Button(new Rect(x, pos.y, buttonWidth, pos.height), new GUIContent("↑"))) {
                prop.enumValueIndex = (int)Phrase.Movement.Up;
            }
            
            
            pos.y += rowHeight;
            property.FindPropertyRelative("lastCalculatedHeight").floatValue = pos.y - position.y;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return property.FindPropertyRelative("lastCalculatedHeight").floatValue;
        }
    }
}
