using UnityEngine;
using System.Collections;

public class GameSelector : MonoBehaviour {

    public enum GameType { TigerStat, TigerSampling}//, TigerPredict }

    public static GameType curGameType = GameType.TigerStat;
    public GameType _curGameType;

	void Start () {
        curGameType = _curGameType;
	}
}
