using UnityEngine;
using System.Collections;

public class DataGraph
{
    private Texture2D dataTex;
    private Vector2[] data;
    private Vector2 maxRange;
    private float fixPosX, fixPosY;
    private string xLabel, yLabel;
    private Rect graphRect;
    private Rect[] unitsRect;

    public DataGraph(Vector2[] dat, string xUnitLabel, string yUnitLabel, Texture2D datTex)
    {
        data = dat;
        xLabel = xUnitLabel;
        yLabel = yUnitLabel;

        // figure out X and Y range (we want to scale the data to it)
        Vector2 max = new Vector2(data[0].x, data[0].y);

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].x > max.x)
                max = new Vector2(data[i].x, max.y);
            if (data[i].y > max.y)
                max = new Vector2(max.x, data[i].y);
        }

        maxRange = new Vector2(max.x * 1.1f, max.y * 1.1f);
        dataTex = datTex;
        graphRect = new Rect(53, 10, 208, 148);
        unitsRect = new Rect[] {
            new Rect(13, 147, 30, 20),
            new Rect(13, 98, 30, 20),
            new Rect(13, 49, 30, 20),
            new Rect(13, 0, 30, 20),
            new Rect(25, 167, 30, 20),
            new Rect(110, 167, 30, 20),
            new Rect(175, 167, 30, 20),
            new Rect(240, 167, 30, 20),
            new Rect(90, 190, 190, 20),
            new Rect(3, 129, 90, 20)};
    }

    public void Draw(Vector2 drawRectStart, Texture2D bgImg, GUIStyle axisStyle)
    {
        GUI.BeginGroup(new Rect(drawRectStart.x, drawRectStart.y, bgImg.width, bgImg.height));
        GUI.DrawTexture(new Rect(0, 0, bgImg.width, bgImg.height), bgImg);

        // draw graph data
        GUI.BeginGroup(graphRect);
        for (int i = 0; i < data.Length; i++)
        {
            // draw our data point
            GUI.DrawTexture(CalculateRect(data[i], graphRect.width, graphRect.height, 4), dataTex);
        }
        GUI.EndGroup();

        // draw axes
        GUI.Label(unitsRect[0], "0", axisStyle);
        GUI.Label(unitsRect[1], (Mathf.Round((maxRange.y * 0.33f) * 10f) / 10f).ToString(), axisStyle);
        GUI.Label(unitsRect[2], (Mathf.Round((maxRange.y * 0.67f) * 10f) / 10f).ToString(), axisStyle);
        GUI.Label(unitsRect[3], (Mathf.Round(maxRange.y * 10f) / 10f).ToString(), axisStyle);

        GUI.Label(unitsRect[4], "0", axisStyle);
        GUI.Label(unitsRect[5], (Mathf.Round((maxRange.x * 0.33f) * 10f) / 10f).ToString(), axisStyle);
        GUI.Label(unitsRect[6], (Mathf.Round((maxRange.x * 0.67f) * 10f) / 10f).ToString(), axisStyle);
        GUI.Label(unitsRect[7], (Mathf.Round(maxRange.x * 10f) / 10f).ToString(), axisStyle);

        // draw labels
        GUI.Label(unitsRect[8], xLabel);
        GUIUtility.RotateAroundPivot(-90, new Vector2(unitsRect[9].x, unitsRect[9].y + 5));
        GUI.Label(unitsRect[9], yLabel);
        GUIUtility.RotateAroundPivot(90, new Vector2(unitsRect[9].x, unitsRect[9].y + 5));

        GUI.EndGroup();
    }

    // calculates rect 
    private Rect CalculateRect(Vector2 point, float width, float height, int imgSize)
    {
        // figure out position of point between ranges on a scale of 0 - maxRange
        fixPosX = (point.x / maxRange.x) * width;
        fixPosY = (1 - point.y / maxRange.y) * height;

        // return new rect
        return new Rect(fixPosX - imgSize / 2, fixPosY - imgSize / 2, imgSize, imgSize);
    }
}