using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopPlacing : MonoBehaviour {
    public static TroopPlacing troopPlacing;
    public GameObject troopUnit, baseCharacter;
    public Text troopUnitName, troopUnitLevel;
    public RawImage troopIcon, troopBackground, maxHealthBar, healthBar,
        maxStamina, stamina;
    public Texture2D unplacedBackground, placedBackground;
    public static List<Person> battleTroop;
    public Dictionary<Person, GameObject> troopDict;
    public Person placingUnit;
    public static bool pointerInPlacingPanel = false;
    public bool initialized, placing, showing;
    public Animator animator;
    private void Awake()
    {
        troopPlacing = this;
        gameObject.SetActive(false);
        
        initialized = false;
        showing = false;
        troopDict = new Dictionary<Person, GameObject>();
    }
    // Use this for initialization
    void Start () {
        showing = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (BattleCentralControl.battleStart && gameObject.activeSelf && BattleCentralControl.playerTurn)
        {
            if (!initialized)
            {
                troopPlacingInitialization();
            }
            if (placing && Input.anyKeyDown)
            {
                if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    if (placeTroop(placingUnit))
                    {
                        troopDict[placingUnit].transform.Find("TroopUnitBackground").GetComponent<RawImage>().texture = placedBackground;
                    }

                }
                else
                {
                    placing = false;
                }

            }
        }
        
	}
    public void troopPlacingInitialization()
    {
        foreach (Person p in battleTroop)
        {
            addToPlacing(p);
        }
        troopUnit.SetActive(false);
        initialized = true;
    }
    void addToPlacing(Person unit)
    {
        troopUnitName.text = unit.name;
        troopUnitLevel.text = unit.exp.level.ToString();
        troopBackground.texture = unplacedBackground;
        troopIcon.texture = TroopImgDataBase.troopImgDataBase.getTroopIcon(unit.faction, unit.troopType, unit.ranking);
        GameObject newTroopUnit = Object.Instantiate(troopUnit);
        newTroopUnit.GetComponent<Button>().onClick.AddListener(delegate { placingTroopUnitOnClick(unit); });
        // BattleInteraction.curControlled.GetComponent<PlayerTroop>().hideIndicators();
        troopDict.Add(unit, newTroopUnit);
        newTroopUnit.transform.SetParent(troopUnit.transform.parent, false);
    }

    void placingTroopUnitOnClick(Person unit)
    {
        if (unit.troop == null) //if havent placed on map
        {
            placingUnit = unit;
            placing = true;
        } else
        {
            if (BattleCentralControl.playerTurn)
            {
                if (BattleInteraction.curControlled != null)
                {
                    BattleInteraction.curControlled.GetComponent<Troop>().cameraFocusOnExit();
                    
                }
                BattleInteraction.curControlled = unit.troop.gameObject;
                BattleInteraction.curControlled.GetComponent<Troop>().cameraFocusOn();
            }
            //BattleCamera.target   
        }
    }
    
    public void removePlacingButton(Person person)
    {
        GameObject.Destroy(troopDict[person]);
        troopDict.Remove(person);
    }

    bool placeTroop(Person unit)
    {
        //write wait for click
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject.transform.parent.gameObject;

            if (interactedObject.tag == "Grid" )
            {
                Grid gridInfo = interactedObject.GetComponent<GridObject>().grid;
                if (gridInfo.z <= BattleCentralControl.playerParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax))
                {
                    Info.clearInfo();
                    if (gridInfo.personOnGrid == null && !unit.inBattle)
                    {
                        var pos = new Vector3(gridInfo.x, 1.5f, gridInfo.z);
                        var rot = new Quaternion(0, 0, 0, 0);
                        GameObject unitToPlace = baseCharacter;
                        GameObject model = Object.Instantiate(TroopDataBase.troopDataBase.getTroopObject(unit.faction, unit.troopType, unit.ranking), new Vector3(0, 0, 0), rot);
                        unitToPlace.GetComponent<Troop>().model = model;
                        model.transform.SetParent(unitToPlace.transform, false);
                        model.SetActive(true);
                        //unitToPlace.GetComponent<Troop>().troopAnimation = model.GetComponent<Animation>
                        if (unitToPlace == null)
                        {
                            Debug.Log(unit.name);
                        }
                        var duplicatedUnitToPlace = interactedObject.GetComponent<GridObject>().placeTroopOnGrid(unitToPlace, pos, rot);
                        duplicatedUnitToPlace.GetComponent<Troop>().placed(unit, gridInfo);
                        duplicatedUnitToPlace.GetComponent<Troop>().importPlacingBarSetting(
                            troopDict[unit].GetComponent<PlacingUnit>().healthBar, troopDict[unit].GetComponent<PlacingUnit>().staminaBar);
                        unit.inBattle = true;
                        placing = false;
                        Object.DestroyImmediate(model);
                        return true;
                    }
                } else
                {
                    Info.displayInfo("Please click on allowed grids: " + BattleCentralControl.playerParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax) + " from your side.");
                }
                
            } else
            {
                Info.displayInfo("Please click on grids");
            }
        }
        return false;
    }
    public void showHidePanel()
    {
        showing = !showing;
        animator.SetBool("showPanel", showing);
    }
    public void pointerEnterPlacing()
    {
        pointerInPlacingPanel = true;
    }
    public void pointerExitPlacing()
    {
        pointerInPlacingPanel = false;
    }
}
