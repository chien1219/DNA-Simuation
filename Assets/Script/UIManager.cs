using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject start;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonClicked(){

		start.SetActive (true);
	
	}
}
