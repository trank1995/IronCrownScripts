using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopImgDataBase : MonoBehaviour {

    public static TroopImgDataBase troopImgDataBase;
    public Texture2D mainCharacterIcon, secCharacterIcon;
    public Texture2D mercenaryRecruitIcon, mercenaryMilitiaCrossbowmanIcon, mercenaryMilitiaMusketeerIcon,
        mercenaryMilitiaSwordsmanIcon, mercenaryMilitiaHalberdierIcon, mercenaryMilitiaCavalryIcon;
    public Texture2D mercenaryVeteranCrossbowmanIcon, mercenaryVeteranMusketeerIcon,
        mercenaryVeteranSwordsmanIcon, mercenaryVeteranHalberdierIcon, mercenaryVeteranCavalryIcon;
    public Texture2D mercenaryEliteCrossbowmanIcon, mercenaryEliteMusketeerIcon,
        mercenaryEliteSwordsmanIcon, mercenaryEliteHalberdierIcon, mercenaryEliteCavalryIcon;

    public Texture2D banditRecruitIcon, banditMilitiaCrossbowmanIcon, banditMilitiaMusketeerIcon,
        banditMilitiaSwordsmanIcon, banditMilitiaHalberdierIcon, banditMilitiaCavalryIcon;
    public Texture2D banditVeteranCrossbowmanIcon, banditVeteranMusketeerIcon,
        banditVeteranSwordsmanIcon, banditVeteranHalberdierIcon, banditVeteranCavalryIcon;
    public Texture2D banditEliteCrossbowmanIcon, banditEliteMusketeerIcon,
        banditEliteSwordsmanIcon, banditEliteHalberdierIcon, banditEliteCavalryIcon;

    public Texture2D italianRecruitIcon, italianMilitiaCrossbowmanIcon, italianMilitiaMusketeerIcon,
        italianMilitiaSwordsmanIcon, italianMilitiaHalberdierIcon, italianMilitiaCavalryIcon;
    public Texture2D italianVeteranCrossbowmanIcon, italianVeteranMusketeerIcon,
        italianVeteranSwordsmanIcon, italianVeteranHalberdierIcon, italianVeteranCavalryIcon;
    public Texture2D italianEliteCrossbowmanIcon, italianEliteMusketeerIcon,
        italianEliteSwordsmanIcon, italianEliteHalberdierIcon, italianEliteCavalryIcon;

    public Texture2D papalRecruitIcon, papalMilitiaCrossbowmanIcon, papalMilitiaMusketeerIcon,
        papalMilitiaSwordsmanIcon, papalMilitiaHalberdierIcon, papalMilitiaCavalryIcon;
    public Texture2D papalVeteranCrossbowmanIcon, papalVeteranMusketeerIcon,
        papalVeteranSwordsmanIcon, papalVeteranHalberdierIcon, papalVeteranCavalryIcon;
    public Texture2D papalEliteCrossbowmanIcon, papalEliteMusketeerIcon,
        papalEliteSwordsmanIcon, papalEliteHalberdierIcon, papalEliteCavalryIcon;

    public Texture2D frenchRecruitIcon, frenchMilitiaCrossbowmanIcon, frenchMilitiaMusketeerIcon,
        frenchMilitiaSwordsmanIcon, frenchMilitiaHalberdierIcon, frenchMilitiaCavalryIcon;
    public Texture2D frenchVeteranCrossbowmanIcon, frenchVeteranMusketeerIcon,
        frenchVeteranSwordsmanIcon, frenchVeteranHalberdierIcon, frenchVeteranCavalryIcon;
    public Texture2D frenchEliteCrossbowmanIcon, frenchEliteMusketeerIcon,
        frenchEliteSwordsmanIcon, frenchEliteHalberdierIcon, frenchEliteCavalryIcon;

    public Texture2D imperialRecruitIcon, imperialMilitiaCrossbowmanIcon, imperialMilitiaMusketeerIcon,
        imperialMilitiaSwordsmanIcon, imperialMilitiaHalberdierIcon, imperialMilitiaCavalryIcon;
    public Texture2D imperialVeteranCrossbowmanIcon, imperialVeteranMusketeerIcon,
        imperialVeteranSwordsmanIcon, imperialVeteranHalberdierIcon, imperialVeteranCavalryIcon;
    public Texture2D imperialEliteCrossbowmanIcon, imperialEliteMusketeerIcon,
        imperialEliteSwordsmanIcon, imperialEliteHalberdierIcon, imperialEliteCavalryIcon;

    public Texture2D mainCharacterProfile, secCharacterProfile;
    public Texture2D mercenaryRecruitProfile, mercenaryMilitiaCrossbowmanProfile, mercenaryMilitiaMusketeerProfile,
        mercenaryMilitiaSwordsmanProfile, mercenaryMilitiaHalberdierProfile, mercenaryMilitiaCavalryProfile;
    public Texture2D mercenaryVeteranCrossbowmanProfile, mercenaryVeteranMusketeerProfile,
        mercenaryVeteranSwordsmanProfile, mercenaryVeteranHalberdierProfile, mercenaryVeteranCavalryProfile;
    public Texture2D mercenaryEliteCrossbowmanProfile, mercenaryEliteMusketeerProfile,
        mercenaryEliteSwordsmanProfile, mercenaryEliteHalberdierProfile, mercenaryEliteCavalryProfile;
    
    public Texture2D defaultProfile;
    // Use this for initialization
    void Awake()
    {
        troopImgDataBase = gameObject.GetComponent<TroopImgDataBase>();
    }


    public TroopInfo getTroopInfo(Faction faction, TroopType tt, Ranking rk)
    {
        switch (faction)
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
                result.icon = mainCharacterIcon;
                result.profile = mainCharacterProfile;
            }
            else
            {
                result.icon = secCharacterIcon;
                result.profile = secCharacterProfile;
            }
            return result;
        }
        switch (tt)
        {
            case TroopType.recruitType:
                result.icon = mercenaryRecruitIcon;
                result.profile = mercenaryRecruitProfile;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = mercenaryMilitiaCrossbowmanIcon;
                        result.profile = mercenaryMilitiaCrossbowmanProfile;
                        break;
                    case Ranking.veteran:
                        result.icon = mercenaryVeteranCrossbowmanIcon;
                        result.profile = mercenaryVeteranCrossbowmanProfile;
                        break;
                    case Ranking.elite:
                        result.icon = mercenaryEliteCrossbowmanIcon;
                        result.profile = mercenaryEliteCrossbowmanProfile;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = mercenaryMilitiaMusketeerIcon;
                        result.profile = mercenaryMilitiaMusketeerProfile;
                        break;
                    case Ranking.veteran:
                        result.icon = mercenaryVeteranMusketeerIcon;
                        result.profile = mercenaryVeteranMusketeerProfile;
                        break;
                    case Ranking.elite:
                        result.icon = mercenaryEliteMusketeerIcon;
                        result.profile = mercenaryEliteMusketeerProfile;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = mercenaryMilitiaSwordsmanIcon;
                        result.profile = mercenaryMilitiaSwordsmanProfile;
                        break;
                    case Ranking.veteran:
                        result.icon = mercenaryVeteranSwordsmanIcon;
                        result.profile = mercenaryVeteranSwordsmanProfile;
                        break;
                    case Ranking.elite:
                        result.icon = mercenaryEliteSwordsmanIcon;
                        result.profile = mercenaryEliteSwordsmanProfile;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = mercenaryMilitiaHalberdierIcon;
                        result.profile = mercenaryMilitiaHalberdierProfile;
                        break;
                    case Ranking.veteran:
                        result.icon = mercenaryVeteranHalberdierIcon;
                        result.profile = mercenaryVeteranHalberdierProfile;
                        break;
                    case Ranking.elite:
                        result.icon = mercenaryEliteHalberdierIcon;
                        result.profile = mercenaryEliteHalberdierProfile;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = mercenaryMilitiaCavalryIcon;
                        result.profile = mercenaryMilitiaCavalryProfile;
                        break;
                    case Ranking.veteran:
                        result.icon = mercenaryVeteranCavalryIcon;
                        result.profile = mercenaryVeteranCavalryProfile;
                        break;
                    case Ranking.elite:
                        result.icon = mercenaryEliteCavalryIcon;
                        result.profile = mercenaryEliteCavalryProfile;
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
                result.icon = banditRecruitIcon;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = banditMilitiaCrossbowmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = banditVeteranCrossbowmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = banditEliteCrossbowmanIcon;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = banditMilitiaMusketeerIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = banditVeteranMusketeerIcon;
                        break;
                    case Ranking.elite:
                        result.icon = banditEliteMusketeerIcon;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = banditMilitiaSwordsmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = banditVeteranSwordsmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = banditEliteSwordsmanIcon;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = banditMilitiaHalberdierIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = banditVeteranHalberdierIcon;
                        break;
                    case Ranking.elite:
                        result.icon = banditEliteHalberdierIcon;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = banditMilitiaCavalryIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = banditVeteranCavalryIcon;
                        break;
                    case Ranking.elite:
                        result.icon = banditEliteCavalryIcon;
                        break;
                }
                break;
        }
        return result;
    }
    public TroopInfo italianGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.icon = italianRecruitIcon;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = italianMilitiaCrossbowmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = italianVeteranCrossbowmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = italianEliteCrossbowmanIcon;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = italianMilitiaMusketeerIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = italianVeteranMusketeerIcon;
                        break;
                    case Ranking.elite:
                        result.icon = italianEliteMusketeerIcon;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = italianMilitiaSwordsmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = italianVeteranSwordsmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = italianEliteSwordsmanIcon;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = italianMilitiaHalberdierIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = italianVeteranHalberdierIcon;
                        break;
                    case Ranking.elite:
                        result.icon = italianEliteHalberdierIcon;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = italianMilitiaCavalryIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = italianVeteranCavalryIcon;
                        break;
                    case Ranking.elite:
                        result.icon = italianEliteCavalryIcon;
                        break;
                }
                break;
        }
        return result;
    }
    public TroopInfo papalGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.icon = papalRecruitIcon;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = papalMilitiaCrossbowmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = papalVeteranCrossbowmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = papalEliteCrossbowmanIcon;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = papalMilitiaMusketeerIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = papalVeteranMusketeerIcon;
                        break;
                    case Ranking.elite:
                        result.icon = papalEliteMusketeerIcon;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = papalMilitiaSwordsmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = papalVeteranSwordsmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = papalEliteSwordsmanIcon;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = papalMilitiaHalberdierIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = papalVeteranHalberdierIcon;
                        break;
                    case Ranking.elite:
                        result.icon = papalEliteHalberdierIcon;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = papalMilitiaCavalryIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = papalVeteranCavalryIcon;
                        break;
                    case Ranking.elite:
                        result.icon = papalEliteCavalryIcon;
                        break;
                }
                break;
        }
        return result;
    }
    public TroopInfo frenchGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.icon = frenchRecruitIcon;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = frenchMilitiaCrossbowmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = frenchVeteranCrossbowmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = frenchEliteCrossbowmanIcon;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = frenchMilitiaMusketeerIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = frenchVeteranMusketeerIcon;
                        break;
                    case Ranking.elite:
                        result.icon = frenchEliteMusketeerIcon;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = frenchMilitiaSwordsmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = frenchVeteranSwordsmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = frenchEliteSwordsmanIcon;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = frenchMilitiaHalberdierIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = frenchVeteranHalberdierIcon;
                        break;
                    case Ranking.elite:
                        result.icon = frenchEliteHalberdierIcon;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = frenchMilitiaCavalryIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = frenchVeteranCavalryIcon;
                        break;
                    case Ranking.elite:
                        result.icon = frenchEliteCavalryIcon;
                        break;
                }
                break;
        }
        return result;
    }
    public TroopInfo imperialGetTroopInfoHelper(TroopType tt, Ranking rk)
    {
        TroopInfo result = new TroopInfo();
        switch (tt)
        {
            case TroopType.recruitType:
                result.icon = imperialRecruitIcon;
                break;
            case TroopType.crossbowman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = imperialMilitiaCrossbowmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = imperialVeteranCrossbowmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = imperialEliteCrossbowmanIcon;
                        break;
                }
                break;
            case TroopType.musketeer:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = imperialMilitiaMusketeerIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = imperialVeteranMusketeerIcon;
                        break;
                    case Ranking.elite:
                        result.icon = imperialEliteMusketeerIcon;
                        break;
                }
                break;
            case TroopType.swordsman:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = imperialMilitiaSwordsmanIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = imperialVeteranSwordsmanIcon;
                        break;
                    case Ranking.elite:
                        result.icon = imperialEliteSwordsmanIcon;
                        break;
                }
                break;
            case TroopType.halberdier:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = imperialMilitiaHalberdierIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = imperialVeteranHalberdierIcon;
                        break;
                    case Ranking.elite:
                        result.icon = imperialEliteHalberdierIcon;
                        break;
                }
                break;
            case TroopType.cavalry:
                switch (rk)
                {
                    case Ranking.militia:
                        result.icon = imperialMilitiaCavalryIcon;
                        break;
                    case Ranking.veteran:
                        result.icon = imperialVeteranCavalryIcon;
                        break;
                    case Ranking.elite:
                        result.icon = imperialEliteCavalryIcon;
                        break;
                }
                break;
        }
        return result;
    }

    public Texture2D getTroopIcon(Faction f, TroopType tt, Ranking rk)
    {
        return getTroopInfo(f, tt, rk).icon;
    }
    public Texture2D getTroopProfile(Faction f, TroopType tt, Ranking rk)
    {
        if (f == Faction.mercenary)
        {
            return getTroopInfo(f, tt, rk).profile;
        } else
        {
            return defaultProfile;
        }
        
    }
    
}
