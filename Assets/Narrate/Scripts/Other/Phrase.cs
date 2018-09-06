using UnityEngine;
using System.Collections;
////////////////////////////////////90-chars///////////////////////////////////////////////
/// <summary>
/// Phrases are an (optional) single audio clip with an (optional) string of text and 
/// (optional) display-duration time
//////////////////////////////////////////////////////////////////////////////////////////
namespace Narrate {
    [System.Serializable]
    public class Phrase {
        public AudioClip audio;
        public string text;
        public float duration = 0;

        //used by Phrase Property Drawer
#if UNITY_EDITOR
        [HideInInspector]
        public bool drawAudio = true;
        [HideInInspector]
        public float lastCalculatedHeight = 16;
        public Movement move = Movement.None;
        public enum Movement {
            None,
            Up,
            Down,
            Delete
        }
#endif

        public bool HasSomethingToPlay() {
            if (audio == null && (text == null || text == ""))
                return false;
            return true;
        }

    }
}
