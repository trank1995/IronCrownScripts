using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridInspectPanel : MonoBehaviour {
    public static Grid grid;
    public GameObject icon, nameTxt, staminaCost, hide, block, staminaCostUnkown, hideUnknown, blockUnkown;
    float BAR_WIDTH, BAR_HEIGHT;
    bool active = false;
    public static bool unknown = true;
    // Use this for initialization
    void Start () {
        BAR_WIDTH = staminaCost.GetComponent<RawImage>().rectTransform.sizeDelta.x;
        BAR_HEIGHT = staminaCost.GetComponent<RawImage>().rectTransform.sizeDelta.y;
        staminaCost.SetActive(false);
        block.SetActive(false);
        hide.SetActive(false);
        staminaCostUnkown.SetActive(false);
        blockUnkown.SetActive(false);
        hideUnknown.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (grid != null)
        {
            showInfo(grid);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void showInfo(Grid g)
    {
        if (!unknown)
        {
            nameTxt.GetComponent<Text>().text = grid.name;
            if (!staminaCost.activeSelf || !block.activeSelf || !hide.activeSelf)
            {
                staminaCost.SetActive(true);
                block.SetActive(true);
                hide.SetActive(true);
                staminaCostUnkown.SetActive(false);
                blockUnkown.SetActive(false);
                hideUnknown.SetActive(false);
            }
            staminaCost.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(BAR_WIDTH * (g.staminaCost / 10), BAR_HEIGHT);
            block.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(BAR_WIDTH * (g.blockRate), BAR_HEIGHT);
            hide.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(BAR_WIDTH * (g.hideRate), BAR_HEIGHT);
        } else
        {
            if (staminaCost.activeSelf || block.activeSelf || hide.activeSelf)
            {
                staminaCost.SetActive(false);
                block.SetActive(false);
                hide.SetActive(false);
                staminaCostUnkown.SetActive(true);
                blockUnkown.SetActive(true);
                hideUnknown.SetActive(true);
            }
            nameTxt.GetComponent<Text>().text = grid.name;
            
        }
        
    }
}
