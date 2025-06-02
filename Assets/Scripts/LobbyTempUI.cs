// Jeff note: Its funny this is the "temp UI" when it ended up being the actual UI
// silly

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class LobbyTempUI : MonoBehaviour
{
    public Rect versionRect, logoRect, screenRect, tieLogoRect;
    private Rect[] buttonRect;
    private Rect nameRect, classRect, continueRect, diffRect, diffBtn1Rect, diffBtn2Rect, diffExplainRect, instructRect, backRect;
    private Rect returnRect, fsRect, dataRect;
    public Texture2D logoTex;
    public Texture2D blackTex;
    public Texture2D tieLogo;
    public GUIStyle buttonStyle;
    public GUIStyle flatStyle;
    public GUIStyle descripStyle;
    public string version;
    private float curAlpha = 1;
    private float screenSize;
    private int curState;
    private float curSize;
    private int preparedLevel;

    private Rect ResizeRect(float x, float y, float width, float height)
    {
        return new Rect(x * screenSize, y * screenSize, width * screenSize, height * screenSize);
    }

    void Start()
    {
        if (MasterObj.DataEntered())
            curState = 1;
        else
            curState = 0;

        ResizeScreen();
        preparedLevel = 0;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine("FadeIn");
    }

    private void ResizeScreen()
    {
        curSize = Screen.width;
        screenSize = curSize / 1024f;
        buttonRect = new Rect[] {
            ResizeRect(-10, 140, 300, 50),
            ResizeRect(-10, 200, 300, 50),
            ResizeRect(-10, 260, 300, 50),
            ResizeRect(-10, 320, 300, 50) };
        backRect = ResizeRect(-10, 450, 160, 50);
        versionRect = ResizeRect(900, 738, 150, 30);
		logoRect =  ResizeRect(377 * 1.28f, 488 * 1.28f, 414 * 1.28f, 86 * 1.28f);
        tieLogoRect = ResizeRect(840, 9, 192, 145);
        screenRect = ResizeRect(0, 0, 1024, 768);
        nameRect = ResizeRect(0, 50, 460, 50);
        classRect = ResizeRect(0, 110, 460, 50);
        diffRect = ResizeRect(0, 170, 170, 50);
        continueRect = ResizeRect(-10, 390, 300, 50);
        diffBtn1Rect = ResizeRect(190, 170, 130, 50);
        diffBtn2Rect = ResizeRect(330, 170, 130, 50);
        diffExplainRect = ResizeRect(50, 234, 350, 96);
        instructRect = ResizeRect(477, 61, 224, 95);
        //returnRect = ResizeRect(0, 701, 449, 50);
        fsRect = ResizeRect(0, 581, 283, 50);
        dataRect = ResizeRect(0, 641, 336, 50);
    }


    void Update()
    {
        if (curSize != Screen.width)
            ResizeScreen();
    }

    private IEnumerator FadeIn()
    {
        curAlpha = 1;
        while (curAlpha > 0)
        {
            curAlpha -= Time.deltaTime;
            yield return false;
        }
        curAlpha = 0;
    }

    void OnGUI()
    {
        if (curState == 0)
        {
            MasterObj.SetPlayerName(GUI.TextField(nameRect, FormatString(MasterObj.GetPlayerName()), buttonStyle));
            MasterObj.SetClassName(GUI.TextField(classRect, FormatString(MasterObj.GetClassName()), buttonStyle));
            if (MasterObj.GetClassName().Length < MasterObj.minSize || MasterObj.GetClassName().Length > MasterObj.maxSize
                || MasterObj.GetPlayerName().Length < MasterObj.minSize || MasterObj.GetPlayerName().Length > MasterObj.maxSize)
                GUI.Label(instructRect, "Names must be between 6-18 characters", descripStyle);

            GUI.Label(diffRect, "Difficulty:", flatStyle);

            int diff = MasterObj.GetDifficulty();

            if (GUI.Button(diffBtn1Rect, "Casual", diff == 1 ? buttonStyle : flatStyle))
                MasterObj.SetDifficulty(1);
            if (GUI.Button(diffBtn2Rect, "Hard", diff == 2 ? buttonStyle : flatStyle))
                MasterObj.SetDifficulty(2);

            if (diff > 0)
                GUI.Label(diffExplainRect, diff == 1 ?
                    "Tigers are not aggressive and are easier to hit." :
                    "Tigers will attack if too close and require precise aiming to hit.", descripStyle);

            GUI.enabled = MasterObj.DataEntered();
            if (GUI.Button(continueRect, "Continue", buttonStyle))
                curState = 1;
            GUI.enabled = true;
        }

        else if (curState == 1)
        {
            if (GUI.Button(buttonRect[0], "Load Tutorial", buttonStyle))
                LoadScene.instance.Load(2);
                //Application.LoadLevel(2);

            if (GUI.Button(buttonRect[1], "Load Mission 1", buttonStyle))
            {
                preparedLevel = 3;
                curState = 2;
                //Application.LoadLevel(2);
            }

            if (GUI.Button(buttonRect[2], "Load Mission 2", buttonStyle))
            {
                preparedLevel = 4;
                curState = 2;
                //Application.LoadLevel(3);
            }

            if (GUI.Button(backRect, "Back", buttonStyle))
                curState = 0;
        }

        else if (curState == 2)
        {
            for (int i = 0; i < TigerDataHolder.TotalDataSets(); i++)
            {
                if (GUI.Button(buttonRect[i], "DataSet " + (i + 1).ToString(), buttonStyle))
                    LoadLevel(preparedLevel, i);
            }

            if (GUI.Button(backRect, "Back", buttonStyle))
                curState = 1;
        }

        // draw button to go back to hunting lodge
        if (GUI.Button(fsRect, "Full Screen", flatStyle))
        {
            Screen.SetResolution(800, 600, !Screen.fullScreen);
            ResizeScreen();
        }

        // data button
        if (GUI.Button(dataRect, "Get Game Data", flatStyle))
            Application.OpenURL(urlLocations.instance.GetWebReporterUrl());

//        if (GUI.Button(returnRect, "Return to Hunting Lodge", flatStyle))
//            Application.OpenURL(urlLocations.huntingLodge);

        // draw logo
        GUI.DrawTexture(logoRect, logoTex);
        //GUI.Label(versionRect, "v" + version);

        // tie logo
        GUI.DrawTexture(tieLogoRect, tieLogo);

        if (curAlpha > 0)
        {
            GUI.color = new Color(1, 1, 1, curAlpha);
            GUI.DrawTexture(screenRect, blackTex);
        }
    }

    Regex rgx = new Regex("[^a-zA-Z0-9 -]");

    private string FormatString(string foo)
    {
        return rgx.Replace(foo, "").Replace(" ", "");
    }

    private void LoadLevel(int levelNum, int dataSetNum)
    {
        TigerDataHolder.SetData(dataSetNum);
        LoadScene.instance.Load(levelNum);
        //Application.LoadLevel(levelNum);
    }
}