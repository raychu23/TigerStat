using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour
{
    public GameObject player;
	private Rect mapGpRect, mapRect, playerRotateRect;
    public Texture2D mapTex, playerTex, tigerNormTex, tigerDataTex, tigerDeadTex;
    private Vector2 playerPivot;

    private float range, fadeRange;
    private int mapSize;
    float screenSize;
    private float curSize;

    private Rect ResizeRect(float x, float y, float width, float height)
    {
        return new Rect(x * screenSize, y * screenSize, width * screenSize, height * screenSize);
    }

    private Vector2 ResizeVector2(float x, float y)
    {
        return new Vector2(x * screenSize, y * screenSize);
    }

    void Start()
    {
        ResizeScreen();
    }

    private void ResizeScreen()
    {
        curSize = Screen.width;
        screenSize = curSize / 1024f;

        range = 300;
        fadeRange = 280;
        mapSize = 94;
        //mapGpRect = ResizeRect(10, 558, 200, 200);
		mapGpRect = ResizeRect(10, 10, 200, 220);
        playerPivot = ResizeVector2(100, 100);
        mapRect = ResizeRect(0, 0, mapTex.width, mapTex.height);
        playerRotateRect = ResizeRect(0, 0, mapTex.width, mapTex.width);
    }

    void Update()
    {
        if (curSize != Screen.width)
            ResizeScreen();
    }

    void OnGUI()
    {
        if (HUD2.This().AreWePaused())
            return;

        GUI.BeginGroup(mapGpRect);

        // draw player heading
        GUIUtility.RotateAroundPivot(player.transform.localEulerAngles.y, playerPivot);
        GUI.DrawTexture(playerRotateRect, playerTex);
        GUIUtility.RotateAroundPivot(-player.transform.localEulerAngles.y, playerPivot);

        // draw minimap image
        GUI.DrawTexture(mapRect, mapTex);

        // draw tigers
        foreach (GameObject foo in GameObject.FindGameObjectsWithTag("Tiger"))
        {
            CheckTigerDraw(foo);
        }

        GUI.EndGroup();
    }

    float dist;

    private void CheckTigerDraw(GameObject foo)
    {
        dist = Vector3.Distance(player.transform.position, foo.transform.position);
        if (dist < fadeRange)
        {
            // draw our tiger blip!
            GUI.DrawTexture(DetermineRect(player.transform.position, foo.transform.position), GetTigerTex(foo));
        }
        else if (dist < range)
        {
                        // draw it faded since its not quite in range
            GUI.color = new Color(1, 1, 1, 1 - ((dist - fadeRange) / (range - fadeRange)));
            GUI.DrawTexture(DetermineRect(player.transform.position, foo.transform.position), GetTigerTex(foo));
            GUI.color = Color.white;
        }
    }

    private Rect DetermineRect(Vector3 pPos, Vector3 tPos)
    {
        return ResizeRect((pPos.x - tPos.x) / range * -mapSize + 100 - (tigerNormTex.width / 2),
            (pPos.z - tPos.z) / range * mapSize + 100 - (tigerNormTex.width / 2),
            tigerNormTex.width, tigerNormTex.height);
    }

    private Texture2D GetTigerTex(GameObject tiger)
    {
        TigerAI.tigerState state = tiger.GetComponent<TigerAI>().curState;

        if (state == TigerAI.tigerState.DataPending)
            return tigerDataTex;

        if (state == TigerAI.tigerState.Dead)
            return tigerDeadTex;

        return tigerNormTex;
    }
}