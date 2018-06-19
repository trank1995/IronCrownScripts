using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Interactable
{
    public string lName;
    public string ID;
    public Texture2D cityIcon;
    public Party cityGuard, cityTrader;
    public int cash, guardBattleValue;
    public int prosperity, encampmentPrice;
    public bool encampmentAvailable = false;
    public bool initialized = false;
    public Vector3 position;
    public List<Item> warehouse;
    public override void Start()
    {
        base.Start();
        dialogue = new string[] { "hello", "hi" };
        interactableType = InteractableType.city;
        cash = 2000;
        guardBattleValue = 200;
        //townGuard.belongedTown = this;
    }
    public override void Update()
    {
        if (MapManagement.mapManagement != null && !initialized)
        {
            lName = gameObject.name;
            ID = lName.Substring(0, 3).ToUpper();
            position = transform.position;
            cityGuard = new Party(lName + " Guard", Faction.italy, guardBattleValue);
            cityGuard.makeParty();
            cityGuard.hasShape = false;
            cityGuard.belongedCity = this;
            cityTrader = new Party(lName + " Trader", Faction.italy, 100);
            cityTrader.hasShape = false;
            cityTrader.belongedCity = this;
            cityTrader.cash = cash;
            initializeTraderInventory();
            warehouse = new List<Item>();
            MapManagement.cities.Add(this);
            MapManagement.mapManagement.loadCitySave();
            initialized = true;
        }
    }
    public override void interact()
    {
        base.interact();
        DialogueSystem.Instance.createDialogue(PanelType.city, cityGuard);
    }



    public int getEncampmentPrice()
    {
        return encampmentPrice;
    }
    public List<Item> getTradeInventory()
    {
        return cityTrader.inventory;
    }
    public List<Item> getWarehouseInventory()
    {
        return warehouse;
    }
    public string[] getGarrisonInfo()
    {
        string[] result = dialogue;

        return result;
    }
    //public List<Quest> getavailableQuests()
    //{
    //    List<Quest> quests = new List<Quest>();
    //    if (!checkPlayerQuestID(QuestType.ASN + ID))
    //    {
    //        quests.Add(QuestDataBase.dataBase.getQuest(QuestType.ASN + ID));
    //    }
    //    if (!checkPlayerQuestID(QuestType.HUN + ID))
    //    {
    //        quests.Add(QuestDataBase.dataBase.getQuest(QuestType.HUN + ID));
    //    }
    //
    //    return quests;
    //}
    bool checkPlayerQuestID(string id)
    {
        foreach (Quest q in Player.mainParty.unfinishedQuests)
        {
            if (q.questID == id)
            {
                return true;
            }
        }
        return false;
    }
    void initializeTraderInventory()
    {
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Parchment"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Wool"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Pottery"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Hemp"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Medicinal Liqueur"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Rose"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Majolica"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Lace"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Embroidery"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Livestock"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Bronzeware"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Leatherware"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Ale"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Wine"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Cheese"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Wheat"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Fruit"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Prosciutto"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Olive Oil"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Fish"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Salt"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Vegetable"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Honey"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Gold Ore"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Marble"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Bronze"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Silk Thread"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Alum"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Timber"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Iron Ore"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Woad"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Crossbow"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Horse"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Armor"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Fire Arm"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Weapon"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Manuscript"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Velvet"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Fur"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Amber"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Slave"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Coral"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Silk Textile"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Antique"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Glassware"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Pepper"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Clove"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Ottoman Tapestry"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("China"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Silverware"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Tanned Leather"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Intricate Gear"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Sturdy Sinew"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Fine Whetstone"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Steel Ingot"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Coal"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Saltpetre"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Sulfur"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Twine"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Iron Mail"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Beewax"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Tools"));
        cityTrader.inventory.Add(ItemDataBase.dataBase.getItem("Supplies"));
    }
}
