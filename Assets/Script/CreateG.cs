using UnityEngine;
using System.Collections;

public class CreateG : MonoBehaviour {
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

	GameObject[] p;
	GameObject[] pb;
	GameObject[] po;
	GameObject[] pob;


	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (0, 0, 0);

		r = (GameObject)Instantiate (center, pos, Quaternion.identity);
		r.transform.Rotate (90, 0, 0);
		a = new GameObject[6];
		b = new GameObject[6];
		o = new GameObject[6];
		ob = new GameObject[6];
		p = new GameObject[3];
		pb = new GameObject[4];
		po = new GameObject[3];
		pob = new GameObject[3];

		Instantiate (center, pos, Quaternion.identity);
		pos += new Vector3 (0, 0, bond_distance * -2);
		a[0] = (GameObject)Instantiate(nitrogen, new Vector3(0, 0, bond_distance * (-2)), Quaternion.identity);
		a [0].transform.Rotate (-30,0,0);
		for (int i = 0; i < 6; i++) {
			b[i] = (GameObject)Instantiate(bonds, a[i].transform.position, a[i].transform.rotation);
			b[i].transform.Rotate (60,0,0);
			b[i].transform.position += b[i].transform.up * bond_distance ;

			switch (i) {

			case 0:
				a[i+1] = (GameObject)Instantiate(carbon, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 1:
				a[i+1] = (GameObject)Instantiate(nitrogen, b[i].transform.position, b[i].transform.rotation);
				a[i+1].transform.position += a[i+1].transform.up * bond_distance ;
				break;
			case 2:
				a[i+1] = (GameObject)Instantiate(carbon, b[i].transform.position, b[i].transform.rotation);
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
				flag = true;
				break;
			case 1:
				o[i] = (GameObject)Instantiate(hydrogen, a[i].transform.position, a[i].transform.rotation);
				break;
			case 2:
				flag = true;
				break;
			case 3:
				flag = true;
				break;
			case 4:
				flag = true;
				break;
			case 5:
				o[i] = (GameObject)Instantiate(nitrogen, a[i].transform.position, a[i].transform.rotation);
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
		hb1 = (GameObject)Instantiate(bonds, o[5].transform.position, o[5].transform.rotation);
		hb1.transform.Rotate (60,0,0);
		hb1.transform.position += hb1.transform.up * bond_distance ;
		h1 = (GameObject)Instantiate(hydrogen, hb1.transform.position, hb1.transform.rotation);
		h1.transform.position += h1.transform.up * bond_distance ;

		hb2 = (GameObject)Instantiate(bonds, o[5].transform.position, o[5].transform.rotation);
		hb2.transform.Rotate (-60,0,0);
		hb2.transform.position += hb2.transform.up * bond_distance ;
		h2 = (GameObject)Instantiate(hydrogen, hb2.transform.position, hb2.transform.rotation);
		h2.transform.position += h2.transform.up * bond_distance ;

		//pentagon
		pb[0] = (GameObject)Instantiate(bonds, a[3].transform.position, a[3].transform.rotation);
		pb[0].transform.Rotate (-54,0,0);
		pb[0].transform.position += pb[0].transform.up * bond_distance ;
		for (int i = 0; i < 3; i++) {
			switch (i) {

			case 0:
				p[i] = (GameObject)Instantiate(nitrogen, pb[i].transform.position, pb[i].transform.rotation);
				p[i].transform.position += p[i].transform.up * bond_distance ;
				break;
			case 1:
				p[i] = (GameObject)Instantiate(carbon, pb[i].transform.position, pb[i].transform.rotation);
				p[i].transform.position += p[i].transform.up * bond_distance ;
				break;
			case 2:
				p[i] = (GameObject)Instantiate(nitrogen, pb[i].transform.position, pb[i].transform.rotation);
				p[i].transform.position += p[i].transform.up * bond_distance ;
				break;
			}

			pb[i+1] = (GameObject)Instantiate(bonds, p[i].transform.position, p[i].transform.rotation);
			pb[i+1].transform.Rotate (72,0,0);
			pb[i+1].transform.position += pb[i+1].transform.up * bond_distance ;

		}

		for (int i = 0; i < 3; i++) {
			bool flag = false;
			switch (i) {

			case 0:
				po[i] = (GameObject)Instantiate(carbon, p[i].transform.position, p[i].transform.rotation);
				break;
			case 1:
				po[i] = (GameObject)Instantiate(hydrogen, p[i].transform.position, p[i].transform.rotation);
				break;
			case 2:
				flag = true;
				break;
			}

			if (flag==false) {
				pob[i] = (GameObject)Instantiate(bonds, p[i].transform.position, p[i].transform.rotation);
				pob[i].transform.Rotate (-54,0,0);
				pob[i].transform.position += pob[i].transform.up * bond_distance ;
				po [i].transform.rotation = pob [i].transform.rotation;
				po [i].transform.position = pob [i].transform.position;
				po [i].transform.position += po[i].transform.up * bond_distance ;
			}
		}


	}

	// Update is calleonce per frame
	void Update () {

	}
}
