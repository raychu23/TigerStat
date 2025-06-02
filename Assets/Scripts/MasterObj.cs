using UnityEngine;
using System.Collections;

public class MasterObj {
    private static string playerName;
    private static string className;
    private static int difficulty;
    public static int minSize = 6;
    public static int maxSize = 18;

    public static string GetPlayerName()
    {
        if (playerName == null)
            playerName = "";

        return playerName;
    }
    public static void SetPlayerName(string foo)
    {
        playerName = foo;
    }

    public static string GetClassName()
    {
        if (className == null)
            className = "";

        return className;
    }
    public static void SetClassName(string foo)
    {
        className = foo;
    }

    public static int GetDifficulty()
    {
        return difficulty;
    }
    public static void SetDifficulty(int foo)
    {
        difficulty = foo;
    }

    public static bool DataEntered()
    {
        if (GetClassName() == "ClassName" || GetClassName() == ""
            || GetClassName().Length  > maxSize || GetClassName().Length < minSize)
            return false;

        if (GetPlayerName() == "PlayerName" || GetPlayerName() == ""
            || GetPlayerName().Length > maxSize || GetPlayerName().Length < minSize)
            return false;

        if (GetDifficulty() == 0)
            return false;

        return true;
    }
}