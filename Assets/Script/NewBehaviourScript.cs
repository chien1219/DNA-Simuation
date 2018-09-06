using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public GameObject carbon;
	public GameObject bonds;
	public GameObject hydrogen;
	public float bond_distance;
	public Vector3 pos;
	GameObject B, C, D;
	GameObject HB, HC, HD;

	// Use this for initialization
	void Start () {
		Instantiate (carbon, new Vector3 (0, 0, 0),	Quaternion.identity);
		pos += new Vector3 (0, bond_distance, 0);
		Instantiate (bonds, pos, Quaternion.identity);
		B = (GameObject)Instantiate (bonds, new Vector3 (0, 0, 0), Quaternion.identity);
		B.transform.Rotate (0, 0, 120);
		B.transform.localPosition += B.transform.up * bond_distance;
		HB = (GameObject)Instantiate (hydrogen, B.transform.position, B.transform.rotation);
		HB.transform.position += HB.transform.up * bond_distance;

		C = (GameObject)Instantiate (bonds, new Vector3 (0, 0, 0), Quaternion.identity);
		C.transform.Rotate (0, 60, -120);
		C.transform.localPosition += C.transform.up * bond_distance;
		HC = (GameObject)Instantiate (hydrogen, C.transform.position, C.transform.rotation);
		HC.transform.position += HC.transform.up * bond_distance;

		D = (GameObject)Instantiate (bonds, new Vector3 (0, 0, 0), Quaternion.identity);
		D.transform.Rotate (0, -60, -120);
		D.transform.localPosition += D.transform.up * bond_distance;
		HD = (GameObject)Instantiate (hydrogen, D.transform.position, D.transform.rotation);
		HD.transform.position += HD.transform.up * bond_distance;

		pos += new Vector3 (0, bond_distance, 0);

		Instantiate (carbon, pos, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
