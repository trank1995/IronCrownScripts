using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MainParty : Party {
    public FactionPerkTree factionPerkTree;
    public List<Quest> unfinishedQuests, finishedQuests;
    public MainParty()
    {

    }
    public MainParty(Person leaderI, string nameI, Faction factionI, int battleValueI)
    {
        unique = true;
        leader = leaderI;
        name = nameI;
        faction = factionI;
        partyMember = new List<Person>();
        inventory = new List<Item>();
        unfinishedQuests = new List<Quest>();
        finishedQuests = new List<Quest>();
        addToParty(leader);
        battleValue = battleValueI;
        PartyInitialization();
    }
    public override void PartyInitialization()
    {
        base.PartyInitialization();
        factionPerkTree = new FactionPerkTree();
        
    }
    public override void plusPrestige(int toAdd)
    {
        base.plusPrestige(toAdd);
    }
    public override void plusNotoriety(int toAdd)
    {
        base.plusNotoriety(toAdd);
    }
    public override int getPartySizeLimit()
    {
        return base.getPartySizeLimit();
    }
    public override float getInventoryWeight()
    {
        return base.getInventoryWeight();
    }
    public override int getInventoryValue()
    {
        return base.getInventoryValue();
    }
    public override float getTravelSpeed()
    {
        
        return base.getTravelSpeed() + 4.0f;
    }
    public override float getVisionRange()
    {
        return base.getVisionRange();
    }
    public override float getTaticRating()
    {
        return base.getTaticRating();
    }
    public override float getConvinceRating()
    {
        return base.getConvinceRating();
    }
    public override float getInventoryWeightLimit()
    {
        return base.getInventoryWeightLimit();
    }
    public Quest getQuest(string id)
    {
        foreach(Quest q in unfinishedQuests)
        {
            if (q.questID == id)
            {
                return q;
            }
        }
        foreach (Quest q in finishedQuests)
        {
            if (q.questID == id)
            {
                return q;
            }
        }
        return null;
    }

    //SAME
    public override bool addToInventory(Item item)
    {
        return base.addToInventory(item);
    }
    public override bool addToParty(Person member)
    {
        return base.addToParty(member);
    }
    public override int getAverageLevel()
    {
        return base.getAverageLevel();
    }
    public override bool removeFromInventory(Item item)
    {
        return base.removeFromInventory(item);
    }
    public override bool removeFromParty(Person member)
    {
        return base.removeFromParty(member);
    }

}

public class FactionPerkTree {
    public Dictionary<string, Perk> perkTreeDict;
    public FactionPerkTree()
    {
        skillTreeInitialization();
    }

    public void skillTreeInitialization()
    {
        perkTreeDict = new Dictionary<string, Perk>();

        perkTreeDict.Add("EMP+1", new Perk("EMP+1", false, "Printing Press", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("EMP+2", new Perk("EMP+2", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("EMP+3", new Perk("EMP+3", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("EMP-1", new Perk("EMP-1", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("EMP-2", new Perk("EMP-2", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("EMP-3", new Perk("EMP-3", false, "Indulgence", "Recruit cheaper", "nothing right now"));

        perkTreeDict.Add("PAP+1", new Perk("PAP+1", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAP+2", new Perk("PAP+2", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAP+3", new Perk("PAP+3", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAP-1", new Perk("PAP-1", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAP-2", new Perk("PAP-2", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAP-3", new Perk("PAP-3", false, "Indulgence", "Recruit cheaper", "nothing right now"));

        perkTreeDict.Add("FRA+1", new Perk("FRA+1", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FRA+2", new Perk("FRA+2", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FRA+3", new Perk("FRA+3", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FRA-1", new Perk("FRA-1", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FRA-2", new Perk("FRA-2", false, "Indulgence", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FRA-3", new Perk("FRA-3", false, "Indulgence", "Recruit cheaper", "nothing right now"));

        perkTreeDict.Add("MIL", new Perk("MIL", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("TOR", new Perk("TOR", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("AST", new Perk("AST", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAR", new Perk("PAR", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("GEN", new Perk("GEN", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("MOD", new Perk("MOD", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("VER", new Perk("VER", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PAD", new Perk("PAD", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("TRE", new Perk("TRE", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("VEN", new Perk("VEN", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FER", new Perk("FER", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("BOL", new Perk("BOL", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("FIR", new Perk("FIR", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("RAV", new Perk("RAV", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("URB", new Perk("URB", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("LUC", new Perk("LUC", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PIS", new Perk("PIS", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("SIE", new Perk("SIE", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("GRO", new Perk("GRO", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("PER", new Perk("PER", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
        perkTreeDict.Add("ROM", new Perk("ROM", false, "Condottieri Duke", "Recruit cheaper", "nothing right now"));
    }
    public Perk getPerk(string ID)
    {
        if (perkTreeDict.ContainsKey(ID))
        {
            return perkTreeDict[ID];
        }
        return null;
    }
}

