using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityNameTag : MonoBehaviour {
    public Text nameTag;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = Camera.main.transform.position - gameObject.transform.position;
        v.x = v.z = 0.0f;
        gameObject.transform.LookAt(Camera.main.transform.position - v);
        gameObject.transform.Rotate(0, 180, 0);
    }
}
