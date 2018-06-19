using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : System.Object
{
    const float maxTravelSpeed = 8.0f;
    public string name { get; set; }
    public Faction faction { get; set; }
    public Person leader { get; set; }
    public List<Person> partyMember { get; set; }
    public List<Item> inventory { get; set; }
    public int battleValue { get; set; }
    public int morale { get; set; }
    public int cash { get; set; }
    public int expToDistribute { get; set; }
    public Dictionary<Faction, int> factionFavors;
    public Dictionary<string, int> locationFavors;
    public int prestige, notoriety;
    public float inventoryWeightLimit, inventoryWeight;
    public bool unique, hasShape;
    public int battling;
    public List<BattlefieldType> battlefieldTypes;
    public string locationName;
    public City belongedCity;
    public Town belongedTown;
    System.Action disbandAction;
    //public 
    public Vector3 position;

    public Party()
    {
        partyMember = new List<Person>();
        inventory = new List<Item>();
    }
    public Party(Person leaderI, string nameI, Faction factionI, int battleValueI)
    {
        unique = true;
        leader = leaderI;
        name = nameI;
        faction = factionI;
        partyMember = new List<Person>();
        inventory = new List<Item>();

        addToParty(leader);
        battleValue = battleValueI;
        PartyInitialization();
        hasShape = true;
    }
    public Party(string nameI, Faction factionI, int battleValueI) //generic parties
    {
        unique = false;
        name = nameI;
        faction = factionI;
        leader = makeGenericPerson(randomTroopType(0, 20, 20, 20, 20, 20), randomRanking(0, 10, 10, 10));
        leader.stats.charisma = Random.Range(3, 7);
        leader.stats.intelligence = Random.Range(3, 7);
        leader.exp.level += (leader.stats.intelligence + leader.stats.charisma - 2);
        partyMember = new List<Person>();
        inventory = new List<Item>();
        addToParty(leader);
        battleValue = battleValueI;
        PartyInitialization();
        hasShape = true;
        MapManagement.parties.Add(this);
    }

    public virtual void PartyInitialization()
    {
        morale = 50;
        prestige = 0;
        notoriety = 0;
        expToDistribute = 0;
        locationName = "";
        battlefieldTypes = new List<BattlefieldType>();
        locationFavors = new Dictionary<string, int>();
        locationFavors.Add("Milano", 0);
        locationFavors.Add("Torino", 0);
        locationFavors.Add("Asti", 0);
        locationFavors.Add("Parma", 0);
        locationFavors.Add("Genova", 0);
        locationFavors.Add("Modena", 0);
        locationFavors.Add("Verona", 0);
        locationFavors.Add("Padova", 0);
        locationFavors.Add("Treviso", 0);
        locationFavors.Add("Venezia", 0);
        locationFavors.Add("Ferrara", 0);
        locationFavors.Add("Bologna", 0);
        locationFavors.Add("Firenze", 0);
        locationFavors.Add("Ravenna", 0);
        locationFavors.Add("Urbino", 0);
        locationFavors.Add("Lucca", 0);
        locationFavors.Add("Pisa", 0);
        locationFavors.Add("Siena", 0);
        locationFavors.Add("Grosseto", 0);
        locationFavors.Add("Perugia", 0);
        locationFavors.Add("Roma", 0);
        initializeFavors();
    }

    public virtual bool addToParty(Person member)
    {
        if (partyMember.Count < getPartySizeLimit())
        {
            partyMember.Add(member);
            return true;
        }
        return false;
    }
    public virtual bool removeFromParty(Person member)
    {
        if (partyMember.Remove(member))
        {
            return true;
        }
        return false;
    }
    public virtual bool addToInventory(Item item)
    {
        if (getInventoryWeight() < getInventoryWeightLimit())
        {
            inventory.Add(item);
            inventoryWeight += item.getWeight();
            return true;
        }
        return false;
    }
    public virtual bool removeFromInventory(Item item)
    {
        if (inventory.Remove(item))
        {
            inventoryWeight += item.getWeight();
            return true;
        }
        return false;
    }
    public virtual void plusPrestige(int toAdd)
    {
        prestige += toAdd;
        Mathf.Clamp(prestige, 0, 100);
    }
    public virtual void plusNotoriety(int toAdd)
    {
        notoriety += toAdd;
        Mathf.Clamp(notoriety, 0, 100);
    }
    public virtual int getFactionFavor(Faction f)
    {
        if (factionFavors.ContainsKey(f))
        {
            return factionFavors[f];
        }
        else
        {
            return 0;
        }
    }
    public virtual int getDefeatAmount(int troopOnField)
    {
        return (int)(leader.stats.charisma * troopOnField / 100);
    }

    public virtual void plusFactionFavor(Faction f, int amount)
    {
        factionFavors[f] += amount;
    }
    public virtual int getPartySizeLimit()
    {
        return leader.stats.charisma * 4 + 5;
    }
    public virtual float getTravelSpeed()
    {
        float travelSpeed = 3 + maxTravelSpeed * (getAverage().agility / 10.0f) - 0.1f * (partyMember.Count + .1f * getInventoryWeight());
        if (battlefieldTypes != null && battlefieldTypes.Contains(BattlefieldType.Woods))
        {
            travelSpeed = travelSpeed * .2f;
        }
        Mathf.Clamp(travelSpeed, 1, 10);
        return travelSpeed;
    }
    public virtual int changeMorale(int toChange)
    {
        morale += toChange;
        return morale;
    }
    public virtual float getVisionRange()
    {
        return (leader.stats.perception + getHighest().perception) * 3.0f;
    }
    public virtual float getTaticRating()
    {
        return (leader.stats.intelligence + getHighest().intelligence) * .005f;
    }
    public virtual float getConvinceRating()
    {
        return (leader.stats.charisma + getHighest().charisma) * .005f;
    }
    public virtual float getInventoryWeightLimit()
    {
        if (hasShape)
        {
            return (getAverage().strength + getAverage().endurance) * 10.0f;
        }
        else
        {
            return Mathf.Infinity;
        }

    }
    public virtual int getBattleValue()
    {
        int curBattleValue = 0;
        foreach (Person p in partyMember)
        {
            curBattleValue += TroopDataBase.troopDataBase.getBattleValue(p.faction, p.troopType, p.ranking);
        }
        return curBattleValue;
    }
    public virtual int getInventoryValue()
    {
        int result = 0;
        foreach (Item item in inventory)
        {
            result += item.value;
        }
        return result;
    }
    public virtual float getInventoryWeight()
    {
        float result = 5;
        if (inventory.Count > 0)
        {
            foreach (Item item in inventory)
            {
                result += item.getWeight();
            }
        }
        return result;
    }
    public virtual int getRequiredBribe(Faction f)
    {
        return (int)((getBattleValue() / 100) + factionFavors[f]);
    }
    public virtual int getAverageLevel()
    {
        int result = 0;
        foreach (Person p in partyMember)
        {
            result += p.exp.level;
        }
        if (partyMember.Count > 0)
        {
            return result / partyMember.Count;
        }
        else
        {
            return 0;
        }

    }
    public virtual bool inventoryContains(string itemName, int amount)
    {
        return true;
        foreach (Item i in inventory)
        {
            if (itemName == i.name)
            {
                amount -= 1;
            }
            if (amount <= 0)
            {
                return true;
            }
        }
        return false;

    }

    public virtual void setPartyDisband(System.Action a)
    {
        disbandAction = a;
    }
    public virtual void partyDisband()
    {
        if (disbandAction != null)
        {
            disbandAction.Invoke();
        }

    }
    public float getMaxHealth()
    {
        float max = 1000;
        foreach (Person p in partyMember)
        {
            if (p.getHealthMax() > max)
            {
                max = p.getHealthMax();
            }
        }
        return max;
    }
    public float getMaxStamina()
    {
        float max = 200;
        foreach (Person p in partyMember)
        {
            if (p.getStaminaMax() > max)
            {
                max = p.getStaminaMax();
            }
        }
        return max;
    }
    public bool electNewLeader()
    {
        if (partyMember.Count > 0)
        {
            int highestLevel = 0;
            foreach (Person p in partyMember)
            {
                if (p.exp.level > highestLevel)
                {
                    leader = p;
                    highestLevel = leader.exp.level;
                }

            }
            return true;
        }
        return false;
    }
    public Stats getAverage()
    {
        Stats result = new Stats(0, 0, 0, 0, 0, 0);
        if (partyMember.Count > 0)
        {
            foreach (Person p in partyMember)
            {
                result.strength += p.stats.strength;
                result.agility += p.stats.agility;
                result.perception += p.stats.perception;
                result.endurance += p.stats.endurance;
                result.charisma += p.stats.charisma;
                result.intelligence += p.stats.intelligence;
            }
            result.strength = result.strength / partyMember.Count;
            result.agility = result.agility / partyMember.Count;
            result.perception = result.perception / partyMember.Count;
            result.endurance = result.endurance / partyMember.Count;
            result.charisma = result.charisma / partyMember.Count;
            result.intelligence = result.intelligence / partyMember.Count;
        }

        return result;
    }
    public Stats getHighest()
    {
        Stats result = new Stats(leader.stats.strength, leader.stats.agility,
            leader.stats.perception, leader.stats.endurance,
            leader.stats.charisma, leader.stats.intelligence);
        foreach (Person p in partyMember)
        {
            if (p.stats.strength > result.strength)
            {
                result.strength = p.stats.strength;
            }
            if (p.stats.agility > result.agility)
            {
                result.agility = p.stats.agility;
            }
            if (p.stats.perception > result.perception)
            {
                result.perception = p.stats.perception;
            }
            if (p.stats.endurance > result.endurance)
            {
                result.agility = p.stats.agility;
            }
            if (p.stats.charisma > result.charisma)
            {
                result.charisma = p.stats.charisma;
            }
            if (p.stats.intelligence > result.intelligence)
            {
                result.intelligence = p.stats.intelligence;
            }
        }
        return result;
    }
    public void makeParty()
    {
        for (int i = 0; i < getPartySizeLimit(); i++)
        {
            TroopType tt = randomTroopType(20, 20, 10, 30, 10, 10);
            Ranking rk = randomRanking(0, 10, 10, 10);
            if (tt == TroopType.recruitType)
            {
                rk = Ranking.recruit;
            }
            Person p = makeGenericPerson(tt, rk);
            if (battleValue >= p.battleValue)
            {
                if (addToParty(makeGenericPerson(tt, rk)))
                {
                    battleValue -= p.battleValue;
                }
            }
        }
        if (battleValue > 0)
        {
            foreach (Person unit in partyMember)
            {
                TroopType tt = unit.troopType;
                Ranking rk = unit.ranking;
                if (unit.ranking == Ranking.recruit)
                {
                    tt = randomTroopType(0, 20, 10, 30, 10, 10);
                    rk = randomRanking(0, 10, 10, 10);
                }
                else if (unit.ranking == Ranking.militia)
                {
                    rk = randomRanking(0, 0, 10, 10);
                }
                else if (unit.ranking == Ranking.veteran)
                {
                    rk = Ranking.elite;
                }
                battleValue = unit.changeRankingTroopType(rk, tt, battleValue, true);
            }
        }
    }
    public void makeInventory(int budget, int amount, bool weightLimited)
    {
        Item newItem;
        foreach (Person p in partyMember)
        {
            addToInventory(ItemDataBase.dataBase.getItem("Supplies"));
        }
        for (int i = 0; i < amount; i++)
        {
            newItem = ItemDataBase.dataBase.getRandomItem();
            if (getInventoryWeight() + newItem.getWeight() < getInventoryWeightLimit()
               || !weightLimited)
            {
                if (budget > newItem.value)
                {
                    addToInventory(newItem);
                    budget -= newItem.value;
                }
                
            }
        }

    }
    public Person makeGenericPerson(TroopType tt, Ranking rk)
    {
        string memberName = TroopDataBase.rankingToString(rk) + " " + TroopDataBase.troopTypeToString(tt);
        Stats gStats = new Stats(1, 1, 1, 1, 1, 1);
        int s = 1;
        int a = 1;
        int p = 1;
        int e = 1;
        if (rk == Ranking.recruit)
        {
            s = Random.Range(1, 2);
            a = Random.Range(1, 2);
            p = Random.Range(1, 2);
            e = Random.Range(1, 2);

        }
        else if (rk == Ranking.militia)
        {
            s = Random.Range(3, 5);
            a = Random.Range(3, 5);
            p = Random.Range(3, 5);
            e = Random.Range(3, 5);
        }
        else if (rk == Ranking.veteran)
        {
            s = Random.Range(5, 7);
            a = Random.Range(5, 7);
            p = Random.Range(5, 7);
            e = Random.Range(5, 7);
        }
        else if (rk == Ranking.elite)
        {
            s = Random.Range(7, 8);
            a = Random.Range(7, 8);
            p = Random.Range(7, 8);
            e = Random.Range(7, 8);
        }
        gStats = new Stats(s, a, p, e, 0, 0);
        int level = gStats.strength + gStats.agility + gStats.perception + gStats.endurance - 4;
        Experience gExp = new Experience(0, level, 0);
        Ranking gRk = rk;
        TroopType gTt = tt;
        Faction gF = faction;
        Person per = new Person(memberName, gStats, gRk, gTt, gF, gExp);
        per.name = TroopDataBase.rankingToString(gRk) + " " + TroopDataBase.troopTypeToString(gTt);
        return per;
    }
    public TroopType randomTroopType(int recruitC, int crossC, int musketC, int swordC, int halbC, int cavC)
    {
        int r = Random.Range(1, recruitC + crossC + musketC + swordC + halbC + cavC);
        int curC = 0;
        if (r <= recruitC)
        {
            return TroopType.recruitType;
        }
        curC += recruitC;
        if (curC < r && r <= curC + crossC)
        {
            return TroopType.crossbowman;
        }
        curC += crossC;
        if (curC < r && r <= curC + musketC)
        {
            return TroopType.musketeer;
        }
        curC += musketC;
        if (curC < r && r <= curC + swordC)
        {
            return TroopType.swordsman;
        }
        curC += swordC;
        if (curC < r && r <= curC + halbC)
        {
            return TroopType.halberdier;
        }
        curC += halbC;
        if (curC < r && r <= curC + cavC)
        {
            return TroopType.cavalry;
        }
        return TroopType.recruitType;
    }
    public Ranking randomRanking(int recruitC, int militiaC, int veteranC, int eliteC)
    {
        int curC = 0;
        int r = Random.Range(1, recruitC + militiaC + veteranC + eliteC);
        if (r <= recruitC)
        {
            return Ranking.recruit;
        }
        curC += recruitC;
        if (curC < r && r <= curC + militiaC)
        {

            return Ranking.militia;
        }
        curC += militiaC;
        if (curC < r && r <= curC + veteranC)
        {
            return Ranking.veteran;
        }
        curC += veteranC;
        if (curC < r && r <= curC + eliteC)
        {
            return Ranking.elite;
        }
        return Ranking.recruit;
    }
    public void initializeFavors()
    {
        factionFavors = new Dictionary<Faction, int>();
        factionFavors.Add(Faction.italy, 0);
        factionFavors.Add(Faction.empire, 0);
        factionFavors.Add(Faction.france, 0);
        factionFavors.Add(Faction.papacy, 0);
        factionFavors.Add(Faction.mercenary, 0);
        factionFavors.Add(Faction.bandits, 0);
        factionFavors[faction] = 100;
        switch (faction)
        {
            case Faction.bandits:
                prestige = 0;
                notoriety = Random.Range(getBattleValue() / 20, getBattleValue() / 20);
                factionFavors[Faction.empire] = -50;
                factionFavors[Faction.france] = -50;
                factionFavors[Faction.papacy] = -50;
                factionFavors[Faction.italy] = -50;
                factionFavors[Faction.mercenary] = -50;
                break;
            case Faction.empire:
                prestige = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                notoriety = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                factionFavors[Faction.bandits] = -50;
                factionFavors[Faction.france] = -20;
                factionFavors[Faction.papacy] = -50;
                factionFavors[Faction.italy] = 0;
                factionFavors[Faction.mercenary] = 0;
                break;
            case Faction.france:
                prestige = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                notoriety = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                factionFavors[Faction.bandits] = -50;
                factionFavors[Faction.empire] = -20;
                factionFavors[Faction.papacy] = -50;
                factionFavors[Faction.italy] = 0;
                factionFavors[Faction.mercenary] = 0;
                break;
            case Faction.papacy:
                prestige = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                notoriety = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                factionFavors[Faction.bandits] = -50;
                factionFavors[Faction.france] = -20;
                factionFavors[Faction.empire] = -50;
                factionFavors[Faction.italy] = 0;
                factionFavors[Faction.mercenary] = 0;
                break;
            case Faction.italy:
                prestige = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                notoriety = Random.Range(getBattleValue() / 40, getBattleValue() / 20);
                factionFavors[Faction.bandits] = -50;
                factionFavors[Faction.france] = 0;
                factionFavors[Faction.papacy] = 0;
                factionFavors[Faction.empire] = 0;
                factionFavors[Faction.mercenary] = 0;
                break;
        }
    }
}



