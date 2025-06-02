using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void OnGUI () {
		if (GUI.Button(new Rect(10, 10, 200, 100), "Send fake data")) {
			List<TigerData> tList = new List<TigerData>();
			
			TigerData t1 = new TigerData();
			t1.setData("Age", "10");
			t1.setData("NoseBlack", "0.15554");
			t1.setData("AValue", "42");
			tList.Add(t1);

			TigerData t2 = new TigerData();
			t2.setData("Age", "2");
			t2.setData("NoseBlack", "0.13153");
			t2.setData("Testing", "whoa");
			tList.Add(t2);
			
			ServiceConnection conn = GetComponent<ServiceConnection>();
			conn.UploadData("matt", "Tie", 12, 2, 10, 2, 2, 8000, tList);
		}
	}
	
}
