using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataBase : MonoBehaviour {
    public static QuestDataBase dataBase;
    public List<Quest> quests;
    public GameObject objective;
    public Texture2D defaultQuestIcon;
	// Use this for initialization
	void Awake () {
        dataBase = this;
        objective.SetActive(false);
        initialization();

    }
	
	// Update is called once per frame
	void Update () {
		if (MapManagement.cities != null && MapManagement.cities.Count > 0)
        {
            //initializeCityQuests();
        }
	}
    void initialization()
    {
        quests = new List<Quest>();
        initializeMAIN1();
        initializeMAIN2();
    }

    void initializeMAIN1()
    {
        Quest quest = new Quest("Chapter I", "MAIN1", true);
        quest.questType = QuestType.MAIN;
        quest.totalProgress = 6;
        quest.currentProgress = 0;
        quest.progressToDescription.Add(0, "I need to go to Milano to secure supplies. ");
        quest.progressToDescription.Add(1, "We are being ambushed !");
        quest.progressToDescription.Add(2, "Navigate to Genova. ");
        quest.progressToDescription.Add(3, "I need to resupply: food for the company, and wine for myself ");
        quest.progressToDescription.Add(4, "I agreed to escort Rachelle back to Milano. ");
        quest.progressToDescription.Add(5, "Ambushed again? How?");
        quest.progressToDescription.Add(6, "So the lady joined us. Hope I won't regret this. ");
        quest.progressToLocation.Add(0, new Vector3(135, 3, 410));
        quest.progressToLocation.Add(2, new Vector3(115, 3, 310));
        quest.progressToLocation.Add(4, new Vector3(135, 3, 420));
        quest.progressToAction.Add(0, main1_0);
        quest.progressToAction.Add(1, main1_1);
        quest.progressToAction.Add(2, main1_2);
        quest.progressToAction.Add(3, main1_3);
        quest.progressToAction.Add(4, main1_4);
        quest.progressToAction.Add(5, main1_5);
        quest.progressToAction.Add(6, main1_6);
        quest.introduction = " ";
        quests.Add(quest);
    }
    void initializeMAIN2()
    {
        Quest quest = new Quest("Chapter II", "MAIN2", true);
        GameObject newObjective = GameObject.Instantiate(objective);
        newObjective.transform.position = new Vector3(250, 3, 250);
        quest.questType = QuestType.MAIN;
        quest.currentProgress = 2;
        quest.progressToDescription.Add(1, "Have 1000f");
        quest.progressToDescription.Add(2, "Pay 800f");
        quest.progressToDescription.Add(3, "Quest Complete");

        quest.introduction = "Venezia";
        quests.Add(quest);
    }
    void initializeCityQuests()
    {
        foreach(City city in MapManagement.cities)
        {
            makeCityAssassinationQuest(city);
            makeCityBanditHuntQuest(city);
        }
    }
    void makeCityBanditHuntQuest(City city)
    {
        Quest quest = new Quest(city.lName + " Bandit Hunt", QuestType.HUN + city.ID, false);
        quest.questType = QuestType.HUN;
        quest.totalProgress = 2;
        quest.currentProgress = 0;
        quest.progressToDescription.Add(0, "Kill Bandits plz");
        quest.progressToDescription.Add(1, "Kill Bandits");
        quest.progressToDescription.Add(2, "Quest Complete");
        quest.progressToLocation.Add(1, new Vector3(-1, -1, -1)); //bandit location
        quest.progressToLocation.Add(2, new Vector3(-1, -1, -1));
        quest.introduction = "Bandits are cool";
        quests.Add(quest);

    }
    void makeCityAssassinationQuest(City city)
    {
        Quest quest = new Quest(city.lName + " Political Assassination", QuestType.ASN + city.ID, false);
        quest.questType = QuestType.ASN;
        quest.totalProgress = 2;
        quest.currentProgress = 0;
        quest.progressToDescription.Add(0, "Kill some dudes plz");
        quest.progressToDescription.Add(1, "Kill Bandits");
        quest.progressToDescription.Add(2, "Quest Complete");
        quest.progressToLocation.Add(1, new Vector3(-1, -1, -1)); //bandit location
        quest.progressToLocation.Add(2, new Vector3(-1, -1, -1));
        quest.introduction = "Bandits are cool";
        quests.Add(quest);

    }
    public GameObject createObjectiveIndicator(Quest q)
    {
        if (q.progressToLocation.ContainsKey(q.currentProgress)) {
            GameObject newIndicator = Instantiate(objective, q.progressToLocation[q.currentProgress], new Quaternion(0, 0, 0, 0));
            newIndicator.transform.SetParent(objective.transform.parent, false);
            newIndicator.SetActive(true);
            newIndicator.GetComponent<Objective>().setActions(q.progressToAction[q.currentProgress]);
            return newIndicator;
        }
        if (q.progressToTarget.ContainsKey(q.currentProgress))
        {
            GameObject newIndicator = Instantiate(objective, q.progressToTarget[q.currentProgress].position, new Quaternion(0, 0, 0, 0));
            newIndicator.transform.SetParent(objective.transform.parent, false);
            newIndicator.SetActive(true);
            q.progressToTarget[q.currentProgress].setPartyDisband(q.progressToAction[q.currentProgress]);
            newIndicator.GetComponent<Objective>().target = q.progressToTarget[q.currentProgress];
            return newIndicator;
        }
        Debug.Log(q.currentProgress + "no obj");
        return null;
    }
    public Quest getQuest(string id)
    {
        Quest result = null;
        foreach(Quest q in quests)
        {
            if (q.questID == id)
            {
                result = new Quest(q);
                return result;
            }
            
        }
        return result;
    }
    void test()
    {
        Debug.Log("text action");
    }
    void main1_0()
    {
        Quest q = Player.mainParty.getQuest("MAIN1");
        q.currentProgress = 1;
        //VideoManager.videoManager.playVideo(1, 0); //before first battle
        //Debug.Log("vid 1_0");
        Person leader = new Person("Enemy Leader", new Stats(5, 5, 5, 5, 8, 10), Ranking.elite, TroopType.swordsman, Faction.bandits, new Experience(43, 30, 0));
        Party npcParty = new Party(leader, "Mysterious Attacker", leader.faction, 1000);
        npcParty.cash = 400;
        npcParty.makeInventory(300, 20, true);
        npcParty.position = new Vector3(135, 3, 415);
        npcParty.unique = true;
        MapManagement.parties.Add(npcParty);
        //npcParty.setPartyDisband(q.pro);
        MapManagement.mapManagement.loadSingleParty(npcParty);
        q.progressToTarget.Add(1, npcParty);
        MapManagement.mapManagement.buildObjective();
        q.introduction += "Now I need to defeat some random enemies. ";
    }
    void main1_1()
    {
        //VideoManager.videoManager.playVideo(1, 1); //after first battle
        //Debug.Log("vid 1_2");
        Quest q = Player.mainParty.getQuest("MAIN1");
        q.currentProgress = 2;
        //MapManagement.mapManagement.buildObjective();
    }
    void main1_2()
    {
        Quest q = Player.mainParty.getQuest("MAIN1");
        q.currentProgress = 3;
        //VideoManager.videoManager.playVideo(1, 1); //reach genova, tutorial
        Debug.Log("vid 1_3");
        main1_3();
        MapManagement.mapManagement.buildObjective();
    }
    void main1_3()
    {
        Quest q = Player.mainParty.getQuest("MAIN1");
        //VideoManager.videoManager.playVideo(1, 3); //reach genova, tutorial
        q.currentProgress = 4;
        Debug.Log("demo panels");
        //show demo panel if have time
        main1_4();
        MapManagement.mapManagement.buildObjective();
    }
    void main1_4()
    {
        Quest q = Player.mainParty.getQuest("MAIN1");
        q.currentProgress = 5;
        //VideoManager.videoManager.playVideo(1, 3); //before sec battle
        Debug.Log("vid 1_4");
        Person leader = new Person("Bandit Leader", new Stats(7, 5, 6, 7, 7, 5), Ranking.elite, TroopType.swordsman, Faction.bandits, new Experience(47, 30, 0));
        Party npcParty = new Party(leader, "Rand Hand Company", leader.faction, 700);
        npcParty.position = new Vector3(240, 3, 170);
        npcParty.unique = true;
        MapManagement.parties.Add(npcParty);
        MapManagement.mapManagement.loadSingleParty(npcParty);
        q.progressToTarget.Add(5, npcParty);
        MapManagement.mapManagement.buildObjective();
    }
    void main1_5()
    {
        Quest q = Player.mainParty.getQuest("MAIN1");
        q.currentProgress = 6;
        //VideoManager.videoManager.playVideo(1, 4); //after sec battle
        Debug.Log("vid 1_5");
        main1_6();
    }
    void main1_6()
    {
        Quest q = Player.mainParty.getQuest("MAIN1");
        q.currentProgress = 6;
        Player.mainParty.unfinishedQuests.Remove(q);
        Player.mainParty.unfinishedQuests.Add(Player.mainParty.getQuest("MAIN2"));
        Player.mainParty.finishedQuests.Add(q);
    }
}

public class Quest
{
    public string questName { get; set; }
    public string questID { get; set; }
    public bool unique;
    public List<string> prerequisites;
    public int totalProgress = 0;
    public int currentProgress = 0;
    public Texture2D questIcon, questProfile;
    public Dictionary<int, string> progressToDescription;
    public Dictionary<int, Vector3> progressToLocation;
    public Dictionary<int, Action> progressToAction;
    public Dictionary<int, Party> progressToTarget;
    public QuestType questType;
    public float colliderSizeMultiplier = 1.0f;
    public bool active = false;
    public bool complete = false;
    public int stack = 0;
    public string introduction;
    public Quest(string name, string ID, bool uniqueI)
    {
        questName = name;
        questID = ID;
        unique = uniqueI;
        prerequisites = new List<string>();
        progressToDescription = new Dictionary<int, string>();
        progressToLocation = new Dictionary<int, Vector3>();
        progressToTarget = new Dictionary<int, Party>();
        progressToAction = new Dictionary<int, Action>();
    }
    public Quest(Quest quest)
    {
        questName = quest.questName;
        questID = quest.questID;
        unique = quest.unique;
        prerequisites = quest.prerequisites;
        totalProgress = quest.totalProgress;
        currentProgress = quest.currentProgress;
        questIcon = quest.questIcon;
        questProfile = quest.questProfile;
        progressToDescription = quest.progressToDescription;
        progressToLocation = quest.progressToLocation;
        progressToTarget = quest.progressToTarget;
        progressToAction = quest.progressToAction;
        questType = quest.questType;
        colliderSizeMultiplier = quest.colliderSizeMultiplier;
        active = quest.active;
        complete = quest.complete;
        stack = quest.stack;
        introduction = quest.introduction;
    }
    public bool fulfilledPrerequisite(List<Quest> completedQuests)
    {
        List<string> completedIDs = new List<string>();
        foreach (Quest q in completedQuests)
        {
            completedIDs.Add(q.questID);

        }
        if (unique && prerequisites.Count > 0)
        {
            foreach (string id in prerequisites)
            {
                if (!completedIDs.Contains(id))
                {
                    return false;
                }
            }
            return true;
        } else {
            foreach (string id in prerequisites)
            {
                if (completedIDs.Contains(id))
                {
                    return false;
                }
            }
            return true;
        }
        
    }

}

public enum QuestType
{
    MAIN,
    HUN,
    ASN

}