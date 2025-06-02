using UnityEngine;
using System.Collections;

public class TigerInfoFade : MonoBehaviour {
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
            mat.SetColor("_TintColor", new Color(.5f, .5f, .5f,
			                                     Alpha(Vector3.Distance(this.transform.position, Camera.main.transform.position))));
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