using UnityEngine;
using System.Collections;

public class LoadLobby : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine("Wait");
        StartCoroutine("WaitForTigerData");
    }

    private IEnumerator WaitForTigerData()
    {
        while (!TigerDataHolder.initialized)
        {
            yield return true;
        }

        LoadScene.instance.Load(1);
    }

    //void Update()
    //{
    //    //if (Application.CanStreamedLevelBeLoaded(1))
    //    //if (WebGLLevelStreaming.CanStreamedLevelBeLoaded(int levelIndex)
    //    if (WebGLLevelStreaming.CanStreamedLevelBeLoaded(1))
    //    {
    //        //if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
    //        //if (!Application.isWebPlayer)
    //        //    Screen.fullScreen = true;
    //        //while (!TigerDataHolder.initialized)
    //        if (TigerDataHolder.initialized)
    //            LoadScene.instance.Load(1);
    //            //Application.LoadLevel(1);
    //    }
    //}

    //void OnGUI()
    //{
    //    string loadText = "Loading";
    //    for (int i = 0; i < (int)Time.time % 4; i++)
    //    {
    //        loadText+= ".";
    //    }

    //    GUI.skin.box.normal.textColor = Color.white;
    //    GUI.Box(new Rect(300, 200, 200, 40), loadText);
    //}

    //private IEnumerator Wait()
    //{
    //    while (!TigerDataHolder.initialized)
    //    {
    //        yield return false;
    //    }

    //    StartCoroutine("LoadLevel1");
    //}

    //private IEnumerator LoadLevel1()
    //{
    //    //while (!Application.CanStreamedLevelBeLoaded(1))
    //    while (!WebGLLevelStreaming.CanStreamedLevelBeLoaded(1))
    //    {
    //        yield return false;
    //    }

    //    Application.LoadLevel(1);
    //}
}