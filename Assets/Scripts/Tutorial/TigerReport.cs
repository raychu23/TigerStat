using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

/// Script basics
/// Each level will have two things that make the level end
///     Either enough tiger data captured or a time limit
/// This script has a running timer and also receives reports of when tiger data was collected
/// It doesn't acutally have any data from the tigers, just that they were accounted for
/// If either end condition is met, pop up a window and stop player input

public class TigerReport : MonoBehaviour
{
    public static int reportCount;
    public static ArrayList tNum = new ArrayList();
    public static List<TigerData> tList = new List<TigerData>();
    public int timeLimit;
    public int tigerLimit;

    void OnEnable()
    {
        dataSent = false;
        Cursor.visible = false;
        //Screen.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        reportCount = 0;
        tNum = new ArrayList();
        tList = new List<TigerData>();
        StartCoroutine("EndConditionCheck");
    }

    private IEnumerator EndConditionCheck()
    {
        while (true)
        {
            if (Time.timeSinceLevelLoad > timeLimit)
            {
                EndScene(0);
                //times up!
                break;
            }

            if (reportCount >= tigerLimit)
            {
                EndScene(1);
                //tigers are data-fied!
                break;
            }

            yield return false;
        }
    }

    public static void DataCollected(int foo)
    {
        tNum.Add(foo);
        reportCount++;


        TigerData t1 = new TigerData();

        for (int i = 0; i < TigerDataHolder.CurData().categories.Count; i++)
        {
            t1.setData(
                TigerDataHolder.CurData().categories[i],
                TigerDataHolder.CurData().GetTigerStatAsNiceData(i, foo, true)
                );
        }

        tList.Add(t1);
    }

    private void EndScene(int foo)
    {
        HUD2.This().endState = 1;
        InputControl.isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // threw this in here too
        HUD2.This().FinishMissionScreen(foo);
    }

    public void FinishScene(bool sendData)
    {

        if (sendData)
        {
            StartCoroutine(Upload(tList));
            
        }
        else
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("LobbyScene");
        }
    }


    public IEnumerator Upload(List<TigerData> tigerData)
    {
        int gameNum = 0;

        WWWForm numForm = new WWWForm();
        numForm.AddField("PlayerID", MasterObj.GetPlayerName());
        numForm.AddField("GroupID", MasterObj.GetClassName());

        
        //Fetch game number
        using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/gettigerstatnum.php", numForm))
        {
            Debug.Log("starting fetching game num");
            yield return www.SendWebRequest();
            Debug.Log("fetched");
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Fetching game number failed.  Error message: ");
            }
            else
            {
                gameNum = int.Parse(www.downloadHandler.text);
                Debug.Log("gamenum: " + gameNum);
            }
        }
        foreach (TigerData entry in tigerData)
        {
            WWWForm form = new WWWForm();
            form.AddField("GameNum", gameNum);
            form.AddField("PlayerID", MasterObj.GetPlayerName());
            form.AddField("GroupID", MasterObj.GetClassName());
            Dictionary<String, String> data = entry.dict();
            form.AddField("Age", data["Age"]);
            form.AddField("NoseBlack", data["NoseBlack"]);
            form.AddField("PawCircumference", data["PawCircumference"]);
            form.AddField("Weight", data["Weight"]);
            form.AddField("Size", data["Length"]);
            form.AddField("Sex", data["Sex"]);

            using (var www = UnityWebRequest.Post("https://stat2games.sites.grinnell.edu/php/sendtigerstatgameinfo.php", form))
            {

                yield return www.SendWebRequest();

                Debug.Log("downloadhandler: " + www.downloadHandler.text);
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Player data created successfully.");
                }
                else
                {
                    Debug.Log("Player data creation failed. Error # " + www.downloadHandler.text);
                }
            }
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("LobbyScene");
    }


    public static bool dataSent;

}
public class TigerData
{
    private Dictionary<String, String> dataValues;

    public TigerData()
    {
        dataValues = new Dictionary<String, String>();
    }

    public void setData(String dataType, String dataValue)
    {
        dataValues[dataType] = dataValue;
    }

    public Dictionary<String, String> dict()
    {
        return dataValues;
    }

    public String valueForKey(String dataType)
    {
        return dataValues[dataType];
    }
}