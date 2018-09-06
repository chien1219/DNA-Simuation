using UnityEngine;
using System.Collections;
using UnityEditor;

/////////////////////////////////////////<summary>//////////////////////////////////////////////////
///
/////////////////////////////////////////<summary>//////////////////////////////////////////////////
namespace Narrate {
    [CustomEditor(typeof(NarrationManager))]
    public class NarrationManagerInspector : Editor {
        bool interactiveFoldout;
        bool narrationDefaults;
        bool textmodeSettingBeforePressToContinue;

        [InitializeOnLoadMethod]
        void FindAManager() {
            if (NarrationManager.instance == null) {
                NarrationManager[] nms = (NarrationManager[])FindObjectsOfType<NarrationManager>();
                if (nms.Length > 0)
                    NarrationManager.instance = nms[0];
                else {
                    Debug.LogWarning("No Instance of NarrationManager in scene - Narrations will not function");
                }
                //disable other narrationManagers in the scene
                if (nms.Length > 1)
                    for (int i = 0; i < nms.Length; i++) {
                        if (nms[i] != NarrationManager.instance) {
                            nms[i].gameObject.SetActive(false);
                            Debug.Log("More than one NarrationManager found.  Disabling non-defaults");
                        }
                    }
            }
        }

        public override void OnInspectorGUI() {
            NarrationManager nm = (NarrationManager)target;
            nm.queueLength = EditorGUILayout.IntSlider(new GUIContent("Queue Length:", "Maximum number" +
                "of narrations that can queued"), nm.queueLength, 1, 50);
            nm.subManager = (SubtitleManager)EditorGUILayout.ObjectField("Subtitle Manager: ", nm.subManager,
                typeof(SubtitleManager), true);

            nm.defaultPhraseDuration = EditorGUILayout.FloatField(new GUIContent("Default Phrase Duration","How long Phrases will played/displayed by default"), nm.defaultPhraseDuration);
            if (nm.defaultPhraseDuration <= .1f)
                nm.defaultPhraseDuration = .1f;

            if (nm.pressToContinue) {
                GUI.enabled = false;
                EditorGUILayout.Toggle(new GUIContent("Text Always On", "The subtitle display will always be on because Press To Continue is enabled."), true);
                GUI.enabled = true;
            } else
                NarrationManager.TextMode = EditorGUILayout.Toggle(new GUIContent("Text Always On", "The " +
                    "subtitle display will always be on."), NarrationManager.TextMode);


            narrationDefaults = EditorGUILayout.Foldout(narrationDefaults, "Narration Defaults");
            if (narrationDefaults) {
                EditorGUI.indentLevel++;
                NarrationManager.alwaysSmartSubs = EditorGUILayout.Toggle(new GUIContent("Smart Subs", "All Narrations use Smart Subs to determine play length"),
                    NarrationManager.alwaysSmartSubs);
                NarrationManager.alwaysSingleAudio = EditorGUILayout.Toggle(new GUIContent("Single Audio", "All Narrations use a single audio clip"),
                    NarrationManager.alwaysSingleAudio);
                NarrationManager.alwaysLoopAudio = EditorGUILayout.Toggle(new GUIContent("Loop Audio", "All Narrations loop audio"),
                    NarrationManager.alwaysLoopAudio);
                EditorGUI.indentLevel--;
            }


            interactiveFoldout = EditorGUILayout.Foldout(interactiveFoldout, "Interactive Settings");
            if (interactiveFoldout) {
                nm.pressToSkip = EditorGUILayout.Toggle(new GUIContent("Press to Skip", "User can hit specified button to skip the narration or hurry it up"),
                    nm.pressToSkip);
                nm.pressToContinue = EditorGUILayout.Toggle(new GUIContent("Press to Continue", "User must hit specified button to continue/close the narration"),
                    nm.pressToContinue);
                if (nm.pressToContinue || nm.pressToSkip)
                    nm.buttonName = EditorGUILayout.TextField(new GUIContent("Button Name", "Name of entry" +
                        " in the Input Manager that is used by the Narration Manager"), nm.buttonName);

                //Disable Character
                nm.disableCharacterWhileNarrating = EditorGUILayout.Toggle(new GUIContent("Disable Chara" +
                    "cter While Narrating", "The player's character won't be able to move while Narrations" +
                    " are playing."), nm.disableCharacterWhileNarrating);
                if (nm.disableCharacterWhileNarrating)
                    nm.characterController = (MonoBehaviour)EditorGUILayout.ObjectField("Character Controller: ", nm.characterController,
                        typeof(MonoBehaviour), true);
            }

                EditorUtility.SetDirty(target);
        }
	}
}
