using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour {
    public Text infoText;
    static string curInfo;
    static bool infoChanged;
	// Use this for initialization
	void Start () {
        infoText.text = "";
        infoChanged = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (infoChanged)
        {
            infoText.text = curInfo;
            infoChanged = false;
        }
	}
    public static void clearInfo()
    {
        if (curInfo != "")
        {
            infoChanged = true;
            curInfo = "";
        }
    }
    public static void displayInfo(string info)
    {
        if (curInfo != info)
        {
            infoChanged = true;
            curInfo = info;
        }
    }
}
