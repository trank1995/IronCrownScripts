using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopSelection : MonoBehaviour
{
    public GameObject battleInitializationPanel, selectingTroopUnitButton;
    public Text selectingTroopName, selectingTroopLevel;
    public RawImage selectingMaxHealthBar, selectingHealthBar, selectingStaminaBar;
    public Text inspectTroopName, inspectTroopLevel, inspectMaxHealthNum, inspectHealthNum,
        inspectStamina, inspectS, inspectA, inspectP, inspectE;
    public RawImage inspectMaxHealthBar, inspectHealthBar, inspectStaminaBar,
        inspectSBar, inspectABar, inspectPBar, inspectEBar;
    public Button addButton, removeButton, startButton;
    public GameObject selectedTroopUnitButton;
    public Text selectedTroopName, selectedTroopLevel;
    public RawImage selectedMaxHealthBar, selectedHealthBar, selectedStaminaBar;
    public Texture2D regularTroopBackground, curSelectingBackgroundImage, curSelectedBackgroundImage;
    public RawImage troopIcon;
    bool inSelecting;
    GameObject curSelectingButton, curSelectedButton;
    Person curSelectingPerson, curSelectedPerson;
    public GameObject troopPlacingPanel, endTurnPanel;
    public List<Person> selectingMembers, selectedMembers;
    Dictionary<Person, GameObject> troopDict;
    float STATS_BAR_WIDTH, STATS_BAR_HEIGHT, UNIT_BAR_WIDTH, UNIT_BAR_HEIGHT;
    bool initialized = false;
    // Use this for initialization
    void Start()
    {

        initialized = false;
        STATS_BAR_WIDTH = inspectSBar.rectTransform.sizeDelta.x;
        STATS_BAR_HEIGHT = inspectSBar.rectTransform.sizeDelta.y;
        UNIT_BAR_WIDTH = selectingMaxHealthBar.rectTransform.sizeDelta.x;
        UNIT_BAR_HEIGHT = selectingMaxHealthBar.rectTransform.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized && BattleCentralControl.playerParty != null)
        {
            initialization();
            selectingTroopUnitButton.SetActive(false);
            selectedTroopUnitButton.SetActive(false);
            arrangeButtons();
            inspectUnit(curSelectingPerson);
            initialized = true;
            updateTroopUnitBackground();
        }
        if (initialized)
        {
            if (inSelecting)
            {
                if (curSelectingPerson != null)
                {
                    inspectUnit(curSelectingPerson);
                }
            }
            else
            {
                if (curSelectedPerson != null)
                {
                    inspectUnit(curSelectedPerson);
                }
            }
            if (selectedMembers.Contains(curSelectingPerson) || curSelectingButton == null || !inSelecting)
            {
                addButton.interactable = false;
            }
            else
            {
                addButton.interactable = true;
            }
            if (selectingMembers.Contains(curSelectedPerson) || curSelectedButton == null || inSelecting)
            {
                removeButton.interactable = false;
            }
            else
            {
                removeButton.interactable = true;
            }
            if (selectedMembers.Count == 0)
            {
                startButton.interactable = false;
            }
            else
            {
                startButton.interactable = true;
            }
        }
    }

    void initialization()
    {
        inSelecting = true;
        selectingMembers = new List<Person>();
        selectingMembers = new List<Person>(BattleCentralControl.playerParty.partyMember);
        curSelectingPerson = BattleCentralControl.playerParty.leader;
        selectedMembers = new List<Person>();
        troopDict = new Dictionary<Person, GameObject>();

        addButton.GetComponent<Button>().onClick.AddListener(delegate { addToSelected(); });
        removeButton.GetComponent<Button>().onClick.AddListener(delegate { removeFromSelected(); });
        startButton.GetComponent<Button>().onClick.AddListener(delegate { startBattle(); });
    }
    GameObject makeTroopSelectingButton(Person unit)
    {
        selectingTroopName.text = unit.name;
        selectingTroopLevel.text = unit.exp.level.ToString();
        selectingMaxHealthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(UNIT_BAR_WIDTH * (unit.getHealthMax() / Player.mainParty.getMaxHealth()), 0, UNIT_BAR_WIDTH), UNIT_BAR_HEIGHT);
        selectingHealthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(selectingMaxHealthBar.rectTransform.sizeDelta.x * (unit.health / unit.getHealthMax()), 0, UNIT_BAR_WIDTH), UNIT_BAR_HEIGHT);
        selectingStaminaBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(UNIT_BAR_WIDTH * (unit.getStaminaMax() / Player.mainParty.getMaxStamina()), 0, UNIT_BAR_WIDTH), UNIT_BAR_HEIGHT);
        GameObject troopUnit = Object.Instantiate(selectingTroopUnitButton);
        troopUnit.transform.Find("TroopUnitBackground").GetComponent<RawImage>().texture
                = regularTroopBackground;
        troopUnit.transform.SetParent(selectingTroopUnitButton.transform.parent, false);
        troopUnit.GetComponent<Button>().onClick.AddListener(delegate {
            curSelectingPerson = unit;
            inSelecting = true; updateTroopUnitBackground();
        });
        troopUnit.SetActive(true);
        return troopUnit;
    }
    GameObject makeTroopSelectedButton(Person unit)
    {
        selectedTroopName.text = unit.name;
        selectedTroopLevel.text = unit.exp.level.ToString();
        selectedMaxHealthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(UNIT_BAR_WIDTH * (unit.getHealthMax() / Player.mainParty.getMaxHealth()), 0, UNIT_BAR_WIDTH), UNIT_BAR_HEIGHT);
        selectedHealthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(selectedMaxHealthBar.rectTransform.sizeDelta.x * (unit.health / unit.getHealthMax()), 0, UNIT_BAR_WIDTH), UNIT_BAR_HEIGHT);
        selectedStaminaBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(UNIT_BAR_WIDTH * (unit.getStaminaMax() / Player.mainParty.getMaxStamina()), 0, UNIT_BAR_WIDTH), UNIT_BAR_HEIGHT);
        GameObject troopUnit = Object.Instantiate(selectedTroopUnitButton);
        troopUnit.transform.Find("TroopUnitBackground").GetComponent<RawImage>().texture
                = regularTroopBackground;
        troopUnit.transform.SetParent(selectedTroopUnitButton.transform.parent, false);
        troopUnit.GetComponent<Button>().onClick.AddListener(delegate {
            curSelectedPerson = unit;
            inSelecting = false; updateTroopUnitBackground();
        });
        troopUnit.SetActive(true);
        return troopUnit;
    }
    void inspectUnit(Person unit)
    {
        troopIcon.texture = TroopImgDataBase.troopImgDataBase.getTroopIcon(unit.faction, unit.troopType, unit.ranking);
        curSelectingPerson = unit;
        curSelectingButton = troopDict[curSelectingPerson];
        inspectTroopName.text = unit.name;
        inspectTroopLevel.text = unit.exp.level.ToString();
        inspectMaxHealthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATS_BAR_WIDTH * (unit.getHealthMax() / Player.mainParty.getMaxHealth()), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectHealthBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(inspectMaxHealthBar.rectTransform.sizeDelta.x * (unit.health / unit.getHealthMax()), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectStaminaBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATS_BAR_WIDTH * (unit.getStaminaMax() / Player.mainParty.getMaxStamina()), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectSBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATS_BAR_WIDTH * (unit.stats.strength / 10.0f), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectABar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATS_BAR_WIDTH * (unit.stats.agility / 10.0f), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectPBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATS_BAR_WIDTH * (unit.stats.perception / 10.0f), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectEBar.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATS_BAR_WIDTH * (unit.stats.endurance / 10.0f), 0, STATS_BAR_WIDTH), STATS_BAR_HEIGHT);
        inspectStamina.text = ((int)unit.getStaminaMax()).ToString();
        inspectHealthNum.text = ((int)unit.health).ToString();
        inspectMaxHealthNum.text = ((int)unit.getHealthMax()).ToString();
        inspectS.text = unit.stats.strength.ToString();
        inspectA.text = unit.stats.agility.ToString();
        inspectP.text = unit.stats.perception.ToString();
        inspectE.text = unit.stats.endurance.ToString();
    }
    void addToSelected()
    {
        int index = 0;
        if (!selectedMembers.Contains(curSelectingPerson))
        {
            selectedMembers.Add(curSelectingPerson);
            index = selectingMembers.IndexOf(curSelectingPerson);
            selectingMembers.Remove(curSelectingPerson);
            reArrangeButtons();
            Object.Destroy(curSelectingButton);
        }
        if (selectingMembers.Count > 0)
        {
            if (index < selectingMembers.Count)
            {
                curSelectingPerson = selectingMembers[index];
            }
            else
            {
                curSelectingPerson = selectingMembers[selectingMembers.Count - 1];
            }

        }
        else
        {
            curSelectingPerson = null;
        }
        updateTroopUnitBackground();
    }
    void removeFromSelected()
    {
        int index = 0;
        if (!selectingMembers.Contains(curSelectedPerson))
        {
            selectingMembers.Add(curSelectingPerson);
            index = selectedMembers.IndexOf(curSelectedPerson);
            selectedMembers.Remove(curSelectedPerson);
            reArrangeButtons();
            Object.Destroy(curSelectedButton);
        }
        if (selectedMembers.Count > 0)
        {
            if (index < selectedMembers.Count)
            {
                curSelectedPerson = selectedMembers[index];
            }
            else
            {
                curSelectedPerson = selectedMembers[selectedMembers.Count - 1];
            }

        }
        else
        {
            curSelectedPerson = null;
        }
        updateTroopUnitBackground();
    }
    void updateTroopUnitBackground()
    {
        foreach (KeyValuePair<Person, GameObject> button in troopDict)
        {
            button.Value.transform.Find("TroopUnitBackground").GetComponent<RawImage>().texture
                = regularTroopBackground;
        }
        if (inSelecting)
        {
            if (curSelectingPerson != null)
            {
                curSelectingButton = troopDict[curSelectingPerson];
            }
            if (curSelectingButton != null)
            {
                curSelectingButton.transform.Find("TroopUnitBackground").GetComponent<RawImage>().texture
                = curSelectingBackgroundImage;
            }
        }
        else
        {
            if (curSelectedPerson != null)
            {
                curSelectedButton = troopDict[curSelectedPerson];
            }
            if (curSelectedButton != null)
            {
                curSelectedButton.transform.Find("TroopUnitBackground").GetComponent<RawImage>().texture
                = curSelectedBackgroundImage;
            }
        }


    }
    void arrangeButtons()
    {
        if (selectingMembers.Count > 0)
        {
            selectingMembers = sortList(selectingMembers);
        }
        foreach (Person p in selectingMembers)
        {
            troopDict.Add(p, makeTroopSelectingButton(p));
        }
    }
    void reArrangeButtons()
    {
        if (selectingMembers.Count > 0)
        {
            selectingMembers = sortList(selectingMembers);
        }
        if (selectedMembers.Count > 0)
        {
            selectedMembers = sortList(selectedMembers);
        }
        foreach (Person p in selectingMembers)
        {
            Object.Destroy(troopDict[p]);
        }
        foreach (Person p in selectedMembers)
        {
            Object.Destroy(troopDict[p]);
        }
        //for (int i = 0; i < selectingTroopUnitButton.gameObject.transform.parent.gameObject.transform.childCount; i++)
        //{
        //    var child = selectingTroopUnitButton.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject;
        //    if (child != null && child.activeSelf)
        //    {
        //        Destroy(child);
        //    }
        //}
        //for (int i = 0; i < selectedTroopUnitButton.gameObject.transform.parent.gameObject.transform.childCount; i++)
        //{
        //    var child = selectedTroopUnitButton.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject;
        //    if (child != null && child.activeSelf)
        //    {
        //        Destroy(child);
        //    }
        //}
        troopDict.Clear();
        foreach (Person p in selectingMembers)
        {

            troopDict.Add(p, makeTroopSelectingButton(p));
        }
        foreach (Person p in selectedMembers)
        {
            troopDict.Add(p, makeTroopSelectedButton(p));
        }
    }
    void startBattle()
    {
        battleInitializationPanel.SetActive(false);
        BattleCamera.startBattle = true;
        BattleCentralControl.battleStart = true;
        BattleCentralControl.playerTotal = selectedMembers.Count;
        TroopPlacing.battleTroop = selectedMembers;
        troopPlacingPanel.SetActive(true);
        endTurnPanel.SetActive(true);
    }
    List<Person> sortList(List<Person> listToSort)
    {
        listToSort.Sort(compareName);
        List<Person> temp = new List<Person>();
        foreach (Person p in listToSort)
        {
            if (p.ranking == Ranking.mainChar)
            {
                temp.Add(p);
            }
        }
        if (temp.Count == 2)
        {
            listToSort.Remove(Player.mainCharacter);
            listToSort.Remove(Player.secCharacter);
            listToSort.Insert(0, Player.secCharacter);
            listToSort.Insert(0, Player.mainCharacter);
        }
        else if (temp.Count == 1)
        {
            listToSort.Remove(temp[0]);
            listToSort.Insert(0, temp[0]);
        }
        return listToSort;

    }
    int compareName(Person a, Person b)
    {
        return a.name.CompareTo(b.name);
    }
}
