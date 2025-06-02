using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TigersTagged : MonoBehaviour {

    private Text thisTextComponent;

	void Start () {
        thisTextComponent = this.GetComponent<Text>();
	}

    void Update()
    {
        thisTextComponent.text = TigerReport.reportCount.ToString();
    }
}