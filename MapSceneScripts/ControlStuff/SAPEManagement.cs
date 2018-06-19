using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SAPEManagement : MonoBehaviour {
    public static SAPEManagement sapeManagement;
    public static bool resetable;
    public Text strengthTxt, agilityTxt, perceptionTxt, enduranceTxt, sparedPoints;
    public Button strengthPlus, agilityPlus, perceptionPlus, endurancePlus;
    public Button s6a, s6b, s7a, s7b, s8a, s8b, s9a, s9b, s10a;
    public Button a6a, a6b, a7a, a7b, a8a, a8b, a9a, a9b, a10a;
    public Button p6a, p6b, p7a, p7b, p8a, p8b, p9a, p9b, p10a;
    public Button e6a, e6b, e7a, e7b, e8a, e8b, e9a, e9b, e10a;
    public Button reset;
    public RawImage strengthBar, agilityBar, perceptionBar, enduranceBar, barMax;
    public GameObject inspectPanel;
    public Text perkName, perkDescription, perkQuote;
    public CanvasScaler scaler;
    float MAX_BAR_HEIGHT, MAX_BAR_WIDTH;
    float INSPECT_OFFSET_X, INSPECT_OFFSET_Y;
    float SCALE_X, SCALE_Y;
    bool initialized = false;
    // Use this for initialization
    void Start () {
        sapeManagement = this;
        //change this
        resetable = true;
		MAX_BAR_HEIGHT = barMax.rectTransform.sizeDelta.y;
        MAX_BAR_WIDTH = barMax.rectTransform.sizeDelta.x;
        SCALE_X = scaler.referenceResolution.x / Screen.width;
        SCALE_Y = scaler.referenceResolution.y / Screen.height;
        INSPECT_OFFSET_X = (s6a.gameObject.GetComponent<RectTransform>().sizeDelta.y + inspectPanel.transform.GetComponent<RectTransform>().sizeDelta.y)/ 2;
        INSPECT_OFFSET_Y = (s6a.gameObject.GetComponent<RectTransform>().sizeDelta.x + inspectPanel.transform.GetComponent<RectTransform>().sizeDelta.x)/ 2;
        
        inspectPanel.SetActive(false);
    }
    private void OnEnable()
    {
        //initialized = false;
    }
    // Update is called once per frame
    void Update () {
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
        strengthTxt.text = Player.mainCharacter.stats.strength.ToString();
        agilityTxt.text = Player.mainCharacter.stats.agility.ToString();
        perceptionTxt.text = Player.mainCharacter.stats.perception.ToString();
        enduranceTxt.text = Player.mainCharacter.stats.endurance.ToString();
        strengthBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.stats.strength, 0, 10) / 10.0f));
        agilityBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.stats.agility, 0, 10) / 10.0f));
        perceptionBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.stats.perception, 0, 10) / 10.0f));
        enduranceBar.rectTransform.sizeDelta = new Vector2(MAX_BAR_WIDTH, MAX_BAR_HEIGHT * (Mathf.Clamp(Player.mainCharacter.stats.endurance, 0, 10) / 10.0f));
        if (Player.mainCharacter.exp.sparedPoint > 0)
        {
            sparedPoints.text = "Skill points left: " + Player.mainCharacter.exp.sparedPoint;
        } else
        {
            sparedPoints.text = "Reset";
        }
        
    }

    public void leaveManagement()
    {
        resetable = false;
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
        } else
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
            //s6a.gameObject.GetComponent<RectTransform>().anchoredPosition;
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
        strengthPlus.onClick.RemoveAllListeners();
        agilityPlus.onClick.RemoveAllListeners();
        perceptionPlus.onClick.RemoveAllListeners();
        endurancePlus.onClick.RemoveAllListeners();
        strengthPlus.onClick.AddListener(delegate () { Player.mainCharacter.incrementS(); });
        agilityPlus.onClick.AddListener(delegate () { Player.mainCharacter.incrementA(); });
        perceptionPlus.onClick.AddListener(delegate () { Player.mainCharacter.incrementP(); });
        endurancePlus.onClick.AddListener(delegate () { Player.mainCharacter.incrementE(); });
        reset.onClick.AddListener(delegate () { Player.mainCharacter.resetPerk(); });
        s6a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S6A").own = true; });
        s6b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S6B").own = true; });
        s7a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S7A").own = true; });
        s7b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S7B").own = true; });
        s8a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S8A").own = true; });
        s8b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S8B").own = true; });
        s9a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S9A").own = true; });
        s9b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S9B").own = true; });
        s10a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("S10A").own = true; });

        a6a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A6A").own = true; });
        a6b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A6B").own = true; });
        a7a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A7A").own = true; });
        a7b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A7B").own = true; });
        a8a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A8A").own = true; });
        a8b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A8B").own = true; });
        a9a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A9A").own = true; });
        a9b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A9B").own = true; });
        a10a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("A10A").own = true; });

        p6a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P6A").own = true; });
        p6b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P6B").own = true; });
        p7a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P7A").own = true; });
        p7b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P7B").own = true; });
        p8a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P8A").own = true; });
        p8b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P8B").own = true; });
        p9a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P9A").own = true; });
        p9b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P9B").own = true; });
        p10a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("P10A").own = true; });

        e6a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E6A").own = true; });
        e6b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E6B").own = true; });
        e7a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E7A").own = true; });
        e7b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E7B").own = true; });
        e8a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E8A").own = true; });
        e8b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E8B").own = true; });
        e9a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E9A").own = true; });
        e9b.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E9B").own = true; });
        e10a.onClick.AddListener(delegate () { Player.mainCharacter.skillTree.getPerk("E10A").own = true; });
        
    }
    

    void buttonUpdate()
    {
        reset.interactable = resetable;
        if (Player.mainCharacter.exp.sparedPoint > 0)
        {
            strengthPlus.gameObject.SetActive(true);
            agilityPlus.gameObject.SetActive(true);
            perceptionPlus.gameObject.SetActive(true);
            endurancePlus.gameObject.SetActive(true);

            if (Player.mainCharacter.stats.strength >= 6)
            {
                s6a.interactable = !Player.mainCharacter.skillTree.getPerk("S6A").own;
                s6b.interactable = !Player.mainCharacter.skillTree.getPerk("S6B").own;
            } else
            {
                s6a.interactable = false;
                s6b.interactable = false;
            }
            if (Player.mainCharacter.stats.strength >= 7
                && (Player.mainCharacter.skillTree.getPerk("S6A").own || Player.mainCharacter.skillTree.getPerk("S6B").own))
            {
                s7a.interactable = !Player.mainCharacter.skillTree.getPerk("S7A").own;
                s7b.interactable = !Player.mainCharacter.skillTree.getPerk("S7B").own;
            } else
            {
                s7a.interactable = false;
                s7b.interactable = false;
            }
            if (Player.mainCharacter.stats.strength >= 8
                && (Player.mainCharacter.skillTree.getPerk("S7A").own || Player.mainCharacter.skillTree.getPerk("S7B").own))
            {
                s8a.interactable = !Player.mainCharacter.skillTree.getPerk("S8A").own;
                s8b.interactable = !Player.mainCharacter.skillTree.getPerk("S8B").own;
            } else
            {
                s8a.interactable = false;
                s8b.interactable = false;
            }
            if (Player.mainCharacter.stats.strength >= 9
                && (Player.mainCharacter.skillTree.getPerk("S8A").own || Player.mainCharacter.skillTree.getPerk("S8B").own))
            {
                s9a.interactable = !Player.mainCharacter.skillTree.getPerk("S9A").own;
                s9b.interactable = !Player.mainCharacter.skillTree.getPerk("S9B").own;
            } else
            {
                s9a.interactable = false;
                s9b.interactable = false;
            }
            if (Player.mainCharacter.stats.strength >= 10
                && (Player.mainCharacter.skillTree.getPerk("S9A").own || Player.mainCharacter.skillTree.getPerk("S9B").own))
            {
                s10a.interactable = !Player.mainCharacter.skillTree.getPerk("S10A").own;
            } else
            {
                s10a.interactable = false;
            }

            if (Player.mainCharacter.stats.agility >= 6)
            {
                a6a.interactable = !Player.mainCharacter.skillTree.getPerk("A6A").own;
                a6b.interactable = !Player.mainCharacter.skillTree.getPerk("A6B").own;
            } else
            {
                a6a.interactable = false;
                a6b.interactable = false;
            }
            if (Player.mainCharacter.stats.agility >= 7
                && (Player.mainCharacter.skillTree.getPerk("A6A").own || Player.mainCharacter.skillTree.getPerk("A6B").own))
            {
                a7a.interactable = !Player.mainCharacter.skillTree.getPerk("A7A").own;
                a7b.interactable = !Player.mainCharacter.skillTree.getPerk("A7B").own;
            } else
            {
                a7a.interactable = false;
                a7b.interactable = false;
            }
            if (Player.mainCharacter.stats.agility >= 8
                && (Player.mainCharacter.skillTree.getPerk("A7A").own || Player.mainCharacter.skillTree.getPerk("A7B").own))
            {
                a8a.interactable = !Player.mainCharacter.skillTree.getPerk("A8A").own;
                a8b.interactable = !Player.mainCharacter.skillTree.getPerk("A8B").own;
            } else
            {
                a8a.interactable = false;
                a8b.interactable = false;
            }
            if (Player.mainCharacter.stats.agility >= 9
                && (Player.mainCharacter.skillTree.getPerk("A8A").own || Player.mainCharacter.skillTree.getPerk("A8B").own))
            {
                a9a.interactable = !Player.mainCharacter.skillTree.getPerk("A9A").own;
                a9b.interactable = !Player.mainCharacter.skillTree.getPerk("A9B").own;
            } else
            {
                a9a.interactable = false;
                a9b.interactable = false;
            }
            if (Player.mainCharacter.stats.agility >= 10
                && (Player.mainCharacter.skillTree.getPerk("A9A").own || Player.mainCharacter.skillTree.getPerk("A9B").own))
            {
                a10a.interactable = !Player.mainCharacter.skillTree.getPerk("A10A").own;
            } else
            {
                a10a.interactable = false;
            }

            if (Player.mainCharacter.stats.perception >= 6)
            {
                p6a.interactable = !Player.mainCharacter.skillTree.getPerk("P6A").own;
                p6b.interactable = !Player.mainCharacter.skillTree.getPerk("P6B").own;
            } else
            {
                p6a.interactable = false;
                p6b.interactable = false;
            }
            if (Player.mainCharacter.stats.perception >= 7
                && (Player.mainCharacter.skillTree.getPerk("P6A").own || Player.mainCharacter.skillTree.getPerk("P6B").own))
            {
                p7a.interactable = !Player.mainCharacter.skillTree.getPerk("P7A").own;
                p7b.interactable = !Player.mainCharacter.skillTree.getPerk("P7B").own;
            } else
            {
                p7a.interactable = false;
                p7b.interactable = false;
            }
            if (Player.mainCharacter.stats.perception >= 8
                && (Player.mainCharacter.skillTree.getPerk("P7A").own || Player.mainCharacter.skillTree.getPerk("P7B").own))
            {
                p8a.interactable = !Player.mainCharacter.skillTree.getPerk("P8A").own;
                p8b.interactable = !Player.mainCharacter.skillTree.getPerk("P8B").own;
            } else
            {
                p8a.interactable = false;
                p8b.interactable = false;
            }
            if (Player.mainCharacter.stats.perception >= 9
                && (Player.mainCharacter.skillTree.getPerk("P8A").own || Player.mainCharacter.skillTree.getPerk("P8B").own))
            {
                p9a.interactable = !Player.mainCharacter.skillTree.getPerk("P9A").own;
                p9b.interactable = !Player.mainCharacter.skillTree.getPerk("P9B").own;
            } else
            {
                p9a.interactable = false;
                p9b.interactable = false;
            }
            if (Player.mainCharacter.stats.perception >= 10
                && (Player.mainCharacter.skillTree.getPerk("P9A").own || Player.mainCharacter.skillTree.getPerk("P9B").own))
            {
                p10a.interactable = !Player.mainCharacter.skillTree.getPerk("P10A").own;
            }
            else
            {
                p10a.interactable = false;
            }

            if (Player.mainCharacter.stats.endurance >= 6)
            {
                e6a.interactable = !Player.mainCharacter.skillTree.getPerk("E6A").own;
                e6b.interactable = !Player.mainCharacter.skillTree.getPerk("E6B").own;
            } else
            {
                e6a.interactable = false;
                e6b.interactable = false;
            }
            if (Player.mainCharacter.stats.endurance >= 7
                && (Player.mainCharacter.skillTree.getPerk("E6A").own || Player.mainCharacter.skillTree.getPerk("E6B").own))
            {
                e7a.interactable = !Player.mainCharacter.skillTree.getPerk("E7A").own;
                e7b.interactable = !Player.mainCharacter.skillTree.getPerk("E7B").own;
            } else
            {
                e7a.interactable = false;
                e7b.interactable = false;
            }
            if (Player.mainCharacter.stats.endurance >= 8
                && (Player.mainCharacter.skillTree.getPerk("E7A").own || Player.mainCharacter.skillTree.getPerk("E7B").own))
            {
                e8a.interactable = !Player.mainCharacter.skillTree.getPerk("E8A").own;
                e8b.interactable = !Player.mainCharacter.skillTree.getPerk("E8B").own;
            } else
            {
                e8a.interactable = false;
                e8b.interactable = false;
            }
            if (Player.mainCharacter.stats.endurance >= 9
                && (Player.mainCharacter.skillTree.getPerk("E8A").own || Player.mainCharacter.skillTree.getPerk("E8B").own))
            {
                e9a.interactable = !Player.mainCharacter.skillTree.getPerk("E9A").own;
                e9b.interactable = !Player.mainCharacter.skillTree.getPerk("E9B").own;
            } else
            {
                e9a.interactable = false;
                e9b.interactable = false;
            }
            if (Player.mainCharacter.stats.endurance >= 10
                && (Player.mainCharacter.skillTree.getPerk("E9A").own || Player.mainCharacter.skillTree.getPerk("E9B").own))
            {
                e10a.interactable = !Player.mainCharacter.skillTree.getPerk("E10A").own;
            } else
            {
                e10a.interactable = false;
            }

        } else
        {
            strengthPlus.gameObject.SetActive(false);
            agilityPlus.gameObject.SetActive(false);
            perceptionPlus.gameObject.SetActive(false);
            endurancePlus.gameObject.SetActive(false);
            s6a.interactable = false;
            s6b.interactable = false;
            s7a.interactable = false;
            s7b.interactable = false;
            s8a.interactable = false;
            s8b.interactable = false;
            s9a.interactable = false;
            s9b.interactable = false;
            s10a.interactable = false;

            a6a.interactable = false;
            a6b.interactable = false;
            a7a.interactable = false;
            a7b.interactable = false;
            a8a.interactable = false;
            a8b.interactable = false;
            a9a.interactable = false;
            a9b.interactable = false;
            a10a.interactable = false;

            p6a.interactable = false;
            p6b.interactable = false;
            p7a.interactable = false;
            p7b.interactable = false;
            p8a.interactable = false;
            p8b.interactable = false;
            p9a.interactable = false;
            p9b.interactable = false;
            p10a.interactable = false;

            e6a.interactable = false;
            e6b.interactable = false;
            e7a.interactable = false;
            e7b.interactable = false;
            e8a.interactable = false;
            e8b.interactable = false;
            e9a.interactable = false;
            e9b.interactable = false;
            e10a.interactable = false;
        }
    }
    
}
