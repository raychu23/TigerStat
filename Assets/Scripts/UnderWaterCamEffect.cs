using UnityEngine;
using System.Collections;

public class UnderWaterCamEffect : MonoBehaviour
{
    public GameObject waterCamPlane;

    void Start()
    {
        StartCoroutine("CheckCamPos");
    }

    private IEnumerator CheckCamPos()
    {
        while (true)
        {
            if (transform.position.y <= 5f)
            {
                if (!waterCamPlane.activeInHierarchy)
                    waterCamPlane.SetActive(true);
            }
            else
            {
                if (waterCamPlane.activeInHierarchy)
                    waterCamPlane.SetActive(false);
            }

            yield return false;
        }
    }
}