using UnityEngine;
using System.Collections;

////////////////////////////////////80-chars/////////////////////////////////////
/// <summary>
/// Narrations are the basic-unit of playable content.  It contains an audioclip 
/// to play on the NarrationManager, a subtitle to display, and what to do
/// if the NarrationManager is already playing something when it tries to be 
/// played.
////////////////////////////////////////////////////////////////////////////////
namespace Narrate {
    [System.Serializable]
    public class Narration {
        public BusyBehavior busyBehavior; //how to behave if the NarrationManager is already busy
        public bool singleAudio_MultiSub; //only one audio clip, multiple subtitles.
        [SerializeField]
        public AudioClip mainAudio; //if phrases for subs only, then they are the subs for this audio
        public Phrase[] phrases; //
        public bool LoopAudioForDuration;

#if UNITY_EDITOR
        //For use in the property drawer
        [HideInInspector]
        public bool expanded = false;
        [HideInInspector]
        public float lastCalculatedHeight = 16;
        [HideInInspector]
        public bool useSmartSubs = false;
#endif

        //BusyBehavior is used to indicate the ways the NarrationManager can(and should) treat this
        //Narration if the NarrationManager is already busy playing something when this Narration
        //tries to be played
        public enum BusyBehavior {
            Queue, //clip will be put at end of NarrationManager queue
            PrioritizeInQueue, //clip will be put at head of narrationManager queue
            Interrupt, //NarrationManager will discard what it's playing and play this instead
            DoNothing   //the NarrationManager will do nothing with it
        }

        public float TotalAudioDuration() {

            if (singleAudio_MultiSub) {
                if (mainAudio != null)
                    return mainAudio.length;
                else {
                    float t = 0;
                    if (phrases != null)
                        for (int i = 0; i < 0; i++) {
                            if (phrases[i] != null)
                                t += phrases[i].duration;
                        }
                    return t;
                }
            }

            float f = 0;
            if (phrases != null)
                for (int i = 0; i < phrases.Length; i++) {
                    if (phrases[i] != null) {
                        if (phrases[i].audio != null)
                            f += phrases[i].audio.length;
                        else
                            f += phrases[i].duration;
                    }
                }
            return f;
        }

        public bool HasSomethingToPlay() {
            if (singleAudio_MultiSub && mainAudio == null && (phrases == null || phrases.Length == 0))
                return false;
            if (phrases != null && phrases.Length > 0) {
                bool hasSomething = false;
                for (int i = 0; i < phrases.Length; i++) {
                    if (phrases[i].HasSomethingToPlay()) {
                        hasSomething = true;
                        break;
                    }
                }
                return hasSomething;
            }
            return false;
        }


    }

}
