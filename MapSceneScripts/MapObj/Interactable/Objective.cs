using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : Interactable {

    public Quest quest;
    public Party target;
    Action action;
    public override void Update()
    {
        base.Update();
        if (target != null)
        {
            gameObject.transform.position = target.position;
        }
    }
    public void setActions(Action objAction)
    {
        action = objAction;
    }
    public override void interact()
    {
        base.interact();
        if (action != null)
        {
            action.Invoke();
        }
        gameObject.SetActive(false);
    }

}
