using UnityEngine;
using System.Collections;

public class Dragobj : MonoBehaviour {
	Vector3 dist;
	float posX;
	float posY;


	void OnMouseDown(){
		dist = Camera.main.WorldToScreenPoint(transform.position);
		posX = Input.mousePosition.x - dist.x;
		posY = Input.mousePosition.y - dist.y;
	}

	void OnMouseDrag(){
		Vector3 curPos = 
			new Vector3(Input.mousePosition.x - posX, 
				Input.mousePosition.y - posY, dist.z);  

		Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
		Vector3 f = worldPos - gameObject.transform.position;
		//f = f / Mathf.Sqrt (f.z*f.z+f.y*f.y+f.x*f.x);
		f *= 300;
		Rigidbody rb = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
		if(rb != null)
		rb.AddForce (f);
	}
}