using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DNA;

namespace DNA{
	public class Helicase : MonoBehaviour {

		private int stage;
		public GameObject helicase, donut, polymerase;
		public GameObject DNA;
		public GameObject dna_string1;
		public GameObject dna_string2;
		public GameObject cpy_dna_str1;
		public GameObject cpy_dna_str2;
		public GameObject A, T, C, G, AT, TA, CG, GC;
		private float move,move1,move2;

		private int divide_t;
		private int divide_cnt;
		private int break_cnt;
		private int merge_cnt;
		private int rebuild_cnt;
		private bool rebuild_done;
		private bool secondDNA;
		private GameObject[] dup_dna;
		private GameObject[] tmp_d;
		// Use this for initialization
		void Start () {

			//initialize variables
			divide_t = 0;
			move = 0;
			move1 = 84.0f;
			move2 = 0;
			stage = 0;
			divide_cnt = 0;
			break_cnt = 0;
			rebuild_cnt = 0;
			rebuild_done = false;
			secondDNA = false;
			DNA.transform.rotation = Quaternion.Euler (new Vector3 (0, -40, 0));
			generate_helicase ();
			dup_dna = new GameObject[30];
			tmp_d = new GameObject[30];
		}

		// Update is called once per frame
		void Update () {

			if (divide_cnt < 30) {
				//now_angle += new Vector3 (0, -36, 0);
				if (divide_t == 0) {
					//Step 1. delete the joint
					DeleteJoint ();
					//Step 2. rotate the layer 
					GameObject parent_1 = dna_string1.transform.GetChild (divide_cnt).gameObject;
					GameObject parent_2 = dna_string2.transform.GetChild (divide_cnt).gameObject;
					if (parent_1.name == "BigT" || parent_1.name == "BigC") {
						parent_1.transform.RotateAround (new Vector3 (0.0f, 2.8f, 0.0f) * divide_cnt, new Vector3 (0, 1, 0), 36 * divide_cnt + 18);
						parent_2.transform.RotateAround (new Vector3 (0.0f, 2.8f, 0.0f) * divide_cnt, new Vector3 (0, 1, 0), 36 * divide_cnt + 18);
					} else {
						parent_1.transform.RotateAround (new Vector3 (0.0f, 2.8f, 0.0f) * divide_cnt, new Vector3 (0, 1, 0), 36 * divide_cnt);
						parent_2.transform.RotateAround (new Vector3 (0.0f, 2.8f, 0.0f) * divide_cnt, new Vector3 (0, 1, 0), 36 * divide_cnt);
					}

					divide_t++;
					//Step 3. rebuild the joint
					RebuildJoint ();
				} else if (divide_t == 3) {
					divide_cnt++;
					divide_t = 0;
				} else {
					divide_t++;
				}
				//force ();


			} else if (stage == 0)
				helicase_work ();
			else if (stage == 1) {
				helicase.SetActive (false);
				polymerase.SetActive (true);
				startduplicate ();
			}
			else if (stage == 2) {
				polymerase.SetActive(false);
				//polymerase.transform.position = new Vector3 (-2, 84.0f, -2);
				rebuild_back ();
				//deleteTmp ();
			}else if(stage == 3){
				donut.SetActive(false);
				if (secondDNA)
					stage = 4;
				Transform reDNA = GetComponent<Transform>();
				reDNA.Translate (new Vector3 (-8, 0, 6));
				if (reDNA.position.x < -80.0f && reDNA.position.z > 60.0f && !secondDNA) {
					stage = 1;
					move1 = 84.0f;
					move2 = 0;
					dna_string1.SetActive (true);
					secondDNA = true;
				}
			} 
		}


		void generate_helicase(){

			helicase.SetActive (false);
			helicase.transform.position = new Vector3(0,0,0);

		}
		void helicase_work(){
			
			helicase.SetActive (true);
			if (move < 2.8 * 30)
				move += Time.deltaTime * 5;
			helicase.transform.position = new Vector3 (0, move, 0);

			break_cnt = (int)(move / 2.8);
			if (break_cnt < 30)
				force ();
			else {
				if (move <= 110) {
					helicase.SetActive (false);
					move += Time.deltaTime * 5;
				} else {
					dna_string1.SetActive (false);
					polymerase.SetActive (true);
					stage = 1;
					move = 0;
				}
			}

		}
		void startduplicate()
		{

			if (move1 >= 0)
				move1 -= Time.deltaTime * 4;
			else {
				if (move1 > -20) {
					move1 -= Time.deltaTime * 5;
					polymerase.SetActive (false);
				} else {
					stage = 2;
					move1 = 0;
					donut.SetActive (true);
				}
			}
		
			polymerase.transform.position = new Vector3 (-2, move1, -2);

			merge_cnt = (int)(move1 / 2.8);
			Vector3 dis;
			if(!secondDNA){
				dis = new Vector3 (3.5f, 0, -6);
			}
			else dis = new Vector3 (-3.5f, 0, 6);
			if (dup_dna [merge_cnt] == null) {
				//GameObject c1 = dna_string1.transform.GetChild (merge_cnt).gameObject;
				GameObject c2 = (secondDNA) ? dna_string1.transform.GetChild (merge_cnt).gameObject : dna_string2.transform.GetChild (merge_cnt).gameObject;
				
				if (c2.name == "BigA") {
					dup_dna [merge_cnt] = (GameObject)Instantiate (T);
					dup_dna [merge_cnt].transform.position = c2.transform.position + dis;
					dup_dna [merge_cnt].transform.rotation = Quaternion.Euler (0, 32, 0);
					
				} else if (c2.name == "BigT") {
					dup_dna [merge_cnt] = (GameObject)Instantiate (A);
					dup_dna [merge_cnt].transform.position = c2.transform.position + dis;
					dup_dna [merge_cnt].transform.rotation = Quaternion.Euler (0, 122, -90);
	
				} else if (c2.name == "BigC") {
					dup_dna [merge_cnt] = (GameObject)Instantiate (G);
					dup_dna [merge_cnt].transform.position = c2.transform.position + dis;
					dup_dna [merge_cnt].transform.rotation = Quaternion.Euler (-180, -58, 90);
				} else if (c2.name == "BigG") {
					dup_dna [merge_cnt] = (GameObject)Instantiate (C);
					dup_dna [merge_cnt].transform.position = c2.transform.position + dis;
					dup_dna [merge_cnt].transform.rotation = Quaternion.Euler (0, -58, 90);
				}
				if(secondDNA) dup_dna [merge_cnt].transform.Rotate (180,0,180);
				if(!secondDNA)
					dup_dna [merge_cnt].transform.SetParent (cpy_dna_str1.transform);
				else
					dup_dna [merge_cnt].transform.SetParent (dna_string2.transform);
			}


		}

		void rebuild_back()
		{
			
			donut.transform.position = new Vector3(0,0, -5);

			if (move2 <= 84)
				move2 += Time.deltaTime * 5;
			else
				donut.SetActive(false);
			donut.transform.position = new Vector3(0, move2, -5);

			rebuild_cnt = (int)(move2 / 2.8);

			GameObject str1 = (!secondDNA) ? dna_string2.transform.GetChild(0).gameObject : dna_string1.transform.GetChild(0).gameObject;
			int num = cpy_dna_str1.transform.GetChildCount();
			GameObject str2 = (!secondDNA) ? cpy_dna_str1.transform.GetChild(29 - rebuild_cnt).gameObject : dna_string2.transform.GetChild(29 - rebuild_cnt).gameObject;
			GameObject new_layer;
			string name = str1.name;

			//str1.transform.SetParent(tmp_d[rebuild_cnt].transform);
			//str2.transform.SetParent(tmp_d[rebuild_cnt].transform);

			Debug.Log("rebuild_cnt: " + rebuild_cnt);
			if (tmp_d[rebuild_cnt] == null)
			{
				if (str1 != null)
				{
					Destroy(str1);
				}
				if (str2 != null)
				{
					Destroy(str2);
				}

				if (name == "BigA")
				{
					tmp_d[rebuild_cnt] = (GameObject)Instantiate(AT, new Vector3(0, 2.8f * rebuild_cnt, 0), Quaternion.Euler(0, -36 * rebuild_cnt, 0));

					tmp_d[rebuild_cnt].transform.Rotate(0, 198, 180);
				}
				else if (name == "BigT")
				{
					tmp_d[rebuild_cnt] = (GameObject)Instantiate(TA, new Vector3(0, 2.8f * rebuild_cnt, 0), Quaternion.Euler(0, -36 * rebuild_cnt, 0));

				}
				else if (name == "BigG")
				{
					tmp_d[rebuild_cnt] = (GameObject)Instantiate(GC, new Vector3(0, 2.8f * rebuild_cnt,0) , Quaternion.Euler(0, -36 * rebuild_cnt, 0));

					tmp_d[rebuild_cnt].transform.Rotate(0, 198, 180);
				}
				else {
					tmp_d[rebuild_cnt] = (GameObject)Instantiate(CG, new Vector3(0, 2.8f * rebuild_cnt, 0), Quaternion.Euler(0, -36 * rebuild_cnt, 0));

				}

				GameObject c1 = tmp_d [rebuild_cnt].transform.GetChild (0).gameObject;
				GameObject c2 = tmp_d [rebuild_cnt].transform.GetChild (1).gameObject;
				if (!secondDNA) {
					c1.transform.SetParent (cpy_dna_str1.transform);
					c2.transform.SetParent (cpy_dna_str2.transform);
				} else {
					c1.transform.SetParent (dna_string1.transform);
					c2.transform.SetParent (dna_string2.transform);
				}

			}
			if (rebuild_cnt == 29) {
				deleteTmp ();
				stage = 3;
			}
			//    str1 = new_layer.transform.GetChild(0).gameObject;
			//    str2 = new_layer.transform.GetChild(1).gameObject;
			//    str1.transform.SetParent(dna_string2.transform);
			//    str2.transform.SetParent(cpy_dna_str1.transform);


		}
		void deleteTmp(){
			//if (!rebuild_done)
			//	return;
			for (int i = 0; i < rebuild_cnt+1; i++) {
				Destroy (tmp_d [i]);
			}
		
		}
		void force(){
			for (int i = (((break_cnt -6) >= 0)?break_cnt -6 : 0) ; i < break_cnt; i++) {
				GameObject p1 = dna_string1.transform.GetChild (i).gameObject;
				GameObject p2 = dna_string2.transform.GetChild (i).gameObject;

				GameObject phos1 = p1.transform.Find ("Phosphate_sugar/Phosphate/Phosphorus").gameObject;
				GameObject phos2 = p2.transform.Find ("Phosphate_sugar/Phosphate/Phosphorus").gameObject;
				Vector3 move1 = phos1.transform.position - (new Vector3 (0.0f, 2.8f, 0.0f)) * i;
				Vector3 move2 = phos2.transform.position - (new Vector3 (0.0f, 2.8f, 0.0f)) * i;

				//p1.GetComponent<Rigidbody> ().AddForce (move1 *100);
				//p2.GetComponent<Rigidbody> ().AddForce (move2 *100);
				p1.transform.position += Time.deltaTime * move1 *(break_cnt -i) /50;
				p2.transform.position += Time.deltaTime * move2 *(break_cnt -i) /50;
			}
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


	}
}

