using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceCoop : MonoBehaviour {
    public static FaceCoop faceCoop;
    public Button facecoop, emperor, france, papacy, italy;
    public GameObject milano, torino, asti, parma, genova,
        modena, verona, padova, treviso, venezia, ferrara, bologna, firenze, ravenne,
        urbino, lucca, pisa, siena, grosseto, perugia, roma;
    public RawImage factionBackground;
    public GameObject posPerk1, posPerk2, posPerk3, negPerk1, negPerk2, negPerk3;
    public GameObject factionPanel, cityPanel, peoplePanel, feedPanel;
    public GameObject personButton;
    public RawImage personIcon;
    public Text selectingPersonName;
    public RawImage inspectPersonProfile;
    public Text inspectPersonName, inspectPersonDescription;
    public GameObject postPanel;
    public RawImage postImg;
    public Text postTitle, poster, postContent;
    int daySCounter, dayECounter;
    List<GameObject> createdButtons;
    Queue<FaceCoopPost> currentPosts;
    Dictionary<FaceCoopPost, GameObject> postDict;
	// Use this for initialization
	void Start () {
        faceCoop = this;
        facecoop.onClick.AddListener(delegate { showFrontPage(); });
        emperor.onClick.AddListener(delegate { displayFactionPerks(Faction.empire); });
        france.onClick.AddListener(delegate { displayFactionPerks(Faction.france); });
        papacy.onClick.AddListener(delegate { displayFactionPerks(Faction.papacy); });
        italy.onClick.AddListener(delegate { displayCityPerks(); });
        closeFactionPerk();
        cityPanel.SetActive(false);
        peoplePanel.SetActive(false);
        personButton.SetActive(false);
        peoplePanel.SetActive(false);
        feedPanel.SetActive(true);
        createdButtons = new List<GameObject>();
        currentPosts = new Queue<FaceCoopPost>();
        postDict = new Dictionary<FaceCoopPost, GameObject>();
        displayPersons();
    }
	
	// Update is called once per frame
	void Update () {
        //daySCounter = TimeSystem.hour;
        //if (daySCounter != dayECounter)
        //{
        //    var postToRemove = currentPosts.Dequeue();
        //    if (postToRemove != null)
        //    {
        //        
        //        postDict.Remove(postToRemove);
        //    }
        //}
        //dayECounter = TimeSystem.hour;
    }

    void displayPersons()
    {
        foreach(KeyValuePair<Person, SNPCTroopInfo> pair in SNPCImgDataBase.dataBase.snpcDict)
        {
            createPersonButton(pair.Key);
        }
    }
    void inspectPerson(Person person)
    {
        inspectPersonProfile.texture = SNPCImgDataBase.dataBase.getSNPCTroopInfo(person.name).profile;
        inspectPersonName.text = person.name;
        inspectPersonDescription.text = SNPCImgDataBase.dataBase.getSNPCTroopInfo(person.name).description[0]; //CHANGE TO QUEST PROGESS LATER
        peoplePanel.SetActive(true);
        cityPanel.SetActive(false);
        factionPanel.SetActive(false);
        feedPanel.SetActive(false);
    }
    GameObject createPersonButton(Person person)
    {
        if (person != null)
        {
            selectingPersonName.text = person.name;
            personIcon.texture = SNPCImgDataBase.dataBase.getSNPCTroopInfo(person.name).icon;
            //personProfile.texture = SNPCImgDataBase.dataBase.getSNPCTroopInfo(person.name).icon;
        }
        GameObject newPersonButton = Object.Instantiate(personButton);
        newPersonButton.transform.SetParent(personButton.transform.parent, false);
        newPersonButton.GetComponent<Button>().onClick.AddListener(delegate { inspectPerson(person);});
        newPersonButton.SetActive(true);
        createdButtons.Add(newPersonButton);
        return newPersonButton;
    }
    void showFrontPage()
    {
        factionPanel.SetActive(false);
        cityPanel.SetActive(false);
        peoplePanel.SetActive(false);
        feedPanel.SetActive(true);
    }
    void displayFactionPerks(Faction faction)
    {
        int factionFavor = 0;
        feedPanel.SetActive(false);
        cityPanel.SetActive(false);
        closeFactionPerk();
        factionPanel.SetActive(true);
        //factionPanel.transform.parent.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        switch (faction)
        {
            case Faction.empire:
                factionFavor = Player.mainParty.getFactionFavor(Faction.empire);
                if (factionFavor < 25 && factionFavor > -25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empDefault;
                } else if (factionFavor < 50 && factionFavor >= 25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empPos1;
                } else if (factionFavor < 75 && factionFavor >= 50)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empPos2;
                } else if (factionFavor <= 101 && factionFavor >= 75)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empPos3;
                } else if (factionFavor > -50 && factionFavor <= -25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empNeg1;
                }
                else if (factionFavor > -75 && factionFavor <= -50)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empNeg2;
                }
                else if (factionFavor >= -101 && factionFavor <= -75)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.empNeg3;
                }
                
                fillInPerk(posPerk1, Player.mainParty.factionPerkTree.getPerk("EMP+1"));
                fillInPerk(posPerk2, Player.mainParty.factionPerkTree.getPerk("EMP+2"));
                fillInPerk(posPerk3, Player.mainParty.factionPerkTree.getPerk("EMP+3"));
                fillInPerk(negPerk1, Player.mainParty.factionPerkTree.getPerk("EMP-1"));
                fillInPerk(negPerk2, Player.mainParty.factionPerkTree.getPerk("EMP-2"));
                fillInPerk(negPerk3, Player.mainParty.factionPerkTree.getPerk("EMP-3"));
                break;
            case Faction.france:
                factionFavor = Player.mainParty.getFactionFavor(Faction.france);
                if (factionFavor < 25 && factionFavor > -25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraDefault;
                }
                else if (factionFavor < 50 && factionFavor >= 25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraPos1;
                }
                else if (factionFavor < 75 && factionFavor >= 50)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraPos2;
                }
                else if (factionFavor <= 101 && factionFavor >= 75)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraPos3;
                }
                else if (factionFavor > -50 && factionFavor <= -25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraNeg1;
                }
                else if (factionFavor > -75 && factionFavor <= -50)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraNeg2;
                }
                else if (factionFavor >= -101 && factionFavor <= -75)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.fraNeg3;
                }

                fillInPerk(posPerk1, Player.mainParty.factionPerkTree.getPerk("FRA+1"));
                fillInPerk(posPerk2, Player.mainParty.factionPerkTree.getPerk("FRA+2"));
                fillInPerk(posPerk3, Player.mainParty.factionPerkTree.getPerk("FRA+3"));
                fillInPerk(negPerk1, Player.mainParty.factionPerkTree.getPerk("FRA-1"));
                fillInPerk(negPerk2, Player.mainParty.factionPerkTree.getPerk("FRA-2"));
                fillInPerk(negPerk3, Player.mainParty.factionPerkTree.getPerk("FRA-3"));
                break;
            case Faction.papacy:
                factionFavor = Player.mainParty.getFactionFavor(Faction.papacy);
                if (factionFavor < 25 && factionFavor > -25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papDefault;
                }
                else if (factionFavor < 50 && factionFavor >= 25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papPos1;
                }
                else if (factionFavor < 75 && factionFavor >= 50)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papPos2;
                }
                else if (factionFavor <= 101 && factionFavor >= 75)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papPos3;
                }
                else if (factionFavor > -50 && factionFavor <= -25)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papNeg1;
                }
                else if (factionFavor > -75 && factionFavor <= -50)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papNeg2;
                }
                else if (factionFavor >= -101 && factionFavor <= -75)
                {
                    factionBackground.texture = MapSceneUIImageDataBase.dataBase.papNeg3;
                }

                fillInPerk(posPerk1, Player.mainParty.factionPerkTree.getPerk("PAP+1"));
                fillInPerk(posPerk2, Player.mainParty.factionPerkTree.getPerk("PAP+2"));
                fillInPerk(posPerk3, Player.mainParty.factionPerkTree.getPerk("PAP+3"));
                fillInPerk(negPerk1, Player.mainParty.factionPerkTree.getPerk("PAP-1"));
                fillInPerk(negPerk2, Player.mainParty.factionPerkTree.getPerk("PAP-2"));
                fillInPerk(negPerk3, Player.mainParty.factionPerkTree.getPerk("PAP-3"));
                break;
        }
    }
    void displayCityPerks()
    {
        closeFactionPerk();
        cityPanel.SetActive(true);
        feedPanel.SetActive(false);
        //cityPanel.transform.parent.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        fillInCityPerks(milano, Player.mainParty.factionPerkTree.getPerk("MIL"));
        fillInCityPerks(torino, Player.mainParty.factionPerkTree.getPerk("TOR"));
    }
    void fillInCityPerks(GameObject toFill, Perk perk)
    {
        GameObject buttonGameObject = toFill.transform.Find("PerkButton").gameObject;
        buttonGameObject.SetActive(true);
        buttonGameObject.transform.Find("Frame").gameObject.SetActive(perk.own);
        GameObject panelGameObject = toFill.transform.Find("ExplainationPanel").gameObject;
        panelGameObject.transform.Find("PerkName").gameObject.GetComponent<Text>().text = perk.skillName;
        panelGameObject.transform.Find("ExplanationDescription").gameObject.GetComponent<Text>().text = perk.description;
        panelGameObject.transform.Find("ExplanationQuote").gameObject.GetComponent<Text>().text = perk.quote;
        buttonGameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonGameObject.GetComponent<Button>().onClick.AddListener(delegate { panelGameObject.SetActive(!panelGameObject.activeSelf); });
    }
    void fillInPerk(GameObject toFill, Perk perk)
    {
        GameObject buttonGameObject = toFill.transform.Find("PerkButton").gameObject;
        buttonGameObject.SetActive(true);
        buttonGameObject.transform.Find("PerkName").gameObject.GetComponent<Text>().text = perk.skillName;
        buttonGameObject.transform.Find("Frame").gameObject.SetActive(perk.own);
        GameObject panelGameObject = toFill.transform.Find("ExplainationPanel").gameObject;
        panelGameObject.transform.Find("ExplanationDescription").gameObject.GetComponent<Text>().text = perk.description;
        panelGameObject.transform.Find("ExplanationQuote").gameObject.GetComponent<Text>().text = perk.quote;
        buttonGameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonGameObject.GetComponent<Button>().onClick.AddListener(delegate { panelGameObject.SetActive(!panelGameObject.activeSelf); });
    }
    void closeFactionPerk()
    {
        closePerkButton(posPerk1);
        closePerkButton(posPerk2);
        closePerkButton(posPerk3);
        closePerkButton(negPerk1);
        closePerkButton(negPerk2);
        closePerkButton(negPerk3);
        factionPanel.SetActive(false);
    }
    void closePerkButton(GameObject toReset)
    {
        GameObject buttonGameObject = toReset.transform.Find("PerkButton").gameObject;
        buttonGameObject.SetActive(false);
        GameObject panelGameObject = toReset.transform.Find("ExplainationPanel").gameObject;
        buttonGameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }
    GameObject createPost(FaceCoopPost post)
    {
        postTitle.text = post.postTitle;
        poster.text = post.poster;
        postContent.text = post.postContent;
        postImg.texture = post.postImg;
        GameObject newPost = Object.Instantiate(postPanel);
        newPost.transform.SetParent(postPanel.transform.parent, false);
        postDict.Add(post, newPost);
        return newPost;
    }

    public void leaveManagement()
    {
        showFrontPage();

    }
}
