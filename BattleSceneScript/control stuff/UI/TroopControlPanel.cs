using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopControlPanel : MonoBehaviour {
    public GameObject panel, lunge, whirlwind, execute, guard, charge, fire, holdSteady, rainOfArrows, quickDraw, chargingMark, holdSteadyingMark;
    public KeyCode lungeKey, whirlwindKey, executeKey, guardKey, chargeKey, fireKey, holdSteadyKey, rainOfArrowsKey, quickDrawkey;
    public GameObject curControlledTroop;
    public static bool initialized;
	// Use this for initialization
	void Start () {
        
        panel.SetActive(false);
        initialized = false;
        initializeButtons();
	}
	
	// Update is called once per frame
	void Update () {
        if (BattleInteraction.curControlled == null)
        {
            hidePanel();
        } else if (BattleInteraction.curControlled != null && !initialized)
        {
            switch (BattleInteraction.curControlled.GetComponent<Troop>().person.troopType)
            {
                case TroopType.mainCharType:
                    makeMainCharacterPanel();
                    break;
                case TroopType.crossbowman:
                    makeCrossbowmanPanel();
                    break;
                case TroopType.musketeer:
                    makeMusketeerPanel();
                    break;
                case TroopType.swordsman:
                    makeSwordsmanPanel();
                    break;
                case TroopType.halberdier:
                    makeHalberdierPanel();
                    break;
                case TroopType.cavalry:
                    makeCavalryPanel();
                    break;
            }
            initialized = true;
        }
        if (gameObject.activeSelf)
        {
            TroopType curTT = BattleInteraction.curControlled.GetComponent<Troop>().person.troopType;
            if (Input.GetKeyUp(lungeKey) && curTT != TroopType.crossbowman)
            {
                BattleInteraction.skillMode = TroopSkill.lunge; hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(whirlwindKey))
            {
                BattleInteraction.skillMode = TroopSkill.whirlwind; hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(executeKey) && (curTT == TroopType.mainCharType || curTT == TroopType.recruitType || curTT == TroopType.swordsman))
            {
                BattleInteraction.skillMode = TroopSkill.execute; hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(guardKey) && (curTT == TroopType.mainCharType || curTT == TroopType.halberdier))
            {
                BattleInteraction.skillMode = TroopSkill.guard; hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(chargeKey) && (curTT == TroopType.mainCharType || curTT == TroopType.cavalry))
            {
                BattleInteraction.curControlled.GetComponent<Troop>().charge(); hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(fireKey) && (curTT == TroopType.mainCharType || curTT == TroopType.musketeer))
            {
                BattleInteraction.skillMode = TroopSkill.fire; hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(holdSteadyKey) && (curTT == TroopType.mainCharType || curTT == TroopType.musketeer))
            {
                BattleInteraction.curControlled.GetComponent<Troop>().holdSteady(); hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(rainOfArrowsKey) && (curTT == TroopType.crossbowman))
            {
                BattleInteraction.skillMode = TroopSkill.rainOfArrows; hideIndicatorsInPanel();
            }
            if (Input.GetKeyUp(quickDrawkey) && (curTT == TroopType.crossbowman))
            {
                BattleInteraction.skillMode = TroopSkill.quickDraw; hideIndicatorsInPanel();
            }
            if (chargingMark.activeSelf != BattleInteraction.curControlled.GetComponent<Troop>().charging)
            {
                chargingMark.SetActive(BattleInteraction.curControlled.GetComponent<Troop>().charging);
            }
            if (holdSteadyingMark.activeSelf != BattleInteraction.curControlled.GetComponent<Troop>().holdSteadying)
            {
                holdSteadyingMark.SetActive(BattleInteraction.curControlled.GetComponent<Troop>().holdSteadying);
            }
        }
	}
    void makeMainCharacterPanel()
    {
        disableAllButton();
        lunge.SetActive(true);
        whirlwind.SetActive(true);
        execute.SetActive(true);
        guard.SetActive(true);
        charge.SetActive(true);
        fire.SetActive(true);
        holdSteady.SetActive(true);
        lungeKey = KeyCode.Alpha1;
        whirlwindKey = KeyCode.Alpha2;
        executeKey = KeyCode.Alpha3;
        guardKey = KeyCode.Alpha4;
        chargeKey = KeyCode.Alpha5;
        fireKey = KeyCode.Alpha6;
        holdSteadyKey = KeyCode.Alpha7;

    }
    void makeCrossbowmanPanel()
    {
        disableAllButton();
        whirlwind.SetActive(true);
        rainOfArrows.SetActive(true);
        quickDraw.SetActive(true);
        whirlwindKey = KeyCode.Alpha1;
        rainOfArrowsKey = KeyCode.Alpha2;
        quickDrawkey = KeyCode.Alpha3;
    }
    void makeMusketeerPanel()
    {
        disableAllButton();
        whirlwind.SetActive(true);
        fire.SetActive(true);
        holdSteady.SetActive(true);
        whirlwindKey = KeyCode.Alpha1;
        fireKey = KeyCode.Alpha2;
        holdSteadyKey = KeyCode.Alpha3;
    }
    void makeSwordsmanPanel()
    {
        disableAllButton();
        lunge.SetActive(true);
        whirlwind.SetActive(true);
        execute.SetActive(true);
        lungeKey = KeyCode.Alpha1;
        whirlwindKey = KeyCode.Alpha2;
        executeKey = KeyCode.Alpha3;
    }
    void makeHalberdierPanel()
    {
        disableAllButton();
        lunge.SetActive(true);
        whirlwind.SetActive(true);
        guard.SetActive(true);
        lungeKey = KeyCode.Alpha1;
        whirlwindKey = KeyCode.Alpha2;
        guardKey = KeyCode.Alpha3;
    }
    void makeCavalryPanel()
    {
        disableAllButton();
        lunge.SetActive(true);
        whirlwind.SetActive(true);
        charge.SetActive(true);
        lungeKey = KeyCode.Alpha1;
        whirlwindKey = KeyCode.Alpha2;
        chargeKey = KeyCode.Alpha3;
    }
    


    void disableAllButton()
    {
        lunge.SetActive(false);
        whirlwind.SetActive(false);
        execute.SetActive(false);
        guard.SetActive(false);
        charge.SetActive(false);
        fire.SetActive(false);
        holdSteady.SetActive(false);
        rainOfArrows.SetActive(false);
        quickDraw.SetActive(false);
    }

    
    Person getInfo(GameObject troop)
    {
        return troop.GetComponent<Troop>().person;
    }

    public void initializePanel()
    {
        initialized = false;
        panel.SetActive(true);
    }
    void initializeButtons()
    {
        lunge.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.lunge; hideIndicatorsInPanel(); });
        whirlwind.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.whirlwind; hideIndicatorsInPanel(); });
        execute.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.execute; hideIndicatorsInPanel(); });
        guard.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.guard; hideIndicatorsInPanel(); });
        charge.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.curControlled.GetComponent<Troop>().charge(); hideIndicatorsInPanel(); });
        fire.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.fire; hideIndicatorsInPanel(); });
        holdSteady.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.curControlled.GetComponent<Troop>().holdSteady(); hideIndicatorsInPanel(); });
        rainOfArrows.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.rainOfArrows; hideIndicatorsInPanel(); });
        quickDraw.GetComponent<Button>().onClick.AddListener(delegate { BattleInteraction.skillMode = TroopSkill.quickDraw; hideIndicatorsInPanel(); });
    }

    void hideIndicatorsInPanel()
    {
        if (BattleInteraction.curControlled != null)
        {
            BattleInteraction.curControlled.GetComponent<Troop>().hideIndicators();
        }
    }
    public void hidePanel()
    {
        initialized = false;
        panel.SetActive(false);
        
    }
    
}
