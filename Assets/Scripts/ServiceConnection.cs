using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ServiceConnection : MonoBehaviour
{
    private static ServiceConnection obj;

    void Start()
    {
        obj = this;
    }

    public static ServiceConnection This()
    {
        return obj;
    }

    public void UploadData(String playerName, String className, int timePlayed,
        int levelPlayed, int shotsFired, int tigersHit, int tigersTagged,
        int distanceTraveled, List<TigerData> tigerData)
    {
        WWW servReq;
        WWWForm form = new WWWForm();
        form.AddField("MAKEMAGICHAPPEN", "yesThisMustBeSent");
        form.AddField("playerName", playerName);
        form.AddField("className", className);
        form.AddField("timePlayed", timePlayed);
        form.AddField("levelPlayed", levelPlayed);
        form.AddField("shotsFired", shotsFired);
        form.AddField("tigersHit", tigersHit);
        form.AddField("tigersTagged", tigersTagged);
        form.AddField("distanceTraveled", distanceTraveled);
        
        foreach (TigerData entry in tigerData)
        {
	        Dictionary<String,String> data = entry.dict();
	        
	        String dataField = "{";
	        Boolean first = true;
	        foreach (String type in data.Keys) {
	        	if (first) {
	        		first = false;
	        	} else {
			        dataField += ",";
	        	}
                if (type == "Sex")
                {
                    dataField += "\"" + type + "\":\"" + (data[type] == "0" || data[type] == "female" ? "female" : "male") + "\"";
                }
                else dataField += "\"" + type + "\":\"" + data[type] + "\"";
	        }
	        dataField += "}";
	        form.AddField("tigerData[]", dataField);
        }

        servReq = new WWW(urlLocations.instance.GetServiceUrl(), form);
        StartCoroutine(WaitForRequest(servReq));
    }
    
    IEnumerator WaitForRequest(WWW www) {
    	yield return www;
    	
    	if (www.error == null) {
    		Debug.Log("Submitted!" + www.text);
    	} else {
    		Debug.Log("FAILED: " + www.error);
    	}

        TigerReport.dataSent = true;
    }
}

//public class TigerData {
//	private Dictionary<String,String> dataValues;
	
//	public TigerData() {
//		dataValues = new Dictionary<String,String>();
//	}
	
//	public void setData(String dataType, String dataValue) {
//		dataValues[dataType] = dataValue;
//	}
	
//	public Dictionary<String,String> dict() {
//		return dataValues;
//	}
	
//	public String valueForKey(String dataType) {
//		return dataValues[dataType];
//	}
//}