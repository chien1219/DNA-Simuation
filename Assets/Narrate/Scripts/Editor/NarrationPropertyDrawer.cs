using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Narrate {
    [CustomPropertyDrawer(typeof(Narration))]
    public class NarrationPropertyDrawer : PropertyDrawer {
        public static GUILayoutOption buttonWidth = GUILayout.Width(30.0f);
        private float rowHeight = 16;
        private bool smartSubsOn = false;
        private bool advFoldout = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            rowHeight = EditorGUIUtility.singleLineHeight + 2;
            label = EditorGUI.BeginProperty(position, label, property);
            property.FindPropertyRelative("expanded").boolValue = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, rowHeight),
                property.FindPropertyRelative("expanded").boolValue, label);
            if (!property.FindPropertyRelative("expanded").boolValue) {
                property.FindPropertyRelative("lastCalculatedHeight").floatValue = 16;
                return;
            }
            EditorGUI.indentLevel++;
            Rect pos = position;
            pos.x = position.x;
            pos.y += rowHeight;

            advFoldout=EditorGUI.Foldout(new Rect(pos.x, pos.y, pos.width, rowHeight), advFoldout, new GUIContent("Advanced"));
            pos.y += rowHeight;

            SerializedProperty m = property.FindPropertyRelative("singleAudio_MultiSub");
            bool singleAud = m.boolValue;
            if (advFoldout) {
                EditorGUI.indentLevel++;
                //busyBehavior    
                m = property.FindPropertyRelative("busyBehavior");
                EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), m, new GUIContent("BusyBehavior:",
                    "How the Narration should behave when a different Narration is already being played."));
                pos.y += rowHeight;

                //Looping Audio
                if (NarrationManager.alwaysLoopAudio) {
                    property.FindPropertyRelative("LoopAudioForDuration").boolValue = true;
                } else if (singleAud) {
                    EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), property.FindPropertyRelative("LoopAudioForDuration"), new GUIContent("Loop Audio Per Subtitle"));
                    pos.y += rowHeight;
                } else {
                    EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), property.FindPropertyRelative("LoopAudioForDuration"), new GUIContent("Loop Audio Per Phrase"));
                    pos.y += rowHeight;
                }

                //Smart Subs
                if (NarrationManager.alwaysSmartSubs) {
                    property.FindPropertyRelative("useSmartSubs").boolValue = true;
                } else {
                    EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), property.FindPropertyRelative("useSmartSubs"), new GUIContent("Use Smart Subs"));
                    pos.y += rowHeight;
                }
                smartSubsOn = property.FindPropertyRelative("useSmartSubs").boolValue;
                EditorGUI.indentLevel--;
            }
            //single audio clip
            m = property.FindPropertyRelative("singleAudio_MultiSub");
            if (NarrationManager.alwaysSingleAudio) {
                m.boolValue = true;
            } else {
                EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), m, new GUIContent("Single Audio Clip"));
                pos.y += rowHeight;
            }
            if (singleAud) {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), property.FindPropertyRelative("mainAudio"), new GUIContent("Audio Clip"));
                pos.y += rowHeight;
                if (property.FindPropertyRelative("mainAudio") != null) {
                    AudioClip c = ((AudioClip)property.FindPropertyRelative("mainAudio").objectReferenceValue);
                    if (c != null) {
                        EditorGUI.indentLevel++;
                        EditorGUI.LabelField(new Rect(pos.x, pos.y, pos.width, rowHeight), new GUIContent("Clip length: " + (Mathf.Round(c.length * 100.0f) * 0.01f) + " seconds"));
                        EditorGUI.indentLevel--;
                        pos.y += rowHeight;
                    }
                }
                EditorGUI.indentLevel--;
            }

            //Phrases
            m = property.FindPropertyRelative("phrases");
            string name = "Phrases";
            if (singleAud) name = "Subtitles";
            EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, rowHeight), m, new GUIContent(name));

            if (m.isExpanded) {
                EditorGUI.indentLevel++;
                for (int i = 0; i < m.arraySize; i++) {
                    SerializedProperty phrase = m.GetArrayElementAtIndex(i);
                    string s = "audio";
                    if (singleAud)
                        s = "noAudio";
                    if (NarrationManager.alwaysSmartSubs || smartSubsOn)
                        s += "smartSubs";
                    EditorGUI.PropertyField(pos, phrase, new GUIContent(s));
                    pos.y += EditorGUI.GetPropertyHeight(phrase);
                }
                //check if array elems need to be shifted
                for(int i = 0; i < m.arraySize; i++) {
                    MovePhrase(m, i);
                }
                //button to incr. size
                if (m.arraySize ==0)
                    pos.y += rowHeight;
                if (GUI.Button(new Rect((pos.x + pos.width) * 0.5f - 30, pos.y, 60, rowHeight), new GUIContent("+")))
                    m.InsertArrayElementAtIndex(m.arraySize);
                pos.y += rowHeight;
            }


            property.FindPropertyRelative("lastCalculatedHeight").floatValue = pos.y - position.y + 16;
            EditorGUI.indentLevel = 0;
            EditorGUI.EndProperty();

            if (NarrationManager.alwaysSmartSubs || smartSubsOn)
                ApplySmartSubs(property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return property.FindPropertyRelative("lastCalculatedHeight").floatValue;
        }

        void MovePhrase(SerializedProperty array, int ind) {
            if (ind < 0 || ind > array.arraySize - 1) return;
            SerializedProperty phrase = array.GetArrayElementAtIndex(ind);
            int dir = phrase.FindPropertyRelative("move").enumValueIndex;
            phrase.FindPropertyRelative("move").enumValueIndex = (int)Phrase.Movement.None;
            switch (dir) {
                case (int)Phrase.Movement.None:
                    return;
                case (int)Phrase.Movement.Up:
                    if (ind > 0) {
                        array.MoveArrayElement(ind, ind - 1);
                    }
                    break;
                case (int)Phrase.Movement.Down:
                    if(ind < array.arraySize - 1)
                        array.MoveArrayElement(ind,ind+1);
                    break;
                case (int)Phrase.Movement.Delete:
                    array.DeleteArrayElementAtIndex(ind);
                    break;
            }
        }


        void ApplySmartSubs(SerializedProperty property) {
            SerializedProperty array = property.FindPropertyRelative("phrases");
            SerializedProperty singleAudio = property.FindPropertyRelative("singleAudio_MultiSub");
            SerializedProperty looping = property.FindPropertyRelative("LoopAudioForDuration");
            SerializedProperty mainAudio = property.FindPropertyRelative("mainAudio");
            //single audio
            if (singleAudio.boolValue) {
                if (!looping.boolValue && ((AudioClip)mainAudio.objectReferenceValue) != null) {
                    float allocTime = ((AudioClip)mainAudio.objectReferenceValue).length / ((float)array.arraySize);
                    for (int i = 0; i < array.arraySize; i++) {
                        SerializedProperty phrase = array.GetArrayElementAtIndex(i);
                        phrase.FindPropertyRelative("duration").floatValue = allocTime;
                    }
                } else {
                    for (int i = 0; i < array.arraySize; i++) {
                        SerializedProperty phrase = array.GetArrayElementAtIndex(i);
                        phrase.FindPropertyRelative("duration").floatValue = NarrationManager.instance.defaultPhraseDuration;
                        if (mainAudio.objectReferenceValue == null)
                            looping.boolValue = false;
                    }
                }
            } //not single audio
            else {
                for (int i = 0; i < array.arraySize; i++) {
                    SerializedProperty phrase = array.GetArrayElementAtIndex(i);
                    SerializedProperty aud = phrase.FindPropertyRelative("audio");
                    SerializedProperty duration = phrase.FindPropertyRelative("duration");
                    if (!looping.boolValue && aud.objectReferenceValue != null) {
                        duration.floatValue = ((AudioClip)aud.objectReferenceValue).length;
                    } else
                        duration.floatValue = NarrationManager.instance.defaultPhraseDuration;
                }
            }
        }
    }

}
