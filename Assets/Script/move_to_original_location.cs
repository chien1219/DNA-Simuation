using UnityEngine;
using System.Collections;

public class move_to_original_location : MonoBehaviour {

	// Use this for initialization
	Vector3 target = new Vector3(0,50,0);
	void Start () {

		transform.LookAt (target);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
