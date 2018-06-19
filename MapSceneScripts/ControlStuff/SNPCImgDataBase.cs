using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNPCImgDataBase : MonoBehaviour {
    public static SNPCImgDataBase dataBase;
    public Dictionary<Person, SNPCTroopInfo> snpcDict;
    //ZERO STEP
    public Texture2D ludvicoProfile, biancaProfile, girolamoProfile, pieroProfile,
        giovanniProfile, cesareProfile, bernaultProfile, mariaProfile, jakobProfile;
    public Texture2D ludvicoIcon, biancaIcon, girolamoIcon, pieroIcon,
        giovanniIcon, cesareIcon, bernaultIcon, mariaIcon, jakobIcon;
    // Use this for initialization
    void Awake () {
        dataBase = gameObject.GetComponent<SNPCImgDataBase>();
        initialization();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void initialization()
    {
        snpcDict = new Dictionary<Person, SNPCTroopInfo>();
        //FIRST STEP
        snpcDict.Add(new Person("Ludvico Sforza", new Stats(10, 9, 8, 10, 10, 10), Ranking.elite, TroopType.cavalry, Faction.italy, new Experience(0, 51, 0)), generateTroopInfo(ludvicoIcon, ludvicoProfile));
        snpcDict.Add(new Person("Bianca Sforza", new Stats(7, 9, 8, 6, 9, 5), Ranking.elite, TroopType.musketeer, Faction.empire, new Experience(0, 38, 0)), generateTroopInfo(biancaIcon, biancaProfile));
        snpcDict.Add(new Person("Girolamo Savonarola", new Stats(3, 3, 4, 5, 6, 7), Ranking.elite, TroopType.halberdier, Faction.italy, new Experience(0, 11, 0)), generateTroopInfo(girolamoIcon, girolamoProfile));
        snpcDict.Add(new Person("Piero Soderini", new Stats(5, 5, 6, 6, 5, 5), Ranking.elite, TroopType.halberdier, Faction.italy, new Experience(0, 26, 0)), generateTroopInfo(pieroIcon, pieroProfile));
        snpcDict.Add(new Person("Giovanni de’ Medici", new Stats(7, 7, 8, 8, 6, 6), Ranking.elite, TroopType.halberdier, Faction.italy, new Experience(0, 36, 0)), generateTroopInfo(giovanniIcon, giovanniProfile));
        snpcDict.Add(new Person("Cesare Borgia", new Stats(10, 10, 10, 10, 10, 10), Ranking.elite, TroopType.cavalry, Faction.papacy, new Experience(0, 54, 0)), generateTroopInfo(cesareIcon, cesareProfile));
        snpcDict.Add(new Person("Bernault Stuart", new Stats(10, 10, 10, 10, 7, 6), Ranking.elite, TroopType.cavalry, Faction.france, new Experience(0, 35, 0)), generateTroopInfo(bernaultIcon, bernaultProfile));
        snpcDict.Add(new Person("Maria de Luna", new Stats(8, 9, 8, 8, 9, 9), Ranking.elite, TroopType.crossbowman, Faction.papacy, new Experience(0, 45, 0)), generateTroopInfo(mariaIcon, mariaProfile));
        snpcDict.Add(new Person("Jakob von Liebenstein", new Stats(8, 9, 8, 8, 10, 10), Ranking.elite, TroopType.musketeer, Faction.empire, new Experience(0, 47, 0)), generateTroopInfo(jakobIcon, jakobProfile));

        addDescription();
    }

    void addDescription()
    {
        //SEC STEP
        getSNPCTroopInfo("Ludvico Sforza").description.Add(0, "A fucktard.");
        getSNPCTroopInfo("Bianca Sforza").description.Add(0, "A basic bitch.");
        //getSNPCTroopInfo("Red Hand Leader").description.Add(0, "A leader.");
        //getSNPCTroopInfo("Army Leader").description.Add(0, "Another leader.");
        getSNPCTroopInfo("Girolamo Savonarola").description.Add(0, "A lunatic.");
        getSNPCTroopInfo("Piero Soderini").description.Add(0, "An old man.");
        getSNPCTroopInfo("Giovanni de’ Medici").description.Add(0, "He's gonna be pope");
        getSNPCTroopInfo("Cesare Borgia").description.Add(0, "A boss.");
        getSNPCTroopInfo("Bernault Stuart").description.Add(0, "A Scot in France.");
        getSNPCTroopInfo("Maria de Luna").description.Add(0, "A lady.");
        getSNPCTroopInfo("Jakob von Liebenstein").description.Add(0, "A bishop.");
    }

    SNPCTroopInfo generateTroopInfo(Texture2D icon, Texture2D profile)
    {
        SNPCTroopInfo result = new SNPCTroopInfo();
        result.icon = icon;
        result.profile = profile;
        result.index = 0;
        result.description = new Dictionary<int, string>();
        return result;
    }

    public SNPCTroopInfo getSNPCTroopInfo(string name)
    {
        foreach(KeyValuePair<Person, SNPCTroopInfo> pair in snpcDict)
        {
            if (name == pair.Key.name)
            {
                return pair.Value;
            }
        }
        return null;
    }
}
public class SNPCTroopInfo
{
    public GameObject model;
    public Texture2D icon;
    public Texture2D profile;
    public GearInfo gear;
    public Dictionary<int, string> description;
    public int index;
    public bool alive;
}