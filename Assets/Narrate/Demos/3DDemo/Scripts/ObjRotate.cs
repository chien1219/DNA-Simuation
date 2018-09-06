using UnityEngine;
using System.Collections;
/*A very rough script for rotating an object - best only to rotate one axis, 
otherwise gimbal lock likely to occur*/
namespace Narrate {
    public class ObjRotate : MonoBehaviour {
        public Vector3 rate = new Vector3(20, 30, 40);

        // Update is called once per frame
        void Update() {
            Vector3 rot = transform.localEulerAngles;
            rot += Time.deltaTime * rate;
            transform.localEulerAngles = rot;
        }
    }
}
