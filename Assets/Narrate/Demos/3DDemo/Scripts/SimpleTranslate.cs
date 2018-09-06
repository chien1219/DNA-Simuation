using UnityEngine;
using System.Collections;
namespace Narrate {
    public class SimpleTranslate : MonoBehaviour {
        public Vector3 moveBy;
        public float duration;
        private Vector3 startPos;
        private float timer;
        private bool active;
        // Use this for initialization
        void Awake() {
            active = false;
            timer = 0;
            startPos = this.transform.position;
            if (duration <= 0) duration = .5f;
        }

        // Update is called once per frame
        void Update() {
            if (!active) return;
            timer += Time.deltaTime;
            float t = timer / duration;
            if (t > 1.0f) t = 1.0f;
            this.transform.position = Vector3.Lerp(startPos, startPos + moveBy, t);
            if (t >= 1) this.enabled = false;
        }

        void OnEnable() {
            active = true;
        }
    }
}
