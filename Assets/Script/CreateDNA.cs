using UnityEngine;
using System.Collections;
//[assembly: InternalsVisibleTo("RotateDNA")]

namespace DNA{

	public class CreateDNA : MonoBehaviour {

		static int mode = 0;
		public GameObject AT, CG, TA, GC;
		public float bond_distance;
		public GameObject dir;
		public GameObject DNA;
		private int divide;
		private int divide_cnt;
		private int timer;
		//private Vector3 now_angle;

		// Use this for initialization
		public GameObject dna_string1;
		public GameObject dna_string2;
		private GameObject[] DNA_Set;

		static class Constants
		{
			public const int DNA_layers = 30; // km per sec.

		}

		public void Start () {
			divide_cnt = 0;
			timer = 0;
			//now_angle = new Vector3 (0, 0, 0);
			DNA_Set = new GameObject[Constants.DNA_layers];

			Random.seed = System.Guid.NewGuid().GetHashCode();
			//Random rnd = new Random ();
			for (int i = 0; i < Constants.DNA_layers; i++) {
				int rand = Random.Range(0,100);
				dir.transform.Rotate (0,-36,0);
				if (rand % 4 == 0) {

					DNA_Set [i] = (GameObject)Instantiate (AT, dir.transform.position + new Vector3 (0.0f, 2.8f, 0.0f) * i, dir.transform.rotation);
					DNA_Set [i].transform.Rotate (0, 198, 180);
					DNA_Set [i].name = i.ToString ();


				} else if (rand % 4 == 1) {

					DNA_Set [i] = (GameObject)Instantiate (CG, dir.transform.position + new Vector3 (0.0f, 2.8f, 0.0f) * i, dir.transform.rotation);
					DNA_Set [i].name = i.ToString ();

				} else if (rand % 4 == 2) {

					DNA_Set [i] = (GameObject)Instantiate (TA, dir.transform.position + new Vector3 (0.0f, 2.8f, 0.0f) * i, dir.transform.rotation);
					DNA_Set [i].name = i.ToString ();

				} else {

					DNA_Set [i] = (GameObject)Instantiate (GC, dir.transform.position + new Vector3 (0.0f, 2.8f, 0.0f) * i, dir.transform.rotation);
					DNA_Set [i].transform.Rotate (0, 198, 180);
					DNA_Set [i].name = i.ToString ();

				}
				AddLayerJoint(i);
				Destroy (DNA_Set [i]);
			}
			AddToString ();

		}

		// Update is called once per frame
		public void Update () {
			timer++;
			if (DNA.active == false) {
				DNA.SetActive (true);
			}
			if (Input.GetKeyDown (KeyCode.R)) {
				//Rebuild DNA
				rebuildDNA();
			}
			if (mode == 0 && timer ==1) {
				timer = 0;
				//DNA.transform.RotateAround(new Vector3(0,0,0),new Vector3(0,1,0),3);
				DNA.transform.Rotate(Vector3.up*10*Time.deltaTime);
			} 
			else if (mode == 1) {//按F解旋
				if (Input.GetKeyDown (KeyCode.F) && divide == 0) {
					divide = 1;
				}

			}
		}

		void FixedUpdate(){
			if (divide == 1) {
				if (divide_cnt < 30) {
					//now_angle += new Vector3 (0, -36, 0);

					//Step 1. delete the joint
					DeleteJoint ();
					//Step 2. rotate the layer 
					GameObject parent_1 = dna_string1.transform.GetChild (divide_cnt).gameObject;
					GameObject parent_2 = dna_string2.transform.GetChild (divide_cnt).gameObject;
					parent_1.transform.RotateAround (new Vector3 (0.0f, 2.8f, 0.0f)*divide_cnt, new Vector3 (0, 1, 0), 36 * divide_cnt);
					parent_2.transform.RotateAround (new Vector3 (0.0f, 2.8f, 0.0f)*divide_cnt, new Vector3 (0, 1, 0), 36 * divide_cnt);

					//Step 3. rebuild the joint
					RebuildJoint ();

					force ();

					divide = 0;
					divide_cnt++;
				}
			}

		}

		public void ondelete(){

			DNA.SetActive(false);
		}

		public void chmode(int a){

			mode = a;
		}

		public void rebuildDNA(){
			for(int i = 0;i<Constants.DNA_layers;i++){
				GameObject c1 = dna_string1.transform.GetChild(i).gameObject;
				GameObject c2 = dna_string2.transform.GetChild(i).gameObject;
				Destroy(c1);
				Destroy (c2);
				if (DNA_Set [i] != null) {
					Destroy (DNA_Set [i]);
				}
			}
			Start ();
		}

		void DeleteJoint(){
			//only delete the joint of the layer which is going to be break
			for (int i = divide_cnt - 1; i <= divide_cnt; i++) {
				if (i < 0)
					continue;
				if (i + 1 >= 30)
					break;
				GameObject p1 = dna_string1.transform.GetChild (i + 1).gameObject;
				GameObject p2 = dna_string2.transform.GetChild (i + 1).gameObject;

				GameObject up_1;
				GameObject up_2;


				if (p1.name == "BigA") {

					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/AT_A_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/AT_T_Oxygen").gameObject;

				} else if (p1.name == "BigT") {

					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/TA_T_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/TA_A_Oxygen").gameObject;

				} else if (p1.name == "BigC") {

					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/CG_C_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/CG_G_Oxygen").gameObject;

				} else {
					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/GC_G_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/GC_C_Oxygen").gameObject;

				}


				GameObject c1 = dna_string1.transform.GetChild (i).gameObject;
				GameObject c2 = dna_string2.transform.GetChild (i).gameObject;

				GameObject down_1;
				GameObject down_2;


				if (c1.name == "BigA") {
					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/AT_A_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/AT_T_Carbon").gameObject;

				} else if (c1.name == "BigT") {

					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/TA_T_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/TA_A_Carbon").gameObject;

				} else if (c1.name == "BigC") {

					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/CG_C_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/CG_G_Carbon").gameObject;

				} else {

					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/GC_G_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/GC_C_Carbon").gameObject;

				}

				if (up_1.GetComponent<HingeJoint> () != null) {
					up_1.GetComponent<HingeJoint> ().connectedBody = null;
				} else {
					up_1.AddComponent<HingeJoint> ();
					up_1.GetComponent<HingeJoint> ().anchor = up_1.transform.position;
					up_1.GetComponent<HingeJoint> ().breakForce = 100000;
					up_1.GetComponent<HingeJoint> ().breakTorque = 100000;
				}
				if (up_2.GetComponent<HingeJoint> () != null) {
					up_2.GetComponent<HingeJoint> ().connectedBody = null;
				} else {
					up_2.AddComponent<HingeJoint> ();
					up_2.GetComponent<HingeJoint> ().anchor = up_2.transform.position;
					up_2.GetComponent<HingeJoint> ().breakForce = 100000;
					up_2.GetComponent<HingeJoint> ().breakTorque = 100000;
				}

				if (down_1.GetComponent<HingeJoint> () != null) {
					down_1.GetComponent<HingeJoint> ().connectedBody = null;
				} else {
					down_1.AddComponent<HingeJoint> ();
					down_1.GetComponent<HingeJoint> ().anchor = down_1.transform.position;
					down_1.GetComponent<HingeJoint> ().breakForce = 100000;
					down_1.GetComponent<HingeJoint> ().breakTorque = 100000;
				}
				if (down_2.GetComponent<HingeJoint> () != null) {
					down_2.GetComponent<HingeJoint> ().connectedBody = null;
				} else {
					down_2.AddComponent<HingeJoint> ();
					down_2.GetComponent<HingeJoint> ().anchor = down_2.transform.position;
					down_2.GetComponent<HingeJoint> ().breakForce = 100000;
					down_2.GetComponent<HingeJoint> ().breakTorque = 100000;
				}

			}



		}

		void RebuildJoint(){
			for (int j = divide_cnt-1; j <= divide_cnt; j++) {
				if (j < 0)
					continue;
				if (j + 1 >= 30)
					break;
				GameObject p1 = dna_string1.transform.GetChild (j + 1).gameObject;
				GameObject p2 = dna_string2.transform.GetChild (j + 1).gameObject;

				GameObject up_1;
				GameObject up_2;


				if (p1.name == "BigA") {

					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/AT_A_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/AT_T_Oxygen").gameObject;

				} else if (p1.name == "BigT") {

					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/TA_T_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/TA_A_Oxygen").gameObject;

				} else if (p1.name == "BigC") {

					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/CG_C_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/CG_G_Oxygen").gameObject;

				} else {
					up_1 = p1.transform.Find ("Phosphate_sugar/Phosphate/GC_G_Oxygen").gameObject;
					up_2 = p2.transform.Find ("Phosphate_sugar/Phosphate/GC_C_Oxygen").gameObject;

				}


				GameObject c1 = dna_string1.transform.GetChild (j).gameObject;
				GameObject c2 = dna_string2.transform.GetChild (j).gameObject;

				GameObject down_1;
				GameObject down_2;


				if (c1.name == "BigA") {
					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/AT_A_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/AT_T_Carbon").gameObject;

				} else if (c1.name == "BigT") {

					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/TA_T_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/TA_A_Carbon").gameObject;

				} else if (c1.name == "BigC") {

					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/CG_C_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/CG_G_Carbon").gameObject;

				} else {

					down_1 = c1.transform.Find ("Phosphate_sugar/Ribose/GC_G_Carbon").gameObject;
					down_2 = c2.transform.Find ("Phosphate_sugar/Ribose/GC_C_Carbon").gameObject;

				}
				//接回去
				up_1.GetComponent<HingeJoint>().connectedBody = down_1.GetComponent<Rigidbody>();
				up_2.GetComponent<HingeJoint>().connectedBody = down_2.GetComponent<Rigidbody>();

				down_1.GetComponent<HingeJoint>().connectedBody = up_1.GetComponent<Rigidbody>();
				down_2.GetComponent<HingeJoint>().connectedBody = up_2.GetComponent<Rigidbody>();

			}
		}

		void force(){
			for (int i = 0; i < divide_cnt; i++) {
				GameObject p1 = dna_string1.transform.GetChild (i).gameObject;
				GameObject p2 = dna_string2.transform.GetChild (i).gameObject;

				GameObject c1;
				GameObject c2;

				c1 = p1.transform.Find ("Phosphate_sugar/Phosphate/Phosphorus").gameObject;
				c2 = p2.transform.Find ("Phosphate_sugar/Phosphate/Phosphorus").gameObject;

				Vector3 move1 = c1.transform.position - (new Vector3(0.0f, 2.8f, 0.0f))*i;
				Vector3 move2 = c2.transform.position - (new Vector3(0.0f, 2.8f, 0.0f))*i;
				move1 += new Vector3 (0, -1, 0);
				move2 += new Vector3 (0, -1, 0);

				p1.GetComponent<Rigidbody> ().AddForce (move1 * (divide_cnt - i) * 30);
				p2.GetComponent<Rigidbody> ().AddForce (move2 * (divide_cnt - i) * 30);
			}
		}

		void AddLayerJoint(int i){
			//Add layer joint
			if(i>0){
				GameObject parent_1 = DNA_Set[i].transform.GetChild (0).gameObject;
				GameObject parent_2 = DNA_Set[i].transform.GetChild (1).gameObject;

				GameObject up_1;
				GameObject up_2;


				if (parent_1.name == "BigA") {

					up_1 = parent_1.transform.Find ("Phosphate_sugar/Phosphate/AT_A_Oxygen").gameObject;
					up_2 = parent_2.transform.Find ("Phosphate_sugar/Phosphate/AT_T_Oxygen").gameObject;

				} else if (parent_1.name == "BigT") {

					up_1 = parent_1.transform.Find ("Phosphate_sugar/Phosphate/TA_T_Oxygen").gameObject;
					up_2 = parent_2.transform.Find ("Phosphate_sugar/Phosphate/TA_A_Oxygen").gameObject;

				} else if (parent_1.name == "BigC") {

					up_1 = parent_1.transform.Find ("Phosphate_sugar/Phosphate/CG_C_Oxygen").gameObject;
					up_2 = parent_2.transform.Find ("Phosphate_sugar/Phosphate/CG_G_Oxygen").gameObject;

				} else {

					up_1 = parent_1.transform.Find ("Phosphate_sugar/Phosphate/GC_G_Oxygen").gameObject;
					up_2 = parent_2.transform.Find ("Phosphate_sugar/Phosphate/GC_C_Oxygen").gameObject;

				}


				GameObject child_1 = DNA_Set[i-1].transform.GetChild (0).gameObject;
				GameObject child_2 = DNA_Set[i-1].transform.GetChild (1).gameObject;

				GameObject down_1;
				GameObject down_2;



				if (child_1.name == "BigA") {

					down_1 = child_1.transform.Find ("Phosphate_sugar/Ribose/AT_A_Carbon").gameObject;
					down_2 = child_2.transform.Find ("Phosphate_sugar/Ribose/AT_T_Carbon").gameObject;

				} else if (child_1.name == "BigT") {

					down_1 = child_1.transform.Find ("Phosphate_sugar/Ribose/TA_T_Carbon").gameObject;
					down_2 = child_2.transform.Find ("Phosphate_sugar/Ribose/TA_A_Carbon").gameObject;

				} else if (child_1.name == "BigC") {

					down_1 = child_1.transform.Find ("Phosphate_sugar/Ribose/CG_C_Carbon").gameObject;
					down_2 = child_2.transform.Find ("Phosphate_sugar/Ribose/CG_G_Carbon").gameObject;

				} else{

					down_1 = child_1.transform.Find ("Phosphate_sugar/Ribose/GC_G_Carbon").gameObject;
					down_2 = child_2.transform.Find ("Phosphate_sugar/Ribose/GC_C_Carbon").gameObject;

				} 

				//上下都接
				up_1.AddComponent<HingeJoint> ();
				up_1.GetComponent<HingeJoint>().connectedBody = down_1.GetComponent<Rigidbody>();
				up_1.GetComponent<HingeJoint> ().breakForce = 1000000;
				up_1.GetComponent<HingeJoint> ().breakTorque = 1000000;


				up_2.AddComponent<HingeJoint> ();
				up_2.GetComponent<HingeJoint>().connectedBody = down_2.GetComponent<Rigidbody>();
				up_2.GetComponent<HingeJoint> ().breakForce = 1000000;
				up_2.GetComponent<HingeJoint> ().breakTorque = 1000000;


				down_1.AddComponent<HingeJoint> ();
				down_1.GetComponent<HingeJoint>().connectedBody = up_1.GetComponent<Rigidbody>();
				down_1.GetComponent<HingeJoint> ().breakForce = 1000000;
				down_1.GetComponent<HingeJoint> ().breakTorque = 1000000;

				down_2.AddComponent<HingeJoint> ();
				down_2.GetComponent<HingeJoint>().connectedBody = up_2.GetComponent<Rigidbody>();
				down_2.GetComponent<HingeJoint> ().breakForce = 1000000;
				down_2.GetComponent<HingeJoint> ().breakTorque = 1000000;

			}

		}
		void AddToString(){
			for (int i = 0; i < Constants.DNA_layers; i++) {
				GameObject c1 = DNA_Set [i].transform.GetChild (0).gameObject;
				GameObject c2 = DNA_Set [i].transform.GetChild (1).gameObject;

				c1.transform.SetParent (dna_string1.transform);
				c2.transform.SetParent (dna_string2.transform);

			}
		}
	}

}