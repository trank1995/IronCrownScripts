using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyVisionIndicator : MonoBehaviour {
    List<Party> partiesInVision = new List<Party>() ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }
    public void setVisionRange(float range)
    {
        transform.localScale = new Vector3(range, .5f, range);
    }
    public List<Party> getPartiesInRange()
    {
        return partiesInVision;
    }
    public void reLook()
    {
        partiesInVision.Clear();
    }
    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "NPC")
        {
            Party party = col.gameObject.GetComponent<NPC>().npcParty;
            if (!partiesInVision.Contains(party) && (MapManagement.parties.Contains(party) || party == Player.mainParty))
            {
                partiesInVision.Add(party);
            }
        }
        if (col.gameObject.transform.tag == "Player")
        {
            if (!partiesInVision.Contains(Player.mainParty))
            {
                partiesInVision.Add(Player.mainParty);
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        //Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "NPC")
        {
            Party party = col.gameObject.GetComponent<NPC>().npcParty;
            if (!partiesInVision.Contains(party) && (MapManagement.parties.Contains(party) || party == Player.mainParty))
            {
                partiesInVision.Add(party);
            }
        }
        if (col.gameObject.tag == "Player")
        {
            if (!partiesInVision.Contains(Player.mainParty))
            {
                partiesInVision.Add(Player.mainParty);
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "NPC")
        {
            Party party = col.gameObject.GetComponent<NPC>().npcParty;
            if (partiesInVision.Contains(party))
            {
                partiesInVision.Remove(party);
            }
        }
        if (col.gameObject.tag == "Player")
        {
            if (partiesInVision.Contains(Player.mainParty))
            {
                partiesInVision.Remove(Player.mainParty);
            }
        }
    }

    
}
