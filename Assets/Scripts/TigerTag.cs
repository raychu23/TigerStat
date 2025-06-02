using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script will:
// Face the main camera when activated
// Set the text to display different things when needed
// When tiger data is available: display "Press E to collect Data"
// When data is captured, display data that was captured
// When text is visible, fade out after the player walks a certain distance

public class TigerTag : MonoBehaviour
{
    private TextMesh[] tMesh;
    private Material textMat_white;
    private List<Material> textMat_black;
    private float fadeEnd, fadeStart;

    void Start()
    {
        tMesh = this.GetComponentsInChildren<TextMesh>();
        textMat_white = this.GetComponent<MeshRenderer>().material;
        textMat_black = new List<Material>();
        for (int i = 0; i < 4; i++)
        {
            textMat_black.Add(this.GetComponentsInChildren<MeshRenderer>()[i+1].material);
        }

        fadeEnd = 6f;
        fadeStart = 5.5f;
    }

    // called when we tag a tiger
    public void DataToCollect()
    {
        foreach (MeshRenderer foo in this.GetComponentsInChildren<MeshRenderer>())
        {
            foo.enabled = true;
        }
        SetText("Press E to collect data");
        StartCoroutine("LookAtPlayer");
    }

    // look at camera from this point on
    IEnumerator LookAtPlayer()
    {
        while (true)
        {
            //transform.LookAt(Camera.mainCamera.transform);
			transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            FadeControl();
            yield return false;
        }
    }

    private void FadeControl()
    {
		float alpha = Alpha(Vector3.Distance(this.transform.position, Camera.main.transform.position));

        textMat_white.color = new Color(1, 1, 1, alpha);
        foreach (Material mat in textMat_black)
        {
            mat.color = new Color(0, 0, 0, alpha);
        }
    }

    private float Alpha(float dist)
    {
        if (dist > fadeEnd)
            return 0f;
        if (dist < fadeStart)
            return 1f;

        return (1 - ((dist - fadeStart) / (fadeEnd - fadeStart)));
    }

    public void DisplayData(int tigerNumber)
    {
        List<string> categories = TigerDataHolder.CurData().categories;
        string dataText = "Tiger tagged";
        for (int i = 0; i < categories.Count; i++)
        {
			string temp = TigerDataHolder.CurData().GetTigerStatAsNiceData(i, tigerNumber, false);
			dataText += "\n" + categories[i] + ": " + temp;
        }

        SetText(dataText);
    }

    private void SetText(string text)
    {
        foreach (TextMesh t in tMesh)
        {
            t.text = text;
        }
    }
}
