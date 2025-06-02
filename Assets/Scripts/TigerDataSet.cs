using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TigerDataSet
{
    public List<string> categories;
    public List<Vector2> graphData;
    public Dictionary<string, List<double>> tData;
	//public List<double> areaSize;
    private int totalDataCount;
	private int totalTigerCount;


    public TigerDataSet(List<string> cat, List<Vector2> gDat, List<List<double>> dat)
    {
        tData = new Dictionary<string,List<double>>();
        categories = cat;
		totalTigerCount = dat.Count;
        totalDataCount = dat[0].Count;

		//areaSize = new List<double>();

//		Debug.Log(dat.GetLength(0));
		for (int dataNumber = 0; dataNumber < totalDataCount; dataNumber++)
        {
            List<double> list = new List<double>();

			for (int tigerNumber = 0; tigerNumber < totalTigerCount; tigerNumber++)
            {
				//list.Add(dat[i][j]);
				list.Add(dat[tigerNumber][dataNumber]);
                //// only do this once. a hack, i know
                //if (i == 0)
                //{
                //    if (dat[j, 0] >= areaSize.Count)
                //    {
                //        areaSize.Add(1);
                //    }
                //    else
                //    {
                //        //Debug.Log((int)dat[j, 0]);
                //        areaSize[(int)dat[j, 0]]++;
                //    }
                //}
            }

            tData[cat[dataNumber]] = list;
        }

        graphData = gDat;
    }

    public List<double> GetTigerStats(int foo)
    {
        return tData[categories[foo]];
    }

    public double GetTigerStat(int catNum, int tigerNum)
    {
        return GetTigerStats(catNum)[tigerNum];
    }

    public int GetTigerRegion(int tigerNum)
    {
        return (int)GetTigerStat(0, tigerNum);
    }

	public string GetTigerStatAsNiceData(int catNum, int tigerNum, bool dataToBeSent)
	{
		double statData = GetTigerStat(catNum, tigerNum);

		if (categories[catNum] == "Sex")
		{
			return (statData == 0 ? "female" : "male");
		}

		if (statData == -500)
			return (dataToBeSent ? "" : "Unavailable");

		return (Mathf.Round((float)statData * 100) / 100).ToString();
	}

	public int GetTotalTigerCount()
	{
		return tData[categories[0]].Count;
	}

    public Vector2 GetGraphSet(int graphNum, int tigerNum)
    {
        // oh god what happened here
        // we get which graph we are looking at and which number tiger we want info from
        // we want to send out two pieces of data from that tiger which our graph needs
        // new vector2 ( tigernum(first piece of data according to our graph), tigernum(second piece of data according to our graph))
		Vector2 dataSet = new Vector2((float)GetTigerStat(GetGraphCatNum(graphNum, 0), tigerNum), (float)GetTigerStat(GetGraphCatNum(graphNum, 1), tigerNum));
		if (dataSet.x == -500 || dataSet.y == -500)
			return new Vector2();

		return dataSet;
    }

    public string[] GetGraphCat(int graphNum)
    {
        return new string[] { categories[(int)graphData[graphNum].x], categories[(int)graphData[graphNum].y] };
    }

    public int GetGraphCatNum(int graphNum, int axisNum)
    {
        //Debug.Log(graphNum + "  " + axisNum + "  " + graphData[graphNum]
        if (axisNum == 0) return (int)graphData[graphNum].x;
        return (int)graphData[graphNum].y;
    }

    public string ProperCategory(int foo)
    {
        string catName = "";

        switch (categories[foo])
        {
            case "Age":
                catName = "tiger_age[]";
                break;
            case "NoseBlack":
                catName = "tiger_nose[]";
                break;
            case "PawCircumference":
                catName = "tiger_paw[]";
                break;
            case "TailLength":
                catName = "tiger_tail[]";
                break;
            default:
                catName = "tiger_missingCategory";
                break;
        }
        return catName;
    }

    //public int GetTotalTigerCount()
    //{
    //    return totalDataCount;
    //}
}