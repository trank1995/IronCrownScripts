using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultDisplay : MonoBehaviour {
    public static BattleResultDisplay instance;
    public static List<Person> totalEnemy, deadEnemy, deadPlayer;
    public static int cash, expGained;
    public static bool showBattleResult;
    public Button close;
    public RawImage totalEnemyBar, deadEnemyBar, injuredEnemyBar,
        totalPlayerBar, deadPlayerBar, injuredPlayerBar;

    public Text totalEnemyNumText, deadEnemyNumText, injuredEnemyNumText, deadPlayerNumText,
        totalPlayerNumText, injuredPlayerNumText;
    public Text totalEnemyText, deadEnemyText, injuredEnemyText, deadPlayerText,
        totalPlayerText, injuredPlayerText;
    public Text cashText, expText;
    float BAR_HEIGHT, BAR_WIDTH;
    bool showed;
    // Use this for initialization
    void Start () {
        instance = this;
        showed = false;
        close.onClick.AddListener(delegate () {
            showBattleResult = false;
            gameObject.SetActive(false);
            BattleCentralControl.endBattle();
        });
        BAR_HEIGHT = totalEnemyBar.rectTransform.sizeDelta.y;
        BAR_WIDTH = totalEnemyBar.rectTransform.sizeDelta.x;
        if (deadEnemy == null)
        {
            deadEnemy = new List<Person>();
        }
        if (totalEnemy == null)
        {
            totalEnemy = new List<Person>();
        }

        if (deadPlayer == null)
        {
            deadPlayer = new List<Person>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (showBattleResult)
        {
            if (!showed && BattleCentralControl.playerParty != null)
            {
                displayBattleResult();
                showed = true;
            }
            
        } else
        {
            gameObject.SetActive(false);
        }
	}

    void displayBattleResult()
    {
        int totalEnemyCount = totalEnemy.Count;
        
        
        deadEnemyBar.rectTransform.sizeDelta = new Vector2(BAR_WIDTH, BAR_HEIGHT * ((float)deadEnemy.Count / (totalEnemyCount + deadEnemy.Count)));
        deadEnemyNumText.text = deadEnemy.Count.ToString();
        deadEnemyText.text = partyMemberToString(deadEnemy);

        injuredEnemyBar.rectTransform.sizeDelta = new Vector2(BAR_WIDTH, BAR_HEIGHT * ((float)getInjured(totalEnemy).Count / (totalEnemyCount + deadEnemy.Count)));
        injuredEnemyNumText.text = deadEnemy.Count.ToString();
        injuredEnemyText.text = partyMemberToString(getInjured(totalEnemy));
        

        //totalEnemyBar.rectTransform.sizeDelta = new Vector2(BAR_WIDTH, BAR_HEIGHT * (float)getHealthy(totalEnemy).Count / (totalEnemyCount + deadEnemy.Count));
        totalEnemyNumText.text = getHealthy(totalEnemy).Count.ToString();
        totalEnemyText.text = partyMemberToString(getHealthy(totalEnemy));

        
        deadPlayerBar.rectTransform.sizeDelta = new Vector2(BAR_WIDTH, BAR_HEIGHT * ((float)deadPlayer.Count / (BattleCentralControl.playerParty.partyMember.Count + deadPlayer.Count)));
        deadPlayerNumText.text = deadPlayer.Count.ToString();
        deadPlayerText.text = partyMemberToString(deadPlayer);

        injuredPlayerBar.rectTransform.sizeDelta = new Vector2(BAR_WIDTH, BAR_HEIGHT * ((float)getInjured(BattleCentralControl.playerParty.partyMember).Count / (BattleCentralControl.playerParty.partyMember.Count + deadPlayer.Count)));
        injuredPlayerNumText.text = getInjured(BattleCentralControl.playerParty.partyMember).Count.ToString();
        injuredPlayerText.text = partyMemberToString(getInjured(BattleCentralControl.playerParty.partyMember));

        //totalPlayerBar.rectTransform.sizeDelta = new Vector2(BAR_WIDTH, BAR_HEIGHT * (float)getHealthy(BattleCentralControl.playerParty.partyMember).Count / (BattleCentralControl.playerParty.partyMember.Count + deadPlayer.Count));
        totalPlayerNumText.text = getHealthy(BattleCentralControl.playerParty.partyMember).Count.ToString();
        totalPlayerText.text = partyMemberToString(getHealthy(BattleCentralControl.playerParty.partyMember));

        cashText.text = "Cash: " + cash.ToString();
        expText.text = "Exp gained: " + expGained.ToString();
    }

    string partyMemberToString(List<Person> partyM)
    {
        string result = "";
        if (partyM.Count == 0)
        {
            return "";
        }
        foreach (Person p in partyM)
        {
            result += p.name + ", ";
        }
        return result.Substring(0, result.Length-2);
    }

    List<Person> getHealthy(List<Person> partyM)
    {
        List<Person> result = new List<Person>();
        foreach (Person p in partyM)
        {
            if (p.health > 0)
            {
                result.Add(p);
            }
        }
        return result;
    }

    List<Person> getInjured(List<Person> partyM)
    {
        List<Person> result = new List<Person>();
        foreach (Person p in partyM)
        {
            if (p.health == 0)
            {
                result.Add(p);
            }
        }
        return result;
    }
    public void loadBattleResult(List<Person> enemyTotalI, List<Person> enemyDeadI, List<Person> playerDeadI, int cashI, int expI)
    {
        showBattleResult = true;
        totalEnemy = enemyTotalI;
        deadEnemy = enemyDeadI;
        deadPlayer = playerDeadI;
        cash = cashI;
        expGained = expI;
        gameObject.SetActive(true);
    }
}
