using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD2 : MonoBehaviour
{
    //public GameObject levelController;
    public List<GameObject> pauseMenuGraphs;

    public static HUD2 This() {return obj;}
    private static HUD2 obj;

    public int endState; // 0 = normal, 1 == out of time or tigers

    public GameObject pauseCanvas, playerCanvas;
    public GameObject MainPauseGp, ViewControlsGp, EndRunGp, TeleportGp;
    public GameObject teleportButton;
    public GameObject woundImageObj, reticleImageObj, scopeImageObj;

    void Start()
    {
        //if (!Application.isWebPlayer)
        //    Screen.fullScreen = true;
        obj = this;
        endState = 0;
    }

    public void ToggleFullScreen()
    {
        Screen.SetResolution(800, 600, !Screen.fullScreen);
    }

    public void Show()
    {
        pauseCanvas.SetActive(true);
        playerCanvas.SetActive(false);
        MainPauseGp.SetActive(true);
        ViewControlsGp.SetActive(false);
        EndRunGp.SetActive(false);
        TeleportGp.SetActive(false);

        // deal with the teleporting button
        if (GameSelector.curGameType == GameSelector.GameType.TigerSampling)
            teleportButton.SetActive(true);
        else
            teleportButton.SetActive(false);

        // setup our graphs
        SetupGraphs();
    }

	public void ShowPredictAsk()
	{
		pauseCanvas.SetActive(true);
		playerCanvas.SetActive(false);
		MainPauseGp.SetActive(false);
		ViewControlsGp.SetActive(false);
		EndRunGp.SetActive(false);
		TeleportGp.SetActive(false);
		
		// setup our graphs
		SetupGraphs();
	}

    private void SetupGraphs()
    {
        for (int i = 0; i < 4; i++)
        {
            pauseMenuGraphs[i].GetComponent<GraphUIControl>().SetupGraph(
                TigerDataHolder.CurData().GetGraphCat(i)[0],
                TigerDataHolder.CurData().GetGraphCat(i)[1],
                TigerDataHolder.CurData().GetGraphCatNum(i, 0),
                TigerDataHolder.CurData().GetGraphCatNum(i, 1));
        }
    }

    public void Hide()
    {
		MainPauseGp.SetActive(false);
		ViewControlsGp.SetActive(false);
		EndRunGp.SetActive(false);
		TeleportGp.SetActive(false);
        teleportButton.SetActive(false);
        pauseCanvas.SetActive(false);
        playerCanvas.SetActive(true);
        
        HideTeleportMap();
    }

    public bool AreWePaused()
    {
        return pauseCanvas.activeInHierarchy;
    }

    public void ResetHud()
    {
        pauseCanvas.SetActive(false);
        playerCanvas.SetActive(true);
        HideTeleportMap();
    }

    public void SetScope(bool on)
    {
        reticleImageObj.SetActive(!on);
        scopeImageObj.SetActive(on);
    }

    // call this when we get hit
    // doesnt actually do anything but warn the player they are getting attacked
    public void BattleDamage()
    {
        woundImageObj.SetActive(true);
    }

    public void ShowOurTeleportMap()
	{
        if (TeleportCamControl.This() != null)
		TeleportCamControl.This().TurnOnCam();
	}

	public void HideTeleportMap()
	{
        if (TeleportCamControl.This() != null)
		    TeleportCamControl.This().TurnOffCam();
	}

    public GameObject OutOfTimeCanvas, OutOfTigersCanvas;

    public void FinishMissionScreen(int foo)
    {
        playerCanvas.SetActive(false);
        SetEndState(1);

        // if foo == 0, our time ran out.  if foo == 1, we got enough tiger data
        if (foo == 0)
        {
            OutOfTimeCanvas.SetActive(true);
        }
        else
        {
            OutOfTigersCanvas.SetActive(true);
        }
    }

    public void SetEndState(int foo)
    {
        endState = foo;
    }

    //private void FinishMissionScreen()
    //{
    //    GUI.Label(finalPauseInstructionRect,
    //        pauseState == PauseState.outOfTime ?
    //        "The timer has expired! Lets submit your current data and return to the lobby." :
    //        Application.loadedLevel == 1 ?
    //        "Thank you for completing the tutorial! Now that you know how to play, try Mission 1 and Mission 2." :
    //        "You've gathered a sufficient amount of data on the tigers in the area. Lets submit our data and return to the lobby.",
    //        pauseBtnStyle);

    //    if (GUI.Button(confirmBtnRect, "Confirm", pauseBtnStyle))
    //        GameObject.Find("LevelController").GetComponent<TigerReport>().FinishScene(Application.loadedLevel == 1 ? false : true);
    //}
}