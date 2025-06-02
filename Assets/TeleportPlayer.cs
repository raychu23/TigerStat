using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportPlayer : MonoBehaviour {

	private static TeleportPlayer obj;
	public GameObject player, playerIndicator;
	public static TeleportPlayer This() { return obj; }
	public List<GameObject> teleportLocations;

	void Start () {
		obj = this;
	}

	void Update () {
//		if (Input.GetKeyDown(KeyCode.Alpha1)) TeleportTo(0);
//		if (Input.GetKeyDown(KeyCode.Alpha2)) TeleportTo(1);
//		if (Input.GetKeyDown(KeyCode.Alpha3)) TeleportTo(2);
//		if (Input.GetKeyDown(KeyCode.Alpha4)) TeleportTo(3);
//		if (Input.GetKeyDown(KeyCode.Alpha5)) TeleportTo(4);
//		if (Input.GetKeyDown(KeyCode.Alpha6)) TeleportTo(5);
//		if (Input.GetKeyDown(KeyCode.Alpha7)) TeleportTo(6);

		playerIndicator.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 400, player.transform.position.z);
		playerIndicator.transform.localRotation = player.transform.localRotation;
	}

    //public void TeleportTo(int pos)
    //{
    //    // if we are teleporting to a spot that doesn't exist
    //    if (pos >= teleportLocations.Count)
    //    {
    //        // back out now
    //        Debug.LogWarning("Teleported to a position that didnt exist: " + pos);
    //        return;
    //    }

    //    // find the gameobject tiger spawner thing that corresponds to the position number and move to it
    //    Vector3 newPos = teleportLocations[pos].transform.position;

    //    // maybe use the Check Y thing to place us
    //    newPos = TigerControl.This().FigureOutY(newPos);

    //    player.transform.position = new Vector3(newPos.x, newPos.y + 1.5f, newPos.z);
    //}

	public void TeleportTo(GameObject obj)
	{
		// find the gameobject tiger spawner thing that corresponds to the position number and move to it
		Vector3 newPos = obj.transform.position;
		
		// maybe use the Check Y thing to place us
		newPos = TigerControl.This().FigureOutY(newPos);
		
		//player.transform.position = new Vector3(newPos.x, newPos.y + 1.5f, newPos.z);
        player.transform.position = new Vector3(newPos.x, newPos.y + 1.5f, newPos.z);
	}
}