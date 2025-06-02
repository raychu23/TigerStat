using UnityEngine;
using System.Collections;

public class angleTest : MonoBehaviour {
    private Vector3 pos1, pos2, pos3;

    void Update()
    {
        pos1 = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);
        pos2 = new Vector3(pos1.x, pos1.y, pos1.z + 1);
        pos3 = new Vector3(pos1.x + 1, pos1.y, pos1.z);

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;

        Physics.Raycast(pos1, -Vector3.up, out hit1, 40);
        Physics.Raycast(pos2, -Vector3.up, out hit2, 40);
        Physics.Raycast(pos3, -Vector3.up, out hit3, 40);

        Debug.Log(Vector3.Angle(hit1.point, hit2.point) + "  " + Vector3.Angle(hit1.point, hit3.point));

        //
        //Vector3 slopeCheck = ourTerrainData.GetInterpolatedNormal(Mathf.InverseLerp(0, 2000, transform.position.x), Mathf.InverseLerp(0, 2000, transform.position.y));

        //Debug.Log(slopeCheck);
    }
}
