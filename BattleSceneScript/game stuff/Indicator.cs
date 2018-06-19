using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : BattleInteractable {
    public List<Grid> collided = new List<Grid>();
    public GameObject dmgCanvases;
    public List<Text> dmgTexts;
    private void Start()
    {

    }

    private void Update()
    {
        lookAtCamera(dmgCanvases);
    }
    private void OnEnable()
    {
        collided.Clear();
    }
    public override void cameraFocusOn()
    {
        //base.cameraFocusOn();
    }
    public void goToIndicatedGrid(GameObject troop) //only works for walk indicator
    {
        Grid curGrid = BattleCentralControl.map[Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)];
        troop.GetComponent<Troop>().troopMoveToPlace(curGrid);
    }
    
    

    public void showDmg(int number)
    {
        if (dmgTexts.Count > 0)
        {
            foreach(Text t in dmgTexts)
            {
                t.text = number.ToString();
            }
        }
    }

    void lookAtCamera(GameObject obj)
    {
        if (obj != null)
        {
            Vector3 v = Camera.main.transform.position - obj.transform.position;
            v.x = v.z = 0.0f;
            obj.transform.LookAt(Camera.main.transform.position - v);
            obj.transform.Rotate(0, 180, 0);
        }
        
    }
}
