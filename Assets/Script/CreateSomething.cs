using UnityEngine;
using System.Collections;

public class CreateSomething : MonoBehaviour {
	public GameObject carbon;
	public GameObject bonds;
	public GameObject hydrogen;
	public GameObject oxygen;
	public GameObject nitrogen;
	public GameObject center;
	public float bond_distance;

	public  GameObject CH4;

	GameObject r;
	GameObject b1, b2, b3, b4, b5, b6;
	GameObject a1, a2, a3, a4;
	GameObject ch4;
	GameObject o1, o2, o3, o4, o5;

	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (0, 0, 0);

		r = (GameObject)Instantiate (center, pos, Quaternion.identity);
		r.transform.Rotate (90, 0, 0);

		Instantiate (center, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance * 2);
		Instantiate (nitrogen, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance);
		Instantiate (bonds, pos, r.transform.rotation);
		pos += new Vector3 (0, 0, bond_distance);
		Instantiate (hydrogen, pos, Quaternion.identity);

		pos += new Vector3 (0, 0, bond_distance * (-6));
		Instantiate (carbon, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance * (-1));
		Instantiate (bonds, pos, r.transform.rotation);
		pos += new Vector3 (0, 0, bond_distance * (-1));
		Instantiate (hydrogen, pos, Quaternion.identity);

		//left up
		b1 = (GameObject)Instantiate(bonds, new Vector3(0, 0, bond_distance * (-2)), Quaternion.identity);
		b1.transform.Rotate (30,0,0);
		b1.transform.position += b1.transform.up * bond_distance ;

		a1 = (GameObject)Instantiate(nitrogen, b1.transform.position, b1.transform.rotation);
		a1.transform.position += a1.transform.up * bond_distance ;

		b2 = (GameObject)Instantiate(bonds, a1.transform.position + new Vector3(0,0,bond_distance), r.transform.rotation);
		a2 = (GameObject)Instantiate(carbon, b2.transform.position + new Vector3(0,0,bond_distance), r.transform.rotation);

		b3 = (GameObject)Instantiate(bonds, a2.transform.position, r.transform.rotation);
		b3.transform.Rotate (60,0,0);
		b3.transform.position += b3.transform.up * bond_distance ;

		b4 = (GameObject)Instantiate(bonds, new Vector3(0,0,bond_distance*2), r.transform.rotation);
		b4.transform.Rotate (120,0,0);
		b4.transform.position += b4.transform.up * bond_distance ;

		a4 = (GameObject)Instantiate(carbon, b4.transform.position, b4.transform.rotation);
		a4.transform.position += a4.transform.up * bond_distance ;

		b5 = (GameObject)Instantiate(bonds, a4.transform.position, a4.transform.rotation);
		b5.transform.Rotate (60,0,0);
		b5.transform.position += b5.transform.up * bond_distance;

		a3 = (GameObject)Instantiate(carbon, b5.transform.position, b5.transform.rotation);
		a3.transform.position += a3.transform.up * bond_distance ;

		b6 = (GameObject)Instantiate(bonds, a3.transform.position, a3.transform.rotation);
		b6.transform.Rotate (60,0,0);
		b6.transform.position += b6.transform.up * bond_distance;

		ch4 = (GameObject)Instantiate(CH4, a3.transform.position, a3.transform.rotation);
		ch4.transform.Rotate (120,0,0);
		ch4.transform.position += ch4.transform.up * bond_distance * -2;

		o1 = (GameObject)Instantiate(bonds, a4.transform.position, a4.transform.rotation);
		o1.transform.Rotate (-60,0,0);
		o1.transform.position += o1.transform.up * bond_distance;

		o2 = (GameObject)Instantiate(oxygen, o1.transform.position, o1.transform.rotation);
		o2.transform.position += o2.transform.up * bond_distance;

		o3 = (GameObject)Instantiate(bonds, a2.transform.position, a2.transform.rotation);
		o3.transform.Rotate (-60,0,0);
		o3.transform.position += o3.transform.up * bond_distance;

		o4 = (GameObject)Instantiate(oxygen, o3.transform.position, o3.transform.rotation);
		o4.transform.position += o4.transform.up * bond_distance;

		o5 = (GameObject)Instantiate(bonds, a1.transform.position, a1.transform.rotation);
		o5.transform.Rotate (-60,0,0);
		o5.transform.position += o5.transform.up * bond_distance;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
