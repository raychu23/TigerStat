using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WoundGraphic : MonoBehaviour
{
    void OnEnable()
    {
        StopCoroutine("ShowWound");
        StartCoroutine("ShowWound");
    }

    private IEnumerator ShowWound()
    {
        float woundAlpha = 1f;
        Image imageComponent = this.GetComponent<Image>();

        while (woundAlpha > 0f)
        {
            imageComponent.color = new Color(1, 1, 1, woundAlpha);
            woundAlpha -= Time.deltaTime;

            yield return false;
        }

        woundAlpha = 0f;
        this.gameObject.SetActive(false);
    }
}
