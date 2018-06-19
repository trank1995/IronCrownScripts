using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopDataBase : MonoBehaviour {
    public static TroopDataBase troopDataBase;
    public GameObject mainCharacter, secCharacter;
    public GameObject mercenaryRecruit, mercenaryMilitiaCrossbowman, mercenaryMilitiaMusketeer,
        mercenaryMilitiaSwordsman, mercenaryMilitiaHalberdier, mercenaryMilitiaCavalry;
    public GameObject mercenaryVeteranCrossbowman, mercenaryVeteranMusketeer,
        mercenaryVeteranSwordsman, mercenaryVeteranHalberdier, mercenaryVeteranCavalry;
    public GameObject mercenaryEliteCrossbowman, mercenaryEliteMusketeer,
        mercenaryEliteSwordsman, mercenaryEliteHalberdier, mercenaryEliteCavalry;

    public GameObject banditRecruit, banditMilitiaCrossbowman, banditMilitiaMusketeer,
        banditMilitiaSwordsman, banditMilitiaHalberdier, banditMilitiaCavalry;
    public GameObject banditVeteranCrossbowman, banditVeteranMusketeer,
        banditVeteranSwordsman, banditVeteranHalberdier, banditVeteranCavalry;
    public GameObject banditEliteCrossbowman, banditEliteMusketeer,
        banditEliteSwordsman, banditEliteHalberdier, banditEliteCavalry;

    public GameObject italianRecruit, italianMilitiaCrossbowman, italianMilitiaMusketeer,
        italianMilitiaSwordsman, italianMilitiaHalberdier, italianMilitiaCavalry;
    public GameObject italianVeteranCrossbowman, italianVeteranMusketeer,
        italianVeteranSwordsman, italianVeteranHalberdier, italianVeteranCavalry;
    public GameObject italianEliteCrossbowman, italianEliteMusketeer,
        italianEliteSwordsman, italianEliteHalberdier, italianEliteCavalry;

    public GameObject papalRecruit, papalMilitiaCrossbowman, papalMilitiaMusketeer,
        papalMilitiaSwordsman, papalMilitiaHalberdier, papalMilitiaCavalry;
    public GameObject papalVeteranCrossbowman, papalVeteranMusketeer,
        papalVeteranSwordsman, papalVeteranHalberdier, papalVeteranCavalry;
    public GameObject papalEliteCrossbowman, papalEliteMusketeer,
        papalEliteSwordsman, papalEliteHalberdier, papalEliteCavalry;

    public GameObject frenchRecruit, frenchMilitiaCrossbowman, frenchMilitiaMusketeer,
        frenchMilitiaSwordsman, frenchMilitiaHalberdier, frenchMilitiaCavalry;
    public GameObject frenchVeteranCrossbowman, frenchVeteranMusketeer,
        frenchVeteranSwordsman, frenchVeteranHalberdier, frenchVeteranCavalry;
    public GameObject frenchEliteCrossbowman, frenchEliteMusketeer,
        frenchEliteSwordsman, frenchEliteHalberdier, frenchEliteCavalry;

    public GameObject imperialRecruit, imperialMilitiaCrossbowman, imperialMilitiaMusketeer,
        imperialMilitiaSwordsman, imperialMilitiaHalberdier, imperialMilitiaCavalry;
    public GameObject imperialVeteranCrossbowman, imperialVeteranMusketeer,
        imperialVeteranSwordsman, imperialVeteranHalberdier, imperialVeteranCavalry;
    public GameObject imperialEliteCrossbowman, imperialEliteMusketeer,
        imperialEliteSwordsman, imperialEliteHalberdier, imperialEliteCavalry;

    
    // Use this for initialization
    void Awake () {
        troopDataBase = gameObject.GetComponent<TroopDataBase>();
	}
	
    
    public TroopInfo getTroopInfo(Faction faction, TroopType tt, Ranking rk)
    {
        switch(faction)
        {
            case Faction.mercenary:
                return mercGetTroopInfoHelper(tt, rk);
            case Faction.bandits:
                return BanditGetTroopInfoHelper(tt, rk);
            case Faction.empire:
                return imperialGetTroopInfoHelper(tt, rk);
            case Faction.france:
                return frenchGetTroopInfoHelper(tt, rk);
            case Faction.papacy:
                return papalGetTroopInfoHelper(tt, rk);
            case Faction.italy:
                return italianGetTroopInfoHelper(tt, rk);
        }
        return new TroopInfo();
    }
    public TroopInfo mercGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        if (rk == Ranking.mainChar)
        {
            if (tt == TroopType.mainCharType)
            {
                result.battleValue = 0;
                result.model = mainCharacter;
                result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
            } else
            {
                result.battleValue = 0;
                result.model = secCharacter;
                result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
            }
            return result;
        }
        switch (tt)
        {
            case TroopType.recruitType:
                result.battleValue = 10;
                result.model = mercenaryRecruit;
                result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f);

                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = mercenaryMilitiaCrossbowman;
                        result.gear = new GearInfo(2.0f, 1.0f, 3.0f, 4.0f, 3.0f, 4.0f, 1.0f, 2.0f, 3.0f);
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = mercenaryVeteranCrossbowman;
                        result.gear = new GearInfo(4.0f, 2.0f, 5.0f, 6.0f, 6.0f, 7.0f, 2.0f, 4.0f, 6.0f);
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = mercenaryEliteCrossbowman;
                        result.gear = new GearInfo(7.0f, 4.0f, 8.0f, 8.0f, 8.0f, 9.0f, 4.0f, 7.0f, 8.0f);
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 40;
                        result.model = mercenaryMilitiaMusketeer;
                        result.gear = new GearInfo(2.0f, 1.0f, 3.0f, 4.0f, 1.0f, 2.0f, 1.0f, 3.0f, 2.0f);
                        break;
                    case Ranking.veteran:
                        result.battleValue = 80;
                        result.model = mercenaryVeteranMusketeer;
                        result.gear = new GearInfo(4.0f, 2.0f, 3.0f, 4.0f, 4.0f, 5.0f, 3.0f, 7.0f, 5.0f);
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = mercenaryEliteMusketeer;
                        result.gear = new GearInfo(7.0f, 5.0f, 8.0f, 7.0f, 7.0f, 7.0f, 5.0f, 9.0f, 7.0f);
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = mercenaryMilitiaSwordsman;
                        result.gear = new GearInfo(3.0f, 2.0f, 3.0f, 1.0f, 3.0f, 0.0f, 4.0f, 0.0f, 3.0f);
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = mercenaryVeteranSwordsman;
                        result.gear = new GearInfo(5.0f, 5.0f, 6.0f, 2.0f, 7.0f, 0.0f, 7.0f, 0.0f, 7.0f);
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = mercenaryEliteSwordsman;
                        result.gear = new GearInfo(8.0f, 7.0f, 8.0f, 5.0f, 9.0f, 0.0f, 9.0f, 0.0f, 9.0f);
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = mercenaryMilitiaHalberdier;
                        result.gear = new GearInfo(4.0f, 4.0f, 1.0f, 2.0f, 1.0f, 0.0f, 4.0f, 0.0f, 1.0f);
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = mercenaryVeteranHalberdier;
                        result.gear = new GearInfo(7.0f, 7.0f, 2.0f, 3.0f, 2.0f, 0.0f, 7.0f, 0.0f, 4.0f);
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = mercenaryEliteHalberdier;
                        result.gear = new GearInfo(9.0f, 9.0f, 4.0f, 6.0f, 5.0f, 0.0f, 9.0f, 0.0f, 6.0f);
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 40;
                        result.model = mercenaryMilitiaCavalry;
                        result.gear = new GearInfo(4.0f, 4.0f, 1.0f, 2.0f, 1.0f, 0.0f, 4.0f, 0.0f, 3.0f);
                        break;
                    case Ranking.veteran:
                        result.battleValue = 80;
                        result.model = mercenaryVeteranCavalry;
                        result.gear = new GearInfo(6.0f, 6.0f, 4.0f, 5.0f, 3.0f, 0.0f, 7.0f, 0.0f, 6.0f);
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = mercenaryEliteCavalry;
                        result.gear = new GearInfo(9.0f, 9.0f, 6.0f, 7.0f, 7.0f, 0.0f, 10.0f, 0.0f, 9.0f);
                        break;
                }
                break;
        }
        return result;
    }
    public TroopInfo BanditGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.battleValue = 10;
                result.model = banditRecruit;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = banditMilitiaCrossbowman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = banditVeteranCrossbowman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = banditEliteCrossbowman;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = banditMilitiaMusketeer;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = banditVeteranMusketeer;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = banditEliteMusketeer;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = banditMilitiaSwordsman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = banditVeteranSwordsman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = banditEliteSwordsman;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = banditMilitiaHalberdier;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = banditVeteranHalberdier;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = banditEliteHalberdier;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = banditMilitiaCavalry;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = banditVeteranCavalry;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = banditEliteCavalry;
                        break;
                }
                break;
        }
        result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        return result;
    }
    public TroopInfo italianGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.battleValue = 10;
                result.model = italianRecruit;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = italianMilitiaCrossbowman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = italianVeteranCrossbowman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = italianEliteCrossbowman;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = italianMilitiaMusketeer;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = italianVeteranMusketeer;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = italianEliteMusketeer;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = italianMilitiaSwordsman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = italianVeteranSwordsman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = italianEliteSwordsman;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = italianMilitiaHalberdier;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = italianVeteranHalberdier;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = italianEliteHalberdier;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = italianMilitiaCavalry;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = italianVeteranCavalry;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = italianEliteCavalry;
                        break;
                }
                break;
        }
        result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        return result;
    }
    public TroopInfo papalGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.battleValue = 10;
                result.model = papalRecruit;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = papalMilitiaCrossbowman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = papalVeteranCrossbowman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = papalEliteCrossbowman;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = papalMilitiaMusketeer;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = papalVeteranMusketeer;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = papalEliteMusketeer;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = papalMilitiaSwordsman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = papalVeteranSwordsman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = papalEliteSwordsman;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = papalMilitiaHalberdier;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = papalVeteranHalberdier;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = papalEliteHalberdier;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = papalMilitiaCavalry;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = papalVeteranCavalry;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = papalEliteCavalry;
                        break;
                }
                break;
        }
        result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        return result;
    }
    public TroopInfo frenchGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.battleValue = 10;
                result.model = frenchRecruit;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = frenchMilitiaCrossbowman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = frenchVeteranCrossbowman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = frenchEliteCrossbowman;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = frenchMilitiaMusketeer;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = frenchVeteranMusketeer;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = frenchEliteMusketeer;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = frenchMilitiaSwordsman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = frenchVeteranSwordsman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = frenchEliteSwordsman;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = frenchMilitiaHalberdier;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = frenchVeteranHalberdier;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = frenchEliteHalberdier;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = frenchMilitiaCavalry;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = frenchVeteranCavalry;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = frenchEliteCavalry;
                        break;
                }
                break;
        }
        result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        return result;
    }
    public TroopInfo imperialGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.battleValue = 10;
                result.model = imperialRecruit;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = imperialMilitiaCrossbowman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = imperialVeteranCrossbowman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = imperialEliteCrossbowman;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = imperialMilitiaMusketeer;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = imperialVeteranMusketeer;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = imperialEliteMusketeer;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = imperialMilitiaSwordsman;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = imperialVeteranSwordsman;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = imperialEliteSwordsman;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = imperialMilitiaHalberdier;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = imperialVeteranHalberdier;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = imperialEliteHalberdier;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.battleValue = 20;
                        result.model = imperialMilitiaCavalry;
                        break;
                    case Ranking.veteran:
                        result.battleValue = 50;
                        result.model = imperialVeteranCavalry;
                        break;
                    case Ranking.elite:
                        result.battleValue = 150;
                        result.model = imperialEliteCavalry;
                        break;
                }
                break;
        }
        result.gear = new GearInfo(1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        return result;
    }

    public GameObject getTroopObject(Faction f, TroopType tt, Ranking rk)
    {
        return getTroopInfo(f, tt, rk).model;
    }
    public int getBattleValue(Faction f, TroopType tt, Ranking rk)
    {
        return getTroopInfo(f, tt, rk).battleValue;
    }
    public GearInfo getGearInfo(Faction f, TroopType tt, Ranking rk)
    {
        return getTroopInfo(f, tt, rk).gear;
    }






    public static string troopTypeToString(TroopType tt)
    {
        switch (tt)
        {
            case TroopType.recruitType:
                return "Recruit";
            case TroopType.crossbowman:
                return "Crossbowman";
            case TroopType.musketeer:
                return "Musketeer";
            case TroopType.swordsman:
                return "Swordsman";
            case TroopType.halberdier:
                return "Halberdier";
            case TroopType.cavalry:
                return "Cavalry";
            case TroopType.mainCharType:
                return "";
        }
        return "Recruit";
    }
    public static string rankingToString(Ranking rk)
    {
        switch (rk)
        {
            case Ranking.recruit:
                return "";
            case Ranking.militia:
                return "Militia";
            case Ranking.veteran:
                return "Veteran";
            case Ranking.elite:
                return "Elite";
            case Ranking.mainChar:
                return "";
        }
        return "";
    }
    public static string factionToString(Faction f)
    {
        switch (f)
        {
            case Faction.mercenary:
                return "Mercenary";
            case Faction.bandits:
                return "Bandit";
            case Faction.italy:
                return "Italian";
            case Faction.papacy:
                return "Papal";
            case Faction.france:
                return "French";
            case Faction.empire:
                return "Imperial";
        }
        return "";
    }
}



public class TroopInfo : System.Object
{
    public int battleValue;
    public GameObject model;
    public Texture2D icon;
    public Texture2D profile;
    public GearInfo gear;
}
