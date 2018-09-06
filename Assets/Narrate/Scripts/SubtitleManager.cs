using UnityEngine;
using System.Collections;
using UnityEngine.UI;

////////////////////////////////////80-chars/////////////////////////////////////
/// <summary>
/// The SubtitleManager is in charge of displaying Subtitles if the user has them
/// enabled (ie if integer <PrefsKey> in PlayerPrefs set to one).
/// It handles display logic, such as resizing fonts based on different screen
/// heights, as well as scrolling subtitles that are too large to fit in the
/// display area all at once.
////////////////////////////////////////////////////////////////////////////////
namespace Narrate {
    public class SubtitleManager : MonoBehaviour {
        public static string PrefsKey = "Subtitles";//key for the integer in PlayerPrefs to see if subs are on. 0 = off, 1 = 0n
        public bool OnByDefault = true;// If no the PrefsKey hasn't been set (such as when a new game is started) should subtitles will be enabled by default
        public GameObject displayArea;// The object to which all visible subtitle-UI elements should be childed
        public Text textUI;// The Text in which the subtitle is written
        public ScrollRect scrollRect;// The scroll rect in which content is displayed

        public float fontSizeModifier = 25;// The percentage of the display's height a line of text should fill
        public Vector2 fontSizeRange = new Vector2(9, 20);// Minimum and maximum text size

        public float timePadding = 3.0f;// When finished scrolling, will display for this much longer
        private float timeLeft;//display time left on current subtitle

        private int oldScreenSize;//used to check if screen height changed so it can resize the text size
        private IEnumerator displayInst; //current instance of the IEnumerator in charge of displaying the subtitle
        private bool hurry = false; //flag used to indicated the player wants to skip dialogue

        public bool typing = false;//Will "animate" the display by typing one letter at a time
        public float defaultDelayBetweenLetters = 0.1f; //how long to wait before displaying the next letter

        void Awake() {
            if (defaultDelayBetweenLetters < 0.02f)
                defaultDelayBetweenLetters = 0.02f;
            displayInst = null;
            if (displayArea == null || textUI == null || scrollRect == null) {
                Debug.LogWarning("Subtitle Manager is missing UI object reference(s).  Disabling subtitles");
                this.enabled = false;
            }
            displayArea.SetActive(false);//hide subs on startup
            timeLeft = -timePadding;

            //if no player preference detected, set subtitles to the default on/off state
            if (!PlayerPrefs.HasKey(PrefsKey)) {
                if (OnByDefault)
                    PlayerPrefs.SetInt(PrefsKey, 1);
                else
                    PlayerPrefs.SetInt(PrefsKey, 0);
            }
        }

        void Start() {
            scrollRect.verticalNormalizedPosition = 1.0f;
            RescaleText();
        }
        /// <summary>
        /// Handles the displaying, scrolling, and hiding when subtitles are not typing-animated
        /// </summary>
        IEnumerator DisplayPlain(string sub, float displayFor, bool hideOnFinish) {
            hurry = false;
            scrollRect.verticalNormalizedPosition = 1.0f;//scroll back to the top
            textUI.text = sub;
            timeLeft = displayFor;
            displayArea.SetActive(true);


            //let user read a tiny bit before scrolling begins
            yield return new WaitForSeconds(0.05f);

            //scroll
            while (timeLeft > 0 && !hurry) {
                //if screen has been resized, rescale the text
                if (oldScreenSize != Screen.height)
                    RescaleText();
                timeLeft -= Time.deltaTime;
                scrollRect.verticalNormalizedPosition = timeLeft / displayFor;
                yield return null;
            }
            yield return null;//wait for UI to update before fixing scroll
            scrollRect.verticalNormalizedPosition = 0;

            //wait for padding
            if (timePadding > 0)
                yield return new WaitForSeconds(timePadding);

            if (hideOnFinish)
                DisplayCleanup();
        }

        /// <summary>
        /// Handles the displaying, typing, scrolling, and hiding when subtitles are  typing-animated
        /// </summary>
        IEnumerator DisplayTyping(string sub, float displayFor, bool hideOnFinish) {
            float timer = 0;
            hurry = false;
            textUI.text = "";
            displayArea.SetActive(true);
            float timeBetweenLetters = (displayFor + .001f) / ((float)sub.Length);
            float bonusPadding = 0; //any displayFor time that's left after typing has finished gets added to the timePadding

            if (timeBetweenLetters > defaultDelayBetweenLetters) {
                timeBetweenLetters = defaultDelayBetweenLetters;
                bonusPadding = displayFor - timeBetweenLetters * sub.Length;
                if (bonusPadding < 0)//for fringe cases
                    bonusPadding = 0;
            }

            while (sub.Length > 0 && !hurry) {
                timer += Time.deltaTime;
                float onDis = Mathf.Round(timer / timeBetweenLetters);
                onDis -= textUI.text.Length;
                for (int i = 0; i < onDis; i++) {
                    textUI.text += sub[0];
                    sub = sub.Remove(0, 1);
                    if (sub.Length <= 0)
                        break;
                }
                scrollRect.verticalNormalizedPosition = 0.0f;//scroll back to the top
                yield return null;
            }

            //if hurried, post the rest of the phrase immediately and set scroll to max
            if (sub.Length > 0)
                textUI.text += sub;
            yield return null;//give UI time to update before fixing scroll position
            scrollRect.verticalNormalizedPosition = 0.0f;

            //use the time padding for finishing reading
            if (timePadding + bonusPadding > 0)
                yield return new WaitForSeconds(timePadding + bonusPadding);
            if (hideOnFinish)
                DisplayCleanup();
        }

        /// <summary>
        /// Rescales the text. Changes the fontsize so the text's height will be 
        /// <fontSizemodifier>-percent of the displayArea's height, and clamps the value
        /// in the min/max fontsize range.
        /// </summary>
        void RescaleText() {
            oldScreenSize = (int)displayArea.GetComponent<RectTransform>().rect.height;
            float size = (int)(((float)oldScreenSize) * (fontSizeModifier * 0.01f));
            if (size < fontSizeRange.x)
                size = fontSizeRange.x;
            else if (size > fontSizeRange.y)
                size = fontSizeRange.y;
            textUI.fontSize = (int)size;
        }

        /// <summary>
        /// Displays a subtitle
        /// </summary>
        /// <param name="sub">The text to display to the subtitle</param>
        /// <param name="displayFor">how long to display for.  If 0 or lower, will use default time</param>
        /// <param name="hideOnFinish">if the subtitle panel should be cleared and hidden when display time has been reached</param>
        public void DisplaySubtitle(string sub, float displayFor = -1, bool hideOnFinish = true) {
            //if subtitles are not on, don't do anything
            if (!NarrationManager.TextMode && PlayerPrefs.GetInt(SubtitleManager.PrefsKey) != 1)
                return;
            //set the to display time if unspecified
            if (displayFor <= 0) {
                if (NarrationManager.instance != null && NarrationManager.instance.defaultPhraseDuration > 0)
                    displayFor = NarrationManager.instance.defaultPhraseDuration;
                else
                    return;
            }
            //if no sub, display display a simple message
            if (sub.Equals(""))
                sub = "<audio>";

            //if there's something already being displayed, stop it and replace it
            if (displayInst != null) {
                StopCoroutine(displayInst);
                textUI.text = "";
                displayInst = null;
            }
            hurry = false;
            //call the display coroutine
            if (!typing)
                displayInst = DisplayPlain(sub, displayFor, hideOnFinish);
            else
                displayInst = DisplayTyping(sub, displayFor, hideOnFinish);

            StartCoroutine(displayInst);
        }

        public void Hurry() {
            hurry = true;
        }

        public void Stop() {
            if (displayInst != null)
                StopCoroutine(displayInst);
            displayInst = null;
            DisplayCleanup();
        }

        /// <summary>
        /// Empty and hide the subtitle display
        /// </summary>
        void DisplayCleanup() {
            textUI.text = "";
            displayArea.SetActive(false);
            displayInst = null;
        }
    }
}
