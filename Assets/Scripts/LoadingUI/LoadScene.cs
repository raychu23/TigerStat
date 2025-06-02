using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
    public static LoadScene instance;

    void Awake()
    {
        instance = this;
    }

    public void Load(int sceneNum)
    {
        // load in our loading canvas
        GameObject.Instantiate(Resources.Load("LoadingCanvas"));

        // load the scene
        StartCoroutine("LoadOurScene", sceneNum);
    }

    private IEnumerator LoadOurScene(int _sceneNum)
    {
#if UNITY_WEBPLAYER
        while (!Application.CanStreamedLevelBeLoaded(_sceneNum)) // WEBPLAYER
        {
            yield return false;
        }
#else
        yield return new WaitForEndOfFrame(); // webgl
#endif

        Application.LoadLevel(_sceneNum);
    }
}