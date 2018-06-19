using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour {
    public static TabMenu tabMenu;
    public GameObject defaultPanel, topPanel;
    public GameObject explainPanel;
    public Text explainMsgText;
    public Text timeDisplay;
    public Button objectiveButton, factionButton, sapeButton, ciButton,
        mainGearButton, secGearButton, inventoryButton, troopButton;
    public Button objectiveButtonQuick, factionButtonQuick, sapeButtonQuick, ciButtonQuick,
        mainGearButtonQuick, secGearButtonQuick, inventoryButtonQuick, troopButtonQuick, closeTabButton;
    public Animator animator;
    bool closing = true;
    // Use this for initialization
    void Start() {
        tabMenu = this;
        gameObject.SetActive(false);
        animator = gameObject.GetComponent<Animator>();
        objectiveButton.onClick.AddListener(delegate () { showPanel(TabPanelType.ObjectivePanel, true); });
        factionButton.onClick.AddListener(delegate () { showPanel(TabPanelType.FactionPanel, true); });
        sapeButton.onClick.AddListener(delegate () { showPanel(TabPanelType.SAPEPanel, true); });
        ciButton.onClick.AddListener(delegate () { showPanel(TabPanelType.CIPanel, true); });
        mainGearButton.onClick.AddListener(delegate () { showPanel(TabPanelType.MainGearPanel, true); });
        secGearButton.onClick.AddListener(delegate () { showPanel(TabPanelType.SecGearPanel, true); });
        inventoryButton.onClick.AddListener(delegate () { showPanel(TabPanelType.InventoryPanel, true); });
        troopButton.onClick.AddListener(delegate () { showPanel(TabPanelType.TroopPanel, true); });
        objectiveButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.ObjectivePanel, true); });
        factionButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.FactionPanel, true); });
        sapeButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.SAPEPanel, true); });
        ciButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.CIPanel, true); });
        mainGearButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.MainGearPanel, true); });
        secGearButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.SecGearPanel, true); });
        inventoryButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.InventoryPanel, true); });
        troopButtonQuick.onClick.AddListener(delegate () { showPanel(TabPanelType.TroopPanel, true); });
        closeTabButton.onClick.AddListener(delegate () { closeTabMenu(); });
        explainPanel.SetActive(false);
    }
    private void OnEnable()
    {
        closing = false;
        defaultPanel.SetActive(true);
        topPanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && tabMenu != null)
        {
            resetLayout();
            if (Time.timeScale != 0.0f)
            {
                Time.timeScale = 0.0f;
                WorldInteraction.worldInteraction.stopEveryone(true);
            }
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (!defaultPanel.activeSelf)
                {
                    showPanel(TabPanelType.DefaultPanel, true);
                } else
                {
                    gameObject.SetActive(false);
                    Time.timeScale = 1.0f;
                    //TroopManagement.troopManagement.leaveManagement();
                    //InventoryManagement.inventoryManagement.leaveManagement();
                }
            }
            if (closing)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Default"))
                {
                    gameObject.SetActive(false);
                    Time.timeScale = 1.0f;
                }
            }
            
        }
        timeDisplay.text = TimeSystem.getTimeDisplay();
    }

    public void showMarket(bool show)
    {
        if (show)
        {
            gameObject.SetActive(show);
            showPanel(TabPanelType.InventoryPanel, show);
        }
        else
        {

        }
        objectiveButtonQuick.gameObject.SetActive(!show);
        factionButtonQuick.gameObject.SetActive(!show);
        sapeButtonQuick.gameObject.SetActive(!show);
        ciButtonQuick.gameObject.SetActive(!show);
        mainGearButtonQuick.gameObject.SetActive(!show);
        secGearButtonQuick.gameObject.SetActive(!show);
        inventoryButtonQuick.gameObject.SetActive(show);
        troopButtonQuick.gameObject.SetActive(!show);
        closeTabButton.gameObject.SetActive(true);

    }
    public void showTroopManagement(bool show, bool upgradable)
    {
        if (show)
        {
            gameObject.SetActive(show);
            showPanel(TabPanelType.TroopPanel, show);
            TroopManagement.upgradable = true;
        }
        else
        {

        }
        objectiveButtonQuick.gameObject.SetActive(!show);
        factionButtonQuick.gameObject.SetActive(!show);
        sapeButtonQuick.gameObject.SetActive(!show);
        ciButtonQuick.gameObject.SetActive(!show);
        mainGearButtonQuick.gameObject.SetActive(!show);
        secGearButtonQuick.gameObject.SetActive(!show);
        inventoryButtonQuick.gameObject.SetActive(!show);
        troopButtonQuick.gameObject.SetActive(show);
        closeTabButton.gameObject.SetActive(true);
        
    }
    public void showPerk(bool show)
    {
        if (show)
        {
            gameObject.SetActive(show);
            showPanel(TabPanelType.SAPEPanel, show);
        }
        objectiveButtonQuick.gameObject.SetActive(!show);
        factionButtonQuick.gameObject.SetActive(!show);
        sapeButtonQuick.gameObject.SetActive(show);
        ciButtonQuick.gameObject.SetActive(show);
        mainGearButtonQuick.gameObject.SetActive(!show);
        secGearButtonQuick.gameObject.SetActive(!show);
        inventoryButtonQuick.gameObject.SetActive(!show);
        troopButtonQuick.gameObject.SetActive(!show);
        closeTabButton.gameObject.SetActive(true);
        
    }
    public void showGear(bool show)
    {
        if (show)
        {
            gameObject.SetActive(show);
            showPanel(TabPanelType.MainGearPanel, show);
        }
        objectiveButtonQuick.gameObject.SetActive(!show);
        factionButtonQuick.gameObject.SetActive(!show);
        sapeButtonQuick.gameObject.SetActive(!show);
        ciButtonQuick.gameObject.SetActive(!show);
        mainGearButtonQuick.gameObject.SetActive(show);
        secGearButtonQuick.gameObject.SetActive(show);
        inventoryButtonQuick.gameObject.SetActive(!show);
        troopButtonQuick.gameObject.SetActive(!show);
        closeTabButton.gameObject.SetActive(true);
        
    }
    public void showPanel(TabPanelType tabPanelType, bool show)
    {
        animator.SetBool("troopUpgradeShow", false);
        if (tabPanelType == TabPanelType.DefaultPanel)
        {
            defaultPanel.SetActive(show);
            if (!show) {
                closing = true;
            }
        } else
        {
            defaultPanel.SetActive(!show);

        }
        if (tabPanelType == TabPanelType.ObjectivePanel)
        {
            animator.SetBool("objectiveShow", show);
        } else
        {
            animator.SetBool("objectiveShow", !show);
        }
        if (tabPanelType == TabPanelType.SAPEPanel)
        {
            animator.SetBool("sapeShow", show);
            if (!show)
            {
                SAPEManagement.sapeManagement.leaveManagement();
            }
        } else
        {
            if (show && animator.GetBool("inventoryShow"))
            {
                SAPEManagement.sapeManagement.leaveManagement();
            }
            animator.SetBool("sapeShow", !show);
        }
        if (tabPanelType == TabPanelType.MainGearPanel)
        {
            animator.SetBool("mainGearShow", show);
        } else
        {
            animator.SetBool("mainGearShow", !show);
        }
        if (tabPanelType == TabPanelType.InventoryPanel)
        {
            animator.SetBool("inventoryShow", show);
            if (!show)
            {
                InventoryManagement.inventoryManagement.leaveManagement();
            } else
            {
                InventoryManagement.inventoryManagement.initialization();
            }
        } else
        {
            if (show && animator.GetBool("inventoryShow"))
            {
                InventoryManagement.inventoryManagement.leaveManagement();
            }
            animator.SetBool("inventoryShow", !show);
        }
        if (tabPanelType == TabPanelType.FactionPanel)
        {
            animator.SetBool("factionShow", show);
            if (!show)
            {
                FaceCoop.faceCoop.leaveManagement();
            }
        } else
        {
            if (show && animator.GetBool("factionShow"))
            {
                FaceCoop.faceCoop.leaveManagement();
            }
            animator.SetBool("factionShow", !show);
        }
        if (tabPanelType == TabPanelType.CIPanel)
        {
            animator.SetBool("ciShow", show);
            if (!show)
            {
                //using sape's resetable, its fine
                SAPEManagement.sapeManagement.leaveManagement();
            }
        } else
        {
            animator.SetBool("ciShow", !show);
        }
        if (tabPanelType == TabPanelType.SecGearPanel)
        {
            animator.SetBool("secGearShow", show);

        } else
        {
            animator.SetBool("secGearShow", !show);
        }
        if (tabPanelType == TabPanelType.TroopPanel)
        {
            animator.SetBool("troopShow", show);
            if (!show && TroopManagement.troopManagement != null) //if show is false
            {
                TroopManagement.troopManagement.leaveManagement();
            }
        } else
        {
            animator.SetBool("troopShow", !show);
            if (show && TroopManagement.troopManagement != null) //if show is true
            {
                TroopManagement.troopManagement.leaveManagement();
            }
        }

    }
    public void closeTabMenu()
    {
        objectiveButtonQuick.gameObject.SetActive(true);
        factionButtonQuick.gameObject.SetActive(true);
        sapeButtonQuick.gameObject.SetActive(true);
        ciButtonQuick.gameObject.SetActive(true);
        mainGearButtonQuick.gameObject.SetActive(true);
        secGearButtonQuick.gameObject.SetActive(true);
        inventoryButtonQuick.gameObject.SetActive(true);
        troopButtonQuick.gameObject.SetActive(true);
        showPanel(TabPanelType.DefaultPanel, true);
        defaultPanel.SetActive(false);
        topPanel.SetActive(false);

        closing = true;
    }
    public void resetLayout ()
    {
        topPanel.GetComponent<HorizontalLayoutGroup>().SetLayoutHorizontal();
    }
    public void explain(string msg)
    {
        if (!explainPanel.activeSelf)
        {
            explainPanel.SetActive(true);
        }
        explainMsgText.text = msg;
        Vector3 pos = Input.mousePosition;
        var SCALE_X = gameObject.GetComponent<CanvasScaler>().referenceResolution.x / Screen.width;
        var SCALE_Y = gameObject.GetComponent<CanvasScaler>().referenceResolution.y / Screen.height;
        var INSPECT_OFFSET_Y = explainPanel.GetComponent<RectTransform>().sizeDelta.y;
        if (pos.y >= Screen.height / 2)
        {
            pos.y -= INSPECT_OFFSET_Y / SCALE_Y;
        }
        else
        {
            pos.y += INSPECT_OFFSET_Y / SCALE_Y;
        }
        explainPanel.transform.GetComponent<RectTransform>().position = pos;
        
    }
    public void hideExplain()
    {
        explainPanel.SetActive(false);
    }
}

public enum TabPanelType
{
    DefaultPanel,
    ObjectivePanel,
    SAPEPanel,
    MainGearPanel,
    InventoryPanel,
    FactionPanel,
    CIPanel,
    SecGearPanel,
    TroopPanel
}