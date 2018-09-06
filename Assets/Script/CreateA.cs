using UnityEngine;
using System.Collections;

public class CreateA : MonoBehaviour {
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
	GameObject a1, a2, a3, a4, a5, a6;
	GameObject ch4;
	GameObject o1, o2, o3, o4, o5, o6, o7, o8;
	GameObject p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;

	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (0, 0, 0);

		r = (GameObject)Instantiate (center, pos, Quaternion.identity);
		r.transform.Rotate (90, 0, 0);

		Instantiate (center, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance * 2);
		a1 = (GameObject)Instantiate (carbon, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance * (-4));
		a2 = (GameObject)Instantiate (nitrogen, pos, Quaternion.identity);

		//left up
		b1 = (GameObject)Instantiate(bonds, new Vector3(0, 0, bond_distance * (-2)), Quaternion.identity);
		b1.transform.Rotate (30,0,0);
		b1.transform.position += b1.transform.up * bond_distance ;

		a3 = (GameObject)Instantiate(carbon, b1.transform.position, b1.transform.rotation);
		a3.transform.position += a3.transform.up * bond_distance ;

		b2 = (GameObject)Instantiate(bonds, a3.transform.position + new Vector3(0,0,bond_distance), r.transform.rotation);
		a4 = (GameObject)Instantiate(nitrogen, b2.transform.position + new Vector3(0,0,bond_distance), r.transform.rotation);

		b3 = (GameObject)Instantiate(bonds, a4.transform.position, r.transform.rotation);
		b3.transform.Rotate (60,0,0);
		b3.transform.position += b3.transform.up * bond_distance ;

		b4 = (GameObject)Instantiate(bonds, new Vector3(0,0,bond_distance*2), r.transform.rotation);
		b4.transform.Rotate (120,0,0);
		b4.transform.position += b4.transform.up * bond_distance ;

		a6 = (GameObject)Instantiate(carbon, b4.transform.position, b4.transform.rotation);
		a6.transform.position += a6.transform.up * bond_distance ;

		b5 = (GameObject)Instantiate(bonds, a6.transform.position, a6.transform.rotation);
		b5.transform.Rotate (60,0,0);
		b5.transform.position += b5.transform.up * bond_distance;

		a5 = (GameObject)Instantiate(carbon, b5.transform.position, b5.transform.rotation);
		a5.transform.position += a5.transform.up * bond_distance ;

		b6 = (GameObject)Instantiate(bonds, a5.transform.position, a5.transform.rotation);
		b6.transform.Rotate (60,0,0);
		b6.transform.position += b6.transform.up * bond_distance;

		o1 = (GameObject)Instantiate(bonds, a5.transform.position, a5.transform.rotation);
		o1.transform.Rotate (-60,0,0);
		o1.transform.position += o1.transform.up * bond_distance;

		o4 = (GameObject)Instantiate(nitrogen, o1.transform.position, o1.transform.rotation);
		o4.transform.position += o4.transform.up * bond_distance;

		o2 = (GameObject)Instantiate(bonds, a3.transform.position, a3.transform.rotation);
		o2.transform.Rotate (-60,0,0);
		o2.transform.position += o2.transform.up * bond_distance;

		o3 = (GameObject)Instantiate(hydrogen, o2.transform.position, o2.transform.rotation);
		o3.transform.position += o3.transform.up * bond_distance;

		o5 = (GameObject)Instantiate(bonds, o4.transform.position, o4.transform.rotation);
		o5.transform.Rotate (-60,0,0);
		o5.transform.position += o5.transform.up * bond_distance;

		o6 = (GameObject)Instantiate(hydrogen, o5.transform.position, o5.transform.rotation);
		o6.transform.position += o6.transform.up * bond_distance;

		o7 = (GameObject)Instantiate(bonds, o4.transform.position, o4.transform.rotation);
		o7.transform.Rotate (60,0,0);
		o7.transform.position += o7.transform.up * bond_distance;

		o8 = (GameObject)Instantiate(hydrogen, o7.transform.position, o7.transform.rotation);
		o8.transform.position += o8.transform.up * bond_distance;

		p1 = (GameObject)Instantiate(bonds, a1.transform.position, a1.transform.rotation);
		p1.transform.Rotate (102,0,0);
		p1.transform.position += p1.transform.up * bond_distance;

		p2 = (GameObject)Instantiate(nitrogen, p1.transform.position, p1.transform.rotation);
		p2.transform.position += p2.transform.up * bond_distance;

		p3 = (GameObject)Instantiate(bonds, p2.transform.position, p2.transform.rotation);
		p3.transform.Rotate (72,0,0);
		p3.transform.position += p3.transform.up * bond_distance;

		p4 = (GameObject)Instantiate(carbon, p3.transform.position, p3.transform.rotation);
		p4.transform.position += p4.transform.up * bond_distance;

		p5 = (GameObject)Instantiate(bonds, p4.transform.position, p4.transform.rotation);
		p5.transform.Rotate (72,0,0);
		p5.transform.position += p5.transform.up * bond_distance;

		p6 = (GameObject)Instantiate(nitrogen, p5.transform.position, p5.transform.rotation);
		p6.transform.position += p6.transform.up * bond_distance;

		p7 = (GameObject)Instantiate(bonds, p6.transform.position, p6.transform.rotation);
		p7.transform.Rotate (72,0,0);
		p7.transform.position += p7.transform.up * bond_distance;

		p8 = (GameObject)Instantiate(bonds, p4.transform.position, p4.transform.rotation);
		p8.transform.Rotate (-(180-(360-108)/2),0,0);
		p8.transform.position += p8.transform.up * bond_distance;

		p9 = (GameObject)Instantiate(hydrogen, p8.transform.position, p8.transform.rotation);
		p9.transform.position += p9.transform.up * bond_distance;

		p10 = (GameObject)Instantiate(bonds, p2.transform.position, p2.transform.rotation);
		p10.transform.Rotate (-(180-(360-108)/2),0,0);
		p10.transform.position += p10.transform.up * bond_distance;

	}

	// Update is called once per frame
	void Update () {

	}
}
