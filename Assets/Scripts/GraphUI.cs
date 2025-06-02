using UnityEngine;
using System.Collections;

public class GraphUI: MonoBehaviour
{
    public Texture2D dataPointTex, graphTex;
    public static bool setupGraph;

    private DataGraph[] testGraph;

    public GUIStyle labelStyle;

    public void Start()
    {
        setupGraph = false;

//        StartCoroutine("TestDataGrab");
    }

    private IEnumerator TestDataGrab()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = 0; i < 5; i++)
                {
                    TigerReport.DataCollected(Random.Range(0, 100));
                }
            }

            yield return false;
        }
    }

    private void SetupOurGraph()
    {
        setupGraph = false;
        testGraph = new DataGraph[TigerDataHolder.CurData().graphData.Count];

        // for each graph that we are drawing
        for (int i = 0; i < TigerDataHolder.CurData().graphData.Count; i++)
        {
            Vector2[] newData = new Vector2[TigerReport.tNum.Count];
            // for each piece of data we have
            for (int j = 0; j < newData.Length; j++)
            {
                newData[j] = TigerDataHolder.CurData().GetGraphSet(i, (int)TigerReport.tNum[j]);
            }

            // if we have no data yet, still turn in something so we can draw a graph
            if (newData.Length == 0) newData = new Vector2[] {new Vector2(0, 0)};
            testGraph[i] = new DataGraph(
                newData,
                TigerDataHolder.CurData().GetGraphCat(i)[0],
                TigerDataHolder.CurData().GetGraphCat(i)[1],
                dataPointTex);
        }
    }

    public void DrawGUI()
    {
        if (setupGraph)
            SetupOurGraph();

        for (int i = 0; i < testGraph.Length; i++)
        {
            if (testGraph.Length > 0)
            {
                testGraph[i].Draw(new Vector2(230 + (i % 2) * 290, 110 + (i / 2) * 250), graphTex, labelStyle);
            }
        }
    }
}