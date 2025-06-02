using UnityEngine;
using System.Collections;

public class MinimapCam : MonoBehaviour
{
    public GameObject player;
    public GameObject minimapPlayerPlane;
    public GameObject tigerMinimapPrefab;
    public GameObject tigerControl;

    public int numTigers;

    private GameObject[] tigerIcons;

    void Start()
    {
        StartCoroutine("FollowPlayer");

        StartCoroutine("MakeTigerIcons");
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            transform.position = new Vector3(player.transform.position.x, 600, player.transform.position.z);

            minimapPlayerPlane.transform.localEulerAngles = new Vector3(-player.transform.localEulerAngles.y - 180, -90, 90);

            yield return false;
        }
    }

    private IEnumerator MakeTigerIcons()
    {
        while (numTigers == 0)
        {
			numTigers = tigerControl.GetComponent<TigerControl>().tigers.Count;
            yield return false;
        }

        tigerIcons = new GameObject[numTigers];

        for (int i = 0; i < numTigers; i++)
        {
            tigerIcons[i] = (GameObject)GameObject.Instantiate(tigerMinimapPrefab);
            tigerIcons[i].transform.parent = this.transform;
        }

        StartCoroutine("TigerIcon");
    }

    private IEnumerator TigerIcon()
    {
        while (true)
        {
            for (int i = 0; i < numTigers; i++)
            {
                tigerIcons[i].transform.position = new Vector3(
                    tigerControl.GetComponent<TigerControl>().tigers[i].transform.position.x,
                    500,
                    tigerControl.GetComponent<TigerControl>().tigers[i].transform.position.z); 
            }

            yield return false;
        }
    }
}