using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

namespace Narrate {
    [CustomEditor(typeof(SubtitleManager))]
public class SubtitleManagerInspector: Editor {
	private bool foldoutSettings = false;
	private bool foldoutObjs = false;
	private bool foldoutFont = false;
	private bool subsInEditor = true;
        public override void OnInspectorGUI() {
            SubtitleManager sm = (SubtitleManager)target;
            subsInEditor = EditorGUILayout.Toggle(new GUIContent("On in Editor:", "Toggle subitles on and off when playing your scene in the UnityEditor"),
                                                  subsInEditor);

            foldoutSettings = EditorGUILayout.Foldout(foldoutSettings, "User Prefs Settings");
            if (foldoutSettings) {
                SubtitleManager.PrefsKey = EditorGUILayout.TextField(new GUIContent("PrefsKey:", "The string that will be used to access whether or not the subtitles are on or off in PlayerPrefs"),
                                                                      SubtitleManager.PrefsKey);
                sm.OnByDefault = EditorGUILayout.Toggle(new GUIContent("On By Default:", "If no PlayerPrefs are detected (eg: a new game), subtitles are automatically turned on"),
                                                         sm.OnByDefault);
            }

            if (subsInEditor) {
                PlayerPrefs.SetInt(SubtitleManager.PrefsKey, 1);
            } else {
                PlayerPrefs.SetInt(SubtitleManager.PrefsKey, 0);
                sm.displayArea.SetActive(false);
            }

            foldoutObjs = EditorGUILayout.Foldout(foldoutObjs, "Canvas Objects");
            if (foldoutObjs) {
                sm.displayArea = (GameObject)EditorGUILayout.ObjectField("Subtitle Display: ", sm.displayArea, typeof(GameObject), true);
                sm.textUI = (Text)EditorGUILayout.ObjectField("TextUI: ", sm.textUI, typeof(Text), true);
                sm.scrollRect = (ScrollRect)EditorGUILayout.ObjectField("ScrollRect: ", sm.scrollRect, typeof(ScrollRect), true);
            }

            foldoutFont = EditorGUILayout.Foldout(foldoutFont, "Font Settings");
            if (foldoutFont) {
                sm.fontSizeRange = EditorGUILayout.Vector2Field("Font Size: ", sm.fontSizeRange);
                sm.fontSizeModifier = EditorGUILayout.Slider("Font Modifier: ", sm.fontSizeModifier, 1, 100);
            }
            sm.timePadding = EditorGUILayout.FloatField(new GUIContent("Time Padding", "How long the subtitle display will stay open after the Phrase has finished"), sm.timePadding);

            //typing-animation related
            sm.typing = EditorGUILayout.Toggle(new GUIContent("Typing Animation", "Text will display one letter at a time, as if being typed"),
                                                sm.typing);
            if (sm.typing) {
                sm.defaultDelayBetweenLetters = EditorGUILayout.FloatField(new GUIContent("Max Time Between Letters:", "How long in seconds to wait between letters. If it takes longer" +
                    "to type the message than Narration's duration, this time will be scaled down automatically for that Narration."),
                                                                            sm.defaultDelayBetweenLetters);
                if (sm.defaultDelayBetweenLetters < 0.02f)
                    sm.defaultDelayBetweenLetters = 0.02f;
            }

            //cleanup/updates
            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
	}
}
