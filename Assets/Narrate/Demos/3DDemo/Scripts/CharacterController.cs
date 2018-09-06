using UnityEngine;
using System.Collections;
namespace Narrate {
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour {
        Rigidbody bod;
        public float movementSpeed = 5;
        public float turnRate = 70;
        public float maxSpeed = 5;
        public Vector2 jumpForce = new Vector2(1, 4);
        bool grounded;
        // Use this for initialization
        void Start() {
            bod = this.GetComponent<Rigidbody>();
        }

        void CheckGrounded() {
            grounded = Physics.CheckSphere(this.transform.position, 2, LayerMask.GetMask("Default"));
        }

        // Update is called once per frame
        void Update() {
            CheckGrounded();
            float forward = Input.GetAxis("Vertical");
            float turn = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump") && grounded) {
                bod.AddRelativeForce(new Vector3(0, jumpForce.y, jumpForce.x), ForceMode.Impulse);
            } else if (forward != 0 && bod.velocity.magnitude < maxSpeed) {
                bod.AddRelativeForce(Vector3.forward * forward * movementSpeed);
            }
            if (turn != 0) {
                Vector3 rot = this.transform.localEulerAngles;
                rot.y += Time.deltaTime * turnRate * turn;
                this.transform.localEulerAngles = rot;
            }
        }
    }
}