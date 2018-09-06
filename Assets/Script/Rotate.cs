using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	Vector3 target = new Vector3(0,70,0);
	[SerializeField]private float speed;//相機環繞移動的速度
	private Vector3 cameraPosition;//相機要移動的位置
	private Vector3 originalPosition;
	private float number;
	private float radius;//移動的半徑
	private int state;
	private int move;

	// Use this for initialization
	void Start () {
		gameObject.transform.position = new Vector3 (50, 70, 0);
		cameraPosition = transform.position;
		originalPosition = transform.position;
		speed = 5;
		state = 0;
		move = 1;

		//計算當前攝影機和目標物件的半徑
		radius = Vector3.Distance(target, transform.position);
		transform.LookAt(target);

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.P) && move == 1) {
			move = 0;
		}
		else if (Input.GetKey (KeyCode.P) && move == 0) {
			move = 1;
		}
		if (move == 1) {

			//使用Time.deltaTime，使得移動時更加平滑
			//將速度進行一定比例縮放，方便控制速度(縮放多少都隨意，自己覺得數值修改方便就好)
			number += Time.deltaTime * speed * 0.1f;

			//計算並設定新的x和y軸位置
			//負數是順時針旋轉，正數是逆時針旋轉
			cameraPosition.x = radius * Mathf.Cos (-number);
			cameraPosition.z = radius * Mathf.Sin (-number);
			transform.position = cameraPosition;
			if (state == 0) {
				radius -= 0.2f;
				if (radius <= 20)
					state = 1;
			} else {
				radius += 0.2f;
				if (radius >= Vector3.Distance (target, originalPosition))
					state = 0;
			}
			//使相機永遠面對著目標物件
			transform.LookAt (target);
		}
	}
	public void resume(){
		move = 1;
	}
}