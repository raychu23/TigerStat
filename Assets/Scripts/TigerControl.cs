using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TigerControl : MonoBehaviour
{
    private static TigerControl obj;
    public static TigerControl This() { return obj; }
    public GameObject tigerPrefabEasy;
    public GameObject tigerPrefabHard;
    public List<GameObject> tigerCenterSpawns_TigerStat, tigerCenterSpawns_TigerSampling;
    private List<GameObject> curCenterSpawns;
    public TerrainData ourTerrainData;
    public bool tutorialOverride;

    public int numberOfTigers = 60;
    public Vector2 range_TigerStat, range_TigerSampling;
    private Vector2 curVersionRange;
    public List<GameObject> tigers;

    void Start()
    {
        obj = this;

        SetupOurValues();

        if (tutorialOverride) SpawnTutorialTigers();
        else SpawnTigers();
    }

    private void SetupOurValues()
    {
        if (GameSelector.curGameType == GameSelector.GameType.TigerStat)
        {
            curVersionRange = range_TigerStat;
            curCenterSpawns = tigerCenterSpawns_TigerStat;
        }
        else
        {
            curVersionRange = range_TigerSampling;
            curCenterSpawns = tigerCenterSpawns_TigerSampling;
        }
    }

    private void SpawnTutorialTigers()
    {
        tigers = new List<GameObject>();
        tigers.Add(CreateTiger(0, 0));
        tigers.Add(CreateTiger(0, 1));
        tigers.Add(CreateTiger(0, 2));
        tigers.Add(CreateTiger(0, 3));
        tigers.Add(CreateTiger(0, 4));

		StartCoroutine("ReportToDataCheck");
    }

    private void SpawnTigers()
    {
		// how much data do we have?
		int totalDataNum = TigerDataHolder.CurData().GetTotalTigerCount();

        // make a list of tigers we actually want to pull (since we have more data than we have tigers
        List<int> tigerPositions = new List<int>();

        // fill a list with as much data as we have
        for (int i = 0; i < totalDataNum; i++) tigerPositions.Add(i);

        // remove random positions until we have the correct number of data left
        if (totalDataNum > numberOfTigers)
        {
            for (int j = 0; j < (totalDataNum - numberOfTigers); j++)
            {
                tigerPositions.RemoveAt(Random.Range(0, tigerPositions.Count));
            }
        }

        tigers = new List<GameObject>();

		// create our tigers in the correct region!
		for (int newTigerToSpawn = 0; newTigerToSpawn < numberOfTigers; newTigerToSpawn++)
		{
			tigers.Add(CreateTiger(
                // WE DONT WANT A REGION if this is tigerStat.
                GameSelector.curGameType == GameSelector.GameType.TigerStat ? 0 :
				    TigerDataHolder.CurData().GetTigerRegion(tigerPositions[newTigerToSpawn]),
				tigerPositions[newTigerToSpawn]
				));
			//tigers.Add(CreateTiger(0, tigerPositions[newTigerToSpawn]));
		}

		StartCoroutine("ReportToDataCheck");
    }

	private IEnumerator ReportToDataCheck()
	{
		while (DataCheck.instance == null) yield return false;

		DataCheck.instance.SetupTigers(tigers);
	}

    private float _distance, _angle;
    private Vector3 _tempPos;

	private GameObject CreateTiger(int spawnNum, int tigerNum)
	{
		// instantiate tiger
		GameObject tiger = (GameObject)GameObject.Instantiate(MasterObj.GetDifficulty() == 1 ? tigerPrefabEasy : tigerPrefabHard);
		tiger.transform.parent = transform;
		tiger.transform.position = Vector3.zero;
		
		// give tiger its identification numbers
		tiger.GetComponent<TigerAI>().tigerNum = tigerNum;
		tiger.GetComponent<TigerAI>().spawnNum = spawnNum;

		// get random distance / angle from center
        _distance = Random.Range(curVersionRange.x, curVersionRange.y);
		_angle = Random.Range(0, 359);
			
		_tempPos.Set(
                curCenterSpawns[spawnNum].transform.position.x + (_distance * Mathf.Cos(_angle)),
				120,
                curCenterSpawns[spawnNum].transform.position.z + (_distance * Mathf.Sin(_angle)) );
			
			// figure out y position to drop tiger at
        tiger.transform.position = FigureOutY(_tempPos);
		
		return tiger;
	}

    private RaycastHit _hit;

    public Vector3 FigureOutY(Vector3 pos)
    {
        if (Physics.Raycast(pos, -Vector3.up, out _hit, 500))
        {
            //pos = hit.point;

            //return new Vector3(pos.x, pos.y + 2, pos.z); // +1 is an attempt to place them above ground
            return _hit.point;
            // mostly so they dont go flipping out of control
        }

        // we got here?  raycast failed!
        //return awkward number so we know to delete this tiger because wtf
        Debug.Log("OH NO WHAT HAPPENED");
        return Vector3.zero;
    }

    // not currently used
    private Vector3 CheckSlope(Vector3 pos)
    {
        Vector3 pos1 = new Vector3(pos.x, pos.y + 20, pos.z);
        Vector3 pos2 = new Vector3(pos1.x, pos1.y, pos1.z + 1);
        Vector3 pos3 = new Vector3(pos1.x + 1, pos1.y, pos1.z);

        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;

        Physics.Raycast(pos1, -Vector3.up, out hit1, 40);
        Physics.Raycast(pos2, -Vector3.up, out hit2, 40);
        Physics.Raycast(pos3, -Vector3.up, out hit3, 40);

        if (Vector3.Angle(hit1.point, hit2.point) > .1f || Vector3.Angle(hit1.point, hit3.point) > .1f)
            return Vector3.zero;

        return pos;
    }
}