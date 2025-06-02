using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    private static int shotsFired;
    private static int shotsHit;
    private static float distTraveled;

    private void Start()
    {
        ResetData();
    }

    public static void ResetData()
    {
        shotsFired = 0;
        shotsHit = 0;
        distTraveled = 0;
    }

    public static void ReportDistance(float foo)
    {
        distTraveled += foo;
    }

    public static void ReportShot()
    {
        shotsFired++;
    }

    public static void ReportHit()
    {
        shotsHit++;
    }

    public static int GetShots()
    {
        return shotsFired;
    }
    public static int GetHits()
    {
        return shotsHit;
    }
    public static int GetDist()
    {
        return Mathf.RoundToInt(distTraveled);
    }
    public static float GetAccuracy()
    {
        if (shotsFired == 0)
            return 0;

        return ((float)shotsHit / (float)shotsFired);
    }
}