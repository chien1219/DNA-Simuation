using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Narrate {
    /// <summary>
    /// A playlist of narrations - NarrationTriggers can draw from a NarrationList instead of
    /// their built-in narrations.  This allows for random shuffle and playing the list on repeat.
    /// </summary>
    public class NarrationList : MonoBehaviour {
        public OnFinish whenEndReached = OnFinish.Disable;
        public bool loopNarrations = false; //if this list should be looped through repeatedly
        public bool shuffle = false; //will shuffle list (play randomly)
        public Narration[] narrations; //the narrations in this list

        private int next; //the index of the clip that will be loaded into curNar next
        private Narration curNar; //the one narration that's going to play next

        public enum OnFinish {
            Disable,
            Destroy,
            DestroyGameObject,
            DisableGameObject,
            Nothing
        }

        void Awake() {
            next = 0;
            PrepNextNarration();
        }

        public bool Play() {
            bool played = true; // if the curNar is null, we'll return true in case of empty cells in the narrations[]
            if (curNar != null)
                played = NarrationManager.instance.PlayNarration(curNar);
            PrepNextNarration();
            return played;
        }


        private void PrepNextNarration() {
            //if list of narrations is empty or doesn't exist, set curNar to null
            if (narrations == null || narrations.Length < 1) {
                curNar = null;
                EndReached();
                return;
            }

            if (!shuffle) {
                //if we've reached the end of the loop
                if (next >= narrations.Length) {
                    //if we don't loop, do nothing.
                    if (!loopNarrations) {
                        curNar = null;
                        EndReached();
                        return;
                    } else//else, loop back to the beginning
                        next %= narrations.Length;
                }
                curNar = narrations[next];
                next++;
            } else {
                int toPlay = Mathf.RoundToInt(Random.value * (narrations.Length - 1));
                curNar = narrations[toPlay];
            }
        }

        private void EndReached() {
            switch (whenEndReached) {
                case OnFinish.Disable:
                    this.enabled = false;
                    break;

                case OnFinish.Destroy:
                    Destroy(this);
                    break;

                case OnFinish.DisableGameObject:
                    this.gameObject.SetActive(false);
                    break;

                case OnFinish.DestroyGameObject:
                    Destroy(this.gameObject);
                    break;
            }


        }
    }

}
