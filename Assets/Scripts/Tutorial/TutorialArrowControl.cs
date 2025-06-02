using UnityEngine;
using System.Collections;

public class TutorialArrowControl : MonoBehaviour {
    private static bool flag = false;

    void Start()
    {
        flag = false;
    }

    void Update()
    {
        if (flag)
            Destroy(this.gameObject);
    }

    public static void TargetHit()
    {
        flag = true;
    }
}
