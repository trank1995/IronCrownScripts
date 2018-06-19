using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectPanel : MonoBehaviour {
    public GameObject inspectPanel;
    public Text partyNameText, leaderNameText,
        tradeGoodValueText, levelText, prestigeText, notorietyText;
	// Use this for initialization
	void Start () {
        inspectPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        lookAtCamera();
    }

    void lookAtCamera()
    {
        Vector3 v = Camera.main.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(Camera.main.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
    public void updateTexts(Party party)
    {
        partyNameText.text = party.name + " (" + party.partyMember.Count.ToString() + ")(" + party.getAverageLevel().ToString() + ")";
        leaderNameText.text = party.leader.name;
        tradeGoodValueText.text = party.getInventoryValue().ToString();
        levelText.text = party.getBattleValue().ToString();
        prestigeText.text = party.prestige.ToString();
        notorietyText.text = party.notoriety.ToString();

    }
}
