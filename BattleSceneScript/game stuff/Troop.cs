using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Troop : BattleInteractable
{

    public Animation troopAnimation;
    public GameObject controlPanel, inspectPanel;
    public Person person { get; set; }
    public Grid curGrid { get; set; }
    public Troop lastAttacker;
    public GameObject statusPanel, troopStaminaBar, troopHealthBar, staminaTxt, healthTxt, nameTxt;
    public GameObject placingTroopStaminaBar, placingTroopHealthBar;
    public GameObject visionIndicator, walkIndicator, curIndicator, lungeIndicator, whirlwindIndicator, executeIndicator, fireIndicator, guardIndicator, rainOfArrowIndicator, quickDrawIndicator, curcontrolledIndicator;
    public GameObject seenStatus;
    public bool controlled, aiControlled, charging, holdSteadying, reachedDestination, seen, dead, inMotion;
    public Dictionary<Person, bool> stealthCheckDict = new Dictionary<Person, bool>();
    public bool activated = false;
    public float chargeStack;
    public List<Grid> guardedGrids;
    public Material invisible;
    public AudioSource audioSource;
    public TroopSkill animationTroopSkill;
    public GameObject healthPopUp, staminaPopUp;
    Dictionary<SkinnedMeshRenderer, Material[]> originalMaterials;
    float STATUS_BAR_HEIGHT, STATUS_BAR_WIDTH;
    float PLACING_STATUS_BAR_HEIGHT, PLACING_STATUS_BAR_WIDTH;
    public bool travelCostFree = false;
    bool staminaCosted = false;
    public GameObject model;
    Vector3 tempDest, finalDest, safetyDest;
    public Vector3 lookTarget;
    Grid destinationGrid;
    NavMeshAgent navMeshAgent;
    SkinnedMeshRenderer[] meshRenderers;
    Grid lastGrid;
    // Use this for initialization
    public void Start()
    {
        if (person != null)
        {
            STATUS_BAR_HEIGHT = troopStaminaBar.GetComponent<RawImage>().rectTransform.sizeDelta.y;
            STATUS_BAR_WIDTH = troopStaminaBar.GetComponent<RawImage>().rectTransform.sizeDelta.x;

            healthPopUp.SetActive(false);
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            //if (model != null)
            //{
            //    meshRenderer = model.GetComponentInChildren<MeshRenderer>();
            //}
            meshRenderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
            originalMaterials = new Dictionary<SkinnedMeshRenderer, Material[]>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                originalMaterials.Add(meshRenderers[i], meshRenderers[i].materials);
            }
            audioSource = gameObject.GetComponent<AudioSource>();

            hideIndicators();
            charging = holdSteadying = false;
            guardedGrids = new List<Grid>();
            stealthCheckRefresh();
            controlled = false;
            aiControlled = false;
            seen = false;
            dead = false;
            inMotion = false;
            animationTroopSkill = TroopSkill.none;
        }

    }
    private void OnEnable()
    {
        hideIndicators();
    }
    public void Update()
    {

        if (activated)
        {

            if (troopAnimation == null)
            {
                troopAnimation = transform.GetComponentInChildren<Animation>();
                troopAnimation.CrossFadeQueued("Idle");
                //transform.Find("Model").GetComponent<Animation>();
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            visionUpdate();
            if (lookTarget != null)
            {
                lookAtVector(transform.position + lookTarget, troopAnimation.gameObject);
            }
            if (!troopAnimation.isPlaying)
            {
                stayOnGird();
            }
            if (BattleCentralControl.playerTurn && person.faction == Faction.mercenary)
            {
                if (controlled)
                {
                    walkUpdate();
                    curcontrolledIndicator.SetActive(true);
                }
                else
                {
                    stayOnGird();
                    curcontrolledIndicator.SetActive(false);
                }
            }
            if (!BattleCentralControl.playerTurn && person.faction != Faction.mercenary)
            {
                if (aiControlled)
                {
                    walkUpdate();
                }
                else
                {
                    stayOnGird();
                }
            }
            if ((int)person.health <= 0)
            {
                outOfHealth();
                activated = false;
            }
            showStatus();
            lookAtCamera(statusPanel);

        }
        else
        {
            if (person == null)
            {
                Debug.Log(gameObject.name);
            }
            if (troopAnimation == null)
            {
                Debug.Log(person.name);
            }
            if (!troopAnimation.isPlaying)
            {

                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
                //GameObject.Destroy(gameObject);
            }

        }
    }
    public override void cameraFocusOn()
    {

        if (BattleCentralControl.playerTurn)
        {
            controlPanel.GetComponent<TroopControlPanel>().initializePanel();
            controlPanel.GetComponent<TroopControlPanel>().curControlledTroop = gameObject;

            controlled = true;
        }
        if (person.faction == Faction.mercenary || seen)
        {
            base.cameraFocusOn();
            BattleInspectPanel.person = person;
            GridInspectPanel.grid = null;
            inspectPanel.SetActive(true);
        }
        if (!BattleCentralControl.playerTurn)
        {
            base.cameraFocusOn();
        }
    }
    public override void cameraFocusOnExit()
    {
        if (BattleCentralControl.playerTurn)
        {
            controlPanel.SetActive(false);
            controlled = false;
            BattleInspectPanel.person = null;
            inspectPanel.SetActive(false);
            hideIndicators();
            BattleInteraction.skillMode = TroopSkill.walk;
        }
        base.cameraFocusOnExit();


    }
    public void troopMoveToPlace(Grid grid)
    {
        reachedDestination = false;
        if (person.faction == Faction.mercenary)
        { //player case
            if (grid != null && grid.personOnGrid == null) //
            {
                if (person.stamina > 0)
                {
                    finalDest = new Vector3(grid.x, 1, grid.z);
                    BattleInteraction.inAction = true;
                    //destinationGrid = grid;
                }
                else
                {
                    destinationGrid = getCurrentGrid();
                }
            }
            else
            {
                if (person.stamina > 0)
                {
                    finalDest = new Vector3(grid.x, 1, grid.z);
                    BattleInteraction.inAction = true;
                    //destinationGrid = grid;
                }
                else
                {
                    destinationGrid = getCurrentGrid();
                }
                //goBackToLastGrid();
            }
        }
        else //enemy case
        {
            if (aiControlled)
            {
                if (grid != null && grid.personOnGrid == null)
                {
                    if (person.stamina > 0)
                    {
                        finalDest = new Vector3(grid.x, 1, grid.z);
                        BattleAIControl.inAction = true;
                        //destinationGrid = grid;
                    }
                }
                else
                {
                    //if (person.stamina > 0)
                    //{
                    //    finalDest = new Vector3(grid.x, 1, grid.z);
                    //    BattleAIControl.inAction = true;
                    //    //destinationGrid = grid;
                    //}
                    //goBackToLastGrid();
                }
            }
        }
    }
    public void placed(Person personI, Grid curGridI)
    {
        if (curGridI.personOnGrid == null)
        {
            personI.stamina = personI.getStaminaMax();
            personI.health = personI.health;
            person = personI;
            curGrid = curGridI;
            finalDest = new Vector3(curGrid.x, 1, curGrid.z);
            tempDest = new Vector3(curGrid.x, 1, curGrid.z);
            safetyDest = new Vector3(curGrid.x, 1, curGrid.z);
            reachedDestination = true;
            curGrid.personOnGrid = person;
            activated = true;
            gameObject.SetActive(true);

            if (person.faction == Faction.mercenary)
            {
                BattleCentralControl.playerTroopOnField.Add(person, gameObject);
                lookTarget = new Vector3(0, 0, 1);
            }
            else
            {
                BattleCentralControl.enemyTroopOnField.Add(person, gameObject);
                lookTarget = new Vector3(0, 0, -1);
            }
            person.troop = gameObject.GetComponent<Troop>();
            
            EndTurnPanel.endTurnPanel.updateBattlemeter();
        }
    }

    public Grid getCurrentGrid()
    {
        return BattleCentralControl.map[Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)];
    }

    public void goBackToLastGrid()
    {
        if (lastGrid != null)
        {
            navMeshAgent.destination = new Vector3(lastGrid.x, 1, lastGrid.z);
            charging = false;
            chargeStack = 0;
            //person.stamina += curGrid.staminaCost;
            curGrid = lastGrid;


        }
    }

    bool movedToNewGrid()
    {
        if (curGrid != getCurrentGrid())
        {
            lastGrid = curGrid;
            curGrid = getCurrentGrid();
            return true;
        }
        return false;
    }



    public void hideIndicators()
    {
        walkIndicator.SetActive(false);
        lungeIndicator.SetActive(false);
        whirlwindIndicator.SetActive(false);
        executeIndicator.SetActive(false);
        fireIndicator.SetActive(false);
        guardIndicator.SetActive(false);
        rainOfArrowIndicator.SetActive(false);
        quickDrawIndicator.SetActive(false);
    }

    public void visionUpdate()
    {
        visionIndicator.transform.localScale = new Vector3(person.getVision(), 1, person.getVision());
        if (!visionIndicator.activeSelf)
        {
            visionIndicator.SetActive(true);
        }
    }


    public void walkUpdate()
    {

        //CHECK IF AT FINAL DEST
        float finalDistance = Vector2.Distance(new Vector2(finalDest.x, finalDest.z), new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));
        if (finalDistance <= 0.1f)
        {
            //troopAnimation.Stop();


            reachedDestination = true;
            if (person.faction == Faction.mercenary)
            {
                BattleInteraction.inAction = false;
            }
            else
            {
                aiControlled = false;
                BattleAIControl.inAction = false;
            }
        }

        float distance = Vector2.Distance(new Vector2(tempDest.x, tempDest.z), new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));
        if (distance <= 0.1f) //when we arrive a grid
        {

            if (curGrid != getCurrentGrid()) //make sure new grid
            {
                if (charging)
                {
                    if (chargeStack <= 20)
                    {
                        chargeStack += 5f / curGrid.getStaminaCost(person.faction);
                    }

                    if (getCurrentGrid().personOnGrid != null && getCurrentGrid().personOnGrid.faction != person.faction)
                    {
                        getCurrentGrid().personOnGrid.troop.takingMeleeAttack(this, .7f * person.getMeleeAttackDmg() * chargeStack);
                    }
                    person.stamina -= BattleCentralControl.map[(int)tempDest.x, (int)tempDest.z].getStaminaCost(person.faction) * 1.5f;
                }
                else
                {
                    if (!travelCostFree)
                    {
                        person.stamina -= BattleCentralControl.map[(int)tempDest.x, (int)tempDest.z].getStaminaCost(person.faction);
                    }
                    else if (travelCostFree && getCurrentGrid().x == safetyDest.x && getCurrentGrid().z == safetyDest.z)
                    {
                        travelCostFree = false;
                    }
                }
                //UPDATE CUR
                if (curGrid.personOnGrid == person)
                {
                    curGrid.personOnGrid = null;
                }
                curGrid = getCurrentGrid();
                if (curGrid.personOnGrid == null)
                {
                    curGrid.personOnGrid = person;
                    safetyDest = new Vector3(curGrid.x, 1, curGrid.z);
                }
            }


            //GUIDANCE
            if (getCurrentGrid().x != finalDest.x || getCurrentGrid().z != finalDest.z) //if we are not at dest yet
            {
                float xDist = finalDest.x - getCurrentGrid().x;
                float zDist = finalDest.z - getCurrentGrid().z;
                if (Mathf.Abs(xDist) >= Mathf.Abs(zDist))
                {
                    tempDest.x += Mathf.Clamp(finalDest.x - getCurrentGrid().x, -1, 1);
                }
                else
                {
                    tempDest.z += Mathf.Clamp(finalDest.z - getCurrentGrid().z, -1, 1);
                }
                Grid nextGrid = BattleCentralControl.map[(int)tempDest.x, (int)tempDest.z];
                if ((person.stamina >= nextGrid.getStaminaCost(person.faction) && !charging)
                    || (person.stamina >= nextGrid.getStaminaCost(person.faction) * 2 && charging)) //LEAVING
                {
                    clearGuard();
                    holdSteadying = false;
                    navMeshAgent.destination = tempDest;
                    staminaCosted = false;
                }
                else //if stamina not enough, set final dest as temp dest  //ARRIVED
                {
                    if (getCurrentGrid().personOnGrid == null)
                    {
                        tempDest = new Vector3(getCurrentGrid().x, 1, getCurrentGrid().z);
                        finalDest = new Vector3(getCurrentGrid().x, 1, getCurrentGrid().z);
                    }
                    else
                    {
                        tempDest = safetyDest;
                        finalDest = safetyDest;
                        travelCostFree = true;
                        charging = false;
                    }
                    navMeshAgent.destination = tempDest;
                    //lookAtVector(tempDest);

                }
            }
            else
            { //ARRIVING
                if (getCurrentGrid().personOnGrid != null && getCurrentGrid().personOnGrid != person)
                {
                    finalDest = safetyDest;
                    travelCostFree = true;
                    charging = false;
                }
                else
                {
                    travelCostFree = false;
                    //tempDest = new Vector3(getCurrentGrid().x, 1, getCurrentGrid().z);
                    //finalDest = new Vector3(getCurrentGrid().x, 1, getCurrentGrid().z);
                    //navMeshAgent.destination = tempDest;

                }
            }

        }
        else //TRAVELLING
        {
            if (!charging)
            {
                if (!troopAnimation.isPlaying || troopAnimation.IsPlaying("Idle") || troopAnimation.IsPlaying("IdleCharge") 
                    || troopAnimation.IsPlaying("IdleHoldeSteady") || troopAnimation.IsPlaying("IdleEnGuard"))
                {
                    troopAnimation.Play("Walk");
                }
                playSound(AudioDataBase.database.footstep, 0.0f * Time.timeScale);

            }
            else
            {
                if (!troopAnimation.isPlaying || troopAnimation.IsPlaying("Idle") || troopAnimation.IsPlaying("IdleCharge")
                    || troopAnimation.IsPlaying("IdleHoldeSteady") || troopAnimation.IsPlaying("IdleEnGuard"))
                {
                    troopAnimation.Play("Charge");
                }
                playSound(AudioDataBase.database.horsestep, 0.0f * Time.timeScale);

            }
            if (Vector3.Distance(navMeshAgent.destination, transform.position) <=.5f)
            {
                lookTarget = navMeshAgent.destination - transform.position;
            }
            if (curGrid != getCurrentGrid())
            {
                navMeshAgent.destination = tempDest;
            }
        }
        tempDest = new Vector3(getCurrentGrid().x, 1, getCurrentGrid().z);
    }
    public void walk()
    {
        if (!walkIndicator.activeSelf)
        {
            walkIndicator.SetActive(true);
            curIndicator = walkIndicator;
        }
        walkIndicator.SetActive(followMouse(walkIndicator));
        lookAtCamera(walkIndicator);
    }
    public void lunge()
    {
        if (!lungeIndicator.activeSelf)
        {
            lungeIndicator.SetActive(true);
            lungeIndicator.GetComponent<Indicator>().showDmg((int)person.getMeleeAttackDmg());
            curIndicator = lungeIndicator;
        }
        //lookAtMouse(gameObject.gameObject);
        lookAtMouse(lungeIndicator);
        lookTarget = lungeIndicator.transform.Find("Model").transform.position - transform.position;
        //lookAtMouse(troopAnimation.gameObject);
    }
    public void whirlwind()
    {
        if (!whirlwindIndicator.activeSelf)
        {
            whirlwindIndicator.SetActive(true);
            whirlwindIndicator.GetComponent<Indicator>().showDmg((int)person.getMeleeAttackDmg());
            curIndicator = whirlwindIndicator;
        }
        lookAtMouse(gameObject);
        if (Vector3.Distance(getMousePos(transform.position), transform.position) > .3f)
        {
            lookTarget = getMousePos(transform.position) - transform.position;
        }
    }
    public void execute()
    {
        if (!executeIndicator.activeSelf)
        {
            executeIndicator.SetActive(true);
            curIndicator = executeIndicator;
        }
        lookAtMouse(gameObject);
        if (Vector3.Distance(getMousePos(transform.position), transform.position) > .3f)
        {
            lookTarget = getMousePos(transform.position) - transform.position;
        }
    }
    public void fire()
    {
        if (!fireIndicator.activeSelf)
        {
            fireIndicator.SetActive(true);
            fireIndicator.GetComponent<Indicator>().showDmg((int)person.getRangedAttackDmg());
            curIndicator = fireIndicator;
        }
        lookAtMouse(fireIndicator);
        lookTarget = fireIndicator.transform.Find("Model").transform.position - transform.position;
    }
    public void quickDraw()
    {
        if (!quickDrawIndicator.activeSelf)
        {
            quickDrawIndicator.SetActive(true);
            quickDrawIndicator.GetComponent<Indicator>().showDmg((int)person.getRangedAttackDmg());
            curIndicator = quickDrawIndicator;
        }
        lookAtMouse(quickDrawIndicator);
        lookTarget = quickDrawIndicator.transform.Find("Model").transform.position - transform.position;
    }
    public void holdSteady()
    {
        if (holdSteadying == false)
        {
            charging = false;
        }
        holdSteadying = !holdSteadying;
        if (holdSteadying)
        {
            troopAnimation.Play("IdleHoldSteady");
        }
        else
        {
            troopAnimation.Play("Idle");
        }
        clearGuard();
        stayOnGird();
    }
    public void guard()
    {
        if (!guardIndicator.activeSelf)
        {
            guardIndicator.SetActive(true);
            guardIndicator.GetComponent<Indicator>().showDmg((int)person.getGuardedIncrease());
            curIndicator = guardIndicator;
        }
    }
    public void clearGuard()
    {
        if (guardedGrids.Count > 0)
        {
            troopAnimation.Play("DisEnGuard");

            foreach (Grid g in guardedGrids)
            {
                if (person.faction == Faction.mercenary)
                {
                    g.enemyTempStaminaCost -= person.getGuardStaminaCost();
                    g.gridObject.GetComponent<GridObject>().guardedByPlayer(false, person);
                }
                else
                {
                    g.playerTempStaminaCost -= person.getGuardStaminaCost();
                    g.gridObject.GetComponent<GridObject>().guardedByEnemy(false, person);
                }
            }
            guardedGrids.Clear();
        }
    }
    public void rainOfArrow()
    {
        if (!rainOfArrowIndicator.activeSelf)
        {
            rainOfArrowIndicator.SetActive(true);
            rainOfArrowIndicator.GetComponent<Indicator>().showDmg((int)person.getRangedAttackDmg());
            curIndicator = rainOfArrowIndicator;
        }
        followMouse(rainOfArrowIndicator);
        lookTarget = rainOfArrowIndicator.transform.Find("Model").transform.position - transform.position;
        lookAtMouse(gameObject);
    }
    public void charge()
    {
        if (charging == false)
        {

            holdSteadying = false;
        }
        charging = !charging;
        if (charging)
        {
            troopAnimation.Play("IdleCharge");
        } else
        {
            troopAnimation.Play("Idle");
        }
        chargeStack = 0;
        clearGuard();
        stayOnGird();
    }

    public void changeHealth(float amount, float delay)
    {
        string text = "0";
        if (gameObject.activeSelf)
        {
            StartCoroutine(WaitAndChangeHealth(amount, delay));
        }
        GameObject newPopUp = Object.Instantiate(healthPopUp);
        if (amount > 0)
        {
            text = "+" + ((int)amount).ToString();
            newPopUp.GetComponent<Text>().color = new Color(0, 137, 0); //regen hp
            person.health = Mathf.Clamp(person.health + amount, person.health, person.getHealthMax());
        }
        else
        {
            text = ((int)amount).ToString();
            newPopUp.GetComponent<Text>().color = new Color(191, 0, 0); //decrease hp
        }
        newPopUp.GetComponent<Text>().text = text;

        newPopUp.transform.SetParent(healthPopUp.transform.parent, false);
        newPopUp.SetActive(true);
        Destroy(newPopUp, 1.5f);
    }

    IEnumerator WaitAndChangeHealth(float amount, float waitTime)
    {
        if (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(waitTime);
            person.health += amount;
        }

    }
    public void displayBlock()
    {
        GameObject newPopUp = Object.Instantiate(healthPopUp);
        newPopUp.GetComponent<Text>().text = "Block";
        newPopUp.GetComponent<Text>().color = new Color(0, 200, 200);
        newPopUp.transform.SetParent(healthPopUp.transform.parent, false);
        newPopUp.SetActive(true);
        Destroy(newPopUp, 1.5f);
    }
    public void displayEvade()
    {
        GameObject newPopUp = Object.Instantiate(healthPopUp);
        newPopUp.GetComponent<Text>().text = "Evade";
        newPopUp.GetComponent<Text>().color = new Color(0, 137, 0);
        newPopUp.transform.SetParent(healthPopUp.transform.parent, false);
        newPopUp.SetActive(true);
        Destroy(newPopUp, 1.5f);
    }
    public void displayMissed()
    {
        GameObject newPopUp = Object.Instantiate(healthPopUp);
        newPopUp.GetComponent<Text>().text = "Missed";
        newPopUp.GetComponent<Text>().color = new Color(0, 137, 0);
        newPopUp.transform.SetParent(healthPopUp.transform.parent, false);
        newPopUp.SetActive(true);
        Destroy(newPopUp, 1.5f);
    }

    public void doSkill(List<Grid> attackedGrid, TroopSkill skillMode)
    {
        if (inMotion)
        {
            Debug.Log("in mo");
            return;
        }
        attackedGrid = sortGridByRange(attackedGrid);
        switch (skillMode)
        {
            case TroopSkill.lunge:
                if (person.stamina >= person.getLungeStaminaCost())
                {
                    clearGuard();
                    troopAnimation.Play("Lunge");
                    playSound(AudioDataBase.database.swordHit, 1.25f * Time.timeScale);
                    foreach (Grid g in attackedGrid)
                    {
                        if (g.personOnGrid != null && g.personOnGrid.faction != person.faction)
                        {
                            g.personOnGrid.troop.takingMeleeAttack(this, person.getMeleeAttackDmg());
                        }
                    }
                    person.stamina -= person.getLungeStaminaCost();
                }
                else if (person.faction == Faction.mercenary)
                {
                    BattleInteraction.skillMode = TroopSkill.none;
                }
                break;
            case TroopSkill.whirlwind:
                if (person.stamina >= person.getWhirlwindStaminaCost())
                {
                    clearGuard();
                    troopAnimation.Play("Whirlwind");
                    playSound(AudioDataBase.database.whirlwind, 0.9f * Time.timeScale);
                    foreach (Grid g in attackedGrid)
                    {
                        if (g.personOnGrid != null && g.personOnGrid.faction != person.faction)
                        {
                            g.personOnGrid.troop.takingMeleeAttack(this, person.getMeleeAttackDmg());
                        }
                    }
                    person.stamina -= person.getWhirlwindStaminaCost();
                }
                else if (person.faction == Faction.mercenary)
                {
                    BattleInteraction.skillMode = TroopSkill.none;
                }
                break;
            case TroopSkill.execute:
                if (controlled)
                {
                    if (person.stamina >= person.getExecuteStaminaCost())
                    {
                        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                        {
                            Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit interactionInfo;
                            if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
                            {
                                GameObject interactedObject = interactionInfo.collider.gameObject.transform.parent.gameObject;
                                Troop attackedTroop = interactedObject.GetComponent<Troop>();
                                if (attackedTroop != null && attackedTroop.seen && person.stamina >= person.getExecuteStaminaCost()) //TODO: remove player troop later
                                {
                                    if (attackedTroop.person.faction != person.faction && attackedGrid.Contains(attackedTroop.curGrid))
                                    {
                                        person.stamina -= person.getExecuteStaminaCost();
                                        clearGuard();
                                        troopAnimation.Play("Execute");
                                        playSound(AudioDataBase.database.execute, 1.25f * Time.timeScale);
                                        lookAtVector(attackedTroop.gameObject.transform.position);
                                        float lostPercentage = 1 - (attackedTroop.person.health / attackedTroop.person.getHealthMax());
                                        attackedTroop.takingMeleeAttack(this, (lostPercentage * 10 + 1) * person.getMeleeAttackDmg());
                                    }
                                }
                                else if (person.faction == Faction.mercenary)
                                {
                                    BattleInteraction.skillMode = TroopSkill.none;
                                }
                            }
                        }
                    }
                    else
                    {
                        BattleInteraction.skillMode = TroopSkill.none;
                    }
                }
                else if (aiControlled)
                {
                    if (person.stamina >= person.getExecuteStaminaCost())
                    {
                        float leastHp = Mathf.Infinity;
                        Person attackedPerson = null;
                        foreach (Grid g in attackedGrid)
                        {
                            if (g.personOnGrid != null && g.personOnGrid.faction != person.faction)
                            {
                                if (leastHp > g.personOnGrid.health)
                                {
                                    leastHp = g.personOnGrid.health;
                                    attackedPerson = g.personOnGrid;
                                }
                            }
                        }
                        if (attackedPerson != null)
                        {
                            clearGuard();
                            troopAnimation.Play("Execute");
                            playSound(AudioDataBase.database.execute, 1.25f * Time.timeScale);
                            lookAtVector(attackedPerson.troop.gameObject.transform.position);
                            float lostPercentage = 1 - (attackedPerson.health / attackedPerson.getHealthMax());
                            attackedPerson.troop.takingMeleeAttack(this, (lostPercentage * 10 + 1) * person.getMeleeAttackDmg());
                            person.stamina -= person.getExecuteStaminaCost();
                        }
                    }


                }

                break;
            case TroopSkill.rainOfArrows:
                if (person.stamina >= person.getRainOfArrowsStaminaCost())
                {
                    clearGuard();
                    troopAnimation.Play("RainOfArrows");
                    playSound(AudioDataBase.database.arrowFire, 1.75f * Time.timeScale);

                    foreach (Grid g in attackedGrid)
                    {
                        if (g.personOnGrid != null)
                        {
                            g.personOnGrid.troop.takingRangedAttack(this, person.getRangedAttackDmg());
                        }
                    }
                    person.stamina -= person.getRainOfArrowsStaminaCost();
                }
                else if (person.faction == Faction.mercenary)
                {
                    BattleInteraction.skillMode = TroopSkill.none;
                }
                break;
            case TroopSkill.charge:
                break;
            case TroopSkill.holdSteady:
                break;
            case TroopSkill.fire:
                if (holdSteadying)
                {
                    if (person.stamina >= person.getFireStaminaCost() + person.getHoldSteadyStaminaCost())
                    {
                        clearGuard();
                        troopAnimation.Play("Fire");
                        playSound(AudioDataBase.database.gunShot, 1.5f * Time.timeScale);
                        foreach (Grid g in attackedGrid)
                        {
                            if (g.personOnGrid != null && g.personOnGrid != person)
                            {
                                g.personOnGrid.troop.takingRangedAttack(this, person.getRangedAttackDmg());
                            }
                        }
                        person.stamina -= (person.getFireStaminaCost() + person.getHoldSteadyStaminaCost());
                    }
                    else if (person.faction == Faction.mercenary)
                    {
                        BattleInteraction.skillMode = TroopSkill.none;
                    }
                }
                else
                {
                    if (person.stamina >= person.getFireStaminaCost())
                    {
                        clearGuard();
                        troopAnimation.Play("Fire");
                        playSound(AudioDataBase.database.gunShot, 1.5f * Time.timeScale);
                        if (attackedGrid.Count == 0)
                        {
                            Debug.Log("attack grid 0");
                        }
                        foreach (Grid g in attackedGrid)
                        {
                            if (g.personOnGrid != null && g.personOnGrid != person)
                            {
                                g.personOnGrid.troop.takingRangedAttack(this, person.getRangedAttackDmg());
                            }
                        }
                        person.stamina -= person.getFireStaminaCost();
                    }
                    else if (person.faction == Faction.mercenary)
                    {
                        BattleInteraction.skillMode = TroopSkill.none;
                    }
                }
                break;
            case TroopSkill.guard:
                if (person.stamina >= person.getGuardStaminaCost())
                {
                    troopAnimation.Play("EnGuard");
                    playSound(AudioDataBase.database.enguard, 1.0f);
                    holdSteadying = false;
                    charging = false;
                    clearGuard();
                    foreach (Grid g in attackedGrid)
                    {
                        if (person.faction == Faction.mercenary)
                        {
                            g.gridObject.GetComponent<GridObject>().guardedByPlayer(true, person);
                        }
                        else
                        {
                            g.gridObject.GetComponent<GridObject>().guardedByEnemy(true, person);
                        }
                        guardedGrids.Add(g);
                    }
                    person.stamina -= person.getGuardStaminaCost();
                }
                else if (person.faction == Faction.mercenary)
                {
                    BattleInteraction.skillMode = TroopSkill.none;
                }
                break;
            case TroopSkill.quickDraw:
                if (person.stamina >= person.getQuickDrawStaminaCost())
                {
                    clearGuard();
                    troopAnimation.Play("QuickDraw");
                    playSound(AudioDataBase.database.arrowFire, 1.25f * Time.timeScale);

                    bool blocked = false;
                    foreach (Grid g in attackedGrid)
                    {
                        if (g.personOnGrid != null && !blocked && g.personOnGrid != person)
                        {
                            g.personOnGrid.troop.takingRangedAttack(this, person.getRangedAttackDmg());
                            blocked = true;
                        }
                    }
                    person.stamina -= person.getQuickDrawStaminaCost();
                }
                else if (person.faction == Faction.mercenary)
                {
                    BattleInteraction.skillMode = TroopSkill.none;
                }
                break;

        }
        inMotion = true;
    }


    public void takingMeleeAttack(Troop attacker, float amount)
    {
        if (attacker.person.ranking == Ranking.mainChar && attacker.person.troopType == TroopType.mainCharType)
        {

        }
        int rand = Random.Range(1, 100);
        if (rand <= person.getEvasion())
        {
            evading();
            displayEvade();
        }
        else
        {
            rand = Random.Range(1, 100);
            if (rand <= person.getBlock())
            {
                blocking();
                displayBlock();
            }
            else
            {
                lastAttacker = attacker;
                troopAnimation.Play("TakingDmg");
                playSound(AudioDataBase.database.hurt, 1.25f * Time.timeScale);
                changeHealth(-amount, 1.25f);
            }
        }

    }

    public void takingRangedAttack(Troop attacker, float amount)
    {
        if (attacker.person.ranking == Ranking.mainChar && attacker.person.troopType == TroopType.mainCharType)
        {

        }
        int rand = Random.Range(1, 150);
        if (rand <= person.getEvasion())
        {
            evading();
            displayEvade();
        }
        else
        {
            rand = Random.Range(1, 150);
            if (rand <= person.getBlock())
            {
                blocking();
                displayBlock();
            }
            else
            {
                rand = Random.Range(1, 200);
                if (attacker.person.getAccuracy() * 1.5f > rand)
                {
                    lastAttacker = attacker;
                    troopAnimation.Play("TakingDmg");
                    playSound(AudioDataBase.database.hurt, 1.25f * Time.timeScale);
                    changeHealth(-amount, 1.25f);
                }
                else
                {
                    displayMissed();
                }

            }
        }

    }

    public void blocking()
    {
        troopAnimation.Play("Block");
        playSound(AudioDataBase.database.block, 1.25f * Time.timeScale);
    }

    public void evading()
    {
        troopAnimation.Play("Evade");
        playSound(AudioDataBase.database.evade, 1.25f * Time.timeScale);
    }

    public void outOfHealth()
    {
        curGrid.personOnGrid = null;
        troopAnimation.Play("Death");
        playSound(AudioDataBase.database.death, 1.5f * Time.timeScale);
        if (lastAttacker != null)
        {
            lastAttacker.person.increaseExp(person.getExp());
        }
        if (Mathf.Abs(person.health) > person.getInjuredHealth() && person.ranking != Ranking.mainChar) //DEATH
        {
            if (person.faction == Faction.mercenary)
            {
                if (BattleCentralControl.playerParty.partyMember.Contains(person))
                {
                    BattleCentralControl.playerParty.partyMember.Remove(person);
                    BattleCentralControl.deadPlayer.Add(person);
                }
            }
            else
            {
                if (BattleCentralControl.enemyParty.partyMember.Contains(person))
                {
                    BattleCentralControl.enemyParty.partyMember.Remove(person);
                    BattleCentralControl.deadEnemy.Add(person);
                }
            }
        }
        else //INJURED
        {
            person.health = 0;
        }
        if (person.faction == Faction.mercenary)
        {
            if (BattleCentralControl.playerTroopOnField.ContainsKey(person))
            {
                BattleCentralControl.playerTroopOnField.Remove(person);
                TroopPlacing.troopPlacing.removePlacingButton(person);
            }
            BattleCentralControl.enemyParty.expToDistribute += person.getExp();
            BattleAIControl.shouldRethink = true;
        }
        else
        {
            if (BattleCentralControl.enemyTroopOnField.ContainsKey(person))
            {
                BattleCentralControl.enemyTroopOnField.Remove(person);
            }
            BattleCentralControl.playerParty.expToDistribute += person.getExp();
        }
        activated = false;
        curGrid.personOnGrid = null;
        EndTurnPanel.endTurnPanel.updateBattlemeter();

    }
    public void stealthCheckRefresh()
    {
        stealthCheckDict.Clear();
        if (person.faction == Faction.mercenary)
        {
            foreach (Person p in BattleCentralControl.enemyParty.partyMember)
            {
                stealthCheckDict.Add(p, false);
            }
        }
        else
        {
            foreach (Person p in BattleCentralControl.playerParty.partyMember)
            {
                stealthCheckDict.Add(p, false);
            }
        }

        hidden();

    }
    public void stealthCheck(Troop watcher)
    {
        if (stealthCheckDict.ContainsKey(watcher.person) && !stealthCheckDict[watcher.person])
        {
            float distance = Vector3.Distance(watcher.gameObject.transform.position, transform.position);
            float rand = Random.Range(distance, watcher.person.getVision() + person.getStealth());
            if (rand < watcher.person.getVision())
            {
                hidden();
            }
            else
            {
                revealed();
            }
            //revealed();  //revealed when enter others' vision
            stealthCheckDict[watcher.person] = true;
        }
    }
    public void hidden()
    {
        seen = false;
        if (person.faction == Faction.mercenary)
        {
            if (seenStatus.activeSelf)
            {
                seenStatus.SetActive(false);
            }
            //for (int j = 0; j < meshRenderers.Length; j++)
            //{
            //
            //    Material[] newMaterials = (Material[])meshRenderers[j].materials.Clone();
            //    for (int i = 0; i < newMaterials.Length; i++)
            //    {
            //        newMaterials[i].color = new Color(meshRenderers[j].materials[i].color.r, meshRenderers[j].materials[i].color.g, meshRenderers[j].materials[i].color.b, .5f);
            //    }
            //    meshRenderers[j].materials = newMaterials;
            //}
        }
        else
        {
            statusPanel.SetActive(false);
            for (int j = 0; j < meshRenderers.Length; j++)
            {

                Material[] newMaterials = (Material[])meshRenderers[j].materials.Clone();
                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = invisible; //new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);
                }
                meshRenderers[j].materials = newMaterials;
            }

        }
    }
    public void revealed()
    {
        seen = true;
        if (person.faction == Faction.mercenary)
        {
            if (!seenStatus.activeSelf)
            {
                seenStatus.SetActive(true);
            }
            //for (int j = 0; j < meshRenderers.Length; j++)
            //{
            //    meshRenderers[j].materials = originalMaterials[meshRenderers[j]];
            //}
            //meshRenderer.materials = originalMaterials;
        }
        else
        {
            statusPanel.SetActive(true);
            for (int j = 0; j < meshRenderers.Length; j++)
            {
                meshRenderers[j].materials = originalMaterials[meshRenderers[j]];
            }

        }
    }


    public List<Grid> indicatedGrid()
    {
        List<Grid> result = new List<Grid>();
        if (curIndicator != null)
        {
            result = curIndicator.GetComponent<Indicator>().collided;
        }
        return result;
    }
    void stayOnGird()
    {
        if (curGrid != null)
        {
            Vector3 pos = new Vector3(getCurrentGrid().x, transform.position.y, getCurrentGrid().z);
            curGrid = getCurrentGrid();
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * 1000);

            if (!troopAnimation.isPlaying && !charging && !holdSteadying)
            {

            }
            if (controlled)
            {
                if (charging)
                {
                    troopAnimation.CrossFadeQueued("IdleCharge");
                }
                else if (holdSteadying)
                {
                    troopAnimation.CrossFadeQueued("IdleHoldSteady");
                }
                else if (guardedGrids.Count > 0)
                {
                    troopAnimation.CrossFadeQueued("IdleEnGuard");
                }
                else
                {
                    troopAnimation.CrossFadeQueued("Idle");
                }
                inMotion = false;
            }
            else
            {
                if (!troopAnimation.isPlaying)
                {
                    if (charging)
                    {
                        troopAnimation.CrossFadeQueued("IdleCharge");
                    }
                    else if (holdSteadying)
                    {
                        troopAnimation.CrossFadeQueued("IdleHoldSteady");
                    }
                    else if (guardedGrids.Count > 0)
                    {
                        troopAnimation.CrossFadeQueued("IdleEnGuard");
                    }
                    else
                    {
                        troopAnimation.CrossFadeQueued("Idle");
                    }
                    inMotion = false;
                }

            }

        }
    }
    Vector3 goNearbyGrid(Grid g)
    {
        int randX = (int)Mathf.Clamp(Random.Range(-1, 2), -1, 1);
        int randZ = (int)Mathf.Clamp(Random.Range(-1, 2), -1, 1);
        while (randX == 0 && randZ == 0)
        {
            randX = (int)Mathf.Clamp(Random.Range(-1, 2), -1, 1);
            randZ = (int)Mathf.Clamp(Random.Range(-1, 2), -1, 1);
        }
        Vector3 pos = new Vector3(curGrid.x + randX, transform.position.y, curGrid.z + randZ);
        navMeshAgent.destination = pos;
        charging = false;
        chargeStack = 0;
        travelCostFree = true;
        destinationGrid = BattleCentralControl.map[curGrid.x + randX, curGrid.z + randZ];
        return pos;
    }
    public void importPlacingBarSetting(GameObject healthBar, GameObject staminaBar)
    {
        placingTroopHealthBar = healthBar;
        placingTroopStaminaBar = staminaBar;
        if (placingTroopHealthBar != null)
        {
            PLACING_STATUS_BAR_HEIGHT = placingTroopStaminaBar.GetComponent<RawImage>().rectTransform.sizeDelta.y;
            PLACING_STATUS_BAR_WIDTH = placingTroopStaminaBar.GetComponent<RawImage>().rectTransform.sizeDelta.x;
        }
    }
    void showStatus()
    {

        if (person != null)
        {
            //Debug.Log("stamina and max: " + person.health + " " + person.getHealthMax());
            troopHealthBar.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATUS_BAR_WIDTH * (person.health / person.getHealthMax()), 0, STATUS_BAR_WIDTH), STATUS_BAR_HEIGHT);
            troopStaminaBar.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(Mathf.Clamp(STATUS_BAR_WIDTH * (person.stamina / person.getStaminaMax()), 0, STATUS_BAR_WIDTH), STATUS_BAR_HEIGHT);
            staminaTxt.GetComponent<Text>().text = ((int)person.stamina).ToString();
            healthTxt.GetComponent<Text>().text = ((int)person.health).ToString();
            nameTxt.GetComponent<Text>().text = person.name + "(" + person.exp.level + ")";
            if (placingTroopHealthBar != null)
            {
                placingTroopHealthBar.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(PLACING_STATUS_BAR_WIDTH, Mathf.Clamp(PLACING_STATUS_BAR_HEIGHT * (person.health / person.getHealthMax()), 0, PLACING_STATUS_BAR_HEIGHT));
                placingTroopStaminaBar.GetComponent<RawImage>().rectTransform.sizeDelta = new Vector2(PLACING_STATUS_BAR_WIDTH, Mathf.Clamp(PLACING_STATUS_BAR_HEIGHT * (person.stamina / person.getStaminaMax()), 0, PLACING_STATUS_BAR_HEIGHT));
            }
        }
    }

    void lookAtVector(Vector3 pos)
    {
        Vector3 v = pos - gameObject.transform.position;
        v.x = v.z = 0.0f;
        gameObject.transform.LookAt(pos - v);
        gameObject.transform.Rotate(0, 180, 0);
    }
    void lookAtVector(Vector3 pos, GameObject obj)
    {
        Vector3 v = pos - gameObject.transform.position;
        v.x = v.z = 0.0f;
        obj.transform.LookAt(pos - v);
        obj.transform.Rotate(0, 180, 0);
    }
    void lookAtCamera(GameObject obj)
    {
        Vector3 v = Camera.main.transform.position - obj.transform.position;
        v.x = v.z = 0.0f;
        obj.transform.LookAt(Camera.main.transform.position - v);
        obj.transform.Rotate(0, 180, 0);
    }

    void lookAtMouse(GameObject obj)
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject pointedObj = interactionInfo.collider.gameObject.transform.parent.gameObject;
            if (pointedObj.tag == "Grid" || pointedObj.tag == "Troop")
            {
                Vector3 v = pointedObj.transform.position - obj.transform.position;
                v.x = v.z = 0.0f;
                obj.transform.LookAt(pointedObj.transform.position - v);
                obj.transform.Rotate(0, 180, 0);
            }
            else if (pointedObj.tag == "DecoTerrain")
            {
                Vector3 v = interactionInfo.point - obj.transform.position;
                v.x = v.z = 0.0f;
                obj.transform.LookAt(pointedObj.transform.position - v);
                obj.transform.Rotate(0, 180, 0);

            }
        }
    }
    bool followMouse(GameObject obj)
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject pointedObj = interactionInfo.collider.gameObject.transform.parent.gameObject;
            if (pointedObj.tag == "Grid")
            {
                if (pointedObj.GetComponentInChildren<GridObject>().grid != curGrid)
                {
                    Vector3 pos = new Vector3(pointedObj.transform.position.x, 0, pointedObj.transform.position.z);
                    obj.transform.position = Vector3.Slerp(obj.transform.position, pos, Time.deltaTime * 1000);
                }

            }
            if (pointedObj.tag == "Troop" && pointedObj.GetComponentInChildren<Troop>() != null)
            {
                if (pointedObj.GetComponentInChildren<Troop>().person != person)
                {
                    Vector3 pos = new Vector3(pointedObj.transform.position.x, 0, pointedObj.transform.position.z);
                    obj.transform.position = Vector3.Slerp(obj.transform.position, pos, Time.deltaTime * 1000);
                }

            }

            return true;
        }
        else
        {
            return false;
        }
    }
    Vector3 getMousePos(Vector3 originalPoint)
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject pointedObj = interactionInfo.collider.gameObject.transform.parent.gameObject;
            if (pointedObj.tag == "Grid" || pointedObj.tag == "Troop")
            {
                return new Vector3(pointedObj.transform.position.x, originalPoint.y, pointedObj.transform.position.z);
            }
        }
        return originalPoint;
    }
    List<Grid> sortGridByRange(List<Grid> gridL)
    {
        if (gridL.Count > 0)
        {
            float smallestDistance, comparingDistance;
            int smallestIndex = 0;
            Grid temp;
            for (int i = 0; i < gridL.Count - 1; i++)
            {
                smallestDistance = Vector2.Distance(new Vector2(curGrid.x, curGrid.z), new Vector2(gridL[i].x, gridL[i].z));
                smallestIndex = i;
                for (int j = i + 1; j < gridL.Count; j++)
                {
                    comparingDistance = Vector2.Distance(new Vector2(curGrid.x, curGrid.z), new Vector2(gridL[j].x, gridL[j].z));
                    if (smallestDistance >= comparingDistance)
                    {
                        smallestIndex = j;
                        smallestDistance = comparingDistance;
                    }
                }
                temp = gridL[i];
                gridL[i] = gridL[smallestIndex];
                gridL[smallestIndex] = temp;
            }
        }
        return gridL;

    }
    public void playSound(AudioClip input, float delay)
    {
        if (audioSource != null)
        {
            audioSource.clip = input;
            audioSource.PlayDelayed((ulong)delay);
        }
    }
}

