using UnityEngine;
using System.Collections;

public class LightenOnHover : MonoBehaviour {
	private Color startColor;
	private float lightenOffset;
	
	void Start () {
		startColor = this.GetComponent<Renderer>().material.color;
		lightenOffset = 1.3f;
	}
	

	void OnMouseEnter () {
		this.GetComponent<Renderer>().material.color = new Color(startColor.r * lightenOffset, startColor.g * lightenOffset, startColor.b * lightenOffset);
	}

	void OnMouseExit() {
		this.GetComponent<Renderer>().material.color = startColor;
	}
}