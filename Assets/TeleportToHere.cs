using UnityEngine;
using System.Collections;

public class TeleportToHere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown()
	{
		//Debug.Log(Time.time);
		TeleportPlayer.This().TeleportTo(this.gameObject);
	}
}