using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    IEnumerator LookAtPlayer()
    {
        while (true)
        {
            transform.LookAt(Camera.main.transform);
            yield return false;
        }
    }

    public void DataToCollect()
    {
        this.GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine("LookAtPlayer");
    }

    public void DataHasBeenCollected()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
        StopAllCoroutines();
    }
}