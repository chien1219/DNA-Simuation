using UnityEngine;
using System.Collections;
/////////////////////////////////80-chars/////////////////////////////////////
/// <summary>
/// A NarrationTrigger is in charge of triggering/playing the 
/// Narration it contains at the "correct" moment.  It is has additional logic 
/// that allows the developer to specify what ought to happen to it depending
/// on whether or not the NarrationManager succeeds in playing it. These behaviors
///  done on success and failure are summarized by the Reaction enum below.
/// 
/// NarrationTrigger doesn't actually contain any logic or code that will 
/// ever trigger the clip.  This logic/code should be implemented in child
/// classes that extend the NarrationTrigger class.
/// </summary>
//////////////////////////////////////////////////////////////////////////////
namespace Narrate {
    public delegate void DisableDestroyEventHandler(object sender, System.EventArgs e);
    public class NarrationTrigger : MonoBehaviour {
        public event DisableDestroyEventHandler DisableEvent;
        public event DisableDestroyEventHandler DestroyEvent;

        public bool UseNarrationList = false;    // Plays a narration from a NarrationList instead of it's built in Narration
        public NarrationList narrationList;
        public Narration theNarration;    //the narration this trigger will play
        public Reaction OnFailure = Reaction.Destroy;    //what should happen if manager couldn't play it.
        public Reaction OnSuccess = Reaction.Destroy;    //what should happen if manager did play it
        public float ResetCooldown = 0;     //how many seconds to delay resetting by
        public float delayPlayingBy = 0;
        private bool resetting; //if this clip is currently in the process of resetting itself, is true. else false

        //The reaction that should occur after a play attempt
        public enum Reaction {
            Destroy, //destroy the trigger
            DestroyGameobject,//destroy the entire gameobject it's attached to
            Disable, //disable the trigger
            DisableGameobject,//disable the entire gameobject it's attached to
            Reset         //reset the trigger after <ResetCooldown> seconds 
        }

        void Awake() {
            resetting = false;
            if (UseNarrationList && narrationList == null) {
                Debug.LogWarning("Warning: NarrationList null in NarrationTrigger attached to " + this.gameObject.name + ". Disabling the trigger script");
                this.enabled = false;
            }
        }

        IEnumerator PlayNarration() {
            if (delayPlayingBy > 0)
                yield return new WaitForSeconds(delayPlayingBy);
            //play from NarrationList
            if (UseNarrationList) {
                if (narrationList == null) {
                    React(OnFailure);
                } else if (!narrationList.enabled) {
                    React(OnFailure);
                } else if (narrationList.Play())
                    React(OnSuccess);
                else
                    React(OnFailure);
            }//Else play the built-in Narration
            else if (NarrationManager.instance != null) {
                if (NarrationManager.instance.PlayNarration(theNarration))
                    React(OnSuccess);
                else
                    React(OnFailure);
            }
        }



        public void PlayingFinished(bool success) {
            if (success)
                React(OnSuccess);
            else
                React(OnFailure);
        }


        /// <summary>
        /// Performs the specified Reaction
        /// </summary>
        void React(Reaction r) {
            switch (r) {
                case Reaction.Destroy:
                    Destroy(this);
                    break;
                case Reaction.DestroyGameobject:
                    Destroy(this.gameObject);
                    break;
                case Reaction.Disable:
                    this.enabled = false;
                    break;
                case Reaction.DisableGameobject:
                    this.gameObject.SetActive(false);
                    break;
                case Reaction.Reset:
                    resetting = true;
                    Invoke("CallReset", ResetCooldown);
                    break;
            }
        }

        void CallReset() {
            Reset();
            resetting = false;
        }

        //Triggers the narration to play by setting triggered to true; if the clip
        //is currently resetting, triggering is not allowed, so nothing happens.
        public void Trigger() {
            if (!resetting) {
                StartCoroutine(PlayNarration());
            }
        }

        /// <summary>
        /// This is always called once at the end of a reset. Override and use in
        /// any derived trigger classes that need to do any special clean up or 
        /// reset any variables.
        /// </summary>
        public virtual void Reset() { }

        void OnDisable() {
            Disabled();
        }
        void OnDestroy() {
            Destroyed();
        }

        protected void Destroyed() {
            if (DestroyEvent != null)
                DestroyEvent(null, new System.EventArgs());
        }
        protected void Disabled() {
            if (DisableEvent != null)
                DisableEvent(null, new System.EventArgs());
        }
    }
}