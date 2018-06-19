using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGearManagement : MonoBehaviour {
    public static MainGearManagement mainGearManagement;
    public static bool upgradable;
    public Animator animator;
    public GearInfo mainGearInfo, curGearInfo;
    public Text armor, evasion, block,
        vision, stealth, accuracy, melee, ranged, mobility;
    public RawImage armorBar, evasionBar, blockBar,
        visionBar, stealthBar, accuracyBar, meleeBar, rangedBar, mobilityBar;
    public Button helmet1, helmet2, helmet3, helmet4A, helmet4B;
    public Button armor1, armor2, armor3, armor4A, armor4B, armor4C;
    public Button clothes1, clothes2, clothes3, clothes4A, clothes4B, clothes4C, clothes4D;
    public Button sword1, sword2, sword3, sword4A, sword4B, sword4C;
    public Button pistol1, pistol2, pistol3, pistol4A, pistol4B, pistol4C;
    public Button boots1, boots2, boots3, boots4A, boots4B;
    public Button left, right;
    
    //public Text inspectArmor, inspectEvasion, inspectBlock,
    //    inspectVision, inspectStealth, inspectAccuracy, inspectMelee, inspectRanged, inspectMobility;
    //public RawImage inspectArmorBar, inspectEvasionBar, inspectBlockBar,
    //    inspectVisionBar, inspectStealthBar, inspectAccuracyBar, inspectMeleeBar, inspectRangedBar, inspectMobilityBar;
    public Text gearName, gearDescription, gearQuote;
    public CanvasScaler scaler;
    public GameObject makeSurePanel;
    public Text makeSureMsg;
    public Button yes, no;
    public GameObject helmetPanel, armorPanel, clothesPanel, swordPanel, pistolPanel, bootsPanel;
    float MAX_BAR_HEIGHT, MAX_BAR_WIDTH;
    //float INSPECT_OFFSET_X, INSPECT_OFFSET_Y;
    float SCALE_X, SCALE_Y;
    bool initialized = false;
    // Use this for initialization
    void Start()
    {
        mainGearManagement = this;
        upgradable = false;
        MAX_BAR_HEIGHT = armorBar.rectTransform.sizeDelta.y;
        MAX_BAR_WIDTH = armorBar.rectTransform.sizeDelta.x;
        SCALE_X = scaler.referenceResolution.x / Screen.width;
        SCALE_Y = scaler.referenceResolution.y / Screen.height;
        //INSPECT_OFFSET_X = (helmet1.gameObject.GetComponent<RectTransform>().sizeDelta.y + inspectPanel.transform.GetComponent<RectTransform>().sizeDelta.y) / 2;
        //INSPECT_OFFSET_Y = (helmet1.gameObject.GetComponent<RectTransform>().sizeDelta.x + inspectPanel.transform.GetComponent<RectTransform>().sizeDelta.x) / 2;
        
        //inspectPanel.SetActive(false);
        //remove this
        upgradable = true;
    }
    private void OnEnable()
    {
        initialized = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(helmetPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(armorPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(clothesPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(swordPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(pistolPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(bootsPanel.GetComponent<RectTransform>());
    }
    // Update is called once per frame
    void Update()
    {
        if (Player.mainCharacter != null)
        {
            if (!initialized)
            {
                initialization();
                initialized = true;
            }
            showStats();
            buttonUpdate();
        }

    }

    
    void showStats()
    {
        armor.text = ((int)Player.mainCharacter.getArmor()).ToString();
        evasion.text = ((int)Player.mainCharacter.getEvasion()).ToString();
        block.text = ((int)Player.mainCharacter.getBlock()).ToString();
        vision.text = ((int)Player.mainCharacter.getVision()).ToString();
        stealth.text = ((int)Player.mainCharacter.getStealth()).ToString();
        accuracy.text = ((int)Player.mainCharacter.getAccuracy()).ToString();
        melee.text = ((int)Player.mainCharacter.getMeleeAttackDmg()).ToString();
        ranged.text = ((int)Player.mainCharacter.getRangedAttackDmg()).ToString();
        mobility.text = ((int)Player.mainCharacter.getMobility()).ToString();

        armorBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().armorRating, 0, 20) / 20.0f));
        evasionBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().evasionRating, 0, 20) / 20.0f));
        blockBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().blockRating, 0, 20) / 20.0f));
        visionBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().visionRating, 0, 20) / 20.0f));
        stealthBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().stealthRating, 0, 20) / 20.0f));
        accuracyBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().accuracyRating, 0, 20) / 20.0f));
        meleeBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().meleeDmgRating, 0, 20) / 20.0f));
        rangedBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().rangedDmgRating, 0, 20) / 20.0f));
        mobilityBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.getGearInfo().mobilityRating, 0, 20) / 20.0f));
        


    }

    public void leaveManagement()
    {
        animator.SetBool("MainGearPage1", true);
        upgradable = false;
    }

    public void inspectGear(string gearID)
    {

        gearName.text = Player.mainCharacter.skillTree.getPerk(gearID).skillName;
        gearDescription.text = Player.mainCharacter.skillTree.getPerk(gearID).description;
        gearQuote.text = Player.mainCharacter.skillTree.getPerk(gearID).quote;
    }

    public void hideGear()
    {
        gearName.text = "";
        gearDescription.text = "";
        gearQuote.text = "";
    }   
    void initialization()
    {
        animator.SetBool("MainGearPage1", true);
        left.interactable = false;
        right.interactable = true;
        left.onClick.AddListener(delegate () { animator.SetBool("MainGearPage1", true); left.interactable = false; right.interactable = true; });
        right.onClick.AddListener(delegate () { animator.SetBool("MainGearPage1", false); right.interactable = false; left.interactable = true; });
        helmet1.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_HELMET1"); });
        helmet2.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_HELMET2"); });
        helmet3.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_HELMET3"); });
        helmet4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_HELMET4A"); });
        helmet4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_HELMET4B"); });

        armor1.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_ARMOR1"); });
        armor2.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_ARMOR2"); });
        armor3.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_ARMOR3"); });
        armor4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_ARMOR4A"); });
        armor4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_ARMOR4B"); });
        armor4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_ARMOR4C"); });

        clothes1.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES1"); });
        clothes2.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES2"); });
        clothes3.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES3"); });
        clothes4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES4A"); });
        clothes4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES4B"); });
        clothes4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES4C"); });
        clothes4D.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_CLOTHES4D"); });

        sword1.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_SWORD1"); });
        sword2.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_SWORD2"); });
        sword3.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_SWORD3"); });
        sword4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_SWORD4A"); });
        sword4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_SWORD4B"); });
        sword4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_SWORD4C"); });

        pistol1.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_PISTOL1"); });
        pistol2.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_PISTOL2"); });
        pistol3.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_PISTOL3"); });
        pistol4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_PISTOL4A"); });
        pistol4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_PISTOL4B"); });
        pistol4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_PISTOL4C"); });

        boots1.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_BOOTS1"); });
        boots2.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_BOOTS2"); });
        boots3.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_BOOTS3"); });
        boots4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_BOOTS4A"); });
        boots4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M1_BOOTS4B"); });

    }

    void upgradeOrDowngrade (string gearID)
    {
        if (!Player.mainCharacter.skillTree.getPerk(gearID).own)
        {
            Player.mainCharacter.skillTree.getPerk(gearID).own = true;
        } else
        {
            makeSurePanel.SetActive(true);
            makeSureMsg.text = "Your upgrade will miss you. Are you sure you want to remove this upgrade?";
            yes.onClick.RemoveAllListeners();
            yes.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk(gearID).own = false; makeSurePanel.SetActive(false); });
            no.onClick.RemoveAllListeners();
            no.onClick.AddListener(delegate () { makeSurePanel.SetActive(false); });
        }
    }

    void buttonUpdate()
    {
        if (upgradable)
        {
            //HELMET
            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_HELMET2").own)
            {
                helmet1.interactable = true;
            }
            else
            {
                helmet1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_HELMET1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_HELMET3").own)
            {
                helmet2.interactable = true;
            }
            else
            {
                helmet2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_HELMET2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_HELMET4A").own
                && !Player.mainCharacter.skillTree.getPerk("M1_HELMET4B").own)
            {
                helmet3.interactable = true;
            }
            else
            {
                helmet3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_HELMET3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                helmet4A.interactable = true;
                helmet4B.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M1_HELMET4A").own)
                {
                    helmet4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_HELMET4B").own)
                {
                    helmet4A.interactable = false;
                }

            }
            else
            {
                helmet4A.interactable = false;
                helmet4B.interactable = false;
            }
            //ARMOR
            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_ARMOR2").own)
            {
                armor1.interactable = true;
            }
            else
            {
                armor1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_ARMOR1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_ARMOR3").own)
            {
                armor2.interactable = true;
            }
            else
            {
                armor2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_ARMOR2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_ARMOR4A").own
                && !Player.mainCharacter.skillTree.getPerk("M1_ARMOR4B").own
                && !Player.mainCharacter.skillTree.getPerk("M1_ARMOR4C").own)
            {
                armor3.interactable = true;
            }
            else
            {
                armor3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_ARMOR3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                armor4A.interactable = true;
                armor4B.interactable = true;
                armor4C.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M1_ARMOR4B").own
                    || Player.mainCharacter.skillTree.getPerk("M1_ARMOR4C").own)
                {
                    armor4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_ARMOR4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_ARMOR4C").own)
                {
                    armor4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_ARMOR4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_ARMOR4B").own)
                {
                    armor4C.interactable = false;
                }
            }
            else
            {
                armor4A.interactable = false;
                armor4B.interactable = false;
                armor4C.interactable = false;
            }

            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_CLOTHES2").own)
            {
                clothes1.interactable = true;
            }
            else
            {
                clothes1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_CLOTHES3").own)
            {
                clothes2.interactable = true;
            }
            else
            {
                clothes2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4A").own
                && !Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4B").own
                && !Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4C").own
                && !Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4D").own)
            {
                clothes3.interactable = true;
            }
            else
            {
                clothes3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                clothes4A.interactable = true;
                clothes4B.interactable = true;
                clothes4C.interactable = true;
                clothes4D.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4B").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4C").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4D").own)
                {
                    clothes4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4C").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4D").own)
                {
                    clothes4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4B").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4D").own)
                {
                    clothes4C.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4B").own
                    || Player.mainCharacter.skillTree.getPerk("M1_CLOTHES4C").own)
                {
                    clothes4D.interactable = false;
                }
            }
            else
            {
                clothes4A.interactable = false;
                clothes4B.interactable = false;
                clothes4C.interactable = false;
                clothes4D.interactable = false;
            }

            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_SWORD2").own)
            {
                sword1.interactable = true;
            }
            else
            {
                sword1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_SWORD1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_SWORD2").own)
            {
                sword2.interactable = true;
            }
            else
            {
                sword2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_SWORD2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_SWORD3").own)
            {
                sword3.interactable = true;
            }
            else
            {
                sword3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_SWORD3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                sword4A.interactable = true;
                sword4B.interactable = true;
                sword4C.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M1_SWORD4B").own
                    || Player.mainCharacter.skillTree.getPerk("M1_SWORD4C").own)
                {
                    sword4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_SWORD4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_SWORD4C").own)
                {
                    sword4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_SWORD4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_SWORD4B").own)
                {
                    sword4C.interactable = false;
                }
            }
            else
            {
                sword4A.interactable = false;
                sword4B.interactable = false;
                sword4C.interactable = false;
            }
            //PISTOL
            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_PISTOL2").own)
            {
                pistol1.interactable = true;
            }
            else
            {
                pistol1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_PISTOL1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_PISTOL3").own)
            {
                pistol2.interactable = true;
            }
            else
            {
                pistol2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_PISTOL2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_PISTOL4A").own
                && !Player.mainCharacter.skillTree.getPerk("M1_PISTOL4B").own
                && !Player.mainCharacter.skillTree.getPerk("M1_PISTOL4C").own)
            {
                pistol3.interactable = true;
            }
            else
            {
                pistol3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_PISTOL3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                pistol4A.interactable = true;
                pistol4B.interactable = true;
                pistol4C.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M1_PISTOL4B").own
                    || Player.mainCharacter.skillTree.getPerk("M1_PISTOL4C").own)
                {
                    pistol4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_PISTOL4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_PISTOL4C").own)
                {
                    pistol4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_PISTOL4A").own
                    || Player.mainCharacter.skillTree.getPerk("M1_PISTOL4B").own)
                {
                    pistol4C.interactable = false;
                }
            }
            else
            {
                pistol4A.interactable = false;
                pistol4B.interactable = false;
                pistol4C.interactable = false;
            }
            //BOOTS
            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_BOOTS2").own)
            {
                boots1.interactable = true;
            }
            else
            {
                boots1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_BOOTS1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_BOOTS3").own)
            {
                boots2.interactable = true;
            }
            else
            {
                boots2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_BOOTS2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M1_BOOTS4A").own
                && !Player.mainCharacter.skillTree.getPerk("M1_BOOTS4B").own)
            {
                boots3.interactable = true;
            }
            else
            {
                boots3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M1_BOOTS3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                boots4A.interactable = true;
                boots4B.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M1_BOOTS4B").own)
                {
                    boots4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M1_BOOTS4A").own)
                {
                    boots4B.interactable = false;
                }
            }
            else
            {
                boots4A.interactable = false;
                boots4B.interactable = false;
            }
        }
        else
        {
            helmet1.interactable = false;
            helmet2.interactable = false;
            helmet3.interactable = false;
            helmet4A.interactable = false;
            helmet4B.interactable = false;


            armor1.interactable = false;
            armor2.interactable = false;
            armor3.interactable = false;
            armor4A.interactable = false;
            armor4B.interactable = false;
            armor4C.interactable = false;


            clothes1.interactable = false;
            clothes2.interactable = false;
            clothes3.interactable = false;
            clothes4A.interactable = false;
            clothes4B.interactable = false;
            clothes4C.interactable = false;
            clothes4D.interactable = false;

            sword1.interactable = false;
            sword2.interactable = false;
            sword3.interactable = false;
            sword4A.interactable = false;
            sword4B.interactable = false;
            sword4C.interactable = false;


            pistol1.interactable = false;
            pistol2.interactable = false;
            pistol3.interactable = false;
            pistol4A.interactable = false;
            pistol4B.interactable = false;
            pistol4C.interactable = false;


            boots1.interactable = false;
            boots2.interactable = false;
            boots3.interactable = false;
            boots4A.interactable = false;
            boots4B.interactable = false;
        }
    }
}
