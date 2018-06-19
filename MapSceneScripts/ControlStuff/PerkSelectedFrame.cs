using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkSelectedFrame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (MapManagement.mapManagement != null)
        {
            if (Player.mainCharacter.skillTree.getPerk(gameObject.transform.parent.gameObject.name).own
            && !gameObject.GetComponent<Image>().enabled)
            {
                gameObject.GetComponent<Image>().enabled = true;
            }
            if (!Player.mainCharacter.skillTree.getPerk(gameObject.transform.parent.gameObject.name).own
                && gameObject.GetComponent<Image>().enabled)
            {
                gameObject.GetComponent<Image>().enabled = false;
            }
        }
        
    }

}
