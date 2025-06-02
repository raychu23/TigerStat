using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ContinueActive : MonoBehaviour {

	public List<Text> textToComplete;
	public List<Toggle> toggleToSelect;

	private Button thisButton;

	void Start()
	{
		thisButton = this.GetComponent<Button>();

		if (textToComplete.Count == 0 && toggleToSelect.Count == 0)
		{
			Debug.LogWarning("ContinueActive on " + this.gameObject.name + " is not set up properly");
			this.enabled = false;
		}

		if (textToComplete.Count != 0 && toggleToSelect.Count != 0)
		{
			Debug.LogWarning("ContinueActive on " + this.gameObject.name + " is not set up properly");
			this.enabled = false;
		}
	}
	
	void Update ()
	{
		thisButton.interactable = ReadyToContinue();
	}

	private bool ReadyToContinue()
	{
		if (textToComplete.Count != 0)
			return TextInputsAreComplete();

		else if (toggleToSelect.Count != 0)
			return ToggleInputsAreComplete();

		else
			return true;
	}

	private bool TextInputsAreComplete()
	{
		foreach (Text textComponent in textToComplete)
		{
			if (textComponent.text == "")
				return false;
		}

		return true;
	}

	private bool ToggleInputsAreComplete()
	{
		foreach (Toggle toggleButton in toggleToSelect)
		{
			if (toggleButton.isOn)
				return true;
		}

		return false;
	}
}