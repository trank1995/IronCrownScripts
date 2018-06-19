using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManagement : MonoBehaviour
{
    public static QuestManagement questManagement;
    public GameObject questButton, finishedQuestButton;
    public Text questName;
    public Text finishedQuestName, finishedQuestAmount;
    public Text inspectQuestName, inspectQuestIntro;
    public GameObject singleObjective;
    public Quest curQuest;
    public Texture2D selectedImg, unselectedImg;
    public Texture2D finishedSelectedImg, finishedUnselectedImg;
    public Texture2D completeImg, incompleteImg;
    public List<GameObject> createdQuestButtons, createdFinishedQuestButton, createdObjectives;

    GameObject curQuestButton;
    bool initialized = false;
    // Use this for initialization
    private void Start()
    {
        questManagement = this;
        questButton.SetActive(false);
        finishedQuestButton.SetActive(false);
        singleObjective.SetActive(false);
        createdObjectives = new List<GameObject>();
        createdQuestButtons = new List<GameObject>();

    }
    private void OnEnable()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Player.mainParty != null && !initialized)
        {
            //Debug.Log()
            displayQuests();
            initialized = true;
        }
    }



    void displayQuests()
    {
        foreach (Quest q in Player.mainParty.unfinishedQuests)
        {
            if (q.currentProgress >= 0)
            {
                createQuestButton(q);
            }
        }
        foreach (Quest q in Player.mainParty.finishedQuests)
        {
            createFinishedQuestButton(q);
        }
    }

    void inspect(Quest quest)
    {
        inspectQuestName.text = quest.questName;
        inspectQuestIntro.text = quest.introduction;
        if (createdObjectives.Count > 0)
        {
            foreach (GameObject g in createdObjectives)
            {
                GameObject.Destroy(g);
            }
            createdObjectives.Clear();
        }

        for (int i = 0; i <= quest.currentProgress; i++)
        {
            if (quest.progressToDescription.ContainsKey(i) && quest.progressToDescription[i] != null)
            {
                displayObjective(quest, i, quest.progressToDescription[i]);
            }
        }
    }
    void displayObjective(Quest quest, int progress, string objectiveDescription)
    {
        singleObjective.transform.Find("CompleteIcon").GetComponent<RawImage>().texture = incompleteImg;
        if (progress != 0 && progress >= quest.currentProgress)
        {
            singleObjective.transform.Find("CompleteIcon").GetComponent<RawImage>().texture = completeImg;
        }
        singleObjective.transform.Find("Description").GetComponent<Text>().text = objectiveDescription;

        GameObject newSingleObjective = Object.Instantiate(singleObjective);
        newSingleObjective.transform.SetParent(singleObjective.transform.parent, false);
        createdObjectives.Add(newSingleObjective);
        newSingleObjective.SetActive(true);
    }

    GameObject createQuestButton(Quest q)
    {
        questButton.GetComponent<RawImage>().texture = unselectedImg;
        if (q != null)
        {
            questName.text = q.questName;
            //questDescription.text = q.progressToDescription[q.currentProgress];

        }
        GameObject newQuestButton = Object.Instantiate(questButton);
        newQuestButton.GetComponent<Button>().onClick.AddListener(delegate { questButtonOnClick(newQuestButton, q); });
        newQuestButton.SetActive(true);
        newQuestButton.transform.SetParent(questButton.transform.parent, false);
        createdQuestButtons.Add(newQuestButton);
        return newQuestButton;
    }

    GameObject createFinishedQuestButton(Quest q)
    {
        finishedQuestButton.GetComponent<RawImage>().texture = finishedUnselectedImg;
        if (q != null)
        {
            finishedQuestName.text = q.questName;
            finishedQuestAmount.text = "";
            //questDescription.text = q.progressToDescription[q.currentProgress];

        }
        GameObject newQuestButton = Object.Instantiate(finishedQuestButton);
        newQuestButton.GetComponent<Button>().onClick.AddListener(delegate {
            curQuest = q;
            changeCurQuestButton(newQuestButton);
            inspect(q);
        });
        newQuestButton.SetActive(true);
        newQuestButton.transform.SetParent(finishedQuestButton.transform.parent, false);
        createdQuestButtons.Add(newQuestButton);
        return newQuestButton;
    }

    void questButtonOnClick(GameObject newQuestButton, Quest q)
    {
        if (curQuest == q)
        {
            WorldInteraction.worldInteraction.curQuest = null;
            curQuest = null;
        }
        else
        {
            WorldInteraction.worldInteraction.curQuest = q;
            curQuest = q;
            changeCurQuestButton(newQuestButton);
            inspect(q);
        }

    }

    void changeCurQuestButton(GameObject buttonObj)
    {
        if (curQuestButton != null)
        {
            curQuestButton.GetComponent<RawImage>().texture = unselectedImg;
        }
        buttonObj.GetComponent<RawImage>().texture = selectedImg;
        curQuestButton = buttonObj;
    }


}


