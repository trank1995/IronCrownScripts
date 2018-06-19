using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Interactable : MonoBehaviour {

    const float INTERACT_DIST = 1;
    public NavMeshAgent playerAgent;
    //public GameObject interactedObject;
    public new string name;
    public InteractableType interactableType;
    public int hostility;
    public string[] dialogue;
    public bool hasInteracted;
    public GameObject inspectPanel;

    public virtual void Start()
    {
        //inspectPanel.SetActive(false);
    }
    public virtual void Update()
    {
    }
    
    public virtual void inspect(bool inspecting)
    {
        if (inspecting)
        {
            inspectPanel.SetActive(true);

        } else
        {
            inspectPanel.SetActive(true);
        }
    }
    public virtual void interact()
    {
        WorldInteraction.chasing = false;
    }
    public virtual void OnTriggerEnter(Collider col)
    {
        WorldInteraction.worldInteraction.stopEveryone(true);
        hasInteracted = true;
    }
    public virtual void OnTriggerExit(Collider col)
    {
        hasInteracted = false;
    }
}

public enum InteractableType
{
    NPC,
    town,
    castle,
    city
};