using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// /////////////////////////////////80-chars/////////////////////////////////////
/// <summary>
/// The NarrationManager is in charge of playing audio/subtitles in the Narrations,
/// one at a time. If it is already playing something when a new clip comes in, it
/// will either queue the new Narration, or interrupt what is currently playing if
/// the new Narration has priority.
/// It orders the SubtitleManager to play subtitles.
//////////////////////////////////////////////////////////////////////////////

namespace Narrate {
    public delegate void InteractionEventHandler(object sender, System.EventArgs e);
    public class NarrationManager : MonoBehaviour {
        public event InteractionEventHandler InteractPressed;

        public int queueLength = 5; //max number of Narrations that can be queued at any time
        public float pauseBetweenNarrations = 0.5f;
        public static NarrationManager instance = null;//the single instance of design manager available
        public SubtitleManager subManager; //the subtitle manager that displays subs
        protected AudioSource src;//the audio source which the manager plays sounds on
        protected List<Narration> clipQueue;//the list of Narrations waiting to be player

        public static bool TextMode = false; //text becomes more important than audio. Subs will always be on
        public bool pressToSkip = false; //finishes audio and if typing, displays all text, time padding is still used
        public bool pressToContinue = false; //subs will stay open until the player presses this.
        public string buttonName = "Submit";//name of the button used for this
        public bool disableCharacterWhileNarrating = false; //character won't be able to move while subs are on display
        public MonoBehaviour characterController; //the character controller to disable/enable

        private IEnumerator PlayerIenum;

        private bool interrupt;
        private Narration interruptNarration;
        public float defaultPhraseDuration = 3.0f;
        public bool isPlaying;
#if UNITY_EDITOR
        public static bool alwaysSmartSubs;
        public static bool alwaysLoopAudio;
        public static bool alwaysSingleAudio;

#endif

        /// <summary>
        /// Sets up the NarrationManager's singleton design pattern - only one instance of
        /// the manager is allowed to exist and is referenced by the variable "instance"
        /// </summary>
        void Awake() {
            if (instance == null) {
                instance = this;
                src = this.GetComponent<AudioSource>();
                if (src == null) {
                    Debug.Log("NarrationManager requires an AudioSource component on the same GameObject. No AudioSource found. Disabling");
                    instance = null;
                    this.enabled = false;
                }
            } else if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(this);
            clipQueue = new List<Narration>();
            interrupt = false;
            if (pressToContinue)
                TextMode = true;
        }

        void OnEnable() {
            PlayerIenum = Player();
            StartCoroutine(PlayerIenum);
            StartCoroutine(InteractionInputCheck());
        }

        /// <summary>
        /// Tries to play the Narration
        /// </summary>
        /// <returns><c>true</c>, if Narration was played or queued, <c>false</c> otherwise.</returns>
        public bool PlayNarration(Narration newNarration) {
            //check for valid arguments
            if (newNarration == null) {
                Debug.Log("In NarrationManager: PlayClip(Narration newClip)" +
                    "was passed a null Narration.");
                return false;
            }
            //Try to play it or queue it, depending on the Narration's busyBehavior
            switch (newNarration.busyBehavior) {
                case Narration.BusyBehavior.Queue:
                    return Queue(newNarration);
                case Narration.BusyBehavior.PrioritizeInQueue:
                    return PriorityQueue(newNarration);
                case Narration.BusyBehavior.DoNothing:
                    if (clipQueue.Count == 0) {
                        clipQueue.Add(newNarration);
                        return true;
                    } else
                        return false;
                case Narration.BusyBehavior.Interrupt:
                    if (interrupt) //already busy with an interrupt
                        return false;
                    else {
                        interrupt = true;
                        interruptNarration = newNarration;
                        return true;
                    }
            }
            //this point should never be reached, but if it is...
            Debug.Log("In NarrationManager: PlayClip(Narration newClip)" +
                       "was passed a Narration with an illegal \"busyBehavior\" field");
            return false;
        }

        IEnumerator Player() {
            IEnumerator slavePlayer = null;
            while (true) {
                if (clipQueue.Count > 0 || interrupt) {
                    isPlaying = true;
                    if (disableCharacterWhileNarrating && characterController != null)
                        characterController.enabled = false;

                    //keep playing interruptions until there are none
                    while (interrupt) {
                        interrupt = false;
                        if (interruptNarration != null) {
                            slavePlayer = SlavePlayer(interruptNarration);
                            yield return StartCoroutine(slavePlayer);
                            slavePlayer = null;
                            if (!interrupt)//if there's been no new interruption, we can delete the old interruptNarration
                                interruptNarration = null;
                        }
                    }
                    //if queue has entries, play the first one
                    if (clipQueue.Count > 0) {
                        Narration curr = clipQueue[0];
                        slavePlayer = SlavePlayer(curr);
                        yield return StartCoroutine(slavePlayer);
                        slavePlayer = null;
                        clipQueue.Remove(curr); //TODO: If want the option to replay interrupted narrations, implement here
                    }

                    //if there's nothing left to play right away, re-enable the character controller
                    if (clipQueue.Count == 0 && !interrupt) {
                        isPlaying = false;
                        if (disableCharacterWhileNarrating && characterController != null)
                            characterController.enabled = true;
                    }
                }
                yield return null;
            }
        }

        IEnumerator SlavePlayer(Narration curr) {
            if (curr == null || !curr.HasSomethingToPlay()) yield break;

            float timer = 0;
            bool skipPressed = false;
            bool continuePressed = false;
            float subDuration = 0;
            float lastTimePressed = 0; //last time continue/skip button was pressed

            src.clip = null;
            //prepare AudioSource if the Narration is singleAudio or looping
            if (curr.singleAudio_MultiSub && curr.mainAudio != null)
                src.clip = curr.mainAudio;
            if (curr.LoopAudioForDuration) {
                src.loop = true;
                subDuration = .01f;//to prevent infinite looping
            } else {
                src.loop = false;
                if (curr.singleAudio_MultiSub && curr.mainAudio != null)
                    src.Play();
            }

            //IF NO PHRASES/SUBS AND MAIN AUDIO IS NON-LOOPING
            if ((curr.phrases == null || curr.phrases.Length <= 0) && curr.singleAudio_MultiSub && curr.mainAudio != null && src.isPlaying) {
                subDuration = curr.mainAudio.length;
                subManager.DisplaySubtitle("", curr.mainAudio.length, !pressToContinue);
                //while time left OR continue isn't yet pressed
                subDuration += .05f;
                while (timer < subDuration) {
                    Debug.Log("nm time: " + timer);
                    if (interrupt) yield break;
                    //check for continue/skip button presses
                    if ((pressToContinue || pressToSkip) && (timer - lastTimePressed > 0.3f)) {
                        if (Input.GetButtonUp(buttonName)) {
                            lastTimePressed = timer;
                            if (pressToSkip && !skipPressed) {
                                skipPressed = true;
                                src.Stop();
                                src.clip = null;
                                subManager.Hurry();
                                if (!pressToContinue) break;
                            } else if (pressToContinue && (!pressToSkip || skipPressed)) {
                                if (!skipPressed) {
                                    src.Stop();
                                    src.clip = null;
                                }
                                subManager.Stop();
                                break;
                            }
                        }
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }
                src.Stop();
                src.clip = null;
                //pause between narrations
                if (!pressToContinue && pauseBetweenNarrations > 0)
                    yield return new WaitForSeconds(pauseBetweenNarrations);
                //Force player to hit the Continue button if necessarys
                while (pressToContinue) {
                    if (interrupt) yield  break;
                    if (Input.GetButtonUp(buttonName)) {
                        subManager.Stop();
                        yield break;
                    }
                    yield return null;
                }
                yield break;
            }

            //ELSE IF HAS PHRASES/SUBS, PROCESS EACH HERE
            for (int i = 0; i < curr.phrases.Length; i++) {
                if (!curr.phrases[i].HasSomethingToPlay()) continue; //is an empty phrase, so skip
                skipPressed = false;
                continuePressed = false;
                timer = 0;
                lastTimePressed = 0;

                Phrase curPhrase = curr.phrases[i];
                subDuration = curPhrase.duration;
                //play the audio
                if (curr.LoopAudioForDuration && curr.singleAudio_MultiSub && curr.mainAudio != null) {
                    src.clip = curr.mainAudio;
                    src.Play();
                } else if (!curr.singleAudio_MultiSub && curPhrase.audio != null) {
                    src.clip = curPhrase.audio;
                    src.Play();
                }
                if (subManager != null)
                    subManager.DisplaySubtitle(curPhrase.text, subDuration, !pressToContinue);
                if (subDuration <= 0) {
                    subDuration = defaultPhraseDuration;
                    if (subManager != null)
                        subDuration = defaultPhraseDuration;
                    if (subDuration <= 0) subDuration = 0.05f;
                }

                //while time left OR continue isn't yet pressed
                subDuration += .05f;
                while (timer < subDuration) {
                    if (interrupt) yield break;
                    //check for and handle continue/skip button presses
                    if ((pressToContinue || pressToSkip) && (timer - lastTimePressed > 0.3f)) {
                        if (Input.GetButtonUp(buttonName)) {
                            lastTimePressed = timer;
                            if (pressToSkip && !skipPressed) {
                                skipPressed = true;
                                src.Stop();
                                src.clip = null;
                                subManager.Hurry();
                                if (!pressToContinue) break;
                            } else if (pressToContinue && (!pressToSkip || skipPressed)) {
                                if (!skipPressed) {
                                    src.Stop();
                                    src.clip = null;
                                }
                                subManager.Stop();
                                break;//end the loop
                            }
                        }
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (!(curr.singleAudio_MultiSub && !curr.LoopAudioForDuration && (i + 1 < curr.phrases.Length))) {
                    src.Stop();
                    src.clip = null;
                }

                //Force player to hit the Continue button if they haven't and is required
                while ((!continuePressed) && pressToContinue) {
                    if (interrupt) yield break;
                    if (Input.GetButtonUp(buttonName)) {
                        subManager.Stop();
                        src.Stop();
                        src.clip = null;
                        break;
                    }
                    yield return null;
                }
            }
            src.Stop();
            src.clip = null;
            yield break;
        }

        /// <summary>
        /// Handles checking if user has pressed the Interact button
        /// </summary>
        /// <returns></returns>
        IEnumerator InteractionInputCheck() {
            while (true) {
                //if press to continue or press to skip, interactions only register with triggers when nothing is being played
                if (((!pressToContinue && !pressToSkip) || !isPlaying) && Input.GetButtonDown(buttonName)) {
                    if (InteractPressed!=null)
                        InteractPressed(this, new System.EventArgs());
                }
                yield return null;
            }
        }

        /// <summary>
        /// Add the Narration at the end of the clipQueue
        /// </summary>
        bool Queue(Narration toAdd) {
            //add if there's room
            if (clipQueue.Count < queueLength) {
                clipQueue.Add(toAdd);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add the Narration at the front of the clipQueue
        /// </summary>
        bool PriorityQueue(Narration toAdd) {
            if (queueLength == 0)
                return false;
            clipQueue.Insert(0, toAdd);
            //trim clipQueue if overflowing
            if (clipQueue.Count >= queueLength)
                clipQueue.RemoveRange(queueLength, clipQueue.Count - queueLength);
            return true;
        }

        void OnDisable() {
            StopAllCoroutines();
        }
    }
}
