using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneUIImageDataBase : MonoBehaviour {

    public Texture2D cityDefaultImg, cityGarrisonImg, cityThreatenImg, cityMarketImg, cityHallImg, cityArmoryImg,
        cityTavernImg, cityBrothelImg, cityChurchImg, cityEncampmentImg;
    public Texture2D townDefaultImg, townGarrisonImg, townThreatenImg, townRestockImg, townRecruitImg, townInvestImg;
    public Texture2D npcDefaultImg, npcAttack, npcTalk, npcAmbush, npcRetreat, npcThreaten, npcBribe;
    public Texture2D empPos1, empPos2, empPos3, empNeg1, empNeg2, empNeg3, empDefault;
    public Texture2D fraPos1, fraPos2, fraPos3, fraNeg1, fraNeg2, fraNeg3, fraDefault;
    public Texture2D papPos1, papPos2, papPos3, papNeg1, papNeg2, papNeg3, papDefault;
    public static MapSceneUIImageDataBase dataBase;

    // Use this for initialization
    void Awake () {
        dataBase = this;
        //dataBase.cityDefaultImg = cityDefaultImg;
        //dataBase.cityGarrisonImg = cityGarrisonImg;
        //dataBase.cityThreatenImg = cityThreatenImg;
        //dataBase.cityMarketImg = cityMarketImg;
        //dataBase.cityHallImg = cityHallImg;
        //dataBase.cityArmoryImg = cityArmoryImg;
        //dataBase.cityTavernImg = cityTavernImg;
        //dataBase.cityBrothelImg = cityBrothelImg;
        //dataBase.cityChurchImg = cityChurchImg;
        //dataBase.cityEncampmentImg = cityEncampmentImg;
        //dataBase.townDefaultImg = townDefaultImg;
        //dataBase.townGarrisonImg = townGarrisonImg;
        //dataBase.townThreatenImg = townThreatenImg;
        //dataBase.townRestockImg = townRestockImg;
        //dataBase.townRecruitImg = townRecruitImg;
        //dataBase.townInvestImg = townInvestImg;
        //dataBase.empPos1 = empPos1;
        //dataBase.empPos2 = empPos2;
        //dataBase.empPos3 = empPos3;
        //dataBase.empNeg1 = empNeg1;
        //dataBase.empNeg2 = empNeg2;
        //dataBase.empNeg3 = empNeg3;
        //dataBase.fraPos1 = fraPos1;
        //dataBase.fraPos2 = fraPos2;
        //dataBase.fraPos3 = fraPos3;
        //dataBase.fraNeg1 = fraNeg1;
        //dataBase.fraNeg2 = fraNeg2;
        //dataBase.fraNeg3 = fraNeg3;
        //dataBase.papPos1 = papPos1;
        //dataBase.papPos2 = papPos2;
        //dataBase.papPos3 = papPos3;
        //dataBase.papNeg1 = papNeg1;
        //dataBase.papNeg2 = papNeg2;
        //dataBase.papNeg3 = papNeg3;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Texture2D getCityDefaultImg()
    {
        return cityDefaultImg;
    }
    public Texture2D getTownDefaultImg()
    {
        return townDefaultImg;
    }
}