using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopUpgrade : MonoBehaviour {

    public Animator animator;
    public Button recruit, militiaCrossbowman, veteranCrossbowman, eliteCrossbowman;
    public Button militiaMusketeer, veteranMusketeer, eliteMusketeer;
    public Button militiaSwordsman, veteranSwordsman, eliteSwordsman;
    public Button militiaHalberdier, veteranHalberdier, eliteHalberdier;
    public Button militiaCavalry, veteranCavalry, eliteCavalry;
    public Button confirmButton, cancelButton;
    public Text curRanking, curTroopType, curArmor, curEvasion, curBlock, curVision, curStealth, curAccuracy, curMelee, curRanged, curMobility;
    public Text newRanking, newTroopType, newArmor, newEvasion, newBlock, newVision, newStealth, newAccuracy, newMelee, newRanged, newMobility;
    public RawImage curIcon, newIcon;
    public RawImage curArmorBar, curEvasionBar, curBlockBar, curVisionBar, curStealthBar, curAccuracyBar, curMeleeBar, curRangedBar, curMobilityBar;
    public RawImage newArmorBar, newEvasionBar, newBlockBar, newVisionBar, newStealthBar, newAccuracyBar, newMeleeBar, newRangedBar, newMobilityBar;
    public Person curPerson, newPerson;
    float STATS_BAR_WIDTH, STATS_BAR_HEIGHT;
	// Use this for initialization
	void Start () {
        STATS_BAR_WIDTH = curArmorBar.rectTransform.sizeDelta.x;
        STATS_BAR_HEIGHT = curArmorBar.rectTransform.sizeDelta.y;
        initialization();
    }
	
	// Update is called once per frame
	void Update () {
		if (curPerson != null)
        {
            showCur();
        }
        if (newPerson != null)
        {
            showNew();
            if (curPerson.ranking == newPerson.ranking && curPerson.troopType == newPerson.troopType)
            {
                confirmButton.interactable = false;
            } else
            {
                confirmButton.interactable = true;
            }
        }
        
	}
    public void showCurPerson(Person unit)
    {
        animator.SetBool("troopUpgradeShow", true);
        curPerson = unit;
        newPerson = new Person(unit.name, unit.stats, unit.ranking, unit.troopType, unit.faction, unit.exp);
    }
    
    void initialization()
    {
        confirmButton.onClick.AddListener(delegate { changeTroop(); });

        recruit.onClick.AddListener(delegate { recruitOnClick(); });
        militiaCrossbowman.onClick.AddListener(delegate { militiaCrossbowmanOnClick(); });
        veteranCrossbowman.onClick.AddListener(delegate { veteranCrossbowmanOnClick(); });
        eliteCrossbowman.onClick.AddListener(delegate { eliteCrossbowmanOnClick(); });

        militiaMusketeer.onClick.AddListener(delegate { militiaMusketeerOnClick(); });
        veteranMusketeer.onClick.AddListener(delegate { veteranMusketeerOnClick(); });
        eliteMusketeer.onClick.AddListener(delegate { eliteMusketeerOnClick(); });

        militiaSwordsman.onClick.AddListener(delegate { militiaSwordsmanOnClick(); });
        veteranSwordsman.onClick.AddListener(delegate { veteranSwordsmanOnClick(); });
        eliteSwordsman.onClick.AddListener(delegate { eliteSwordsmanOnClick(); });

        militiaHalberdier.onClick.AddListener(delegate { militiaHalberdierOnClick(); });
        veteranHalberdier.onClick.AddListener(delegate { veteranHalberdierOnClick(); });
        eliteHalberdier.onClick.AddListener(delegate { eliteHalberdierOnClick(); });

        militiaCavalry.onClick.AddListener(delegate { militiaCavalryOnClick(); });
        veteranCavalry.onClick.AddListener(delegate { veteranCavalryOnClick(); });
        eliteCavalry.onClick.AddListener(delegate { eliteCavalryOnClick(); });
    }


    void showNew()
    {
        newIcon.texture = TroopImgDataBase.troopImgDataBase.getTroopIcon(newPerson.faction, newPerson.troopType, newPerson.ranking);
        newRanking.text = TroopDataBase.rankingToString(newPerson.ranking);
        newTroopType.text = TroopDataBase.troopTypeToString(newPerson.troopType);
        TroopInfo troopInfo = TroopDataBase.troopDataBase.getTroopInfo(newPerson.faction, newPerson.troopType, newPerson.ranking);
        newArmor.text = (troopInfo.gear.armorRating).ToString();
        newEvasion.text = (troopInfo.gear.evasionRating).ToString();
        newBlock.text = (troopInfo.gear.blockRating).ToString();
        newVision.text = (troopInfo.gear.visionRating).ToString();
        newStealth.text = (troopInfo.gear.stealthRating).ToString();
        newAccuracy.text = (troopInfo.gear.accuracyRating).ToString();
        newMelee.text = (troopInfo.gear.meleeDmgRating).ToString();
        newRanged.text = ((troopInfo.gear.rangedDmgRating + troopInfo.gear.accuracyRating)/2).ToString();
        newMobility.text = (troopInfo.gear.mobilityRating).ToString();
        
        newArmorBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.armorRating) / 10);
        newEvasionBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.evasionRating) /10);
        newBlockBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.blockRating) /10);
        newVisionBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.visionRating)  / 10);
        newStealthBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.stealthRating) / 10);
        newAccuracyBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.accuracyRating) / 10);
        newMeleeBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.meleeDmgRating) /10);
        newRangedBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.rangedDmgRating) /20);
        newMobilityBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.armorRating) / 10);
    }

    void showCur()
    {
        curIcon.texture = TroopImgDataBase.troopImgDataBase.getTroopIcon(curPerson.faction, curPerson.troopType, curPerson.ranking);
        curRanking.text = TroopDataBase.rankingToString(curPerson.ranking);
        curTroopType.text = TroopDataBase.troopTypeToString(curPerson.troopType);
        TroopInfo troopInfo = TroopDataBase.troopDataBase.getTroopInfo(curPerson.faction, curPerson.troopType, curPerson.ranking);
        curArmor.text = (troopInfo.gear.armorRating).ToString();
        curEvasion.text = (troopInfo.gear.evasionRating).ToString();
        curBlock.text = (troopInfo.gear.blockRating).ToString();
        curVision.text = (troopInfo.gear.visionRating).ToString();
        curStealth.text = (troopInfo.gear.stealthRating).ToString();
        curAccuracy.text = (troopInfo.gear.accuracyRating).ToString();
        curMelee.text = (troopInfo.gear.meleeDmgRating).ToString();
        curRanged.text = ((troopInfo.gear.rangedDmgRating + troopInfo.gear.accuracyRating) / 2).ToString();
        curMobility.text = (troopInfo.gear.mobilityRating).ToString();

        curArmorBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.armorRating) / 10);
        curEvasionBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.evasionRating) / 10);
        curBlockBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.blockRating) / 10);
        curVisionBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.visionRating) / 10);
        curStealthBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.stealthRating) / 10);
        curAccuracyBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.accuracyRating) / 10);
        curMeleeBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.meleeDmgRating) / 10);
        curRangedBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.rangedDmgRating) / 20);
        curMobilityBar.rectTransform.sizeDelta = new Vector2(STATS_BAR_WIDTH, STATS_BAR_HEIGHT * (troopInfo.gear.armorRating) / 10);
    }

    void changeTroop()
    {
        //Player.mainParty.partyMember.Remove(curPerson);
        //Player.mainParty.partyMember.Add(newPerson);
        curPerson.ranking = newPerson.ranking;
        curPerson.troopType = newPerson.troopType;
        if (!curPerson.renamed)
        {
            curPerson.setDefaultName();
        }
        TroopManagement.troopManagement.upgradeCurPerson(curPerson);
        //curPerson = newPerson;
        //newPerson = new Person(newPerson.name, newPerson.stats, newPerson.ranking, newPerson.troopType, newPerson.faction, newPerson.exp);
    }


    void recruitOnClick()
    {
        newPerson.ranking = Ranking.recruit;
        newPerson.troopType = TroopType.recruitType;
    }
    void militiaCrossbowmanOnClick()
    {
        newPerson.ranking = Ranking.militia;
        newPerson.troopType = TroopType.crossbowman;
    }
    void veteranCrossbowmanOnClick()
    {
        newPerson.ranking = Ranking.veteran;
        newPerson.troopType = TroopType.crossbowman;
    }
    void eliteCrossbowmanOnClick()
    {
        newPerson.ranking = Ranking.elite;
        newPerson.troopType = TroopType.crossbowman;
    }
    void militiaMusketeerOnClick()
    {
        newPerson.ranking = Ranking.militia;
        newPerson.troopType = TroopType.musketeer;
    }
    void veteranMusketeerOnClick()
    {
        newPerson.ranking = Ranking.veteran;
        newPerson.troopType = TroopType.musketeer;
    }
    void eliteMusketeerOnClick()
    {
        newPerson.ranking = Ranking.elite;
        newPerson.troopType = TroopType.musketeer;
    }
    void militiaSwordsmanOnClick()
    {
        newPerson.ranking = Ranking.militia;
        newPerson.troopType = TroopType.swordsman;
    }
    void veteranSwordsmanOnClick()
    {
        newPerson.ranking = Ranking.veteran;
        newPerson.troopType = TroopType.swordsman;
    }
    void eliteSwordsmanOnClick()
    {
        newPerson.ranking = Ranking.elite;
        newPerson.troopType = TroopType.swordsman;
    }
    void militiaHalberdierOnClick()
    {
        newPerson.ranking = Ranking.militia;
        newPerson.troopType = TroopType.halberdier;
    }
    void veteranHalberdierOnClick()
    {
        newPerson.ranking = Ranking.veteran;
        newPerson.troopType = TroopType.halberdier;
    }
    void eliteHalberdierOnClick()
    {
        newPerson.ranking = Ranking.elite;
        newPerson.troopType = TroopType.halberdier;
    }
    void militiaCavalryOnClick()
    {
        newPerson.ranking = Ranking.militia;
        newPerson.troopType = TroopType.cavalry;
    }
    void veteranCavalryOnClick()
    {
        newPerson.ranking = Ranking.veteran;
        newPerson.troopType = TroopType.cavalry;
    }
    void eliteCavalryOnClick()
    {
        newPerson.ranking = Ranking.elite;
        newPerson.troopType = TroopType.cavalry;
    }
}
