using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyVisionIndicator : MonoBehaviour {

    public void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "NPC")
        {
            col.GetComponent<NPC>().showSelf();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "NPC")
        {
            col.GetComponent<NPC>().hideSelf();
        }
    }
}
