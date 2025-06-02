using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LobbyUIControl : MonoBehaviour {

	public InputField nameText, classText;

	public GameObject setNameGroup, selectMissionGroup, selectDifficultyGroup, selectDataSet;

	public Text difficultyParagraphText, datasetParagraphText;

	public int levelToLoad;

	// Use this for initialization
	void Start () {
		levelToLoad = -1;

		if (MasterObj.GetPlayerName() == "") LoadGroup_Name();
		else LoadGroup_Mission();
	}

	public void SetName()
	{
		MasterObj.SetPlayerName(nameText.text);
		MasterObj.SetClassName(classText.text);
	}

	public void SetMission(int missionNumber)
	{
		levelToLoad = missionNumber;
	}

	public void SetDifficulty(int num)
	{
		MasterObj.SetDifficulty(num);

		difficultyParagraphText.text = 
			num == 1 ?
				"Tigers are not aggressive and are easier to tranquilize."
				:
				"Tigers will attack if too close and require precise aiming to tranquilize."
				;
	}

	public void SetDataset(int dataNum)
	{
		TigerDataHolder.SetData(dataNum);

		datasetParagraphText.text = 
            GameSelector.curGameType == GameSelector.GameType.TigerStat ?
            (   
				// tigerstat instructions
			    dataNum == 0 ?
				// dataset 1
				"Basic information is provided on each tiger that is captured. This includes age, weight, length, paw circumference, percentage nose black, and sex."
				:
				// dataset 2
				"Basic information is provided on each tiger that is captured. This includes age, weight, length, paw circumference, percentage nose black, and sex. However, there are several errors and missing values in the data."
				    
            ) :
            (
				// tigersampling instructions
                dataNum == 0 ?
				// dataset 1
				"Basic information is provided on each tiger that is captured. This includes region where the tiger data was collected, age, percentage nose black, paw circumference and tail length."
				:
				// dataset 2
				"Basic information is provided on each tiger that is captured. This includes region where the tiger data was collected, age, percentage nose black, paw circumference and tail length. However, there are several errors and missing values in the data."
            ) ;
	}

	public void LoadOurMission()
	{
		if(levelToLoad == 2)
        {
			SceneManager.LoadScene("TutorialScene");
        } else if (levelToLoad == 3)
		{
			SceneManager.LoadScene("Mission1");
        }
        else
        {
			SceneManager.LoadScene("Mission2");
		}
		//LoadScene.instance.Load(levelToLoad);
        //if (WebGLLevelStreaming.CanStreamedLevelBeLoaded(levelToLoad))

		    //Application.LoadLevel(levelToLoad);
	}

	public void SetFullScreen()
	{
		Screen.SetResolution(800, 600, !Screen.fullScreen);
	}

	public void GetGameData()
	{
        Application.OpenURL(urlLocations.instance.GetWebReporterUrl());
	}

	public void LoadGroup_Name()
	{
		setNameGroup.SetActive(true);
		selectMissionGroup.SetActive(false);
		selectDifficultyGroup.SetActive(false);
		selectDataSet.SetActive(false);

        // if we already had a name and class, set it
        string curPlayerName = MasterObj.GetPlayerName();
        string curClassName = MasterObj.GetClassName();

        nameText.text = curPlayerName;
        classText.text = curClassName;
	}

	public void LoadGroup_Mission()
	{
		setNameGroup.SetActive(false);
		selectMissionGroup.SetActive(true);
		selectDifficultyGroup.SetActive(false);
		selectDataSet.SetActive(false);
	}

	public void LoadGroup_Difficulty()
	{
		setNameGroup.SetActive(false);
		selectMissionGroup.SetActive(false);
		selectDifficultyGroup.SetActive(true);
		selectDataSet.SetActive(false);
	}

	public void LoadGroup_DataSet()
	{
        // if we are doing the tutorial level, just load the tutorial level
        if (levelToLoad == 2)
        {
            LoadOurMission();
            return;
        }

		setNameGroup.SetActive(false);
		selectMissionGroup.SetActive(false);
		selectDifficultyGroup.SetActive(false);
		selectDataSet.SetActive(true);
	}
}