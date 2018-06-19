using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionIndicator : MonoBehaviour {
    
    public void OnTriggerStay(Collider col)
    {
        if (gameObject.transform.parent.transform.parent.tag == "Troop")
        {
            Troop troop = gameObject.transform.parent.transform.parent.GetComponent<Troop>();
            if (col.gameObject.transform.parent.gameObject.tag == "Grid")
            {
                if (troop.person.faction == Faction.mercenary) //reveal map if owner of this indicator is player troop
                {
                    col.gameObject.transform.parent.GetComponent<GridObject>().becomeSeen();
                }
                col.gameObject.transform.parent.GetComponent<GridObject>().checkTroopOnGrid(troop);
            }

        }
        
    }

    public void OnTriggerExit(Collider col)
    {
        if (gameObject.transform.parent.transform.parent.tag == "Troop")
        {
            if (col.gameObject.transform.parent.gameObject.tag == "Grid")
            {
                //col.gameObject.transform.parent.GetComponent<GridObject>().becomeUnseen();
            }
        }
    }
}
