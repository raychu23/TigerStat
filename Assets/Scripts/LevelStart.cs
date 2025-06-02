using UnityEngine;
using System.Collections;

public class LevelStart : MonoBehaviour {

	void Awake () {
        InitializeLevel();
	}

    void InitializeLevel()
    {
	    // input class
        ResetInput();
        // Time / reporting class
        ResetReport();
        // hud class
        ResetHud();
	}

    private static void ResetInput()
    {
        InputControl.lockMovement = false;
        InputControl.zoomState = 0;
        InputControl.isPaused = false;
    }

    private static void ResetReport()
    {
        TigerReport.reportCount = 0;
    }

    private static void ResetHud()
    {
        if (HUD2.This() != null)
            HUD2.This().ResetHud();
    }
}
