using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacter : Person {

    public SkillTree skillTree;
    public MainCharacter()
    {

    }
    public MainCharacter(string nameI, Stats statsI, Ranking rk, TroopType tt, Faction factionI, Experience expI)
    {
        initialization(nameI, statsI, rk, tt, factionI, expI);
        skillTree = new SkillTree();
    }
    public override void initialization(string nameI, Stats statsI, Ranking rk, TroopType tt, Faction factionI, Experience expI)
    {
        base.initialization(nameI, statsI, rk, tt, factionI, expI);
    }
    public override void resetPerk()
    {
        base.resetPerk();
        skillTree.skillTreeInitialization();
        exp.sparedPoint = 420; //REMEMBER TO CHANGE THIS TO LEVEL
    }
    
    public override GearInfo getGearInfo()
    {
        
        GearInfo result = new GearInfo(1, 1, 1, 1, 1, 1, 1, 1, 1);
        if (skillTree != null)
        {
            if (troopType == TroopType.mainCharType)
            {
                if (skillTree.getPerk("M1_HELMET1").own)
                {
                    result.visionRating += 2;
                    result.armorRating += 2;
                    if (skillTree.getPerk("M1_HELMET2").own)
                    {
                        result.visionRating += 2;
                        result.armorRating += 2;
                        if (skillTree.getPerk("M1_HELMET3").own)
                        {
                            result.visionRating += 2;
                            result.armorRating += 2;
                            if (skillTree.getPerk("M1_HELMET4A").own)
                            {
                                result.visionRating += 3;
                            }
                            else if (skillTree.getPerk("M1_HELMET4B").own)
                            {
                                result.armorRating += 3;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M1_ARMOR1").own)
                {
                    result.armorRating += 2;
                    result.blockRating += 2;
                    result.evasionRating += 2;
                    if (skillTree.getPerk("M1_ARMOR2").own)
                    {
                        result.armorRating += 2;
                        result.blockRating += 2;
                        result.evasionRating += 2;
                        if (skillTree.getPerk("M1_ARMOR3").own)
                        {
                            result.armorRating += 2;
                            result.blockRating += 2;
                            result.evasionRating += 2;
                            if (skillTree.getPerk("M1_ARMOR4A").own)
                            {
                                result.armorRating += 3;
                            }
                            else if (skillTree.getPerk("M1_ARMOR4B").own)
                            {
                                result.blockRating += 3;
                            }
                            else if (skillTree.getPerk("M1_ARMOR4C").own)
                            {
                                result.evasionRating += 3;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M1_CLOTHES1").own)
                {
                    result.stealthRating += 2;
                    result.mobilityRating += 2;
                    result.evasionRating += 2;
                    result.visionRating += 2;
                    if (skillTree.getPerk("M1_CLOTHES2").own)
                    {
                        result.stealthRating += 2;
                        result.mobilityRating += 2;
                        result.evasionRating += 2;
                        result.visionRating += 2;
                        if (skillTree.getPerk("M1_CLOTHES3").own)
                        {
                            result.stealthRating += 2;
                            result.mobilityRating += 2;
                            result.evasionRating += 2;
                            result.visionRating += 2;
                            if (skillTree.getPerk("M1_CLOTHES4A").own)
                            {
                                result.stealthRating += 3;
                            }
                            else if (skillTree.getPerk("M1_CLOTHES4B").own)
                            {
                                result.mobilityRating += 3;
                            }
                            else if (skillTree.getPerk("M1_CLOTHES4C").own)
                            {
                                result.evasionRating += 3;
                            }
                            else if (skillTree.getPerk("M1_CLOTHES4D").own)
                            {
                                result.visionRating += 3;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M1_SWORD1").own)
                {
                    result.meleeDmgRating += 4;
                    result.blockRating += 2;
                    if (skillTree.getPerk("M1_SWORD2").own)
                    {
                        result.meleeDmgRating += 4;
                        result.blockRating += 2;
                        if (skillTree.getPerk("M1_SWORD3").own)
                        {
                            result.meleeDmgRating += 4;
                            result.blockRating += 2;
                            if (skillTree.getPerk("M1_SWORD4A").own)
                            {
                                result.meleeDmgRating += 7;
                            }
                            else if (skillTree.getPerk("M1_SWORD4B").own)
                            {
                                result.blockRating += 3;
                            }
                            else if (skillTree.getPerk("M1_SWORD4C").own)
                            {
                                //reduce stamina cost
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M1_PISTOL1").own)
                {
                    result.rangedDmgRating += 4;
                    result.accuracyRating += 4;
                    if (skillTree.getPerk("M1_PISTOL2").own)
                    {
                        result.rangedDmgRating += 4;
                        result.accuracyRating += 4;
                        if (skillTree.getPerk("M1_PISTOL3").own)
                        {
                            result.rangedDmgRating += 4;
                            result.accuracyRating += 4;
                            if (skillTree.getPerk("M1_PISTOL4A").own)
                            {
                                result.rangedDmgRating += 7;
                            }
                            else if (skillTree.getPerk("M1_PISTOL4B").own)
                            {
                                result.accuracyRating += 7;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M1_BOOTS1").own)
                {
                    result.mobilityRating += 2;
                    result.stealthRating += 2;
                    if (skillTree.getPerk("M1_BOOTS2").own)
                    {
                        result.mobilityRating += 2;
                        result.stealthRating += 2;
                        if (skillTree.getPerk("M1_BOOTS3").own)
                        {
                            result.mobilityRating += 2;
                            result.stealthRating += 2;
                            if (skillTree.getPerk("M1_BOOTS4A").own)
                            {
                                result.mobilityRating += 3;
                            }
                            else if (skillTree.getPerk("M1_BOOTS4B").own)
                            {
                                result.stealthRating += 3;
                            }
                        }
                    }
                }
            }
            if (troopType == TroopType.crossbowman)
            {
                skillTree = Player.mainCharacter.skillTree;
                if (skillTree.getPerk("M2_HELMET1").own)
                {
                    result.visionRating += 2;
                    result.armorRating += 2;
                    if (skillTree.getPerk("M2_HELMET2").own)
                    {
                        result.visionRating += 2;
                        result.armorRating += 2;
                        if (skillTree.getPerk("M2_HELMET3").own)
                        {
                            result.visionRating += 2;
                            result.armorRating += 2;
                            if (skillTree.getPerk("M2_HELMET4A").own)
                            {
                                result.visionRating += 3;
                            }
                            else if (skillTree.getPerk("M2_HELMET4B").own)
                            {
                                result.armorRating += 3;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M2_ARMOR1").own)
                {
                    result.armorRating += 2;
                    result.blockRating += 2;
                    result.evasionRating += 2;
                    if (skillTree.getPerk("M2_ARMOR2").own)
                    {
                        result.armorRating += 2;
                        result.blockRating += 2;
                        result.evasionRating += 2;
                        if (skillTree.getPerk("M2_ARMOR3").own)
                        {
                            result.armorRating += 2;
                            result.blockRating += 2;
                            result.evasionRating += 2;
                            if (skillTree.getPerk("M2_ARMOR4A").own)
                            {
                                result.armorRating += 3;
                            }
                            else if (skillTree.getPerk("M2_ARMOR4B").own)
                            {
                                result.blockRating += 3;
                            }
                            else if (skillTree.getPerk("M2_ARMOR4C").own)
                            {
                                result.evasionRating += 3;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M2_CLOTHES1").own)
                {
                    result.stealthRating += 2;
                    result.mobilityRating += 2;
                    result.evasionRating += 2;
                    result.visionRating += 2;
                    if (skillTree.getPerk("M2_CLOTHES2").own)
                    {
                        result.stealthRating += 2;
                        result.mobilityRating += 2;
                        result.evasionRating += 2;
                        result.visionRating += 2;
                        if (skillTree.getPerk("M2_CLOTHES3").own)
                        {
                            result.stealthRating += 2;
                            result.mobilityRating += 2;
                            result.evasionRating += 2;
                            result.visionRating += 2;
                            if (skillTree.getPerk("M2_CLOTHES4A").own)
                            {
                                result.stealthRating += 3;
                            }
                            else if (skillTree.getPerk("M2_CLOTHES4B").own)
                            {
                                result.mobilityRating += 3;
                            }
                            else if (skillTree.getPerk("M2_CLOTHES4C").own)
                            {
                                result.evasionRating += 3;
                            }
                            else if (skillTree.getPerk("M2_CLOTHES4D").own)
                            {
                                result.visionRating += 3;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M2_DAGGER1").own)
                {
                    result.meleeDmgRating += 4;
                    result.blockRating += 2;
                    if (skillTree.getPerk("M2_DAGGER2").own)
                    {
                        result.meleeDmgRating += 4;
                        result.blockRating += 2;
                        if (skillTree.getPerk("M2_DAGGER3").own)
                        {
                            result.meleeDmgRating += 4;
                            result.blockRating += 2;
                            if (skillTree.getPerk("M2_DAGGER4A").own)
                            {
                                result.meleeDmgRating += 7;
                            }
                            else if (skillTree.getPerk("M2_DAGGER4B").own)
                            {
                                result.blockRating += 3;
                            }
                            else if (skillTree.getPerk("M2_DAGGER4C").own)
                            {
                                //reduce stamina cost
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M2_CROSSBOW1").own)
                {
                    result.rangedDmgRating += 4;
                    result.accuracyRating += 4;
                    if (skillTree.getPerk("M2_CROSSBOW2").own)
                    {
                        result.rangedDmgRating += 4;
                        result.accuracyRating += 4;
                        if (skillTree.getPerk("M2_CROSSBOW3").own)
                        {
                            result.rangedDmgRating += 4;
                            result.accuracyRating += 4;
                            if (skillTree.getPerk("M2_CROSSBOW4A").own)
                            {
                                result.rangedDmgRating += 7;
                            }
                            else if (skillTree.getPerk("M2_CROSSBOW4B").own)
                            {
                                result.accuracyRating += 7;
                            }
                        }
                    }
                }
                if (skillTree.getPerk("M2_BOOTS1").own)
                {
                    result.mobilityRating += 2;
                    result.stealthRating += 2;
                    if (skillTree.getPerk("M2_BOOTS2").own)
                    {
                        result.mobilityRating += 2;
                        result.stealthRating += 2;
                        if (skillTree.getPerk("M2_SWORD3").own)
                        {
                            result.mobilityRating += 2;
                            result.stealthRating += 2;
                            if (skillTree.getPerk("M2_SWORD4A").own)
                            {
                                result.mobilityRating += 3;
                            }
                            else if (skillTree.getPerk("M2_SWORD4B").own)
                            {
                                result.stealthRating += 3;
                            }
                        }
                    }
                }
            }
        }
        
        return result;
    }
}

public class SkillTree
{
    public Dictionary<string, Perk> skillTreeDict;
    public SkillTree()
    {
        skillTreeInitialization();
        gearInitialization();
    }
    public void gearInitialization()
    {
        skillTreeDict.Add("M1_HELMET1", new Perk("M1_HELMET1", false, "Helmet Upgrade I", "Increase vision and armor", " "));
        skillTreeDict.Add("M1_HELMET2", new Perk("M1_HELMET2", false, "Helmet Upgrade II", "Increase vision and armor", " "));
        skillTreeDict.Add("M1_HELMET3", new Perk("M1_HELMET3", false, "Helmet Upgrade III", "Increase vision and armor", " "));
        skillTreeDict.Add("M1_HELMET4A", new Perk("M1_HELMET4A", false, "Helmet Upgrade IV Armor", "Increase armor", " "));
        skillTreeDict.Add("M1_HELMET4B", new Perk("M1_HELMET4B", false, "Helmet Upgrade IV Vision", "Increase vision", " "));

        skillTreeDict.Add("M1_ARMOR1", new Perk("M1_ARMOR1", false, "Armor Upgrade I", "Increase armor, block, evasion", " "));
        skillTreeDict.Add("M1_ARMOR2", new Perk("M1_ARMOR2", false, "Armor Upgrade II", "Increase armor, block, evasion", " "));
        skillTreeDict.Add("M1_ARMOR3", new Perk("M1_ARMOR3", false, "Armor Upgrade III", "Increase armor, block, evasion", " "));
        skillTreeDict.Add("M1_ARMOR4A", new Perk("M1_ARMOR4A", false, "Armor Upgrade IV Armor", "Increase armor", " "));
        skillTreeDict.Add("M1_ARMOR4B", new Perk("M1_ARMOR4B", false, "Armor Upgrade IV Block", "Increase block", " "));
        skillTreeDict.Add("M1_ARMOR4C", new Perk("M1_ARMOR4C", false, "Armor Upgrade IV Evasion", "Increase vision", " "));

        skillTreeDict.Add("M1_CLOTHES1", new Perk("M1_CLOTHES1", false, "Clothes Upgrade I", "Increase stealth, block, evasion, vision", " "));
        skillTreeDict.Add("M1_CLOTHES2", new Perk("M1_CLOTHES2", false, "Clothes Upgrade II", "Increase stealth, block, evasion, vision", " "));
        skillTreeDict.Add("M1_CLOTHES3", new Perk("M1_CLOTHES3", false, "Clothes Upgrade III", "Increase stealth, block, evasion, vision", " "));
        skillTreeDict.Add("M1_CLOTHES4A", new Perk("M1_CLOTHES4A", false, "Clothes Upgrade IV Clothes", "Increase armor", " "));
        skillTreeDict.Add("M1_CLOTHES4B", new Perk("M1_CLOTHES4B", false, "Clothes Upgrade IV Block", "Increase block", " "));
        skillTreeDict.Add("M1_CLOTHES4C", new Perk("M1_CLOTHES4C", false, "Clothes Upgrade IV Evasion", "Increase evasion", " "));
        skillTreeDict.Add("M1_CLOTHES4D", new Perk("M1_CLOTHES4D", false, "Clothes Upgrade IV Vision", "Increase vision", " "));

        skillTreeDict.Add("M1_SWORD1", new Perk("M1_SWORD1", false, "Sword Upgrade I", "Increase damage, block, decrease stamina cost", " "));
        skillTreeDict.Add("M1_SWORD2", new Perk("M1_SWORD2", false, "Sword Upgrade II", "Increase damage, block, decrease stamina cost", " "));
        skillTreeDict.Add("M1_SWORD3", new Perk("M1_SWORD3", false, "Sword Upgrade III", "Increase damage, block, decrease stamina cost", " "));
        skillTreeDict.Add("M1_SWORD4A", new Perk("M1_SWORD4A", false, "Sword Upgrade IV Damage", "Increase damage", " "));
        skillTreeDict.Add("M1_SWORD4B", new Perk("M1_SWORD4B", false, "Sword Upgrade IV Block", "Increase block", " "));
        skillTreeDict.Add("M1_SWORD4C", new Perk("M1_SWORD4C", false, "Sword Upgrade IV Stamina Cost", "Decrease stamina cost for lunge, whirlwind, execute", " "));

        skillTreeDict.Add("M1_PISTOL1", new Perk("M1_PISTOL1", false, "Pistol Upgrade I", "Increase damage, accuracy, decrease stamina cost", " "));
        skillTreeDict.Add("M1_PISTOL2", new Perk("M1_PISTOL2", false, "Pistol Upgrade II", "Increase damage, accuracy, decrease stamina cost", " "));
        skillTreeDict.Add("M1_PISTOL3", new Perk("M1_PISTOL3", false, "Pistol Upgrade III", "Increase damage, accuracy, decrease stamina cost", " "));
        skillTreeDict.Add("M1_PISTOL4A", new Perk("M1_PISTOL4A", false, "Pistol Upgrade IV Damage", "Increase damage", " "));
        skillTreeDict.Add("M1_PISTOL4B", new Perk("M1_PISTOL4B", false, "Pistol Upgrade IV Accuracy", "Increase accuracy", " "));
        skillTreeDict.Add("M1_PISTOL4C", new Perk("M1_PISTOL4C", false, "Pistol Upgrade IV Stamina Cost", "Decrease stamina cost for fire and hold steady", " "));

        skillTreeDict.Add("M1_BOOTS1", new Perk("M1_BOOTS1", false, "Boots Upgrade I", "Increase mobility and stealth", " "));
        skillTreeDict.Add("M1_BOOTS2", new Perk("M1_BOOTS2", false, "Boots Upgrade II", "Increase mobility and stealth", " "));
        skillTreeDict.Add("M1_BOOTS3", new Perk("M1_BOOTS3", false, "Boots Upgrade III", "Increase mobility and stealth", " "));
        skillTreeDict.Add("M1_BOOTS4A", new Perk("M1_BOOTS4A", false, "Boots Upgrade IV mobility", "Increase mobility", " "));
        skillTreeDict.Add("M1_BOOTS4B", new Perk("M1_BOOTS4B", false, "Boots Upgrade IV stealth", "Increase stealth", " "));


        skillTreeDict.Add("M2_HELMET1", new Perk("M2_HELMET1", false, "Helmet Upgrade I", "Increase vision and armor", " "));
        skillTreeDict.Add("M2_HELMET2", new Perk("M2_HELMET2", false, "Helmet Upgrade II", "Increase vision and armor", " "));
        skillTreeDict.Add("M2_HELMET3", new Perk("M2_HELMET3", false, "Helmet Upgrade III", "Increase vision and armor", " "));
        skillTreeDict.Add("M2_HELMET4A", new Perk("M2_HELMET4A", false, "Helmet Upgrade IV Armor", "Increase armor", " "));
        skillTreeDict.Add("M2_HELMET4B", new Perk("M2_HELMET4B", false, "Helmet Upgrade IV Vision", "Increase vision", " "));

        skillTreeDict.Add("M2_ARMOR1", new Perk("M2_ARMOR1", false, "Armor Upgrade I", "Increase armor, block, evasion", " "));
        skillTreeDict.Add("M2_ARMOR2", new Perk("M2_ARMOR2", false, "Armor Upgrade II", "Increase armor, block, evasion", " "));
        skillTreeDict.Add("M2_ARMOR3", new Perk("M2_ARMOR3", false, "Armor Upgrade III", "Increase armor, block, evasion", " "));
        skillTreeDict.Add("M2_ARMOR4A", new Perk("M2_ARMOR4A", false, "Armor Upgrade IV Armor", "Increase armor", " "));
        skillTreeDict.Add("M2_ARMOR4B", new Perk("M2_ARMOR4B", false, "Armor Upgrade IV Block", "Increase block", " "));
        skillTreeDict.Add("M2_ARMOR4C", new Perk("M2_ARMOR4C", false, "Armor Upgrade IV Evasion", "Increase vision", " "));

        skillTreeDict.Add("M2_CLOTHES1", new Perk("M2_CLOTHES1", false, "Clothes Upgrade I", "Increase stealth, block, evasion, vision", " "));
        skillTreeDict.Add("M2_CLOTHES2", new Perk("M2_CLOTHES2", false, "Clothes Upgrade II", "Increase stealth, block, evasion, vision", " "));
        skillTreeDict.Add("M2_CLOTHES3", new Perk("M2_CLOTHES3", false, "Clothes Upgrade III", "Increase stealth, block, evasion, vision", " "));
        skillTreeDict.Add("M2_CLOTHES4A", new Perk("M2_CLOTHES4A", false, "Clothes Upgrade IV Clothes", "Increase armor", " "));
        skillTreeDict.Add("M2_CLOTHES4B", new Perk("M2_CLOTHES4B", false, "Clothes Upgrade IV Block", "Increase block", " "));
        skillTreeDict.Add("M2_CLOTHES4C", new Perk("M2_CLOTHES4C", false, "Clothes Upgrade IV Evasion", "Increase evasion", " "));
        skillTreeDict.Add("M2_CLOTHES4D", new Perk("M2_CLOTHES4D", false, "Clothes Upgrade IV Vision", "Increase vision", " "));

        skillTreeDict.Add("M2_DAGGER1", new Perk("M2_DAGGER1", false, "Dagger Upgrade I", "Increase damage, block, decrease stamina cost", " "));
        skillTreeDict.Add("M2_DAGGER2", new Perk("M2_DAGGER2", false, "Dagger Upgrade II", "Increase damage, block, decrease stamina cost", " "));
        skillTreeDict.Add("M2_DAGGER3", new Perk("M2_DAGGER3", false, "Dagger Upgrade III", "Increase damage, block, decrease stamina cost", " "));
        skillTreeDict.Add("M2_DAGGER4A", new Perk("M2_DAGGER4A", false, "Dagger Upgrade IV Damage", "Increase damage", " "));
        skillTreeDict.Add("M2_DAGGER4B", new Perk("M2_DAGGER4B", false, "Dagger Upgrade IV Block", "Increase block", " "));
        skillTreeDict.Add("M2_DAGGER4C", new Perk("M2_DAGGER4C", false, "Dagger Upgrade IV Stamina Cost", "Decrease stamina cost for lunge, whirlwind, execute", " "));

        skillTreeDict.Add("M2_CROSSBOW1", new Perk("M2_CROSSBOW1", false, "Crossbow Upgrade I", "Increase damage, accuracy, decrease stamina cost", " "));
        skillTreeDict.Add("M2_CROSSBOW2", new Perk("M2_CROSSBOW2", false, "Crossbow Upgrade II", "Increase damage, accuracy, decrease stamina cost", " "));
        skillTreeDict.Add("M2_CROSSBOW3", new Perk("M2_CROSSBOW3", false, "Crossbow Upgrade III", "Increase damage, accuracy, decrease stamina cost", " "));
        skillTreeDict.Add("M2_CROSSBOW4A", new Perk("M2_CROSSBOW4A", false, "Crossbow Upgrade IV Damage", "Increase damage", " "));
        skillTreeDict.Add("M2_CROSSBOW4B", new Perk("M2_CROSSBOW4B", false, "Crossbow Upgrade IV Accuracy", "Increase accuracy", " "));
        skillTreeDict.Add("M2_CROSSBOW4C", new Perk("M2_CROSSBOW4C", false, "Crossbow Upgrade IV Stamina Cost", "Decrease stamina cost for fire and hold steady", " "));

        skillTreeDict.Add("M2_BOOTS1", new Perk("M2_BOOTS1", false, "Boots Upgrade I", "Increase mobility and stealth", " "));
        skillTreeDict.Add("M2_BOOTS2", new Perk("M2_BOOTS2", false, "Boots Upgrade II", "Increase mobility and stealth", " "));
        skillTreeDict.Add("M2_BOOTS3", new Perk("M2_BOOTS3", false, "Boots Upgrade III", "Increase mobility and stealth", " "));
        skillTreeDict.Add("M2_BOOTS4A", new Perk("M2_BOOTS4A", false, "Boots Upgrade IV mobility", "Increase mobility", " "));
        skillTreeDict.Add("M2_BOOTS4B", new Perk("M2_BOOTS4B", false, "Boots Upgrade IV stealth", "Increase stealth", " "));
    }
    public void skillTreeInitialization()
    {
        skillTreeDict = new Dictionary<string, Perk>();
        skillTreeDict.Add("S6A", new Perk("S6A", false, "Test of Will", "Execution deals more damage based on target's lost health", " "));
        skillTreeDict.Add("S6B", new Perk("S6B", false, "Steady Hand", "Hold steady increase damage as well", " "));
        skillTreeDict.Add("S7A", new Perk("S7A", false, "Vendetta", "Blocking reflects damage. ", " "));
        skillTreeDict.Add("S7B", new Perk("S7B", false, "Brute Force", "Your blocked attacks deals minor damage. ", " "));
        skillTreeDict.Add("S8A", new Perk("S8A", false, "Damascus Steel", "increase melee damage. ", " "));
        skillTreeDict.Add("S8B", new Perk("S8B", false, "Refined Gunpowder", "increase ranged damage. ", " "));
        skillTreeDict.Add("S9A", new Perk("S9A", false, "Last Laugh", "On each sucessful hit, reduce the armor of the target. ", " "));
        skillTreeDict.Add("S9B", new Perk("S9B", false, "Every Strike Counts", "On each sucessful hit, gain attack damage. ", " "));
        skillTreeDict.Add("S10A", new Perk("S10A", false, "Hercules Reborn", "All your attacks deals double damage. ", " "));
        
        skillTreeDict.Add("A6A", new Perk("A6A", false, "Battleborn", "Increase stamina regeneration. ", " "));
        skillTreeDict.Add("A6B", new Perk("A6B", false, "Silent but Deadly", "If not sighted, increase damage and accuracy. ", " "));
        skillTreeDict.Add("A7A", new Perk("A7A", false, "Who's Next?", "Gain staminao on successful execution. ", " "));
        skillTreeDict.Add("A7B", new Perk("A7B", false, "Requiesta in Pace", "On successful execution, gain stealth and hide chance for 1 turn", " "));
        skillTreeDict.Add("A8A", new Perk("A8A", false, "Acrobat", "Reduce stamina cost for traveling on the battlefield. ", " "));
        skillTreeDict.Add("A8B", new Perk("A8B", false, "Tight Grip", "If your stamina is more than half, your damage is increaded", " "));
        skillTreeDict.Add("A9A", new Perk("A9A", false, "Retaliate", "After a successful dodge, your damage next turn is increased.  ", " "));
        skillTreeDict.Add("A9B", new Perk("A9B", false, "The Ball Just Started", "If your stamina is more than half, your dodge chance is increaded. ", " "));
        skillTreeDict.Add("A10A", new Perk("A10A", false, "Elusive", "Your stamina is doubled. ", " "));

        skillTreeDict.Add("P6A", new Perk("P6A", false, "Quick Reload", "Decrease range attack stamina cost. ", " "));
        skillTreeDict.Add("P6B", new Perk("P6B", false, "Focus Fire", "On successful range attack, decrease target block and dodge chance. ", " "));
        skillTreeDict.Add("P7A", new Perk("P7A", false, "Suppressive Fire", "Ranged attacks have a chance to decrease enemy morale. ", " "));
        skillTreeDict.Add("P7B", new Perk("P7B", false, "Can't Hide Forever", "If an attack is blocked or dodged, increase accuracy for 1 turn. ", " "));
        skillTreeDict.Add("P8A", new Perk("P8A", false, "Hunter's Mark", "On successful range attack, increase accuracy for 1 turn. ", " "));
        skillTreeDict.Add("P8B", new Perk("P8B", false, "Follow Through", "If an attack is blocked or dodged, increase damage for 1 turn. ", " "));
        skillTreeDict.Add("P9A", new Perk("P9A", false, "Salt the Wound", "On successful range attack, increase damage for 1 turn. ", " "));
        skillTreeDict.Add("P9B", new Perk("P9B", false, "Fractured Armor", "On successful range attack, decrease target armor. ", " "));
        skillTreeDict.Add("P10A", new Perk("P10A", false, "Eyes of an Eagle", "Double accuracy and vision range. ", " "));

        skillTreeDict.Add("E6A", new Perk("E6A", false, "Outnumbered", "Gain armor based on enemy number. ", " "));
        skillTreeDict.Add("E6B", new Perk("E6B", false, "Spirit before Battle", "Regenerate health every turn. ", " "));
        skillTreeDict.Add("E7A", new Perk("E7A", false, "Knife's Edge", "If health is less than 30%, gain major block and dodge chance bonus. ", " "));
        skillTreeDict.Add("E7B", new Perk("E7B", false, "Shield Bash", "When an enemy unit dies, gain armor for 1 turn. ", " "));
        skillTreeDict.Add("E8A", new Perk("E8A", false, "Unstoppable", "Health regeneration increases if morale is more than 90%", " "));
        skillTreeDict.Add("E8B", new Perk("E8B", false, "Hidden Strength", "If your health is less than 50%, increase health regeneration. ", " "));
        skillTreeDict.Add("E9A", new Perk("E9A", false, "Blood Bath", "When an enemy unit dies, gain health. ", " "));
        skillTreeDict.Add("E9B", new Perk("E9B", false, "Battle Trance", "If your health is more than 50%, increase stamina regeneration. ", " "));
        skillTreeDict.Add("E10A", new Perk("E10A", false, "Survivor", "Your health and armor. ", " "));

        skillTreeDict.Add("C6A", new Perk("C6A", false, "Porto Franco", "Trade good buying price decreases. ", " "));
        skillTreeDict.Add("C6B", new Perk("C6B", false, "Exotic Spices", "Trade good selling price increases. ", "  "));
        skillTreeDict.Add("C6C", new Perk("C6C", false, "Logistic Master", "Supplies are cheaper. ", " "));
        skillTreeDict.Add("C6D", new Perk("C6D", false, "Drill Sergeant", "Decrease troop upgrade cost. ", " "));
        skillTreeDict.Add("C7A", new Perk("C7A", false, "Patron of th Arts", "Increase prestige gained. ", " "));
        skillTreeDict.Add("C7B", new Perk("C7B", false, "Persona Non Grata", "Increase notoriety gained. ", " "));
        skillTreeDict.Add("C7C", new Perk("C7C", false, "Robber Baron", "Increase profit from threatening action. ", " "));
        skillTreeDict.Add("C7D", new Perk("C7D", false, "Curator", "Increase loot drop after battle. ", " "));
        skillTreeDict.Add("C8A", new Perk("C8A", false, "Papal Legate", "Increase Papal reputaion gain. ", " "));
        skillTreeDict.Add("C8B", new Perk("C8B", false, "Lingua Franca", "Increase French reputation gained. ", " "));
        skillTreeDict.Add("C8C", new Perk("C8C", false, "Imperial Vicar", "Increase Imperial reputation gained. ", " "));
        skillTreeDict.Add("C8D", new Perk("C8D", false, "Merchant Wagons", "Increase inventory weight limit. ", " "));
        skillTreeDict.Add("C9A", new Perk("C9A", false, "Holier than Thou", "Deal more damage based on your prestige and enemy's notoriety. ", " "));
        skillTreeDict.Add("C9B", new Perk("C9B", false, "Bane of the Hyprocrites", "Deal more damage based on your notoriety and enemy's prestige. ", " "));
        skillTreeDict.Add("C9C", new Perk("C9C", false, "Inspring Leader", "Decrease recruit cost. ", " "));
        skillTreeDict.Add("C9D", new Perk("C9D", false, "TBD", "Execution cost less stamina based on S", " "));
        skillTreeDict.Add("C10A", new Perk("C10A", false, "Retirement Plan", "Recieve dividend from your current saving. ", " "));
        skillTreeDict.Add("C10B", new Perk("C10B", false, "TBD", "Execution cost less stamina based on S", " "));

        skillTreeDict.Add("I6A", new Perk("I6A", false, "Bird of Feather", "The less intelligence you have, the more to recruit. ", " "));
        skillTreeDict.Add("I6B", new Perk("I6B", false, "Rational", "Suppies weigh less. ", " "));
        skillTreeDict.Add("I6C", new Perk("I6C", false, "Herbal Medicine", "Increase out of combat healing. ", " "));
        skillTreeDict.Add("I6D", new Perk("I6D", false, "Share the Burden", "Having more cavalry increase your travel speed. ", " "));
        skillTreeDict.Add("I7A", new Perk("I7A", false, "Genoese Crossbow", "Crossbowman range increases. ", " "));
        skillTreeDict.Add("I7B", new Perk("I7B", false, "Excellent Swordsmith", "Swordsman damage increases. ", " "));
        skillTreeDict.Add("I7C", new Perk("I7C", false, "Spanish Musket", "Musketeer aim increases.", " "));
        skillTreeDict.Add("I7D", new Perk("I7D", false, "Milanese Armor", "Halberdier armor increases. ", " "));
        skillTreeDict.Add("I8A", new Perk("I8A", false, "Rorate Caerli", "Crossbowman hail of arrows damage increases. ", " "));
        skillTreeDict.Add("I8B", new Perk("I8B", false, "Bolognese Trainingr", "ESwordsman dodge rate increases", " "));
        skillTreeDict.Add("I8C", new Perk("I8C", false, "Harder than Steel", "Musketeer penetrates armor. ", " "));
        skillTreeDict.Add("I8D", new Perk("I8D", false, "Second Wind", "Halberdier heals more per turn. ", " "));
        skillTreeDict.Add("I9A", new Perk("I9A", false, "Song of Roland", "Cavalry deals more damage with charge. ", " "));
        skillTreeDict.Add("I9B", new Perk("I9B", false, "Anatolian Breed", "Cavalry has more stamina. ", " "));
        skillTreeDict.Add("I9C", new Perk("I9C", false, "Silver Tongue", "Easier to convince enemy to leave. ", " "));
        skillTreeDict.Add("I9D", new Perk("I9D", false, "Shrewd like a Fox", "Easier to ambush. ", " "));
        skillTreeDict.Add("I10A", new Perk("I10A", false, "Cautious", "Easier to retreat. ", " "));
        skillTreeDict.Add("I10B", new Perk("I10B", false, "Strange Eyeglasses", "Greater sight range. ", " "));
    }
    public Perk getPerk(string ID)
    {
        if (skillTreeDict.ContainsKey(ID))
        {
            return skillTreeDict[ID];
        }
        return null;
    }
}

public class Perk
{
    public string skillName { get; set;}
    public string description { get; set; }
    public string quote { get; set; }
    public string skillPointID { get; set; }
    public bool own { get; set; }
    public Button button { get; set; }
    public Perk(string skillPointIDI, bool ownI, string skillNameI, string descriptionI, string quoteI)
    {
        skillPointID = skillPointIDI;
        own = ownI;
        skillName = skillNameI;
        description = descriptionI;
        quote = quoteI;
    }
}
