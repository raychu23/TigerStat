using UnityEngine;
using System.Collections;
using System.IO;

public class urlLocations : MonoBehaviour
{
    //private static string dataURL_tigerStat_staging = "http://statgamesstaging.tietronix.com/tigerstat/webreporter.php";
    //private static string dataURL_tigerStat = "http://localhost/tigerstat/webreporter.php";
    //private static string dataURL_tigerSampling_staging = "http://statgamesstaging.tietronix.com/tigersampling/webreporter.php";
    //private static string dataURL_tigerSampling = "http://localhost/tigersampling/webreporter.php";

    //public static string serviceURL_tigerStat_staging = "http://statgamesstaging.tietronix.com/tigerstat/addreport.php";
    //public static string serviceURL_tigerStat = "http://localhost/tigerstat/addreport.php";
    //public static string serviceURL_tigerSampling_staging = "http://statgamesstaging.tietronix.com/tigersampling/addreport.php";
    //public static string serviceURL_tigerSampling = "http://localhost/tigersampling/addreport.php";

    public static urlLocations instance;

    private string serviceURL, webreporterURL;


    private void Start()
    {
        instance = this;
        Initialize();
    }

    private void Initialize()
    {
        StartCoroutine("LoadUrls");
    }

    public string GetServiceUrl()
    {
        return serviceURL;
    }

    public string GetWebReporterUrl()
    {
        return webreporterURL;
    }

    private IEnumerator LoadUrls()
    {
        string path = Application.dataPath + "/StreamingAssets/url.txt";
        if (!path.Contains("://"))
            path = "file://" + path;
        WWW www = new WWW(path);
        yield return www;
        string fileText = System.Text.Encoding.UTF8.GetString(www.bytes);
        if (!string.IsNullOrEmpty(fileText))
        {
            serviceURL = LoadUrl(fileText, 0);
            webreporterURL = LoadUrl(fileText, 1);
        }
    }

    private string LoadUrl(string fileContents, int num)
    {
        var lines = fileContents.Split("\n"[0]);

        // make sure we dont do something dumb
        if (lines.Length <= num)
        {
            Debug.LogWarning("we didnt pull enough urls from our text file");
            return null;
        }

        var splitAgain = lines[num].Split(',');

        if (splitAgain.Length != 2)
        {
            Debug.LogWarning("incorrectly formatted text file for urls");
            return null;
        }

        return splitAgain[1];
    }

}