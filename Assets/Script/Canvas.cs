using UnityEngine;
using System.Collections;

public class Canvas : MonoBehaviour {

	public GameObject canvas;
	bool flag;

	// Use this for initialization
	void Start () {
		flag = true;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.C) && flag == true) {
			canvas.SetActive (false);
			flag = false;
		} else if(Input.GetKey (KeyCode.C) && flag == false) {
			canvas.SetActive (true);
			flag = true;
		}
	}
}
