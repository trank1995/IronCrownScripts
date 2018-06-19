using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bandits : NPC {
    public GameObject spawnPoint;
    
    // Use this for initialization
    public override void Start () {
        base.Start();
        if (MapManagement.mapManagement == null || !MapManagement.parties.Contains(npcParty))
        {
            gameObject.SetActive(false);
        }
        

    }
    // Update is called once per frame
    public override void Update () {
        if (npcParty != null)
        {
            base.Update();
            
        }
	}

    public override void initialization()
    {
        base.initialization();
        if (MapManagement.mapManagement != null)
        {
            npcParty.prestige = 0;
            npcParty.notoriety = 80;
            makeParty();
            //roam();
        }
    }
    public override Vector3 getRoamTarget()
    {
        return base.getRoamTarget();
    }
    public void setSpawnPoint(GameObject sp)
    {
        spawnPoint = sp;
    }
    public override void grow()
    {
        base.grow();
    }
    public override void makeParty()
    {
        for (int i = 0; i < npcParty.getPartySizeLimit(); i ++)
        {
            TroopType tt = npcParty.randomTroopType(20, 20, 10, 30, 10, 10);
            Ranking rk = npcParty.randomRanking(0, 10, 10, 10);
            if (tt == TroopType.recruitType)
            {
                rk = Ranking.recruit;
            }
            Person p = npcParty.makeGenericPerson(tt, rk);
            if (npcParty.battleValue >= p.battleValue)
            {
                if (npcParty.addToParty(npcParty.makeGenericPerson(tt, rk)))
                {
                    npcParty.battleValue -= p.battleValue;
                }
            }
        }
        if (npcParty.battleValue > 0)
        {
            foreach(Person unit in npcParty.partyMember)
            {
                TroopType tt = unit.troopType;
                Ranking rk = unit.ranking;
                if (unit.ranking == Ranking.recruit)
                {
                    tt = npcParty.randomTroopType(0, 20, 10, 30, 10, 10);
                    rk = npcParty.randomRanking(0, 10, 10, 10);
                } else if (unit.ranking == Ranking.militia)
                {
                    rk = npcParty.randomRanking(0, 0, 10, 10);
                } else if (unit.ranking == Ranking.veteran)
                {
                    rk = Ranking.elite;
                }
                npcParty.battleValue = unit.changeRankingTroopType(rk, tt, npcParty.battleValue, true);
            }
        }
    }
}
