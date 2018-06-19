using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour {
    public static InventoryManagement inventoryManagement;
    public GameObject currentItemButton, selectingItemButton;
    public RawImage currentItemIcon, selectingItemIcon, inspectIcon;
    public Text currentName, currentWeight, currentValue, currentAmount;
    public Text selectingName, selectingWeight, selectingValue, selectingAmount;
    public Texture2D selectedImg, unselectedImg;
    public Button currentAll, currentUpgrade, currentLuxury, currentIndustry, currentCommon, currentFood, currentSupplies;  
    public Button currentAlpha, currentWeightSingle, currentWeightTotal, currentValueSingle,
        currentValueTotal, currentAmountTotal;
    public Button selectingAll, selectingUpgrade, selectingLuxury, selectingIndustry, selectingCommon, selectingFood, selectingSupplies;
    public Button selectingAlpha, selectingWeightSingle, selectingWeightTotal, selectingValueSingle,
        selectingValueTotal, selectingAmountTotal;
    public Text playerCash, selectingCash;
    public Text playerWeight;
    public Text inspectName, price, weight, amount;
    public Slider amountSlider;
    public Button buyButton, sellButton;
    public Text buyText, sellText;
    public static List<Item> originalCurrentInventory, originalSelectingInventory;
    public static float shopCash;
    public static InventoryManagementMode managementMode = InventoryManagementMode.dropping;
    public bool initialized;
    List<List<Item>> currentInventory, selectingInventory;
    bool inCurrent;
    Party tradingParty;
    City storingCity;
    List<Item> curSameItems;
    GameObject curButton;
    List<GameObject> createdCurrentButtons, createdSelectingButtons;
    ItemCategory currentCategory, selectingCategory;
    SortingMode currentSortingMode, selectingSortingMode;
    
    private void Start()
    {
        inventoryManagement = this;
        currentAll.onClick.AddListener(delegate { currentCategory = ItemCategory.all; showCurrent(); });
        currentUpgrade.onClick.AddListener(delegate { currentCategory = ItemCategory.upgrade; showCurrent(); });
        currentLuxury.onClick.AddListener(delegate { currentCategory = ItemCategory.luxury; showCurrent(); });
        currentIndustry.onClick.AddListener(delegate { currentCategory = ItemCategory.industry; showCurrent(); });
        currentCommon.onClick.AddListener(delegate { currentCategory = ItemCategory.common; showCurrent(); });
        currentFood.onClick.AddListener(delegate { currentCategory = ItemCategory.food; showCurrent(); });
        currentSupplies.onClick.AddListener(delegate { currentCategory = ItemCategory.supplies; showCurrent(); });

        currentAlpha.onClick.AddListener(delegate { currentSortingMode = SortingMode.alphabet; showCurrent(); });
        currentWeightSingle.onClick.AddListener(delegate { currentSortingMode = SortingMode.weightSingle; showCurrent(); });
        currentWeightTotal.onClick.AddListener(delegate { currentSortingMode = SortingMode.weightTotal; showCurrent(); });
        currentValueSingle.onClick.AddListener(delegate { currentSortingMode = SortingMode.valueSingle; showCurrent(); });
        currentValueTotal.onClick.AddListener(delegate { currentSortingMode = SortingMode.valueTotal; showCurrent(); });
        currentAmountTotal.onClick.AddListener(delegate { currentSortingMode = SortingMode.amount; showCurrent(); });

        selectingAll.onClick.AddListener(delegate { selectingCategory = ItemCategory.all; showSelecting(); });
        selectingUpgrade.onClick.AddListener(delegate { selectingCategory = ItemCategory.upgrade; showSelecting(); });
        selectingLuxury.onClick.AddListener(delegate { selectingCategory = ItemCategory.luxury; showSelecting(); });
        selectingIndustry.onClick.AddListener(delegate { selectingCategory = ItemCategory.industry; showSelecting(); });
        selectingCommon.onClick.AddListener(delegate { selectingCategory = ItemCategory.common; showSelecting(); });
        selectingFood.onClick.AddListener(delegate { selectingCategory = ItemCategory.food; showSelecting(); });
        selectingSupplies.onClick.AddListener(delegate { selectingCategory = ItemCategory.supplies; showSelecting(); });

        selectingAlpha.onClick.AddListener(delegate { selectingSortingMode = SortingMode.alphabet; showSelecting(); });
        selectingWeightSingle.onClick.AddListener(delegate { selectingSortingMode = SortingMode.weightSingle; showSelecting(); });
        selectingWeightTotal.onClick.AddListener(delegate { selectingSortingMode = SortingMode.weightTotal; showSelecting(); });
        selectingValueSingle.onClick.AddListener(delegate { selectingSortingMode = SortingMode.valueSingle; showSelecting(); });
        selectingValueTotal.onClick.AddListener(delegate { selectingSortingMode = SortingMode.valueTotal; showSelecting(); });
        selectingAmountTotal.onClick.AddListener(delegate { selectingSortingMode = SortingMode.amount; showSelecting(); });
    }
    private void OnEnable()
    {
        if (inventoryManagement == null)
        {
            inventoryManagement = gameObject.GetComponent<InventoryManagement>();
        }
        initialized = false;
        inspectItemList(null);
    }
    
    public void initialization ()
    {
        amount.text = (0).ToString();
        price.text = "Price: " + "N/A";
        weight.text = "Weight: " + "N/A";

        buyButton.interactable = false;
        sellButton.interactable = false;
        currentCategory = ItemCategory.all;
        currentSortingMode = SortingMode.alphabet;
        selectingCategory = ItemCategory.all;
        selectingSortingMode = SortingMode.alphabet;
        clearCurrent();
        clearSelecting();
        createdCurrentButtons = new List<GameObject>();
        createdSelectingButtons = new List<GameObject>();

        if (Player.mainParty != null)
        {
            originalCurrentInventory = new List<Item>(Player.mainParty.inventory);
            currentInventory = collapseList(originalCurrentInventory);
            playerWeight.text = ((int)Player.mainParty.getInventoryWeight()).ToString();
        }
        
        if (managementMode != InventoryManagementMode.shopping)
        {
            if (originalSelectingInventory == null)
            {
                originalSelectingInventory = new List<Item>();
            }
            buyText.text = "Take";
            sellText.text = "Drop";
        }
        else
        {
            buyText.text = "Buy";
            sellText.text = "Sell";
        }
        selectingInventory = collapseList(originalSelectingInventory);
        currentItemButton.SetActive(false);
        selectingItemButton.SetActive(false);
        showCurrent();
        showSelecting();
    }
    // Update is called once per frame
    void Update () {
        if (curSameItems != null)
        {
            inspectItemList(curSameItems);
        } else
        {
            buyButton.interactable = false;
            sellButton.interactable = false;
        }
        if (!initialized)
        {
            initialization();
            initialized = true;
        }
    }


    public void inputShopSetting(Party party, int shopCashI)
    {
        tradingParty = party;
        originalSelectingInventory = new List<Item>(tradingParty.inventory);
        shopCash = shopCashI;
        managementMode = InventoryManagementMode.shopping;
        initialization();
    }

    public void inputShopSetting(City city, int shopCashI)
    {
        storingCity = city;
        originalSelectingInventory = new List<Item>(city.warehouse);
        shopCash = shopCashI;
        managementMode = InventoryManagementMode.shopping;
        initialization();
    }

    public void leaveManagement()
    {
        if (originalCurrentInventory != null)
        {
            Player.mainParty.inventory = new List<Item>(originalCurrentInventory);
            originalCurrentInventory.Clear();
        }
        if (managementMode == InventoryManagementMode.shopping)
        {
            if (tradingParty != null)
            {
                tradingParty.inventory = new List<Item>(originalSelectingInventory);
            }
            if (storingCity != null)
            {
                storingCity.warehouse = new List<Item>(originalSelectingInventory);
            }
        } else
        {
            if (storingCity != null)
            {
                storingCity.warehouse = new List<Item>(originalSelectingInventory);
            }
        }
        originalSelectingInventory.Clear();
        managementMode = InventoryManagementMode.dropping;
        clearCurrent();
        clearSelecting();
        //originalSelectingInventory.Clear();
    }

    void showCurrent()
    {
        clearCurrent();
        if (currentInventory != null && currentInventory.Count > 0)
        {
            switch(currentSortingMode)
            {
                case SortingMode.alphabet:
                    currentInventory.Sort(compareSameItemsAlpha);
                    break;
                case SortingMode.valueSingle:
                    currentInventory.Sort(compareSameItemsSingleValue);
                    break;
                case SortingMode.valueTotal:
                    currentInventory.Sort(compareSameItemsTotalValue);
                    break;
                case SortingMode.weightSingle:
                    currentInventory.Sort(compareSameItemsSingleWeight);
                    break;
                case SortingMode.weightTotal:
                    currentInventory.Sort(compareSameItemsTotalWeight);
                    break;
                case SortingMode.amount:
                    currentInventory.Sort(compareSameItemsAmount);
                    break;
            }
            foreach (List<Item> sameItems in currentInventory)
            {
                if (currentCategory == ItemCategory.all || sameItems[0].category == currentCategory)
                {
                    if (curButton == null && inCurrent)
                    {
                        curButton = createCurrentButton(sameItems);
                        curButton.GetComponent<RawImage>().texture = selectedImg;
                        curSameItems = sameItems;
                    }
                    else
                    {
                        createCurrentButton(sameItems);
                    }
                }
                
            }
        } else
        {
            if (inCurrent)
            {
                curSameItems = null;
            }
        }
    }
    void showSelecting()
    {
        clearSelecting();
        if (selectingInventory != null && selectingInventory.Count > 0)
        {
            switch (selectingSortingMode)
            {
                case SortingMode.alphabet:
                    selectingInventory.Sort(compareSameItemsAlpha);
                    break;
                case SortingMode.valueSingle:
                    selectingInventory.Sort(compareSameItemsSingleValue);
                    break;
                case SortingMode.valueTotal:
                    selectingInventory.Sort(compareSameItemsTotalValue);
                    break;
                case SortingMode.weightSingle:
                    selectingInventory.Sort(compareSameItemsSingleWeight);
                    break;
                case SortingMode.weightTotal:
                    selectingInventory.Sort(compareSameItemsTotalWeight);
                    break;
                case SortingMode.amount:
                    selectingInventory.Sort(compareSameItemsAmount);
                    break;
            }
            foreach (List<Item> sameItems in selectingInventory)
            {
                if (selectingCategory == ItemCategory.all || sameItems[0].category == selectingCategory)
                {
                    if (curButton == null && !inCurrent)
                    {
                        curButton = createSelectingButton(sameItems);
                        curButton.GetComponent<RawImage>().texture = selectedImg;
                        curSameItems = sameItems;
                    }
                    else
                    {
                        createSelectingButton(sameItems);
                    }
                }

            }
        } else
        {
            if (!inCurrent)
            {
                curSameItems = null;
            }
        }
    }
    void inspectItemList(List<Item> toInspect)
    {
        if (toInspect != null && toInspect.Count > 0)
        {
            for (int i = 0; i < inspectIcon.gameObject.transform.parent.gameObject.transform.childCount; i++)
            {
                var child = inspectIcon.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }
            }
            int singlePrice = 0;
            int singleWeight = 0;
            int maxAmount = 0;
            if (inCurrent)
            {
                if (managementMode == InventoryManagementMode.shopping)
                {
                    singlePrice = toInspect[0].getSellingPrice();
                    maxAmount = (int)Mathf.Min(toInspect.Count, (int)Mathf.Abs(((int)shopCash / singlePrice)));
                } else
                {
                    maxAmount = (int)toInspect.Count;
                }
                inspectName.text = toInspect[0].name;
                inspectIcon.texture = toInspect[0].icon;
                singleWeight = -(int)toInspect[0].getWeight();
                sellButton.interactable = true;
                buyButton.interactable = false;
            } else
            {
                inspectIcon.texture = toInspect[0].icon;
                inspectName.text = toInspect[0].name;
                singlePrice = - toInspect[0].getBuyingPrice();
                singleWeight = (int)toInspect[0].getWeight();
                if (managementMode == InventoryManagementMode.shopping)
                {
                    maxAmount = Mathf.Min(toInspect.Count, (int)Mathf.Abs(Player.mainParty.cash / singlePrice));
                } else
                {
                    maxAmount = (int)toInspect.Count;
                }
                sellButton.interactable = false;
                buyButton.interactable = true;
            }
            int tradingAmount = Mathf.Abs((int)amountSlider.value);
            if (tradingAmount <= 0 || toInspect == null)
            {
                sellButton.interactable = false;
                buyButton.interactable = false;
            }
            amountSlider.maxValue = maxAmount;
            amountSlider.minValue = 0;
            amount.text = tradingAmount.ToString();
            price.text = "Price: " + ((int)singlePrice * (int)amountSlider.value) + "(" + singlePrice + ")";
            weight.text = "Weight: " + ((int)singleWeight * (int)amountSlider.value) + "(" + singleWeight + ")";
            updateWeightAndCash();
            buyButton.onClick.RemoveAllListeners();
            sellButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(delegate { selectingToCurrent(curSameItems, tradingAmount, managementMode); updateWeightAndCash(); });
            sellButton.onClick.AddListener(delegate { currentToSelecting(curSameItems, tradingAmount, managementMode); updateWeightAndCash(); });
        } else
        {
            for (int i = 0; i < inspectIcon.gameObject.transform.parent.gameObject.transform.childCount; i++)
            {
                var child = inspectIcon.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(false);
                } 
            }
        }
    }
    GameObject createCurrentButton(List<Item> sameItems)
    {
        if (sameItems != null && sameItems.Count != 0)
        {
            currentItemButton.GetComponent<RawImage>().texture = unselectedImg;  
            currentName.text = sameItems[0].name;
            currentValue.text = (int)sameItems[0].value + " / " + ((int) sameItems[0].value * sameItems.Count).ToString();
            int weight = (int) sameItems[0].getWeight();
            currentWeight.text = weight + " / " + ((int) weight * sameItems.Count).ToString();
            currentAmount.text = sameItems.Count.ToString();
            currentItemIcon.texture = sameItems[0].icon;
        }
        GameObject newItemButton = Object.Instantiate(currentItemButton);
        newItemButton.transform.SetParent(currentItemButton.transform.parent, false);
        newItemButton.GetComponent<Button>().onClick.AddListener(delegate {
            curSameItems = sameItems;
            inCurrent = true;
            changeCurSelectedButton(newItemButton);
        });
        newItemButton.SetActive(true);
        createdCurrentButtons.Add(newItemButton);
        return newItemButton;
    }
    GameObject createSelectingButton(List<Item> sameItems)
    {
        if (sameItems != null && sameItems.Count != 0)
        {
            selectingItemButton.GetComponent<RawImage>().texture = unselectedImg;
            selectingName.text = sameItems[0].name;
            selectingValue.text = (int)sameItems[0].value + " / " + ((int)sameItems[0].value * sameItems.Count).ToString();
            int weight = (int)sameItems[0].getWeight();
            selectingWeight.text = weight + " / " + ((int)weight * sameItems.Count).ToString();
            selectingAmount.text = sameItems.Count.ToString();
            selectingItemIcon.texture = sameItems[0].icon;
        }
        GameObject newItemButton = Object.Instantiate(selectingItemButton);
        newItemButton.transform.SetParent(selectingItemButton.transform.parent, false);
        newItemButton.GetComponent<Button>().onClick.AddListener(delegate {
            curSameItems = sameItems;
            inCurrent = false;
            changeCurSelectedButton(newItemButton);
        });
        newItemButton.SetActive(true);
        createdSelectingButtons.Add(newItemButton);
        return newItemButton;
    }
    void changeCurSelectedButton(GameObject newButton)
    {
        if (curButton != newButton)
        {
            newButton.GetComponent<RawImage>().texture = selectedImg;
            if (curButton != null)
            {
                curButton.GetComponent<RawImage>().texture = unselectedImg;
            }
            curButton = newButton;
        }
    }
    
    void clearCurrent()
    {
        if (createdCurrentButtons != null)
        {
            foreach (GameObject button in createdCurrentButtons)
            {
                GameObject.Destroy(button);
            }
            createdCurrentButtons.Clear();
        }
    }
    void clearSelecting()
    {
        if (createdSelectingButtons != null)
        {
            foreach (GameObject button in createdSelectingButtons)
            {
                GameObject.Destroy(button);
            }
        }
    }

    public void addSelectingItems(List<Item> list)
    {
        selectingInventory = collapseList(list);
    }
    List<List<Item>> collapseList(List<Item> list)
    {
        List<List<Item>> result = new List<List<Item>>();
        if (list != null)
        {
            foreach (Item item in list)
            {
                bool added = false;
                foreach (List<Item> sameItems in result)
                {
                    if (sameItems[0] != null && sameItems[0].name == item.name)
                    {
                        sameItems.Add(item);
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    List<Item> newSlot = new List<Item>();
                    newSlot.Add(item);
                    result.Add(newSlot);
                }
            }
        }
        
        return result;
    }
    void currentToSelecting(List<Item> toMove, int amount, InventoryManagementMode mode)
    {
        if (toMove != null)
        {
            for (int i = 0; i < amount; i++)
            {
                originalCurrentInventory.Remove(toMove[i]);
                originalSelectingInventory.Add(toMove[i]);
                if (mode == InventoryManagementMode.shopping)
                {
                    Player.mainParty.cash += toMove[i].getSellingPrice();
                    shopCash -= toMove[i].getSellingPrice();
                }
            }
            currentInventory = collapseList(originalCurrentInventory);
            selectingInventory = collapseList(originalSelectingInventory);
            curButton = null;
            showCurrent();
            showSelecting();
        }
        
    }
    void selectingToCurrent(List<Item> toMove, int amount, InventoryManagementMode mode)
    {
        if (toMove != null)
        {
            for (int i = 0; i < amount; i++)
            {
                originalCurrentInventory.Add(toMove[i]);
                originalSelectingInventory.Remove(toMove[i]);
                if (mode == InventoryManagementMode.shopping)
                {
                    Player.mainParty.cash -= toMove[i].getBuyingPrice();
                    shopCash += toMove[i].getBuyingPrice();
                }
            }
            currentInventory = collapseList(originalCurrentInventory);
            selectingInventory = collapseList(originalSelectingInventory);
            curButton = null;
            showCurrent();
            showSelecting();
        }
    }
    void updateWeightAndCash()
    {
        int result = 0;
        foreach (Item item in originalCurrentInventory)
        {
            result += (int)item.getWeight();
        }
        playerWeight.text = result.ToString();
        playerCash.text = ((int)Player.mainParty.cash).ToString();
        selectingCash.text = ((int)shopCash).ToString();
    }
    int compareSameItemsSingleValue(List<Item> a, List<Item> b)
    {
        return b[0].value.CompareTo(a[0].value);
    }
    int compareSameItemsTotalValue(List<Item> a, List<Item> b)
    {
        return (b[0].value * b.Count).CompareTo((a[0].value * a.Count));
    }
    int compareSameItemsSingleWeight(List<Item> a, List<Item> b)
    {
        return b[0].getWeight().CompareTo(a[0].getWeight());
    }
    int compareSameItemsTotalWeight(List<Item> a, List<Item> b)
    {
        return (int)(b[0].getWeight() * b.Count).CompareTo(a[0].getWeight() * a.Count);
    }
    int compareSameItemsAmount(List<Item> a, List<Item> b)
    {
        return b.Count.CompareTo(a.Count);
    }

    int compareSameItemsAlpha(List<Item> a, List<Item> b)
    {
        return a[0].name.CompareTo(b[0].name);
    }

}

public enum InventoryManagementMode
{
    shopping,
    dropping,
    looting
}
public enum SortingMode
{
    alphabet,
    valueSingle,
    valueTotal,
    weightSingle,
    weightTotal,
    amount
}