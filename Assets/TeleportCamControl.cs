using UnityEngine;
using System.Collections;

public class TeleportCamControl : MonoBehaviour {

	private static TeleportCamControl obj;
	public static TeleportCamControl This() { return obj; }
	public Camera cam;

	void Start () {
		obj = this;
		cam = this.GetComponent<Camera>();
		TurnOffCam();
	}

	public void TurnOnCam()
	{
		this.GetComponent<Camera>().enabled = true;
	}

	public void TurnOffCam()
	{
		cam.enabled = false;
	}
}