using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInspectPanel : MonoBehaviour {
    public static Person person;
    public GameObject icon, nameTxt, stamina, health, staminaTxt, healthTxt, armor, evasion, block,
        vision, stealth, accuracy, melee, ranged, mobility;
    float BAR_WIDTH, BAR_HEIGHT;
    bool active = false;
    // Use this for initialization
    void Start () {
        BAR_WIDTH = stamina.GetComponent<RawImage>().rectTransform.sizeDelta.x;
        BAR_HEIGHT = stamina.GetComponent<RawImage>().rectTransform.sizeDelta.y;
    }
	
	// Update is called once per frame
	void Update () {
		if (person != null)
        {
            showInfo(person);
            gameObject.SetActive(true);
        } else
        {
            gameObject.SetActive(false);
        }
    }
    public void showInfo(Person p)
    {
        icon.GetComponent<RawImage>().texture = TroopImgDataBase.troopImgDataBase.getTroopIcon(p.faction, p.troopType, p.ranking);
        health.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(BAR_WIDTH * (person.health / person.getHealthMax()), BAR_HEIGHT);
        stamina.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(BAR_WIDTH * (person.stamina / person.getStaminaMax()), BAR_HEIGHT);
        staminaTxt.GetComponent<Text>().text = person.stamina.ToString();
        healthTxt.GetComponent<Text>().text = person.health.ToString();
        armor.GetComponent<Text>().text = person.getArmor().ToString();
        evasion.GetComponent<Text>().text = person.getEvasion().ToString();
        block.GetComponent<Text>().text = person.getBlock().ToString();
        vision.GetComponent<Text>().text = person.getVision().ToString();
        stealth.GetComponent<Text>().text = person.getStealth().ToString();
        accuracy.GetComponent<Text>().text = person.getAccuracy().ToString();
        melee.GetComponent<Text>().text = person.getMeleeAttackDmg().ToString();
        ranged.GetComponent<Text>().text = person.getRangedAttackDmg().ToString();
        mobility.GetComponent<Text>().text = person.getMobility().ToString();
        nameTxt.GetComponent<Text>().text = person.name;
    }
}
