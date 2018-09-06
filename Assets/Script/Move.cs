using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public GameObject character;
	public float speed = 2.0f;
	Vector3 target = new Vector3(0,70,0);
	void Start () {
		transform.position = new Vector3 (100, 70, 0);
		transform.LookAt (target);
	}

	void Update () {
		/*
		if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += character.transform.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.LeftArrow)){
			transform.position += character.transform.right * -speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.position += character.transform.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			transform.position += character.transform.forward* -speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.W)){
			transform.Rotate (new Vector3 (36, 0, 0) * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S)){
			transform.Rotate (new Vector3 (-36, 0, 0) * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A)){
			
			transform.Rotate (new Vector3 (0, -36, 0) * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D)){
			
			transform.Rotate (new Vector3 (0, 36, 0) * speed * Time.deltaTime);
		}*/
	}
	public void resume(){
	
		transform.position = new Vector3 (100, 70, 0);
		transform.LookAt (target);
	
	}
}
