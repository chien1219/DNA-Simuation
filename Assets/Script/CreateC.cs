using UnityEngine;
using System.Collections;

public class CreateC : MonoBehaviour {
	public GameObject carbon;
	public GameObject bonds;
	public GameObject hydrogen;
	public GameObject oxygen;
	public GameObject nitrogen;
	public GameObject center;
	public float bond_distance;
	public  GameObject CH4;

	GameObject r;
	GameObject[] b;
	GameObject[] a;
	GameObject[] o;
	GameObject[] ob;
	GameObject h1, h2, hb1, hb2;


	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (0, 0, 0);

		r = (GameObject)Instantiate (center, pos, Quaternion.identity);
		r.transform.Rotate (90, 0, 0);
		a = new GameObject[6];
		b = new GameObject[6];
		o = new GameObject[6];
		ob = new GameObject[6];
		Instantiate (center, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance * -2);
		a[0] = (GameObject)Instantiate(carbon, new Vector3(0, 0, bond_distance * (-2)), Quaternion.identity);
		a [0].transform.Rotate (-30,0,0);
		for (int i = 0; i < 6; i++) {
			b[i] = (GameObject)Instantiate(bonds, a[i].transform.position, a[i].transform.rotation);
			b[i].transform.Rotate (60,0,0);
			b[i].transform.position += b[i].transform.up * bond_distance ;
	
			switch (i) {

			case 0:
				a[i+1] = (GameObject)Instantiate(nitrogen, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 1:
				a[i+1] = (GameObject)Instantiate(carbon, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 2:
				a[i+1] = (GameObject)Instantiate(nitrogen, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 3:
				a[i+1] = (GameObject)Instantiate(carbon, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 4:
				a[i+1] = (GameObject)Instantiate(carbon, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 5:
				break;
			}
		}
		for (int i = 0; i < 6; i++) {
			bool flag = false;
			switch (i) {

			case 0:
				o[i] = (GameObject)Instantiate(hydrogen, a[i].transform.position, a[i].transform.rotation);
				break;
			case 1:
				o[i] = (GameObject)Instantiate(carbon, a[i].transform.position, a[i].transform.rotation);
				break;
			case 2:
				o[i] = (GameObject)Instantiate(oxygen, a[i].transform.position, a[i].transform.rotation);
				break;
			case 3:
				flag = true;
				break;
			case 4:
				o[i] = (GameObject)Instantiate(nitrogen, a[i].transform.position, a[i].transform.rotation);
				break;
			case 5:
				o[i] = (GameObject)Instantiate(hydrogen, a[i].transform.position, a[i].transform.rotation);
				break;
			}

			if (flag==false) {
				ob[i] = (GameObject)Instantiate(bonds, a[i].transform.position, a[i].transform.rotation);
				ob[i].transform.Rotate (-60,0,0);
				ob[i].transform.position += ob[i].transform.up * bond_distance ;
				o [i].transform.rotation = ob [i].transform.rotation;
				o [i].transform.position = ob [i].transform.position;
				o [i].transform.position += o[i].transform.up * bond_distance ;
			}
		}

		//other element
		hb1 = (GameObject)Instantiate(bonds, o[4].transform.position, o[4].transform.rotation);
		hb1.transform.Rotate (60,0,0);
		hb1.transform.position += hb1.transform.up * bond_distance ;
		h1 = (GameObject)Instantiate(hydrogen, hb1.transform.position, hb1.transform.rotation);
		h1.transform.position += h1.transform.up * bond_distance ;

		hb2 = (GameObject)Instantiate(bonds, o[4].transform.position, o[4].transform.rotation);
		hb2.transform.Rotate (-60,0,0);
		hb2.transform.position += hb2.transform.up * bond_distance ;
		h2 = (GameObject)Instantiate(hydrogen, hb2.transform.position, hb2.transform.rotation);
		h2.transform.position += h2.transform.up * bond_distance ;

	}

	// Update is calleonce per frame
	void Update () {

	}
}
