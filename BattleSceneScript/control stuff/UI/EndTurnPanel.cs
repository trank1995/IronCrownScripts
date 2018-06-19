using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnPanel : MonoBehaviour {
    public static EndTurnPanel endTurnPanel;
    public GameObject endTurnButton;
    public GameObject leaveButton;
    public Text leaveButtonText;
    public RawImage battlemeterBackground, battlemeterPlayerWon, battlemeterPlayerUpper, battlemeterEnemyUpper, battlemeterEnemyWon, marker, buffer;
    public Button faster, slower, normal;
    public static BattleResult battleResult;
    float BATTLEMETER_FULL_WIDTH, BATTLEMETER_FULL_HEIGHT;
    float BATTLEMETER_CONTENT_HEIGHT, MARKER_WIDTH, BUFFER_HEIGHT;
    int totalAmount, playerWonReq, playerUpperReq, enemyUpperReq;
    bool madeBattlemeter = false;
	// Use this for initialization
	void Start () {
        endTurnPanel = this;
        totalAmount = 1;
        BATTLEMETER_FULL_HEIGHT = battlemeterBackground.rectTransform.sizeDelta.y;
        BATTLEMETER_FULL_WIDTH = battlemeterBackground.rectTransform.sizeDelta.x;
        BATTLEMETER_CONTENT_HEIGHT = battlemeterPlayerWon.rectTransform.sizeDelta.y;
        MARKER_WIDTH = marker.rectTransform.sizeDelta.x;
        BUFFER_HEIGHT = buffer.rectTransform.sizeDelta.y;
        endTurnButton.GetComponent<Button>().onClick.AddListener(
            delegate { BattleCentralControl.endTurnPrep();
                BattleCentralControl.playerTurn = false;
                BattleCentralControl.startTurnPrep();
                updateBattlemeter();
            });
        leaveButton.GetComponent<Button>().onClick.AddListener(
            delegate {
                updateBattlemeter();
                BattleResultDisplay.instance.loadBattleResult(BattleCentralControl.enemyParty.partyMember, BattleCentralControl.deadEnemy, BattleCentralControl.deadPlayer,
                    BattleCentralControl.enemyParty.cash, BattleCentralControl.playerParty.expToDistribute);
            });
        slower.onClick.AddListener(delegate { Time.timeScale -= .2f; Time.timeScale = Mathf.Clamp(Time.timeScale, .4f, 1.0f); });
        faster.onClick.AddListener(delegate { Time.timeScale += .2f; Time.timeScale = Mathf.Clamp(Time.timeScale, 1.6f, 1.0f); });
        normal.onClick.AddListener(delegate { Time.timeScale = 1.0f; });
        battleResult = BattleResult.enemyWon;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (BattleCentralControl.battleStart && !madeBattlemeter)
        {
            makeBattlemeter();
            updateBattlemeter();
            battleResult = BattleResult.enemyWon;
            madeBattlemeter = true;
        }
        
		if (BattleCentralControl.playerTurn && !BattleInteraction.inAction
            && BattleCentralControl.playerTroopOnField.Count >= 1)
        {
            if (!endTurnButton.GetComponent<Button>().interactable)
            {
                endTurnButton.GetComponent<Button>().interactable = true;
                leaveButton.GetComponent<Button>().interactable = true;
            }
        }
        else{
            if (endTurnButton.GetComponent<Button>().interactable)
            {
                endTurnButton.GetComponent<Button>().interactable = false;
                leaveButton.GetComponent<Button>().interactable = false;
            }
        }


    }
    void makeBattlemeter()
    {
        //initialize reqs
        totalAmount = BattleCentralControl.playerTotal + BattleCentralControl.enemyTotal;
        playerWonReq = totalAmount - BattleCentralControl.enemyParty.getDefeatAmount(BattleCentralControl.enemyTotal);
        playerUpperReq = BattleCentralControl.enemyTotal;
        enemyUpperReq = BattleCentralControl.playerParty.getDefeatAmount(BattleCentralControl.playerTotal);
        Debug.Log(totalAmount + "|" + playerWonReq + "|" + playerUpperReq + "|" + enemyUpperReq);
        //get lengths
        float playerWonLength = (totalAmount - playerWonReq) / (float)totalAmount;
        float playerUpperLength = (playerWonReq - playerUpperReq) / (float)totalAmount;
        float enemyWonLength = (enemyUpperReq) / (float)totalAmount;
        float enemyUpperLength = (playerUpperReq - enemyUpperReq) / (float)totalAmount;
        battlemeterPlayerWon.rectTransform.sizeDelta = new Vector2(BATTLEMETER_FULL_WIDTH * playerWonLength, BATTLEMETER_CONTENT_HEIGHT);
        battlemeterPlayerUpper.rectTransform.sizeDelta = new Vector2(BATTLEMETER_FULL_WIDTH * playerUpperLength, BATTLEMETER_CONTENT_HEIGHT);
        battlemeterEnemyWon.rectTransform.sizeDelta = new Vector2(BATTLEMETER_FULL_WIDTH * enemyWonLength, BATTLEMETER_CONTENT_HEIGHT);
        battlemeterEnemyUpper.rectTransform.sizeDelta = new Vector2(BATTLEMETER_FULL_WIDTH * enemyUpperLength, BATTLEMETER_CONTENT_HEIGHT);

    }
    public void updateBattlemeter()
    {
        BattleCentralControl.playerTotal = BattleCentralControl.playerTroopOnField.Count;
        BattleCentralControl.enemyTotal = BattleCentralControl.enemyTroopOnField.Count;
        float length = BATTLEMETER_FULL_WIDTH * ((float)BattleCentralControl.playerTotal) / totalAmount - (MARKER_WIDTH / 2);
        buffer.rectTransform.sizeDelta = new Vector2(length, BUFFER_HEIGHT);
        float progress = totalAmount * ((float)BattleCentralControl.playerTotal / BattleCentralControl.playerTotal + BattleCentralControl.enemyTotal);
        //Debug.Log(progress);
        if (progress < enemyUpperReq || BattleCentralControl.playerParty.partyMember.Count == 0)
        {
            battleResult = BattleResult.enemyWon;
        } else if (progress >= enemyUpperReq)
        {
            battleResult = BattleResult.enemyUpper;
        } else if (progress >= enemyUpperReq)
        {
            battleResult = BattleResult.playerUpper;
        } else if (progress >= enemyUpperReq || BattleCentralControl.enemyParty.partyMember.Count == 0)
        {
            battleResult = BattleResult.playerWon;
        }
        
    }

}

public enum BattleResult {
    playerWon,
    playerUpper,
    enemyUpper,
    enemyWon
}