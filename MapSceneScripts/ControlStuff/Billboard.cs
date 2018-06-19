using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour {
    public static Billboard billboard;
    public List<GameObject> createdPostedQuestButtons;
    List<Quest> curBillBoardQuests;
    public GameObject postedQuestButton, closePostedQuestButton;
    public Text postName, postDescription;
    public RawImage postIcon;
    // Use this for initialization
    void Start () {
        billboard = this;
        createdPostedQuestButtons = new List<GameObject>();
        gameObject.SetActive(false);
        postedQuestButton.SetActive(false);
        closePostedQuestButton.GetComponent<Button>().onClick.AddListener(delegate {
            gameObject.SetActive(false);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDisable()
    {
        if (createdPostedQuestButtons.Count > 0)
        {
            foreach (GameObject g in createdPostedQuestButtons)
            {
                GameObject.Destroy(g);
            }
            createdPostedQuestButtons.Clear();
        }
    }


    public void displayPostedQuests(List<Quest> inBillBoardQuests)
    {
        curBillBoardQuests = inBillBoardQuests;
        gameObject.SetActive(true);
        if (createdPostedQuestButtons.Count > 0)
        {
            foreach (GameObject g in createdPostedQuestButtons)
            {
                GameObject.Destroy(g);
            }
            createdPostedQuestButtons.Clear();
        }
        foreach (Quest q in inBillBoardQuests)
        {
            createPostedQuestButton(q);
        }
        
    }
    GameObject createPostedQuestButton(Quest q)
    {
        if (q != null)
        {
            postName.text = q.questName;
            postDescription.text = q.progressToDescription[q.currentProgress];

        }
        GameObject newQuestButton = Object.Instantiate(postedQuestButton);
        newQuestButton.GetComponent<Button>().onClick.AddListener(delegate {
            q.currentProgress++;
            Player.mainParty.unfinishedQuests.Add(q);
            curBillBoardQuests.Remove(q);
            displayPostedQuests(curBillBoardQuests);
        });
        newQuestButton.SetActive(true);
        newQuestButton.transform.SetParent(postedQuestButton.transform.parent, false);
        createdPostedQuestButtons.Add(newQuestButton);
        return newQuestButton;
    }
}
