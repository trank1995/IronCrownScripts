using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleInteraction : MonoBehaviour {
    const float INTERACT_DIST = 1;
    GameObject player;
    List<GameObject> inspectedList = new List<GameObject>();
    public static bool inAction;
    public static TroopSkill skillMode;
    public static GameObject curControlled;
    public static GameObject interactedObject;
    public static GameObject inspectedObject;
    public GameObject walkIndicator;
    // Use this for initialization
    void Start()
    {
        skillMode = TroopSkill.walk;
        inAction = false;
    }


    // Update is called once per frame
    void Update()
    {
        inputKeysActions();
        if (curControlled != null)
        {
            showIndicator();
        }
        
        
    }
    void inputKeysActions()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (BattleCentralControl.playerTurn && curControlled != null && skillMode != TroopSkill.walk)
            {
                curControlled.GetComponent<Troop>().doSkill(curControlled.GetComponent<Troop>().indicatedGrid(), skillMode);
                if (Input.GetMouseButtonDown(0))
                {
                    skillMode = TroopSkill.walk;
                }
            } else
            {   
                if (!inAction)
                {
                    selectObject();
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (curControlled != null && BattleCentralControl.playerTurn)
            {

                if (skillMode == TroopSkill.walk)
                {
                    if (curControlled.GetComponent<Troop>().reachedDestination)
                    {
                        curControlled.GetComponent<Troop>().cameraFocusOn();
                        walkToObj();
                    }
                    
                }
                else if (skillMode == TroopSkill.none && Input.GetMouseButtonDown(1))
                {
                    skillMode = TroopSkill.walk;
                }
                else
                {
                    skillMode = TroopSkill.none;
                    curControlled.GetComponent<Troop>().hideIndicators();
                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (BattleCentralControl.playerTurn)
            {
                if (curControlled != null)
                {
                    curControlled.GetComponent<Troop>().cameraFocusOnExit();
                    curControlled = getNextPersonOnField(curControlled.GetComponent<Troop>().person).troop.gameObject;
                    curControlled.GetComponent<Troop>().cameraFocusOn();
                } else
                {
                    foreach (KeyValuePair<Person, GameObject> pair in BattleCentralControl.playerTroopOnField)
                    {
                        BattleInteraction.curControlled = pair.Value;
                        BattleInteraction.curControlled.GetComponent<Troop>().cameraFocusOn();
                        break;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (skillMode == TroopSkill.walk)
            {
                SceneManager.LoadScene("MenuScene");
            }
            else
            {
                skillMode = TroopSkill.walk;
                
            }
        }
    }
    void selectObject()
    {
        if (curControlled != null)
        {
            curControlled.GetComponent<Troop>().controlled = false;
            curControlled.GetComponent<Troop>().cameraFocusOnExit();
            curControlled = null;
        }
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            if (interactedObject != null && interactedObject.GetComponent<BattleInteractable>() != null)
            {
                interactedObject.GetComponent<BattleInteractable>().cameraFocusOnExit();
            }
            
            interactedObject = interactionInfo.collider.gameObject.transform.parent.gameObject;
            
            if (interactedObject.tag == "Troop")
            {
                interactedObject.GetComponent<Troop>().cameraFocusOn();
                if (interactedObject.GetComponent<Troop>().person.faction == Faction.mercenary
                    && BattleCentralControl.playerTurn)
                {
                    curControlled = interactedObject;
                    skillMode = TroopSkill.walk;
                }
            } else if (interactedObject.tag == "Grid")
            {
                interactedObject.GetComponent<GridObject>().cameraFocusOn();
            } else
            {
                Debug.Log(interactedObject.name);
            }
            
        }
    }

    void walkToObj()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            interactedObject = interactionInfo.collider.gameObject.transform.parent.gameObject;
            if (interactedObject.tag == "Grid")
            {
                if (skillMode == TroopSkill.walk)
                {
                    interactedObject.GetComponent<GridObject>().moveTroopToGrid(curControlled);
                }
                else
                {
                    interactedObject.GetComponent<GridObject>().cameraFocusOn();
                }
            } else
            {
                curControlled.GetComponent<Troop>().walkIndicator.GetComponent<Indicator>().goToIndicatedGrid(curControlled);
            }
        } else
        {
            if (curControlled.GetComponent<Troop>().walkIndicator.activeSelf)
            {
                curControlled.GetComponent<Troop>().walkIndicator.GetComponent<Indicator>().goToIndicatedGrid(curControlled);
            }
        }
    }
    
    void showIndicator()
    {
        switch(skillMode)
        {
            case TroopSkill.walk:
                curControlled.GetComponent<Troop>().hideIndicators();
                curControlled.GetComponent<Troop>().walk();
                break;
            case TroopSkill.lunge:
                curControlled.GetComponent<Troop>().lunge();
                break;
            case TroopSkill.whirlwind:
                curControlled.GetComponent<Troop>().whirlwind();
                break;
            case TroopSkill.execute:
                curControlled.GetComponent<Troop>().execute();
                break;
            case TroopSkill.fire:
                curControlled.GetComponent<Troop>().fire();
                break;
            case TroopSkill.holdSteady:
                curControlled.GetComponent<Troop>().holdSteady();
                break;
            case TroopSkill.quickDraw:
                curControlled.GetComponent<Troop>().quickDraw();
                break;
            case TroopSkill.rainOfArrows:
                curControlled.GetComponent<Troop>().rainOfArrow();
                break;
            case TroopSkill.guard:
                curControlled.GetComponent<Troop>().guard();
                break;
            case TroopSkill.charge:
                curControlled.GetComponent<Troop>().charge();
                break;
        }

    }

    Person getNextPersonOnField(Person curPerson)
    {
        int index = BattleCentralControl.playerParty.partyMember.IndexOf(curPerson);
        if (index == BattleCentralControl.playerParty.partyMember.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        Person result = BattleCentralControl.playerParty.partyMember[index];
        while (result.troop == null || !result.troop.activated)
        {
            if (index == BattleCentralControl.playerParty.partyMember.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            result = BattleCentralControl.playerParty.partyMember[index];
        }
        return result;
    }
}
