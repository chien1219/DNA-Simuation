using UnityEngine;
using System.Collections;
namespace Narrate {
    [RequireComponent(typeof(Collider2D))]
    public class Elevator : MonoBehaviour {
        public float upperLim;
        public float lowerLim;
        public bool playerRidesDown = false;
        public float mvmtSpeed;

        private float upperExtent;
        private float lowerExtent;
        private float drawHeight = .2f;
        private bool playerOnboard;
        public bool overrideOn = false;
        // Use this for initialization
        void Start() {
            float yPos = this.transform.position.y;
            upperExtent = yPos + upperLim;
            lowerExtent = yPos - lowerLim;

            playerOnboard = false;
            if (playerRidesDown)
                mvmtSpeed *= -1;
        }

        // Update is called once per frame
        void Update() {
            Vector3 pos = transform.position;
            //moveUp
            if ((playerOnboard && transform.position.y < upperExtent) || overrideOn)
                pos.y += mvmtSpeed * Time.deltaTime;
            //moveDown
            else if (!playerOnboard && transform.position.y > lowerExtent)
                pos.y -= mvmtSpeed * Time.deltaTime;

            transform.position = pos;
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag.Equals("Player")) {
                playerOnboard = true;
            }
        }

        void OnTriggerExit2D(Collider2D other) {
            if (other.tag.Equals("Player"))
                playerOnboard = false;
        }

        public void overrideON() {
            overrideOn = true;
        }

        void OnDrawGizmos() {
            float ypos = transform.position.y;
            upperExtent = ypos + upperLim;
            lowerExtent = ypos - lowerLim;

            //upper and lower
            Vector3 upperPoint = this.transform.position;
            upperPoint.y = upperExtent;
            Vector3 lowerPoint = this.transform.position;
            lowerPoint.y = lowerExtent;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(lowerPoint, upperPoint);
            Gizmos.DrawLine(new Vector3(upperPoint.x + drawHeight, upperPoint.y, upperPoint.z),
                             new Vector3(upperPoint.x - drawHeight, upperPoint.y, upperPoint.z));
            Gizmos.DrawLine(new Vector3(lowerPoint.x + drawHeight, lowerPoint.y, lowerPoint.z),
                             new Vector3(lowerPoint.x - drawHeight, lowerPoint.y, lowerPoint.z));
        }
    }
}
