using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CorrectLogo : MonoBehaviour {

    public Sprite tigerStatLogo, tigerSamplingLogo;

	// Use this for initialization
	void Start () {
        Image thisImage = this.GetComponent<Image>();
        if (GameSelector.curGameType == GameSelector.GameType.TigerStat)
        {
            thisImage.sprite = tigerStatLogo;
        }
        else
        {
            thisImage.sprite = tigerSamplingLogo;
        }
	}
}
