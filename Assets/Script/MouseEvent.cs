using UnityEngine;
using System.Collections;

public class MouseEvent : MonoBehaviour {

	bool onMouse = false;
	Vector3 scale;
	void Start(){
		scale = transform.localScale;
	}

	void OnMouseEnter(){
		onMouse = true;
		transform.localScale = scale * 2;

	}

	void OnMouseExit(){
		onMouse = false;
		transform.localScale = scale;
	}

	void Update(){
		if (onMouse) {
			//transform.localScale = scale * 2;
		} else {
			//transform.localScale = scale;
		}
	}

	void OnGUI(){
		if (onMouse) {
			//GUI.Label(new Rect(transform.position.x, transform.position.y, 100, 20), "Hello World!");
			var position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			GUI.Label(new Rect(position.x, Screen.height - position.y, 100, 20), name);
		}
	}

}