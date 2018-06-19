using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecGearManagement : MonoBehaviour {

    public static SecGearManagement secGearManagement;
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
    public Button dagger1, dagger2, dagger3, dagger4A, dagger4B, dagger4C;
    public Button crossbow1, crossbow2, crossbow3, crossbow4A, crossbow4B, crossbow4C;
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
    public GameObject helmetPanel, armorPanel, clothesPanel, daggerPanel, crossbowPanel, bootsPanel;
    float MAX_BAR_HEIGHT, MAX_BAR_WIDTH;
    //float INSPECT_OFFSET_X, INSPECT_OFFSET_Y;
    float SCALE_X, SCALE_Y;
    bool initialized = false;
    // Use this for initialization
    void Start()
    {
        secGearManagement = this;
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(daggerPanel.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(crossbowPanel.GetComponent<RectTransform>());
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
        armor.text = ((int)Player.secCharacter.getArmor()).ToString();
        evasion.text = ((int)Player.secCharacter.getEvasion()).ToString();
        block.text = ((int)Player.secCharacter.getBlock()).ToString();
        vision.text = ((int)Player.secCharacter.getVision()).ToString();
        stealth.text = ((int)Player.secCharacter.getStealth()).ToString();
        accuracy.text = ((int)Player.secCharacter.getAccuracy()).ToString();
        melee.text = ((int)Player.secCharacter.getMeleeAttackDmg()).ToString();
        ranged.text = ((int)Player.secCharacter.getRangedAttackDmg()).ToString();
        mobility.text = ((int)Player.secCharacter.getMobility()).ToString();

        armorBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().armorRating, 0, 20) / 20.0f));
        evasionBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().evasionRating, 0, 20) / 20.0f));
        blockBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().blockRating, 0, 20) / 20.0f));
        visionBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().visionRating, 0, 20) / 20.0f));
        stealthBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().stealthRating, 0, 20) / 20.0f));
        accuracyBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().accuracyRating, 0, 20) / 20.0f));
        meleeBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().meleeDmgRating, 0, 20) / 20.0f));
        rangedBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().rangedDmgRating, 0, 20) / 20.0f));
        mobilityBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.secCharacter.getGearInfo().mobilityRating, 0, 20) / 20.0f));



    }

    public void leaveManagement()
    {
        animator.SetBool("SecGearPage1", true);
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
        animator.SetBool("SecGearPage1", true);
        left.interactable = false;
        right.interactable = true;
        left.onClick.AddListener(delegate () { animator.SetBool("SecGearPage1", true); left.interactable = false; right.interactable = true; });
        right.onClick.AddListener(delegate () { animator.SetBool("SecGearPage1", false); right.interactable = false; left.interactable = true; });
        helmet1.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_HELMET1"); });
        helmet2.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_HELMET2"); });
        helmet3.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_HELMET3"); });
        helmet4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_HELMET4A"); });
        helmet4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_HELMET4B"); });

        armor1.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_ARMOR1"); });
        armor2.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_ARMOR2"); });
        armor3.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_ARMOR3"); });
        armor4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_ARMOR4A"); });
        armor4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_ARMOR4B"); });
        armor4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_ARMOR4C"); });

        clothes1.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES1"); });
        clothes2.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES2"); });
        clothes3.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES3"); });
        clothes4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES4A"); });
        clothes4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES4B"); });
        clothes4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES4C"); });
        clothes4D.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CLOTHES4D"); });

        dagger1.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_DAGGER1"); });
        dagger2.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_DAGGER2"); });
        dagger3.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_DAGGER3"); });
        dagger4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_DAGGER4A"); });
        dagger4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_DAGGER4B"); });
        dagger4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_DAGGER4C"); });

        crossbow1.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CROSSBOW1"); });
        crossbow2.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CROSSBOW2"); });
        crossbow3.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CROSSBOW3"); });
        crossbow4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CROSSBOW4A"); });
        crossbow4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CROSSBOW4B"); });
        crossbow4C.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_CROSSBOW4C"); });

        boots1.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_BOOTS1"); });
        boots2.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_BOOTS2"); });
        boots3.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_BOOTS3"); });
        boots4A.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_BOOTS4A"); });
        boots4B.onClick.AddListener(delegate () { upgradeOrDowngrade("M2_BOOTS4B"); });

    }

    void upgradeOrDowngrade(string gearID)
    {
        if (!Player.mainCharacter.skillTree.getPerk(gearID).own)
        {
            Player.mainCharacter.skillTree.getPerk(gearID).own = true;
        }
        else
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
                && !Player.mainCharacter.skillTree.getPerk("M2_HELMET2").own)
            {
                helmet1.interactable = true;
            }
            else
            {
                helmet1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_HELMET1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_HELMET3").own)
            {
                helmet2.interactable = true;
            }
            else
            {
                helmet2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_HELMET2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_HELMET4A").own
                && !Player.mainCharacter.skillTree.getPerk("M2_HELMET4B").own)
            {
                helmet3.interactable = true;
            }
            else
            {
                helmet3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_HELMET3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                helmet4A.interactable = true;
                helmet4B.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M2_HELMET4A").own)
                {
                    helmet4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_HELMET4B").own)
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
                && !Player.mainCharacter.skillTree.getPerk("M2_ARMOR2").own)
            {
                armor1.interactable = true;
            }
            else
            {
                armor1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_ARMOR1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_ARMOR3").own)
            {
                armor2.interactable = true;
            }
            else
            {
                armor2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_ARMOR2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_ARMOR4A").own
                && !Player.mainCharacter.skillTree.getPerk("M2_ARMOR4B").own
                && !Player.mainCharacter.skillTree.getPerk("M2_ARMOR4C").own)
            {
                armor3.interactable = true;
            }
            else
            {
                armor3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_ARMOR3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                armor4A.interactable = true;
                armor4B.interactable = true;
                armor4C.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M2_ARMOR4B").own
                    || Player.mainCharacter.skillTree.getPerk("M2_ARMOR4C").own)
                {
                    armor4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_ARMOR4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_ARMOR4C").own)
                {
                    armor4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_ARMOR4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_ARMOR4B").own)
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
                && !Player.mainCharacter.skillTree.getPerk("M2_CLOTHES2").own)
            {
                clothes1.interactable = true;
            }
            else
            {
                clothes1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_CLOTHES3").own)
            {
                clothes2.interactable = true;
            }
            else
            {
                clothes2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4A").own
                && !Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4B").own
                && !Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4C").own
                && !Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4D").own)
            {
                clothes3.interactable = true;
            }
            else
            {
                clothes3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                clothes4A.interactable = true;
                clothes4B.interactable = true;
                clothes4C.interactable = true;
                clothes4D.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4B").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4C").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4D").own)
                {
                    clothes4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4C").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4D").own)
                {
                    clothes4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4B").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4D").own)
                {
                    clothes4C.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4B").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CLOTHES4C").own)
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
                && !Player.mainCharacter.skillTree.getPerk("M2_DAGGER2").own)
            {
                dagger1.interactable = true;
            }
            else
            {
                dagger1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_DAGGER1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_DAGGER2").own)
            {
                dagger2.interactable = true;
            }
            else
            {
                dagger2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_DAGGER2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_DAGGER3").own)
            {
                dagger3.interactable = true;
            }
            else
            {
                dagger3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_DAGGER3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                dagger4A.interactable = true;
                dagger4B.interactable = true;
                dagger4C.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M2_DAGGER4B").own
                    || Player.mainCharacter.skillTree.getPerk("M2_DAGGER4C").own)
                {
                    dagger4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_DAGGER4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_DAGGER4C").own)
                {
                    dagger4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_DAGGER4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_DAGGER4B").own)
                {
                    dagger4C.interactable = false;
                }
            }
            else
            {
                dagger4A.interactable = false;
                dagger4B.interactable = false;
                dagger4C.interactable = false;
            }
            //CROSSBOW
            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW2").own)
            {
                crossbow1.interactable = true;
            }
            else
            {
                crossbow1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW3").own)
            {
                crossbow2.interactable = true;
            }
            else
            {
                crossbow2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4A").own
                && !Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4B").own
                && !Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4C").own)
            {
                crossbow3.interactable = true;
            }
            else
            {
                crossbow3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                crossbow4A.interactable = true;
                crossbow4B.interactable = true;
                crossbow4C.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4B").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4C").own)
                {
                    crossbow4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4C").own)
                {
                    crossbow4B.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4A").own
                    || Player.mainCharacter.skillTree.getPerk("M2_CROSSBOW4B").own)
                {
                    crossbow4C.interactable = false;
                }
            }
            else
            {
                crossbow4A.interactable = false;
                crossbow4B.interactable = false;
                crossbow4C.interactable = false;
            }
            //BOOTS
            if (Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_BOOTS2").own)
            {
                boots1.interactable = true;
            }
            else
            {
                boots1.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_BOOTS1").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_BOOTS3").own)
            {
                boots2.interactable = true;
            }
            else
            {
                boots2.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_BOOTS2").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3)
                && !Player.mainCharacter.skillTree.getPerk("M2_BOOTS4A").own
                && !Player.mainCharacter.skillTree.getPerk("M2_BOOTS4B").own)
            {
                boots3.interactable = true;
            }
            else
            {
                boots3.interactable = false;
            }
            if (Player.mainCharacter.skillTree.getPerk("M2_BOOTS3").own && Player.mainParty.cash >= 200 && Player.mainParty.inventoryContains("", 3))
            {
                boots4A.interactable = true;
                boots4B.interactable = true;
                if (Player.mainCharacter.skillTree.getPerk("M2_BOOTS4B").own)
                {
                    boots4A.interactable = false;
                }
                if (Player.mainCharacter.skillTree.getPerk("M2_BOOTS4A").own)
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

            dagger1.interactable = false;
            dagger2.interactable = false;
            dagger3.interactable = false;
            dagger4A.interactable = false;
            dagger4B.interactable = false;
            dagger4C.interactable = false;


            crossbow1.interactable = false;
            crossbow2.interactable = false;
            crossbow3.interactable = false;
            crossbow4A.interactable = false;
            crossbow4B.interactable = false;
            crossbow4C.interactable = false;


            boots1.interactable = false;
            boots2.interactable = false;
            boots3.interactable = false;
            boots4A.interactable = false;
            boots4B.interactable = false;
        }
    }
}
