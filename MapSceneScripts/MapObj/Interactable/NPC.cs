using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPC : Interactable {
    public NavMeshAgent npcAgent;
    public GameObject model;
    public PartyVisionIndicator partyVisionIndicator;
    public Party npcParty = null;
    
    public Material invisibleMaterial;
    Dictionary<MeshRenderer, Material[]> originalMaterials;
    MeshRenderer[] meshRenderers;
    bool initialized = false;
    bool seen;
    int hrSCounter, hrECounter, daySCounter, dayECounter, monthSCounter, monthECounter;
    const int roamRange = 60;
    private void Awake()
    {

    }
    public virtual void OnEnable()
    {
        
        
    }
    public override void Start()
    {
        base.Start();
        npcAgent = transform.GetComponent<NavMeshAgent>();
        meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
        originalMaterials = new Dictionary<MeshRenderer, Material[]>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalMaterials.Add(meshRenderers[i], (Material[])meshRenderers[i].materials.Clone());
        }
        
    }
    public override void Update()
    {
        base.Update();
        partyUpdate();
        if (!initialized && npcParty != null)
        {
            initialization();
            initialized = true;
            hideSelf();
        } else if (!initialized && npcParty == null)
        {
            return;
        }
        if (npcParty.battlefieldTypes == null)
        {
            Debug.Log("bt s null");
            npcParty.battlefieldTypes = new List<BattlefieldType>();
        }
        lookAtCamera(model);
        
        if (npcParty != null)
        {
            npcParty.position = transform.position;
        }
        else
        {
            gameObject.SetActive(false);
        }
        if (npcParty.partyMember.Count == 0)
        {
            MapManagement.parties.Remove(npcParty);
            GameObject.Destroy(gameObject);
        }
        npcAgent.speed = npcParty.getTravelSpeed();
        if (!TimeSystem.pause)
        {
            //npcAgent = transform.GetComponent<NavMeshAgent>();
            //npcAgent.isStopped = false;
            if (npcAgent.isActiveAndEnabled)
            {
                npcAgent.destination = getRoamTarget(); //Player.mainParty.position;
            }
            else
            {
                npcAgent.Warp(transform.position);
            }

        }
        else
        {
            npcAgent.destination = transform.position;
            npcAgent.isStopped = true;
        }
        inspectPanel.GetComponent<InspectPanel>().updateTexts(npcParty);
        if (npcParty.leader == null)
        {
            npcParty.electNewLeader();
        }
    }
    public void FixedUpdate()
    {
        
    }
    void partyUpdate()
    {
        //PER HR
        hrSCounter = TimeSystem.hour;
        if (hrSCounter != hrECounter)
        {
            if (npcParty.battling <= 1)
            {
                npcParty.battling = 0;
            }
            if (npcParty.battling > 0)
            {
                npcParty.battling -= 1;
            }
        }
        hrECounter = TimeSystem.hour;

        //PER DAY
        daySCounter = TimeSystem.day;
        if (daySCounter != dayECounter)
        {
            foreach (Person p in npcParty.partyMember)
            {
                if (p.health < p.getHealthMax())
                {
                    p.health = Mathf.Clamp(p.health + p.healthRegenPerDay(), p.health, p.getHealthMax());
                }
            }
        }
        dayECounter = TimeSystem.day;

        //PER MONTH
        monthSCounter = TimeSystem.month;
        if (monthSCounter != monthECounter)
        {
            grow();
        }
        monthECounter = TimeSystem.month;
    }
    public virtual void initialization()
    {
        
        if (npcParty.battlefieldTypes == null)
        {
            npcParty.battlefieldTypes = new List<BattlefieldType>();
        }
        npcParty.makeInventory(2000, npcParty.partyMember.Count * 2, npcParty.hasShape);
    }
    public override void interact()
    {
        base.interact();
        DialogueSystem.Instance.createDialogue(PanelType.NPC, npcParty);
        //DialogueSystem.Instance.addNewDialogue(npcParty, dialogue, PanelType.NPC);
        //DialogueSystem.Instance.createDialogue(PanelType.NPC, npcParty);
    }
    public override void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player" && npcParty.factionFavors[Faction.mercenary] < 0)
        {
            //start dialogue
            DialogueSystem.Instance.addNewDialogue(npcParty, dialogue, PanelType.NPC);
            DialogueSystem.Instance.createDialogue(PanelType.NPC, npcParty);

        }
        if (col.gameObject.tag == "NPC" && npcParty.battling < 1)
        {
            NPC encountered = col.gameObject.GetComponent<NPC>();
            if (npcParty.factionFavors[encountered.npcParty.faction] < 0 && encountered.npcParty.battling == 0)
            {
                MapManagement.mapManagement.battleSimulation(this, col.gameObject.GetComponent<NPC>(), npcParty.battlefieldTypes);
                partyVisionIndicator.reLook();
            }
            
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "NPC" && npcParty.battling < 1)
        {
            NPC encountered = col.gameObject.GetComponent<NPC>();
            if (npcParty.factionFavors[encountered.npcParty.faction] < 0 && encountered.npcParty.battling == 0)
            {
                MapManagement.mapManagement.battleSimulation(this, col.gameObject.GetComponent<NPC>(), npcParty.battlefieldTypes);
                partyVisionIndicator.reLook();
            }

        }
    }
    public override void inspect(bool inspecting)
    {
        if (inspecting && seen)
        {
            inspectPanel.SetActive(true);

        }
        else
        {
            inspectPanel.SetActive(false);
        }
    }

    public virtual Vector3 getRoamTarget()
    {
        Vector3 result = transform.position + new Vector3(Random.Range(-roamRange, roamRange), 0, Random.Range(-roamRange, roamRange));
        Party closestEnemy = null;
        if (npcParty != null && npcParty.battling == 0)
        {

            if (getNpcInVision().Count > 0)
            {
                int hate = 0;
                foreach (Party p in getNpcInVision())
                {
                    if (!p.factionFavors.ContainsKey(npcParty.faction))
                    {
                        Debug.Log(npcParty.name + " doesnt have " + p.faction);
                    }
                    if (p.factionFavors[npcParty.faction] < 0)
                    {
                        if (closestEnemy == null || Vector3.Distance(npcParty.position, p.position) < Vector3.Distance(npcParty.position, closestEnemy.position))
                        {
                            closestEnemy = p;
                        }
                    }
                    
                    if (npcParty.factionFavors[p.faction] < hate)
                    {
                        hate = npcParty.factionFavors[p.faction];
                        if (hate < 0)
                        {
                            result = p.position;
                        }
                    }
                }
                if (closestEnemy != null && closestEnemy.getBattleValue() * .7f > npcParty.getBattleValue())
                {
                    result = 2 * npcParty.position - closestEnemy.position;
                }
            } 
        } else
        {
            result = transform.position;
        }
        //Debug.Log(result);
        return result;
    }

    public virtual void grow()
    {
        //foreach (Item i in npcParty.inventory)
        //{
        //    npcParty.cash += i.getSellingPrice();
        //
        //}
        //npcParty.battleValue += npcParty.cash / 2;
        if (npcParty.partyMember.Count < npcParty.getPartySizeLimit())
        {
            //makeParty();
        }
        foreach (Person p in npcParty.partyMember)
        {
            while (p.exp.sparedPoint >= 1)
            {
                int rand = Random.Range(0, 5);
                switch (rand) {
                    case 0:
                        p.incrementS();
                        break;
                    case 1:
                        p.incrementA();
                        break;
                    case 2:
                        p.incrementP();
                        break;
                    case 3:
                        p.incrementE();
                        break;
                    case 4:
                        p.incrementC();
                        break;
                    case 5:
                        p.incrementI();
                        break;
                }
            }
        }
    }
    public List<Party> getNpcInVision()
    {
        if (npcParty != null)
        {
            partyVisionIndicator.setVisionRange(npcParty.getVisionRange());
        }
        //Debug.Log(partyVisionIndicator.getPartiesInRange().Count);
        List<Party> result = partyVisionIndicator.getPartiesInRange();
        result.Remove(npcParty);
        return result;
    }

    public virtual void makeParty()
    {

    }
    void lookAtCamera(GameObject gameObj)
    {
        Vector3 v = Camera.main.transform.position - gameObj.transform.position;
        v.x = v.z = 0.0f;
        gameObj.transform.LookAt(Camera.main.transform.position - v);
        gameObj.transform.Rotate(0, 180, 0);
    }
    public void hideSelf ()
    {
        for (int j = 0; j < meshRenderers.Length; j++)
        {

            Material[] newMaterials = (Material[])meshRenderers[j].materials.Clone();
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = invisibleMaterial; //new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);
            }
            meshRenderers[j].materials = newMaterials;
        }
        seen = false;
    }
    public void showSelf()
    {
        for (int j = 0; j < meshRenderers.Length; j++)
        {
            meshRenderers[j].materials = originalMaterials[meshRenderers[j]];
        }
        seen = true;
    }
}
