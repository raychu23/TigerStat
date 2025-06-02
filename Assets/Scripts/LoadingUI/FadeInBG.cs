using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInBG : MonoBehaviour {
    private Image thisImageComponent;
    private Text thisTextComponent;
    public float endAlpha;

	// Use this for initialization
	void Start () {
        thisImageComponent = this.GetComponent<Image>();
        if (thisImageComponent != null)
            StartCoroutine("FadeInImage");

        thisTextComponent = this.GetComponent<Text>();
        if (thisTextComponent != null)
            StartCoroutine("FadeInText");
	}

    private IEnumerator FadeInImage()
    {
        thisImageComponent.color = new Color(
            thisImageComponent.color.r, thisImageComponent.color.g, thisImageComponent.color.b, 0);

        while (thisImageComponent.color.a < endAlpha)
        {
            thisImageComponent.color = new Color(
                thisImageComponent.color.r, thisImageComponent.color.g, thisImageComponent.color.b, thisImageComponent.color.a + Time.deltaTime);

            yield return false;
        }

        thisImageComponent.color = new Color(
            thisImageComponent.color.r, thisImageComponent.color.g, thisImageComponent.color.b, endAlpha);
    }

    private IEnumerator FadeInText()
    {
        thisTextComponent.color = new Color(
            thisTextComponent.color.r, thisTextComponent.color.g, thisTextComponent.color.b, 0);

        while (thisTextComponent.color.a < endAlpha)
        {
            thisTextComponent.color = new Color(
                thisTextComponent.color.r, thisTextComponent.color.g, thisTextComponent.color.b, thisTextComponent.color.a + Time.deltaTime);

            yield return false;
        }

        thisTextComponent.color = new Color(
            thisTextComponent.color.r, thisTextComponent.color.g, thisTextComponent.color.b, endAlpha);
    }
}
