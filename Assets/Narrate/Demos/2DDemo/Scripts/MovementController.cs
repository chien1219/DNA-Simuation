using UnityEngine;
using System.Collections;
namespace Narrate {
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour {
        private Rigidbody2D bod;
        private bool isGrounded;
        public bool useImpulseWalk = false;
        public bool rotateToSlope = false;
        public Transform leftFoot;
        public Transform rightFoot;
        public float WalkForce = 5.0f;
        public float MaxWalkSpeed = 3.0f;
        public float AirVelocity = 1.5f;

        public Vector2 JumpForceVector = new Vector2(3, 1);
        public float groundedBuffer = .2f; //if this close to ground, consider grounded

        public Animator anim;
        private AnimState lastState;
        private AnimState nextState;
        protected enum AnimState {
            Walk,
            Jump,
            Airborne,
            Idle
        }

        //unity handle
        void Start() {
            bod = this.GetComponent<Rigidbody2D>();
            isGrounded = true;
            lastState = AnimState.Idle;
            nextState = AnimState.Idle;
        }

        // unity handle
        void Update() {
            CheckGrounded();
            float x = Input.GetAxis("Horizontal");
            DirectionFacer(x);

            if (isGrounded && Input.GetButtonDown("Jump")) {
                JumpMovement(x);
            } else if (isGrounded && x == 0) {
                Idle();
            } else if (isGrounded) {
                WalkMovement(x);
            } else
                AirMovement(x);

            if (nextState != lastState)
                TriggerAnimation();

        }

        void Idle() {
            bod.velocity = new Vector2(0, bod.velocity.y);
            nextState = AnimState.Idle;
        }

        void WalkMovement(float x) {
            bod.velocity = new Vector2(WalkForce * x, bod.velocity.y);
            nextState = AnimState.Walk;
        }

        void JumpMovement(float x) {
            transform.eulerAngles = Vector3.zero;
            bod.AddForce(new Vector2(JumpForceVector.x * x, JumpForceVector.y), ForceMode2D.Impulse);
            nextState = AnimState.Jump;
        }

        void AirMovement(float x) {
            bod.velocity = new Vector2(x * AirVelocity, bod.velocity.y);
            transform.eulerAngles = Vector3.zero;
            nextState = AnimState.Airborne;
        }

        void CheckGrounded() {
            RaycastHit2D hitL = Physics2D.Raycast(new Vector2(leftFoot.position.x, leftFoot.position.y), Vector2.down,
                                                  groundedBuffer);
            RaycastHit2D hitR = Physics2D.Raycast(new Vector2(rightFoot.position.x, rightFoot.position.y), Vector2.down,
                                                   groundedBuffer);
            //rotate to match ground
            if (rotateToSlope && (lastState == AnimState.Walk || lastState == AnimState.Idle)) {
                Vector3 l = leftFoot.position;
                Vector3 r = rightFoot.position;
                l.y -= hitL.distance;
                r.y -= hitR.distance;
                Vector3 v = r - l;
                if (v.x == 0)
                    v.x = 0.0001f;
                float theta = Mathf.Atan(v.y / v.x);

                transform.eulerAngles = new Vector3(0, 0, theta * Mathf.Rad2Deg);
            }
            if ((hitL.collider != null) || (hitR.collider != null))
                isGrounded = true;
            else
                isGrounded = false;

        }

        void DirectionFacer(float x) {
            if ((x > 0 && transform.localScale.x < 0) || (x < 0 && this.transform.localScale.x > 0)) {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                this.transform.localScale = scale;
            }
        }

        float CurrentlyFacing() {
            return (transform.localScale.x / Mathf.Abs(this.transform.localScale.x));
        }

        void TriggerAnimation() {
            if (nextState == AnimState.Idle)
                anim.SetInteger("State", 0);
            else if (nextState == AnimState.Walk)
                anim.SetInteger("State", 1);
            else if (nextState == AnimState.Jump)
                anim.SetInteger("State", 2);
            else if (nextState == AnimState.Airborne)
                anim.SetInteger("State", 3);
            lastState = nextState;
        }

        public void ResendAnimCommand() {
            TriggerAnimation();
        }

        void OnDisable() {
            anim.SetInteger("State", 0);
            lastState = AnimState.Idle;
        }
    }
}
