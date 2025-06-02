using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TigerDataHolder : MonoBehaviour
{
    public static TigerDataSet[] dataSet;
    private static int curSet;
    public static bool initialized = false;

    void Start()
    {
        initialized = false;
        DontDestroyOnLoad(this.gameObject);
        curSet = 0;
        dataSet = new TigerDataSet[] {
            GetDataSet("TigerStat_Data01"),
			GetDataSet("TigerStat_Data02"),
           // GetDataSet("TigerSampling_Data01"),
           // GetDataSet("TigerSampling_Data02")
			//DataSet3()
        };
        initialized = true;
    }

    public static void SetData(int foo)
    {
		Debug.Log("dataset length: " + dataSet.Length);
        if (dataSet.Length < foo)
        {
            Debug.LogError("Data number doesn't exist!");
            curSet = 0;
        }

        if (GameSelector.curGameType == GameSelector.GameType.TigerStat)
        {
            curSet = foo;
        }
        else// if (GameSelector.curGameType == GameSelector.GameType.TigerSampling)
        {
            curSet = foo + 2;
        }
    }

    public static TigerDataSet CurData()
    {
        return dataSet[curSet];
    }

    public static int TotalDataSets()
    {
        return dataSet.Length;
    }

	private TigerDataSet GetDataSet(string dataName)
	{
		TextAsset dataToLoad = (TextAsset)Resources.Load("TigerData/" + dataName); // Load the tigerData from resources 

		return DeserializeCSV(dataToLoad);
	}

	/// <summary>
	/// Deserializes the CSV file that contains dialogue.
	/// </summary>
	/// <param name="textToDeserialize">The dialogue text to deserialize.</param>
	public TigerDataSet DeserializeCSV(TextAsset textToDeserialize)
	{
		//Split the file from the new line character
		var csvFileContents = textToDeserialize.text.Split ('\n');

		//empty tiger data set to hold data
		List<string> categories = new List<string>();
		List<Vector2> gDat = new List<Vector2>();
		List<List<double>> dat = new List<List<double>>();

		int counter = 0;

		//Iterate through the text array 
		foreach(string dataElement in csvFileContents)
		{
			if(dataElement != "")
			{
				//Split the dialogue element by tabs 
				var dataValues = dataElement.Split('\t');



				// error handling
				if (dataValues[0] == "") break;


				// if this is our first row, it is our categories
				// CREATE CATEGORIES FROM THIS DATA
				if (counter == 0) 
				{
					foreach (string category in dataValues)
					{
						if (category != "")
							categories.Add(category.Trim());
					}
				}

				// otherwise, we are creating our data
				// CREATE TIGERDATA FROM THIS DATA
				else
				{
					List<double> tempDataHolder = new List<double>();

					foreach (string data in dataValues)
					{
						string fixedData = data.Trim();
						if (fixedData == "female")
							tempDataHolder.Add(0);
						else if (fixedData == "male")
							tempDataHolder.Add(1);
						else if (fixedData == "")
							tempDataHolder.Add(-500);
						else
						{
							tempDataHolder.Add(double.Parse(fixedData));
						}
					}

					dat.Add(tempDataHolder);
				}
			}

			counter++;
		}

		// before we are done, make our graph data (in a stupid way! fix this later)

		gDat = new List<Vector2>();
        int curYGraph;
        int curXGraph;

        if (GameSelector.curGameType == GameSelector.GameType.TigerStat)
            curYGraph = 0;
        else
            curYGraph = 1;

        curXGraph = curYGraph + 1;

		// example of what this does
		// makes 4 sets of graphs
		// if we have 4 categories, it should make:
		// 0,1		0,2		0,3		1,2
		// 
		// if we have 6 categories, it should make:
		// 0,1		0,2		0,3		0,4
		//
		// if we have 3 categories, it should make: JEFF NOTE, THIS WOULD BREAK EVERYTHING
		// 0,1		0,2		1,2		ERROR


		for (int graphCount = 0; graphCount < 4; graphCount++)
		{
			if (curXGraph < categories.Count)
			{
				gDat.Add(new Vector2(curXGraph, curYGraph));
				curXGraph++;
			}
			else
			{
				curYGraph++;
				curXGraph = curYGraph+1;
				gDat.Add(new Vector2(curXGraph, curYGraph));
			}
		}

		return new TigerDataSet(categories, gDat, dat);
	}

    public static string NOSE_BLACK_NAME = "NoseBlack";
    public static string PAW_CIRC_NAME = "PawCircumference";
    public static string LENGTH_NAME = "Length";
    public static string REGION_NAME = "Region";
    public static string SEX_NAME = "Sex";
    public static string AGE_NAME = "Age";
    public static string WEIGHT_NAME = "Weight";
    public static string TAIL_LENGTH_NAME = "TailLength";
	
}