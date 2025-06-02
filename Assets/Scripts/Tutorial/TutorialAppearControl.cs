using UnityEngine;
using System.Collections;

public class TutorialAppearControl : MonoBehaviour {

    private static bool flag = false;

    void Start()
    {
        StartCoroutine("FlagCheck");
    }

    IEnumerator FlagCheck()
    {
        while (!flag)
        {
            yield return false;
        }

        StartCoroutine("FadeOn");
    }

    public static void TargetHit()
    {
        flag = true;
    }

    private IEnumerator FadeOn()
    {
        // make material transparent
        this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);

        float alpha = 0;
        // fade up transparency
        while (alpha < .8f)
        {
            alpha += Time.deltaTime;
            this.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, alpha);
            yield return false;
        }

        // turn renderer on
        this.GetComponent<InfoFade>().enabled = true;
    }
}
