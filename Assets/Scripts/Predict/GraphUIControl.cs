using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GraphUIControl : MonoBehaviour {

    public GameObject dataPointPrefab, predictLetterPrefab;
    public GameObject dataHolder, predictLetterHolder;
    public List<Text> xAxisTicLabels, yAxisTicLabels;
    public Text xAxisLabel, yAxisLabel;

    private List<GameObject> currentGraphPrefabs, currentPredictLetterPrefabs;
    public List<Color> predictLetterColors;
    // pixel values of the graph
    private Vector2 graphPixelSize;// graphPixelWidth, graphPixelHeight;
    // the x and y axis max values
    private Vector2 graphSize;


	void Start () {
        //InitializeGraphData();

        //List<Vector2> temp = new List<Vector2>();
        //temp.Add(new Vector2(.5f, .5f));
        //temp.Add(new Vector2(14f, 3f));
        //temp.Add(new Vector2(4f, 1.5f));
        //SetupGraph(temp, "xAxisText here", "This is the y axis");
    }

    private void InitializeGraphData()
    {
        if (currentGraphPrefabs == null) currentGraphPrefabs = new List<GameObject>();
			//remove any graph data if we had some
		if (currentPredictLetterPrefabs != null) foreach (GameObject prefabInScene in currentPredictLetterPrefabs) { Destroy(prefabInScene); }
        currentPredictLetterPrefabs = new List<GameObject>();
        graphPixelSize = new Vector2(300f, 200f);
        graphSize = new Vector2(1f, 1f);
	}

    // we are just making a graph. not showing any prediction data
    public void SetupGraph(string xAxisName, string yAxisName, int xCatNum, int yCatNum)
    {
        SetupGraph(xAxisName, yAxisName, xCatNum, yCatNum, null, null);
    }

    // we are making a graph with prediction data!
    public void SetupGraph(string xAxisName, string yAxisName, int xCatNum, int yCatNum, List<int> tigerPredictNums, TigerDataSet predictData)
    {
        List<Vector2> graphData = new List<Vector2>();

        // for each piece of tiger data we have
        for (int i = 0; i < TigerReport.tNum.Count; i++)
        {
            // add it to our list
            graphData.Add( new Vector2(
                (float)TigerDataHolder.CurData().GetTigerStat(xCatNum, (int)TigerReport.tNum[i]),
                (float)TigerDataHolder.CurData().GetTigerStat(yCatNum, (int)TigerReport.tNum[i])
                ));
        }

        SetupGraph(graphData, xAxisName, yAxisName);
        if (predictData != null)
            AddPredictionData(predictData, tigerPredictNums, xCatNum);
    }

    private void SetupGraph(List<Vector2> graphData, string xAxisName, string yAxisName)
    {
        InitializeGraphData();

        // first, if we had graph data already
        if (currentGraphPrefabs.Count != 0)
        {
            //remove any graph data if we had some
            foreach (GameObject prefabInScene in currentGraphPrefabs)
            {
                Destroy(prefabInScene);
            }

            currentGraphPrefabs = new List<GameObject>();
        }

        // next, determine x and y max width and height
        //graphSize = MaxAxisSize(graphData);
        graphSize = MaxAxisSize(xAxisName, yAxisName);

        // set the text for the labels and such
        SetLabels(xAxisName, yAxisName);

        // create a prefab for each of our points of data and place them correctly
        for (int i = 0; i < graphData.Count; i++)
        {
            currentGraphPrefabs.Add(CreateDataPrefab(graphData[i]));
        }
    }

    private void SetLabels(string xName, string yName)
    {
        // set xAxis
        xAxisLabel.text = xName;

        // set xAxis numbers
        for (int x = 0; x < xAxisTicLabels.Count; x++)
        {
            xAxisTicLabels[x].text = (((float)x / (xAxisTicLabels.Count - 1f)) * graphSize.x).ToString();
        }

        // set yAxis
        yAxisLabel.text = yName;

        // set yAxis numbers
        for (int y = 0; y < xAxisTicLabels.Count; y++)
        {
            yAxisTicLabels[y].text = (((float)y / (yAxisTicLabels.Count - 1f)) * graphSize.y).ToString();
        }
    }

    private Vector2 MaxAxisSize(string xCategoryName, string yCategoryName)
    {
        return new Vector2(FetchSize(xCategoryName), FetchSize(yCategoryName));
    }

    private float FetchSize(string catName)
    {
        if (catName == TigerDataHolder.NOSE_BLACK_NAME) return 1f;
        if (catName == TigerDataHolder.PAW_CIRC_NAME) return 8f;
        if (catName == TigerDataHolder.LENGTH_NAME) return 100f;
        if (catName == TigerDataHolder.TAIL_LENGTH_NAME) return 20f;
        if (catName == TigerDataHolder.REGION_NAME) return 8f;
        if (catName == TigerDataHolder.SEX_NAME) return 1f;
        if (catName == TigerDataHolder.AGE_NAME) return 20f;
        if (catName == TigerDataHolder.WEIGHT_NAME) return 500f;

        Debug.LogWarning("Category name: " + catName + " was not found");
        return 1f;
    }

    private Vector2 MaxAxisSize(List<Vector2> graphData)
    {
        // find our largest data size for X and Y
        float xAxis = 1f;
        float yAxis = 1f;
        foreach (Vector2 dataPoint in graphData)
        {
            if (dataPoint.x > xAxis)
                xAxis = dataPoint.x;

            if (dataPoint.y > yAxis)
                yAxis = dataPoint.y;
        }

        // round our values up to get nice even numbers and return our new axis
        return new Vector2(RoundUpAxis(xAxis), RoundUpAxis(yAxis));
    }

    private float RoundUpAxis(float value)
    {
        if (value > 40) value = 100;
        else if (value > 20) value = 40;
        else if (value > 10) value = 20;
        else if (value > 4) value = 10;
        else if (value > 1) value = 4;
        else if (value >= 0) value = 1;

        return value;
    }

    private GameObject CreateDataPrefab(Vector2 dataPoint)
    {
        GameObject temp = (GameObject)Instantiate(dataPointPrefab);
        temp.transform.SetParent(dataHolder.transform);
        temp.transform.localPosition = CalcDataPos(dataPoint, graphSize);
        temp.transform.localScale = new Vector3(1.33f, 1.33f, 1.33f);
        return temp;
    }

    private Vector3 CalcDataPos(Vector2 dataPoint, Vector2 graphSize)
    {
        float x = (dataPoint.x / graphSize.x) * graphPixelSize.x - (graphPixelSize.x / 2f);
        float y = (dataPoint.y / graphSize.y) * graphPixelSize.y - (graphPixelSize.y / 2f);
        return new Vector3(x, y, 0);
    }

    private void AddPredictionData(TigerDataSet predictData, List<int> tigerPredictNums, int xCatNum)
    {
        // for each value in our predictData
        for (int i = 0; i < tigerPredictNums.Count; i++)
        {
            // get out x axis value (depends on which graph we are looking at)
            int tigerPosNum = tigerPredictNums[i];
            float xValue = (float)predictData.GetTigerStat(xCatNum, tigerPosNum);

            // next, figure out where on the graph each value should sit (look at min and max of x axis
            float offset = xValue / graphSize.x * graphPixelSize.x;

            // place a prefab on the bottom of the graph and set the position
            GameObject tempPrefab = (GameObject)GameObject.Instantiate(predictLetterPrefab);
            tempPrefab.transform.SetParent(predictLetterHolder.transform);
            tempPrefab.transform.localPosition = new Vector3(offset, 0, 0);

            // set the text to be the right letter and color
            Text prefabTextComponent = tempPrefab.GetComponent<Text>();
            if (prefabTextComponent != null)
                SetTextAndColor(prefabTextComponent, i);

            // lastly, add prefab to a list so we can delete it later if we need to
            currentPredictLetterPrefabs.Add(tempPrefab);
        }
    }

    private void SetTextAndColor(Text textComponent, int num)
    {
        textComponent.text = GetLetterBasedOnNumber(num);
        textComponent.color = GetColorBasedOnNumber(num);
    }

    private string GetLetterBasedOnNumber(int num)
    {
        if (num == 0) return "A";
        if (num == 1) return "B";
        if (num == 2) return "C";
        if (num == 3) return "D";

        // if we got here, something weird happened
        Debug.LogWarning("We don't have a letter for " + num);
        return "";
    }

    private Color GetColorBasedOnNumber(int num)
    {
        return predictLetterColors[num];
    }
}