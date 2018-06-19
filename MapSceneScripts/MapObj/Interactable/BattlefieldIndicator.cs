using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldIndicator : MonoBehaviour {
    public BattlefieldType battlefieldType;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "NPC")
        {
            Party npcParty = col.gameObject.GetComponent<NPC>().npcParty;
            if (npcParty.battlefieldTypes != null && !npcParty.battlefieldTypes.Contains(battlefieldType))
            {
                npcParty.battlefieldTypes.Add(battlefieldType);
            }
        }
        if (col.gameObject.tag == "Player")
        {
            if (!Player.mainParty.battlefieldTypes.Contains(battlefieldType)){
                Player.mainParty.battlefieldTypes.Add(battlefieldType);
            }
            
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "NPC")
        {
            Party npcParty = col.gameObject.GetComponent<NPC>().npcParty;
            if (npcParty.battlefieldTypes != null && npcParty.battlefieldTypes.Contains(battlefieldType))
            {
                npcParty.battlefieldTypes.Remove(battlefieldType);
            }
        }
        if (col.gameObject.tag == "Player")
        {
            if (Player.mainParty.battlefieldTypes.Contains(battlefieldType))
            {
                Player.mainParty.battlefieldTypes.Remove(battlefieldType);
            }

        }
    }
}
