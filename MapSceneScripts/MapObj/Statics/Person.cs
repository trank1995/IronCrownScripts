using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {


    public string name { get; set; }
    public Stats stats { get; set; }
    public Ranking ranking { get; set; }
    public TroopType troopType { get; set; }
    public Faction faction { get; set; }
    public Experience exp { get; set; }
    public bool inBattle, renamed;
    public Troop troop;
    //battleStats
    public int battleValue;
    public float stamina { get; set; }
    public float staminaMax;
    public float health { get; set; }
    public float healthMax;
    //gear
    public GearInfo gearInfo;

    public Person()
    {

    }

    public Person(string nameI, Stats statsI, Ranking rk, TroopType tt, Faction factionI, Experience expI)
    {
        
        initialization(nameI, statsI, rk, tt, factionI, expI);
    }

    //ini gear
    public virtual void initialization(string nameI, Stats statsI, Ranking rk, TroopType tt, Faction factionI, Experience expI)
    {
        gearInfo = TroopDataBase.troopDataBase.getGearInfo(faction, troopType, ranking);
        name = nameI;
        stats = statsI;
        ranking = rk;
        troopType = tt;
        faction = factionI;
        exp = expI;
        battleValue = getBattleValue();
        inBattle = false;
        renamed = false;
        stamina = getStaminaMax();
        health = getHealthMax();
        
    }


    public void setName(string newName)
    {
        if (ranking != Ranking.mainChar)
        {
            name = newName;
            renamed = true;
        }
    }
    public void setDefaultName()
    {
        if (ranking != Ranking.mainChar)
        {
            name = TroopDataBase.rankingToString(ranking)
            + " " + TroopDataBase.troopTypeToString(troopType);
            renamed = false;
        }
    }
    public virtual int changeRankingTroopType(Ranking rankingN, TroopType ttN, int money, bool initializing)
    {
        int originalBattleValue = TroopDataBase.troopDataBase.getBattleValue(faction, troopType, ranking);
        int newBattleValue = TroopDataBase.troopDataBase.getBattleValue(faction, ttN, rankingN);
        if (money > newBattleValue - originalBattleValue/2 && !initializing)
        {
            ranking = rankingN;
            troopType = ttN;
            return money - newBattleValue + originalBattleValue / 2;
        } else if (money > newBattleValue - originalBattleValue && initializing)
        {
            ranking = rankingN;
            troopType = ttN;
            return money - newBattleValue + originalBattleValue;
        }
        else
        {
            return money;
        }
        
    }
    public virtual void resetPerk()
    {
        stats.strength = 1;
        stats.agility = 1;
        stats.perception = 1;
        stats.endurance = 1;
        stats.charisma = 1;
        stats.intelligence = 1;
        exp.sparedPoint = exp.level;
    }

    public virtual void incrementS()
    {
        stats.strength += 1;
        exp.sparedPoint -= 1;
    }
    public virtual void incrementA()
    {
        stats.agility += 1;
        exp.sparedPoint -= 1;
    }
    public virtual void incrementP()
    {
        stats.perception += 1;
        exp.sparedPoint -= 1;
    }
    public virtual void incrementE()
    {
        stats.endurance += 1;
        exp.sparedPoint -= 1;
    }
    public virtual void incrementC()
    {
        stats.charisma += 1;
        exp.sparedPoint -= 1;
    }
    public virtual void incrementI()
    {
        stats.intelligence += 1;
        exp.sparedPoint -= 1;
    }

    public virtual float getStaminaMax()
    {
        return stats.agility * 10 * ((getGearInfo().mobilityRating + 10) / 10);
    }
    public virtual float getHealthMax()
    {
        return stats.endurance * 100 * ((getGearInfo().armorRating + 10) / 10);
    }

    public virtual float getWhirlwindStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getLungeStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getExecuteStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getFireStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getHoldSteadyStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getGuardStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getQuickDrawStaminaCost()
    {
        return 10.0f;
    }
    public virtual float getRainOfArrowsStaminaCost()
    {
        return 10.0f;
    }



    public virtual float getGuardedIncrease()
    {
        return stats.endurance * ((getGearInfo().armorRating + 10) / 10);
    }
    public virtual float getArmor()
    {
        return stats.endurance * ((getGearInfo().armorRating + 10) / 10);
    }
    public virtual float getBlock()
    {
        return ((stats.perception + stats.strength) / 2) * ((getGearInfo().armorRating + 10) / 10);
    }
    public virtual float getEvasion()
    {
        return ((stats.perception + stats.strength) / 2) * ((getGearInfo().armorRating + 10) / 10);
    }
    public virtual float getVision()
    {
        return stats.perception * ((getGearInfo().visionRating + 10) / 10);
    }
    public virtual float getStealth()
    {
        return stats.agility * ((getGearInfo().stealthRating + 10) / 10);
    }
    public virtual float getAccuracy()
    {
        return stats.perception * ((getGearInfo().accuracyRating + 10) / 10);
    }
    public virtual float getMeleeAttackDmg()
    {
        return stats.strength * ((getGearInfo().meleeDmgRating + 10) / 1);
    }
    public virtual float getRangedAttackDmg()
    {
        return (stats.strength + stats.perception) * ((getGearInfo().meleeDmgRating + 10) / 1);
    }
    public virtual float getMobility()
    {
        return (stats.agility) * ((getGearInfo().mobilityRating + 10) / 10);
    }

    public virtual int getInjuredHealth()
    {
        return (int)(getHealthMax() * .05f);
    }

    public void increaseExp(int amount)
    {
        exp.exp += amount;
        if (exp.exp >= exp.getLevelExp())
        {
            exp.exp -= (int)exp.getLevelExp();
            exp.level += 1;
            exp.sparedPoint += 1;
        }
    }

    public int getExp()
    {
        return exp.level * 100;
    }

    public virtual int getTroopPlacingRange(int maxRange)
    {
        return 2 + (int) 2 * stats.intelligence * maxRange / 100;
    }
    public virtual int getTroopMaxNum()
    {
        return 3 + (int)2 * stats.charisma;
    }
    public virtual float healthRegenPerTurn()
    {
        return (getHealthMax() * .005f);
    }
    public virtual float healthRegenPerDay()
    {
        return (getHealthMax() * .020f);
    }
    public virtual float getRecruitRating()
    {
        return stats.charisma;
    }


    public int getBattleValue()
    {
        int result = TroopDataBase.troopDataBase.getTroopInfo(faction, troopType, ranking).battleValue;
        //Debug.Log(result);
        if (result == 0)
        {
            //Debug.Log("troop value zero nad its name is: " + name);
        }
        return result;
    }

    public virtual GearInfo getGearInfo()
    {
        if (TroopDataBase.troopDataBase != null)
        {
            return TroopDataBase.troopDataBase.getGearInfo(faction, troopType, ranking);
        } else
        {
            return new GearInfo(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        }
        
    }



}


