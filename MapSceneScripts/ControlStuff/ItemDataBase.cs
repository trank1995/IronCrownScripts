using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase dataBase;
    //TODO
    public Texture2D parchmentImg, woolImg, potteryImg, hempImg, medicinalliqueurImq,
        roseImg, majolicaImg, laceImg, embroideryImg, livestockImg, bronzewareImg,
        aleImg, wineImg, cheeseImg, wheatImg, fruitImg, prosciuttoImg, oliveoilImg,
        fishImg, saltImg, vegetableImg, honeyImg, goldoreImg, marbleImg, bronzeImg,
        silkthreadImg, alumImg, timberImg, ironoreImg, woadImg, manuscriptImg, velvetImg,
        furImg, amberImg, slaveImg, coralImg, silktextileImg, antiqueImg, glasswareImg, pepperImg,
        cloveImg, ottomantapestryImg, chinaImg, silverwareImg, leatherwareImg, crossbowImg,
        horseImg, armorImg, firearmImg, weaponImg, tannedleatherImg, intricategearImg,
        sturdysinewImg, finewhetstoneImg, steelingotImg, coalImg, saltpetreImg, sulfurImg,
        twineImg, ironmailImg, beewaxImg, toolsImg, suppliesImg;

    public List<Item> itemList;




    // Use this for initialization
    void Awake()
    {
        dataBase = gameObject.GetComponent<ItemDataBase>();
        itemList = new List<Item>();
        itemInitialization();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public Item getItem(string itemName)
    {
        //Item result = null;
        foreach (Item item in itemList)
        {
            if (item.name == itemName)
            {
                //result = new Item(item);
                return item;
                //break;
            }
        }
        return null;
        //if (result == null)
        //{
        //    Debug.Log(itemName);
        //}
        //return result;
    }

    void itemInitialization()
    {
        //TODO
        itemList.Add(new Item("Parchment", ItemCategory.common, 20, ItemWeightClass.light, parchmentImg, "This is parchment"));
        itemList.Add(new Item("Wool", ItemCategory.common, 10, ItemWeightClass.medium, woolImg, "This is wool"));
        itemList.Add(new Item("Pottery", ItemCategory.common, 10, ItemWeightClass.heavy, potteryImg, "Made of dirt and cheap as dirt."));
        itemList.Add(new Item("Hemp", ItemCategory.common, 10, ItemWeightClass.medium, hempImg, "This is for rope. Really!"));
        itemList.Add(new Item("Medicinal Liqueur", ItemCategory.common, 40, ItemWeightClass.light, medicinalliqueurImq, "Approved by Florentine Dominican Abbey"));
        itemList.Add(new Item("Rose", ItemCategory.common, 40, ItemWeightClass.light, roseImg, "Makes you smell better......than others."));
        itemList.Add(new Item("Majolica", ItemCategory.common, 40, ItemWeightClass.medium, majolicaImg, "Bright blue, bring yellow, and bright green."));
        itemList.Add(new Item("Lace", ItemCategory.common, 30, ItemWeightClass.light, laceImg, "This is lace"));
        itemList.Add(new Item("Embroidery", ItemCategory.common, 30, ItemWeightClass.light, embroideryImg, "This is embroidery"));
        itemList.Add(new Item("Livestock", ItemCategory.common, 20, ItemWeightClass.heavy, livestockImg, "This is livestock"));
        itemList.Add(new Item("Bronzeware", ItemCategory.common, 30, ItemWeightClass.medium, bronzewareImg, "This is bronzeware"));
        itemList.Add(new Item("Leatherware", ItemCategory.common, 20, ItemWeightClass.medium, leatherwareImg, "Belt, purses, and all kinds of stuff."));
        itemList.Add(new Item("Ale", ItemCategory.food, 20, ItemWeightClass.medium, aleImg, "This get you drunk."));
        itemList.Add(new Item("Wine", ItemCategory.food, 20, ItemWeightClass.medium, wineImg, "This get you drunk with style."));
        itemList.Add(new Item("Cheese", ItemCategory.food, 20, ItemWeightClass.light, cheeseImg, "This is cheese"));
        itemList.Add(new Item("Wheat", ItemCategory.food, 10, ItemWeightClass.light, wheatImg, "This is wheat"));
        itemList.Add(new Item("Fruit", ItemCategory.food, 40, ItemWeightClass.medium, fruitImg, "This is fruit"));
        itemList.Add(new Item("Prosciutto", ItemCategory.food, 30, ItemWeightClass.light, prosciuttoImg, "This is prosciutto"));
        itemList.Add(new Item("Olive Oil", ItemCategory.food, 10, ItemWeightClass.medium, oliveoilImg, "This is olive oil"));
        itemList.Add(new Item("Fish", ItemCategory.food, 10, ItemWeightClass.medium, fishImg, "This is fish"));
        itemList.Add(new Item("Salt", ItemCategory.food, 20, ItemWeightClass.light, saltImg, "This is salt"));
        itemList.Add(new Item("Vegetable", ItemCategory.food, 10, ItemWeightClass.medium, vegetableImg, "This is vegetable"));
        itemList.Add(new Item("Honey", ItemCategory.food, 50, ItemWeightClass.light, honeyImg, "This is honey"));
        itemList.Add(new Item("Gold Ore", ItemCategory.industry, 60, ItemWeightClass.medium, goldoreImg, "This is gold ore"));
        itemList.Add(new Item("Marble", ItemCategory.industry, 60, ItemWeightClass.heavy, marbleImg, "This is marble"));
        itemList.Add(new Item("Bronze", ItemCategory.industry, 40, ItemWeightClass.heavy, bronzeImg, "This is bronze"));
        itemList.Add(new Item("Silk Thread", ItemCategory.industry, 70, ItemWeightClass.medium, silkthreadImg, "This is silk thread"));
        itemList.Add(new Item("Alum", ItemCategory.industry, 50, ItemWeightClass.heavy, alumImg, "This is alum"));
        itemList.Add(new Item("Timber", ItemCategory.industry, 20, ItemWeightClass.heavy, timberImg, "This is timber"));
        itemList.Add(new Item("Iron Ore", ItemCategory.industry, 30, ItemWeightClass.heavy, ironoreImg, "This is iron ore"));
        itemList.Add(new Item("Woad", ItemCategory.industry, 30, ItemWeightClass.medium, woadImg, "This is woad"));
        itemList.Add(new Item("Crossbow", ItemCategory.industry, 60, ItemWeightClass.heavy, crossbowImg, "This is crossbow"));
        itemList.Add(new Item("Horse", ItemCategory.industry, 60, ItemWeightClass.heavy, horseImg, "This is horse"));
        itemList.Add(new Item("Armor", ItemCategory.industry, 70, ItemWeightClass.heavy, armorImg, "This is armor"));
        itemList.Add(new Item("Fire Arm", ItemCategory.industry, 90, ItemWeightClass.heavy, firearmImg, "This is fire arm"));
        itemList.Add(new Item("Weapon", ItemCategory.industry, 70, ItemWeightClass.heavy, weaponImg, "This is weapon"));
        itemList.Add(new Item("Manuscript", ItemCategory.luxury, 50, ItemWeightClass.light, manuscriptImg, "This is manuscript"));
        itemList.Add(new Item("Velvet", ItemCategory.luxury, 80, ItemWeightClass.light, velvetImg, "This is velvet"));
        itemList.Add(new Item("Fur", ItemCategory.luxury, 90, ItemWeightClass.light, furImg, "This is fur"));
        itemList.Add(new Item("Amber", ItemCategory.luxury, 100, ItemWeightClass.light, amberImg, "This is amber"));
        itemList.Add(new Item("Slave", ItemCategory.luxury, 70, ItemWeightClass.medium, slaveImg, "This is slave"));
        itemList.Add(new Item("Coral", ItemCategory.luxury, 100, ItemWeightClass.light, coralImg, "This is coral"));
        itemList.Add(new Item("Silk Textile", ItemCategory.luxury, 100, ItemWeightClass.light, silktextileImg, "This is silk textile"));
        itemList.Add(new Item("Antique", ItemCategory.luxury, 70, ItemWeightClass.medium, antiqueImg, "This is antique"));
        itemList.Add(new Item("Glassware", ItemCategory.luxury, 50, ItemWeightClass.medium, glasswareImg, "This is glassware"));
        itemList.Add(new Item("Pepper", ItemCategory.luxury, 100, ItemWeightClass.light, pepperImg, "This is pepper"));
        itemList.Add(new Item("Clove", ItemCategory.luxury, 100, ItemWeightClass.light, cloveImg, "This is clove"));
        itemList.Add(new Item("Ottoman Tapestry", ItemCategory.luxury, 80, ItemWeightClass.medium, ottomantapestryImg, "This is Ottoman tapestry"));
        itemList.Add(new Item("China", ItemCategory.luxury, 80, ItemWeightClass.medium, chinaImg, "This is china"));
        itemList.Add(new Item("Silverware", ItemCategory.luxury, 70, ItemWeightClass.light, silverwareImg, "This is silverware"));
        itemList.Add(new Item("Tanned Leather", ItemCategory.upgrade, 60, ItemWeightClass.heavy, tannedleatherImg, "This is tanned leather"));
        itemList.Add(new Item("Intricate Gear", ItemCategory.upgrade, 90, ItemWeightClass.heavy, intricategearImg, "This is intricate gear"));
        itemList.Add(new Item("Sturdy Sinew", ItemCategory.upgrade, 60, ItemWeightClass.heavy, sturdysinewImg, "This is sturdy sinew"));
        itemList.Add(new Item("Fine Whetstone", ItemCategory.upgrade, 50, ItemWeightClass.heavy, finewhetstoneImg, "This is fine whetstone"));
        itemList.Add(new Item("Steel Ingot", ItemCategory.upgrade, 50, ItemWeightClass.heavy, steelingotImg, "This is steel ingot"));
        itemList.Add(new Item("Coal", ItemCategory.upgrade, 60, ItemWeightClass.heavy, coalImg, "This is coal"));
        itemList.Add(new Item("Saltpetre", ItemCategory.upgrade, 70, ItemWeightClass.heavy, saltpetreImg, "This is saltpetre"));
        itemList.Add(new Item("Sulfur", ItemCategory.upgrade, 80, ItemWeightClass.heavy, sulfurImg, "This is sulfur"));
        itemList.Add(new Item("Twine", ItemCategory.upgrade, 40, ItemWeightClass.heavy, twineImg, "This is twine"));
        itemList.Add(new Item("Iron Mail", ItemCategory.upgrade, 50, ItemWeightClass.heavy, ironmailImg, "This is iron mail"));
        itemList.Add(new Item("Beewax", ItemCategory.upgrade, 60, ItemWeightClass.heavy, beewaxImg, "This is beewax"));
        itemList.Add(new Item("Tools", ItemCategory.upgrade, 60, ItemWeightClass.heavy, toolsImg, "This is tools"));
        itemList.Add(new Item("Supplies", ItemCategory.supplies, 10, ItemWeightClass.light, suppliesImg, "Matters that will keep your troops alive"));

    }

    public Item getRandomItem()
    {
        int rand = Random.Range(0, itemList.Count - 1);
        return itemList[rand];
    }
}

public class Item
{
    public string name;
    public ItemCategory category;
    public int value;
    public ItemWeightClass weight;
    public string description;
    public Texture2D icon;
    public Item(string nameI, ItemCategory categoryI, int valueI, ItemWeightClass weightI, Texture2D iconI, string descriptionI)
    {
        name = nameI;
        category = categoryI;
        value = valueI;
        weight = weightI;
        description = descriptionI;
        icon = iconI;
    }
    public Item(Item item)
    {
        name = item.name;
        category = item.category;
        value = item.value;
        weight = item.weight;
        description = item.description;
        icon = item.icon;
    }
    public int getBuyingPrice()
    {
        return (int)(value * 1.2);
    }
    public int getSellingPrice()
    {
        return (int)(value * 0.8);
    }
    public float getWeight()
    {
        float result = 0;
        switch (weight)
        {
            case ItemWeightClass.none:
                result = 0;
                break;
            case ItemWeightClass.light:
                result = 1;
                break;
            case ItemWeightClass.medium:
                result = 2;
                break;
            case ItemWeightClass.heavy:
                result = 3;
                break;
        }
        return result;
    }
}

public enum ItemCategory
{
    common,
    food,
    luxury,
    industry,
    upgrade,
    supplies,
    all
}

public enum ItemWeightClass
{
    none,
    light,
    medium,
    heavy
}