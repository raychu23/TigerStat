using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataCheck : MonoBehaviour
{
	public static DataCheck instance;
    private List<GameObject> tigers;
	private List<TigerAI> tigerAIs;

    private static bool grabFlag;
    private float dataDistance;

    // Use this for initialization
    void Start()
    {
		instance = this;
        dataDistance = 6;
        grabFlag = false;
	}

	public void SetupTigers(List<GameObject> tigerObjectsInScene)
	{
		tigers = new List<GameObject>();
		tigerAIs = new List<TigerAI>();

		foreach (GameObject tigerInScene in tigerObjectsInScene)
		{
			tigers.Add(tigerInScene);
			tigerAIs.Add(tigerInScene.GetComponent<TigerAI>());
     	}

        StartCoroutine("CheckTigerProximity");
    }

    private IEnumerator CheckTigerProximity()
    {
        while (true)
        {
            //foreach (GameObject tiger in tigers)
			for (int tigerNum = 0; tigerNum < tigers.Count; tigerNum++)
            {
                if (Vector3.Distance(transform.position, tigers[tigerNum].transform.position) < dataDistance)
                    if (tigerAIs[tigerNum].curState == TigerAI.tigerState.DataPending)
                    {
                        if (grabFlag)
                        {
                            GetThatData(tigers[tigerNum], tigerNum);
                        }
                    }
            }

            grabFlag = false;

            yield return false;
        }
    }

    public static void GrabbingData()
    {
        grabFlag = true;
    }

    private void GetThatData(GameObject tiger, int arrayNum)
    {
        if (!AnimControl.This().anim.IsPlaying("notes"))
        {
            InputControl.lockMovement = true;
            AnimControl.This().PlayAnim("notes");
            StartCoroutine("WaitForAnim");
        }

        tigerAIs[arrayNum].curState = TigerAI.tigerState.Dead;
        //tiger.GetComponentInChildren<Billboard>().DataHasBeenCollected();
        tiger.GetComponentInChildren<TigerTag>().DisplayData(tiger.GetComponent<TigerAI>().tigerNum);
        // other data grabbing stuff would go in here
		TigerReport.DataCollected(tigerAIs[arrayNum].tigerNum);
    }

    private IEnumerator WaitForAnim()
    {
        while (AnimControl.This().anim.IsPlaying("notes"))
        {
            yield return false;
        }

        InputControl.lockMovement = false;
    }
}