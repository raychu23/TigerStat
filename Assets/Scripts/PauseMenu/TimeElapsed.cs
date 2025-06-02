using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeElapsed : MonoBehaviour {

    private Text thisTextComponent;

	void Start () {
        thisTextComponent = this.GetComponent<Text>();
	}

    void Update()
    {
        thisTextComponent.text = FormatTime(Time.timeSinceLevelLoad);
    }

    private string FormatTime(float foo)
    {
        int sec = Mathf.FloorToInt(foo);
        int min = sec / 60;

        if (sec < 0)
            return "0:00";

        return (min) + ":" + (sec - min * 60 < 10 ? "0" : "") + (sec - min * 60);
    }
}