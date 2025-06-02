using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutImage : MonoBehaviour {

    public bool startOpaque;

	// Use this for initialization
	void Start () {
        StartCoroutine("FadeOut");
	}

    private IEnumerator FadeOut()
    {
        Image thisImage = this.GetComponent<Image>();
        float animTime = 2f;

        if (startOpaque) thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 1);

        float t = 0.0f;
        //while (t <= 1.0f)
        while (thisImage.color.a > .05f)
        {
            t += Time.deltaTime / animTime;
            thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b,
                Mathf.Lerp(thisImage.color.a, 0, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return false;
        }

        thisImage.color = new Color(0, 0, 0, 0);
        this.gameObject.SetActive(false);
    }
}