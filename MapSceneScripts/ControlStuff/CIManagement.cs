using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CIManagement : MonoBehaviour {
    public static CIManagement ciManagement;
    public Text charismaTxt, intelligenceTxt, sparedPoints;
    public Button charismaPlus, intelligencePlus;
    public Button c6a, c6b, c6c, c6d, c7a, c7b, c7c, c7d, c8a, c8b, c8c, c8d,
        c9a, c9b, c9c, c9d, c10a, c10b;
    public Button i6a, i6b, i6c, i6d, i7a, i7b, i7c, i7d, i8a, i8b, i8c, i8d,
        i9a, i9b, i9c, i9d, i10a, i10b;
    public Button reset;
    public RawImage charismaBar, intelligenceBar, barMax;
    public GameObject inspectPanel;
    public Text perkName, perkDescription, perkQuote;
    public CanvasScaler scaler;
    float MAX_BAR_HEIGHT, MAX_BAR_WIDTH;
    float INSPECT_OFFSET_X, INSPECT_OFFSET_Y;
    float SCALE_X, SCALE_Y;
    bool initialized = false;
    // Use this for initialization
    void Start () {
        ciManagement = this;
        MAX_BAR_HEIGHT = barMax.rectTransform.sizeDelta.y;
        MAX_BAR_WIDTH = barMax.rectTransform.sizeDelta.x;
        SCALE_X = scaler.referenceResolution.x / Screen.width;
        SCALE_Y = scaler.referenceResolution.y / Screen.height;
        INSPECT_OFFSET_X = (c6a.gameObject.GetComponent<RectTransform>().sizeDelta.y + inspectPanel.transform.GetComponent<RectTransform>().sizeDelta.y) / 2;
        INSPECT_OFFSET_Y = (c6a.gameObject.GetComponent<RectTransform>().sizeDelta.x + inspectPanel.transform.GetComponent<RectTransform>().sizeDelta.x) / 2;
        
    }

    private void OnEnable()
    {
        //initialized = false;
    }
    // Update is called once per frame
    void Update () {
        if (!SaveLoadSystem.loading)
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
    
    public void inspectPerk(string ID)
    {
        Perk perk = Player.mainCharacter.skillTree.getPerk(ID);
        if (!inspectPanel.activeSelf)
        {
            inspectPanel.SetActive(true);
        }

        Vector3 pos = Input.mousePosition;
        SCALE_X = scaler.referenceResolution.x / Screen.width;
        SCALE_Y = scaler.referenceResolution.y / Screen.height;
        if (pos.x <= Screen.width / 2)
        {
            pos.x += INSPECT_OFFSET_X / SCALE_X;
        }
        else
        {
            pos.x -= INSPECT_OFFSET_X / SCALE_X;
        }
        if (pos.y >= Screen.height / 2)
        {
            pos.y -= INSPECT_OFFSET_Y / SCALE_Y;
        }
        else
        {
            pos.y += INSPECT_OFFSET_Y / SCALE_Y;
        }
        inspectPanel.transform.GetComponent<RectTransform>().position = pos;
        perkName.text = perk.skillName;
        perkDescription.text = perk.description;
        perkQuote.text = perk.quote;
    }

    public void hidePerk()
    {
        inspectPanel.SetActive(false);
    }

    void initialization()
    {
        charismaPlus.onClick.RemoveAllListeners();
        intelligencePlus.onClick.RemoveAllListeners();
        charismaPlus.onClick.AddListener(delegate () { Player.mainCharacter.incrementC(); });
        intelligencePlus.onClick.AddListener(delegate () { Player.mainCharacter.incrementI(); });
        reset.onClick.AddListener(delegate () { Player.mainCharacter.resetPerk(); });
        c6a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C6A").own = true; });
        c6b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C6B").own = true; });
        c6c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C6C").own = true; });
        c6d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C6D").own = true; });
        c7a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C7A").own = true; });
        c7b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C7B").own = true; });
        c7c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C7C").own = true; });
        c7d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C7D").own = true; });
        c8a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C8A").own = true; });
        c8b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C8B").own = true; });
        c8c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C8C").own = true; });
        c8d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C8D").own = true; });
        c9a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C9A").own = true; });
        c9b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C9B").own = true; });
        c9c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C9C").own = true; });
        c9d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C9D").own = true; });
        c10a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C10A").own = true; });
        c10b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("C10B").own = true; });

        i6a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I6A").own = true; });
        i6b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I6B").own = true; });
        i6c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I6C").own = true; });
        i6d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I6D").own = true; });
        i7a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I7A").own = true; });
        i7b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I7B").own = true; });
        i7c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I7C").own = true; });
        i7d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I7D").own = true; });
        i8a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I8A").own = true; });
        i8b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I8B").own = true; });
        i8c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I8C").own = true; });
        i8d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I8D").own = true; });
        i9a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I9A").own = true; });
        i9b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I9B").own = true; });
        i9c.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I9C").own = true; });
        i9d.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I9D").own = true; });
        i10a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I10A").own = true; });
        i10b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("I10B").own = true; });

        
    }
    void showStats()
    {
        charismaTxt.text = Player.mainCharacter.stats.charisma.ToString();
        intelligenceTxt.text = Player.mainCharacter.stats.intelligence.ToString();
        charismaBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.stats.charisma, 0, 10) / 10.0f));
        intelligenceBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.stats.intelligence, 0, 10) / 10.0f));
        if (Player.mainCharacter.exp.sparedPoint > 0)
        {
            sparedPoints.text = "Skill points left: " + Player.mainCharacter.exp.sparedPoint;
        }
        else
        {
            sparedPoints.text = "Reset";
        }

    }
    void buttonUpdate()
    {
        reset.interactable = SAPEManagement.resetable;
        if (Player.mainCharacter.exp.sparedPoint > 0)
        {
            charismaPlus.gameObject.SetActive(true);
            intelligencePlus.gameObject.SetActive(true);

            if (Player.mainCharacter.stats.charisma >= 6)
            {
                c6a.interactable = !Player.mainCharacter.skillTree.getPerk("C6A").own;
                c6b.interactable = !Player.mainCharacter.skillTree.getPerk("C6B").own;
                c6c.interactable = !Player.mainCharacter.skillTree.getPerk("C6C").own;
                c6d.interactable = !Player.mainCharacter.skillTree.getPerk("C6D").own;
            }
            else
            {
                c6a.interactable = false;
                c6b.interactable = false;
                c6c.interactable = false;
                c6d.interactable = false;
            }
            if (Player.mainCharacter.stats.charisma >= 7
                && (Player.mainCharacter.skillTree.getPerk("C6A").own
                || Player.mainCharacter.skillTree.getPerk("C6B").own
                || Player.mainCharacter.skillTree.getPerk("C6C").own
                || Player.mainCharacter.skillTree.getPerk("C6D").own))
            {
                c7a.interactable = !Player.mainCharacter.skillTree.getPerk("C7A").own;
                c7b.interactable = !Player.mainCharacter.skillTree.getPerk("C7B").own;
                c7c.interactable = !Player.mainCharacter.skillTree.getPerk("C7C").own;
                c7d.interactable = !Player.mainCharacter.skillTree.getPerk("C7D").own;
            }
            else
            {
                c7a.interactable = false;
                c7b.interactable = false;
                c7c.interactable = false;
                c7d.interactable = false;
            }
            if (Player.mainCharacter.stats.charisma >= 8
                && (Player.mainCharacter.skillTree.getPerk("C7A").own
                || Player.mainCharacter.skillTree.getPerk("C7B").own
                || Player.mainCharacter.skillTree.getPerk("C7C").own
                || Player.mainCharacter.skillTree.getPerk("C7D").own))
            {
                c8a.interactable = !Player.mainCharacter.skillTree.getPerk("C8A").own;
                c8b.interactable = !Player.mainCharacter.skillTree.getPerk("C8B").own;
                c8c.interactable = !Player.mainCharacter.skillTree.getPerk("C8C").own;
                c8d.interactable = !Player.mainCharacter.skillTree.getPerk("C8D").own;
            }
            else
            {
                c8a.interactable = false;
                c8b.interactable = false;
                c8c.interactable = false;
                c8d.interactable = false;
            }
            if (Player.mainCharacter.stats.charisma >= 9
                && (Player.mainCharacter.skillTree.getPerk("C8A").own
                || Player.mainCharacter.skillTree.getPerk("C8B").own
                || Player.mainCharacter.skillTree.getPerk("C8C").own
                || Player.mainCharacter.skillTree.getPerk("C8D").own))
            {
                c9a.interactable = !Player.mainCharacter.skillTree.getPerk("C9A").own;
                c9b.interactable = !Player.mainCharacter.skillTree.getPerk("C9B").own;
                c9c.interactable = !Player.mainCharacter.skillTree.getPerk("C9C").own;
                c9d.interactable = !Player.mainCharacter.skillTree.getPerk("C9D").own;
            }
            else
            {
                c9a.interactable = false;
                c9b.interactable = false;
                c9c.interactable = false;
                c9d.interactable = false;
            }
            if (Player.mainCharacter.stats.charisma >= 10
                && (Player.mainCharacter.skillTree.getPerk("C9A").own
                || Player.mainCharacter.skillTree.getPerk("C9B").own
                || Player.mainCharacter.skillTree.getPerk("C9C").own
                || Player.mainCharacter.skillTree.getPerk("C9D").own))
            {
                c10a.interactable = !Player.mainCharacter.skillTree.getPerk("C10A").own;
                c10b.interactable = !Player.mainCharacter.skillTree.getPerk("C10B").own;
            }
            else
            {
                c10a.interactable = false;
                c10b.interactable = false;
            }


            if (Player.mainCharacter.stats.intelligence >= 6)
            {
                i6a.interactable = !Player.mainCharacter.skillTree.getPerk("I6A").own;
                i6b.interactable = !Player.mainCharacter.skillTree.getPerk("I6B").own;
                i6c.interactable = !Player.mainCharacter.skillTree.getPerk("I6C").own;
                i6d.interactable = !Player.mainCharacter.skillTree.getPerk("I6D").own;
            }
            else
            {
                i6a.interactable = false;
                i6b.interactable = false;
                i6c.interactable = false;
                i6d.interactable = false;
            }
            if (Player.mainCharacter.stats.intelligence >= 7
                && (Player.mainCharacter.skillTree.getPerk("I6A").own
                || Player.mainCharacter.skillTree.getPerk("I6B").own
                || Player.mainCharacter.skillTree.getPerk("I6C").own
                || Player.mainCharacter.skillTree.getPerk("I6D").own))
            {
                i7a.interactable = !Player.mainCharacter.skillTree.getPerk("I7A").own;
                i7b.interactable = !Player.mainCharacter.skillTree.getPerk("I7B").own;
                i7c.interactable = !Player.mainCharacter.skillTree.getPerk("I7C").own;
                i7d.interactable = !Player.mainCharacter.skillTree.getPerk("I7D").own;
            }
            else
            {
                i7a.interactable = false;
                i7b.interactable = false;
                i7c.interactable = false;
                i7d.interactable = false;
            }
            if (Player.mainCharacter.stats.intelligence >= 8
                && (Player.mainCharacter.skillTree.getPerk("I7A").own
                || Player.mainCharacter.skillTree.getPerk("I7B").own
                || Player.mainCharacter.skillTree.getPerk("I7C").own
                || Player.mainCharacter.skillTree.getPerk("I7D").own))
            {
                i8a.interactable = !Player.mainCharacter.skillTree.getPerk("I8A").own;
                i8b.interactable = !Player.mainCharacter.skillTree.getPerk("I8B").own;
                i8c.interactable = !Player.mainCharacter.skillTree.getPerk("I8C").own;
                i8d.interactable = !Player.mainCharacter.skillTree.getPerk("I8D").own;
            }
            else
            {
                i8a.interactable = false;
                i8b.interactable = false;
                i8c.interactable = false;
                i8d.interactable = false;
            }
            if (Player.mainCharacter.stats.intelligence >= 9
                && (Player.mainCharacter.skillTree.getPerk("I8A").own
                || Player.mainCharacter.skillTree.getPerk("I8B").own
                || Player.mainCharacter.skillTree.getPerk("I8C").own
                || Player.mainCharacter.skillTree.getPerk("I8D").own))
            {
                i9a.interactable = !Player.mainCharacter.skillTree.getPerk("I9A").own;
                i9b.interactable = !Player.mainCharacter.skillTree.getPerk("I9B").own;
                i9c.interactable = !Player.mainCharacter.skillTree.getPerk("I9C").own;
                i9d.interactable = !Player.mainCharacter.skillTree.getPerk("I9D").own;
            }
            else
            {
                i9a.interactable = false;
                i9b.interactable = false;
                i9c.interactable = false;
                i9d.interactable = false;
            }
            if (Player.mainCharacter.stats.intelligence >= 10
                && (Player.mainCharacter.skillTree.getPerk("I9A").own
                || Player.mainCharacter.skillTree.getPerk("I9B").own
                || Player.mainCharacter.skillTree.getPerk("I9C").own
                || Player.mainCharacter.skillTree.getPerk("I9D").own))
            {
                i10a.interactable = !Player.mainCharacter.skillTree.getPerk("I10A").own;
                i10b.interactable = !Player.mainCharacter.skillTree.getPerk("I10B").own;
            }
            else
            {
                i10a.interactable = false;
                i10b.interactable = false;
            }

        } else
        {

            charismaPlus.gameObject.SetActive(false);
            intelligencePlus.gameObject.SetActive(false);
            c6a.interactable = false;
            c6b.interactable = false;
            c6c.interactable = false;
            c6d.interactable = false;
            c7a.interactable = false;
            c7b.interactable = false;
            c7c.interactable = false;
            c7d.interactable = false;
            c8a.interactable = false;
            c8b.interactable = false;
            c8c.interactable = false;
            c8d.interactable = false;
            c9a.interactable = false;
            c9b.interactable = false;
            c9c.interactable = false;
            c9d.interactable = false;
            c10a.interactable = false;
            c10b.interactable = false;
            i6a.interactable = false;
            i6b.interactable = false;
            i6c.interactable = false;
            i6d.interactable = false;
            i7a.interactable = false;
            i7b.interactable = false;
            i7c.interactable = false;
            i7d.interactable = false;
            i8a.interactable = false;
            i8b.interactable = false;
            i8c.interactable = false;
            i8d.interactable = false;
            i9a.interactable = false;
            i9b.interactable = false;
            i9c.interactable = false;
            i9d.interactable = false;
            i10a.interactable = false;
            i10b.interactable = false;
        }
    }

}
