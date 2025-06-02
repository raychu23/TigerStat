using UnityEngine;
using System.Collections;

public class InfoFade : MonoBehaviour {
    public float fadeEnd;
    public float fadeStart;
    private Material mat;

	void Start () {
        mat = this.GetComponent<MeshRenderer>().material;

        StartCoroutine("FadeCheck");
	}


    IEnumerator FadeCheck()
    {
        while (true)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b,
			                      Alpha(Vector3.Distance(this.transform.position, Camera.main.transform.position)));
            yield return false;
        }
	}

    private float Alpha(float dist)
    {
        if (dist > fadeEnd)
            return 0;
        if (dist < fadeStart)
            return .8f;

        return (1 - ((dist - fadeStart) / (fadeEnd - fadeStart))) * .8f;
    }
}