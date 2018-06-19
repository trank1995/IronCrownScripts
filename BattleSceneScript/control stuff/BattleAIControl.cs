using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAIControl : MonoBehaviour {

    public static bool inAction = false; //if any troop is doing action
    public GameObject enemyBaseCharacter; //the character object we are going to place
    public bool enemyPlaced;
    public static bool enemyFinished; //if ai finsh all moves this turn
    public Troop curControlled; //the data of current controlled character
    public Vector2 mapSize, troopCenter; //troop center is the center of whole party formation
    public int frontLine, midLine, backLine;
    public AIAttackMode attackMode; //ai behaviors: aggressive, neutral, cautious
    //vars for waiting
    float timer;
    bool waited = false;
    bool tick = false;
    bool turnInitialized = false;

    bool scouted = false;
    bool probed = false;
    bool madeAttack = false;
    //the queue of troop actions
    Queue<AIAction> actionQueue = new Queue<AIAction>(); 
    //lists so we can sort troop types
    List<Person> halberdiers, swordsmen, cavalries, crossbowmen, musketeer;
    //record what grids are going to be occupied so we dont make two person standing on 1 grid
    public static List<Grid> futureGrids;

    public static AIAction curAIAction;
    public static bool skippThisAction;
    public static bool shouldRethink;
    // Use this for initialization
    void Start() {
        enemyPlaced = false;
        frontLine = 0;
        midLine = 0;
        backLine = 0;
        enemyFinished = false;
        attackMode = AIAttackMode.neutral;
        halberdiers = new List<Person>();
        swordsmen = new List<Person>();
        cavalries = new List<Person>();
        crossbowmen = new List<Person>();
        musketeer = new List<Person>();
        futureGrids = new List<Grid>();
        skippThisAction = false;
        shouldRethink = false;
    }

    // Update is called once per frame
    void Update() {
        if (!enemyPlaced && BattleCentralControl.battleStart) //place troop when battle start
        {
            mapSize = new Vector2(BattleCentralControl.gridXMax, BattleCentralControl.gridZMax);
            decideAttackMode();
            categorizeTroop();
            placeEnemyOnMap();
            enemyPlaced = true;
        } //occur on battle start



        if (!BattleCentralControl.playerTurn) // if it's ai turn
        {
            if (enemyFinished) //ai has done its job, return controll to player
            {
                BattleCentralControl.endTurnPrep();
                enemyFinished = false;
                curAIAction = null;
                turnInitialized = false;
                scouted = false;
                madeAttack = false;
                BattleCentralControl.playerTurn = true;
                BattleCentralControl.startTurnPrep();
                return;
            } else  //ai doing its job here
            {
                if (!turnInitialized)
                {
                    categorizeTroop();
                    actionQueue.Clear();
                    turnInitialized = true;
                }
                aiControl();
                doAction();
            }

        } //occur on player finish their moves


    }
    /** this function will place enemy characters on map based on their troop type
     * 
     */
    void placeEnemyOnMap()
    {
        int randX = (int)Random.Range(0, BattleCentralControl.gridXMax);
        troopCenter = new Vector2(randX, mapSize.y - ((int)BattleCentralControl.enemyParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax) / 2) - 1);
        int memberInBattle = 0;
        frontLine = (int)(mapSize.y - BattleCentralControl.enemyParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax));
        midLine = (int)(troopCenter.y);
        backLine = (int)(mapSize.y - 1);
        memberInBattle += 1;

        switch (BattleCentralControl.enemyParty.leader.troopType) {
            case TroopType.cavalry:
                placeTroop((int)troopCenter.x, midLine, BattleCentralControl.enemyParty.leader);
                break;
            case TroopType.swordsman:
                placeTroop((int)troopCenter.x, midLine, BattleCentralControl.enemyParty.leader);
                break;
            case TroopType.recruitType:
                placeTroop((int)troopCenter.x, midLine, BattleCentralControl.enemyParty.leader);
                break;
            case TroopType.halberdier:
                placeTroop((int)troopCenter.x, frontLine, BattleCentralControl.enemyParty.leader);
                break;
            case TroopType.musketeer:
                placeTroop((int)troopCenter.x, backLine, BattleCentralControl.enemyParty.leader);
                break;
            case TroopType.crossbowman:
                placeTroop((int)troopCenter.x, backLine, BattleCentralControl.enemyParty.leader);
                break;
        }
        int frontLineNum = halberdiers.Count;
        int midLineNum = cavalries.Count + swordsmen.Count;
        int backLineNum = crossbowmen.Count + musketeer.Count;
        //Debug.Log(frontLine + "||" + midLine + "||" + backLine + " ||| " + mapSize.y);
        int index = -((int)frontLineNum / 2);
        foreach (Person unit in halberdiers)
        {
            int posX = (int)Mathf.Clamp(troopCenter.x + index * getOffsetX(frontLineNum), 0, mapSize.x - 1);
            while (BattleCentralControl.map[posX, frontLine].personOnGrid != null)
            {
                posX++;
                if (posX > mapSize.x - 1)
                {
                    posX = 0;
                }
            }
            if (memberInBattle < BattleCentralControl.enemyParty.leader.getTroopMaxNum() && unit.troop == null)
            {
                placeTroop(posX, frontLine, unit);
                memberInBattle++;
            }
            index++;
        }
        index = -((int)backLineNum / 2);
        foreach (Person unit in crossbowmen)
        {
            int posX = (int)Mathf.Clamp(troopCenter.x + index * getOffsetX(backLineNum), 0, mapSize.x - 1);
            while (BattleCentralControl.map[posX, backLine].personOnGrid != null)
            {
                posX++;
                if (posX > mapSize.x - 1)
                {
                    posX = 0;
                }
            }
            if (memberInBattle < BattleCentralControl.enemyParty.leader.getTroopMaxNum() && unit.troop == null)
            {
                placeTroop(posX, backLine, unit);
                memberInBattle++;
            }
            index++;
        }
        index = -((int)backLineNum / 2);
        foreach (Person unit in musketeer)
        {
            int posX = (int)Mathf.Clamp(troopCenter.x + index * getOffsetX(backLineNum), 0, mapSize.x - 1);
            while (BattleCentralControl.map[posX, backLine].personOnGrid != null)
            {
                posX++;
                if (posX > mapSize.x - 1)
                {
                    posX = 0;
                }
            }
            if (memberInBattle < BattleCentralControl.enemyParty.leader.getTroopMaxNum() && unit.troop == null)
            {
                placeTroop(posX, backLine, unit);
                memberInBattle++;
            }
            index++;
        }
        //MIDLINE
        midLineNum = Mathf.Clamp(midLineNum, midLineNum, BattleCentralControl.enemyParty.leader.getTroopMaxNum() - memberInBattle);
        index = -((int)midLineNum / 2);
        foreach (Person unit in cavalries)
        {
            int posX = (int)Mathf.Clamp(troopCenter.x + index * getOffsetX(midLineNum), 0, mapSize.x - 1);
            while (BattleCentralControl.map[posX, midLine].personOnGrid != null)
            {
                posX++;
                if (posX > mapSize.x - 1)
                {
                    posX = 0;
                }
            }
            if (memberInBattle < BattleCentralControl.enemyParty.leader.getTroopMaxNum() && unit.troop == null)
            {
                placeTroop(posX, midLine, unit);
                memberInBattle++;
            }
            index++;
        }
        foreach (Person unit in swordsmen)
        {
            int posX = (int)Mathf.Clamp(troopCenter.x + index * getOffsetX(midLineNum), 0, mapSize.x - 1);
            while (BattleCentralControl.map[posX, midLine].personOnGrid != null)
            {
                posX++;
                if (posX > mapSize.x - 1)
                {
                    posX = 0;
                }
            }
            if (memberInBattle < BattleCentralControl.enemyParty.leader.getTroopMaxNum()
                 && unit.troopType == TroopType.swordsman && unit.troop == null)
            {
                placeTroop(posX, midLine, unit);
                memberInBattle++;
            }
            index++;
        }
        foreach (Person unit in swordsmen)
        {
            int posX = (int)Mathf.Clamp(troopCenter.x + index * getOffsetX(midLineNum), 0, mapSize.x - 1);
            while (BattleCentralControl.map[posX, midLine].personOnGrid != null)
            {
                posX++;
                if (posX > mapSize.x - 1)
                {
                    posX = 0;
                }
            }
            if (memberInBattle < BattleCentralControl.enemyParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax)
                && unit.troopType == TroopType.recruitType && unit.troop != null)
            {
                placeTroop(posX, midLine, unit);
                memberInBattle++;
            }
            index++;
        }

        BattleCentralControl.enemyTotal = BattleCentralControl.enemyParty.partyMember.Count;

    }
    /**
     * this function made ai actions queue, aka thinking
     */
    void aiControl()
    {

        if (getSeen().Count == 0)
        {
            if (!scouted)
            {
                decideLinesBlind();
                if (!probed)
                {
                    probeAttack();
                }
                forwardToLines();
                guard();
                scouted = true;
            }
        }
        else
        {
            if (!madeAttack)
            {
                actionQueue.Clear();
                decideLinesSeen();
                backLineEngage();
                midLineAttack();
                frontLineAttack();
                
                madeAttack = true;
            }

            //Debug.Log(getSeen().Count);
        }
        actionQueue.Enqueue(new AIAction(BattleCentralControl.enemyParty.leader.troop, TroopSkill.none));
        shouldRethink = false;
    }
    /**
     * executre aiActions 
     */
    void doAction()
    {
        if (actionQueue.Count != 0 && !inAction)
        {
            
            if (curAIAction == null) //read first action
            {
                curAIAction = actionQueue.Dequeue();
            }
            else if (curAIAction != null && curAIAction.finished && !curAIAction.troop.inMotion) //read next action only if finished the last one
            {
                curAIAction.troop.aiControlled = false;
                curAIAction = actionQueue.Dequeue();
            }

        }
        if (!curAIAction.troop.inMotion)
        {
            if (!inAction && curAIAction != null && !curAIAction.finished) //if we have an action to do
            {
                if (curAIAction.skillMode == TroopSkill.none)
                {
                    enemyFinished = true;
                    actionQueue.Clear();
                }
                curAIAction.doAIAction(); //do the first half (placing hitbox and etc)
                //Debug.Log(curAIAction.troop.person.name + "||" + curAIAction.skillMode + "||" + curAIAction.troop.charging);
                if (curAIAction.troop.activated)
                {
                    curAIAction.troop.cameraFocusOn();
                }
                if (!waited) //reset clock
                {
                    timer = 0;
                    tick = false;
                    waited = true;
                }
                // wait for a bit
                clockTick();
                if (tick)
                {
                    curAIAction.finishAIAction();
                    waited = false;
                    curAIAction.finished = true;
                    curAIAction.troop.cameraFocusOnExit();
                }
                if (skippThisAction)
                {
                    waited = false;
                    curAIAction.finished = true;
                    skippThisAction = false;
                    curAIAction.troop.cameraFocusOnExit();
                }

            }
            else if (shouldRethink)
            {
                actionQueue.Clear();
                aiControl();
            }
        }
        
    }
    
    void frontLineAttack()
    {
        halberdierEngage();
    }
    void midLineAttack()
    {
        cavalriesEngage();
        swordsmenEngage();
    }
    void backLineEngage()
    {
        musketeerEngage();
        crossbowmenEngage();
    }
    /**
     * this function keeps the formation
     */
    void forwardToLines()
    {
        foreach (Person unit in cavalries)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, midLine], Direction.negZ)));
            }
        }
        foreach (Person unit in swordsmen)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, midLine], Direction.negZ)));
            }
        }
        foreach (Person unit in halberdiers)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, frontLine], Direction.negZ)));
            }
        }
        foreach (Person unit in musketeer)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, backLine], Direction.negZ)));
            }
        }
        foreach (Person unit in crossbowmen)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, backLine], Direction.negZ)));
            }
        }

    }
    void probeAttack()
    {
        foreach (Person unit in musketeer)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, backLine], Direction.negZ)));
            }
        }
        foreach (Person unit in crossbowmen)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, backLine], Direction.negZ)));
            }
        }
        foreach (Person unit in musketeer)
        {
            if (unit.troop != null)
            {
                float staminaLeft = unit.stamina * .7f;
                while (staminaLeft >= 0)
                {
                    int assumedZ = (int)Random.Range(2, BattleCentralControl.playerParty.leader.getTroopPlacingRange((int)mapSize.y));
                    int assumedX = (int)Random.Range(unit.troop.curGrid.x - 5, unit.troop.curGrid.x + 5);
                    assumedX = Mathf.Clamp(assumedX, 0, (int)mapSize.x - 1);
                    assumedZ = Mathf.Clamp(assumedZ, 0, (int)mapSize.y - 1);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.fire, BattleCentralControl.map[assumedX, assumedZ].gridObject));
                    staminaLeft -= unit.getFireStaminaCost();
                }
            }
        }
        foreach (Person unit in crossbowmen)
        {
            if (unit.troop != null)
            {
                float staminaLeft = unit.stamina * .7f;
                while (staminaLeft >= 0)
                {
                    int assumedZ = (int)Random.Range(2, BattleCentralControl.playerParty.leader.getTroopPlacingRange((int)mapSize.y));
                    int assumedX = (int)Random.Range(unit.troop.curGrid.x - 5, unit.troop.curGrid.x + 5);
                    assumedX = Mathf.Clamp(assumedX, 0, (int)mapSize.x - 1);
                    assumedZ = Mathf.Clamp(assumedZ, 0, (int)mapSize.y - 1);
                    float decider = Random.Range(-1, 1);
                    if (decider >= 0)
                    {
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.quickDraw, BattleCentralControl.map[assumedX, assumedZ].gridObject));
                        staminaLeft -= unit.getQuickDrawStaminaCost();
                    } else
                    {
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.rainOfArrows, BattleCentralControl.map[assumedX, assumedZ].gridObject));
                        staminaLeft -= unit.getRainOfArrowsStaminaCost();
                    }
                }
            }
        }
    }


    void swordsmenEngage()
    {
        foreach (Person unit in swordsmen)
        {
            if (unit.troop != null)
            {
                //Debug.Log(unit.name);
                Troop target = getNearest(unit.troop);

                Grid futureGrid = findEmptyBlockGrid(BattleCentralControl.map[target.curGrid.x, target.curGrid.z], 2);
                if (futureGrid != null)
                {
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                    futureGrids.Add(futureGrid);

                    float staminaLeft = unit.stamina;
                    float distance = getDistance(futureGrid, target.curGrid);
                    while (staminaLeft >= 0)
                    {
                        if (distance < 2)
                        {
                            if (getNearbyTroop(unit.troop, 1).Count > 1)
                            {
                                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.whirlwind, target.gameObject));
                                staminaLeft -= unit.getWhirlwindStaminaCost();
                            }
                            else
                            {
                                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.execute));
                                staminaLeft -= unit.getExecuteStaminaCost();
                            }
                        }
                        else if (distance >= 2 && distance <= 3)
                        {
                            actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.lunge, target.gameObject));
                            staminaLeft -= unit.getLungeStaminaCost();
                        }
                        else
                        {
                            Debug.Log("not attacking" + distance);
                        }
                    }
                    
                }


            }

        }
    }
    void cavalriesEngage()
    {
        foreach (Person unit in cavalries)
        {
            if (unit.troop != null)
            {
                Troop target = getNearest(unit.troop);
                //if (attackMode == AIAttackMode.aggressive)
                //{
                //    target = getHPLeast();
                //}
                Grid futureGrid;
                if (Mathf.Abs(target.curGrid.x - unit.troop.curGrid.x) <= Mathf.Abs(target.curGrid.z - unit.troop.curGrid.z)) //match z first
                {
                    Direction dir = Direction.negZ;
                    if (unit.troop.curGrid.z - target.curGrid.z < 0)
                    {
                        dir = Direction.posZ;
                    }
                    if ((int)Mathf.Abs(target.curGrid.x - unit.troop.curGrid.x) != 0)
                    {
                        futureGrid = findEmptyGrid(BattleCentralControl.map[unit.troop.curGrid.x, target.curGrid.z], dir);
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                        futureGrids.Add(futureGrid);
                    }
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.charge));
                    futureGrid = findEmptyGrid(BattleCentralControl.map[target.curGrid.x, target.curGrid.z], dir);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                    futureGrids.Add(futureGrid);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.charge));
                } else
                {
                    Direction dir = Direction.negX;
                    if (unit.troop.curGrid.x - target.curGrid.x < 0)
                    {
                        dir = Direction.posX;
                    }
                    if ((int)Mathf.Abs(target.curGrid.z - unit.troop.curGrid.z) == 0)
                    {
                        futureGrid = findEmptyGrid(BattleCentralControl.map[target.curGrid.x, unit.troop.curGrid.z], dir);
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                        futureGrids.Add(futureGrid);
                    }
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.charge));
                    futureGrid = findEmptyGrid(BattleCentralControl.map[target.curGrid.x, target.curGrid.z], dir);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                    futureGrids.Add(futureGrid);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.charge));
                }
                if (futureGrid != null)
                {
                    float staminaLeft = unit.stamina;
                    float distance = getDistance(futureGrid, target.curGrid);
                    while (staminaLeft >= 0)
                    {
                        if (distance < 2)
                        {
                            actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.whirlwind, target.gameObject));
                            staminaLeft -= unit.getWhirlwindStaminaCost();
                        }
                        else if (distance >= 2 && distance <= 3)
                        {
                            actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.lunge, target.gameObject));
                            staminaLeft -= unit.getLungeStaminaCost();
                        }
                        else
                        {
                            Debug.Log("cav not attacking" + distance);
                        }
                    }
                }
            }
        }
    }
    void halberdierEngage()
    {
        foreach (Person unit in halberdiers)
        {
            if (unit.troop != null)
            {
                //Debug.Log(unit.name);
                if (getNearbyTroop(unit.troop, (int) unit.getVision() * 2).Count > 0)
                {
                    Troop target = getNearest(unit.troop);
                    Grid futureGrid = findEmptyBlockGrid(BattleCentralControl.map[target.curGrid.x, target.curGrid.z], 2);
                    if (futureGrid != null)
                    {
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                        futureGrids.Add(futureGrid);

                        float staminaLeft = unit.stamina * .6f;
                        float distance = getDistance(futureGrid, target.curGrid);
                        while (staminaLeft >= 0)
                        {
                            if (distance < 2)
                            {
                                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.whirlwind, target.gameObject));
                                staminaLeft -= unit.getWhirlwindStaminaCost();
                            }
                            else if (distance >= 2 && distance <= 3)
                            {
                                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.lunge, target.gameObject));
                                staminaLeft -= unit.getLungeStaminaCost();
                            }
                            else
                            {
                                Debug.Log("not attacking" + distance);
                            }
                        }
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.guard));
                    }
                } else
                {
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, BattleCentralControl.map[unit.troop.curGrid.x, frontLine]));
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.guard));
                }
            }
        }
    }
    void musketeerEngage()
    {
        foreach (Person unit in musketeer)
        {
            if (unit.troop != null)
            {
                if (getNearbyTroop(unit.troop, 2).Count > 0)
                {
                    Troop target = getNearest(unit.troop);
                    Grid futureGrid = findEmptyBlockGrid(BattleCentralControl.map[target.curGrid.x, target.curGrid.z], 1);
                    if (futureGrid != null)
                    {
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                        futureGrids.Add(futureGrid);

                        float staminaLeft = unit.stamina * .6f;
                        float distance = getDistance(futureGrid, target.curGrid);
                        while (staminaLeft >= 0)
                        {
                            if (distance < 2)
                            {
                                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.whirlwind, target.gameObject));
                                staminaLeft -= unit.getWhirlwindStaminaCost();
                            }
                        }

                    }
                }
                else
                {
                    Grid futureGrid = findEmptyBlockGrid(BattleCentralControl.map[unit.troop.curGrid.x, backLine], 2);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                    futureGrids.Add(futureGrid);
                    //Finding target
                    Troop target = getHPLeast();
                    float distance = getDistance(futureGrid, target.curGrid);
                    List<Troop> checkedTroop = new List<Troop>();
                    checkedTroop.Add(target);
                    while (distance >= 50 && target != null)
                    {
                        target = getHPLeast(checkedTroop);
                    }
                    if (target == null)
                    {
                        target = getNearest(unit.troop);
                    }
                    //attacking
                    float staminaLeft = unit.stamina * .8f;
                    bool goingToHoldSteady = false;
                    if (unit.stamina >= unit.getFireStaminaCost() + unit.getHoldSteadyStaminaCost())
                    {
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.holdSteady));
                        goingToHoldSteady = true;
                    }
                    while (staminaLeft >= 0)
                    {
                        if (distance <= 50)
                        {

                            actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.fire, target.gameObject));
                            staminaLeft -= unit.getFireStaminaCost();
                            if (goingToHoldSteady)
                            {
                                staminaLeft -= unit.getHoldSteadyStaminaCost();
                            }
                        }
                    }
                }
            }
        }
    }
    void crossbowmenEngage()
    {
        foreach (Person unit in crossbowmen)
        {
            if (unit.troop != null)
            {
                if (getNearbyTroop(unit.troop, 2).Count > 0)
                {
                    Troop target = getNearest(unit.troop);
                    Grid futureGrid = findEmptyBlockGrid(BattleCentralControl.map[target.curGrid.x, target.curGrid.z], 1);
                    if (futureGrid != null)
                    {
                        actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                        futureGrids.Add(futureGrid);

                        float staminaLeft = unit.stamina * .6f;
                        float distance = getDistance(futureGrid, target.curGrid);
                        while (staminaLeft >= 0)
                        {
                            if (distance < 2)
                            {
                                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.whirlwind, target.gameObject));
                                staminaLeft -= unit.getWhirlwindStaminaCost();
                            }
                        }

                    }
                }
                else
                {
                    Grid futureGrid = findEmptyBlockGrid(BattleCentralControl.map[unit.troop.curGrid.x, backLine], 2);
                    actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.walk, futureGrid));
                    futureGrids.Add(futureGrid);
                    //Finding target
                    Troop target = getHPLeast();
                    float distance = getDistance(futureGrid, target.curGrid);
                    List<Troop> checkedTroop = new List<Troop>();
                    checkedTroop.Add(target);
                    while (distance >= 60 && target != null)
                    {
                        target = getHPLeast(checkedTroop);
                    }
                    if (target == null)
                    {
                        target = getNearest(unit.troop);
                    }
                    //attacking
                    float staminaLeft = unit.stamina * .8f;
                    while (staminaLeft >= 0)
                    {
                        if (distance >= 60 && getNearbyTroop(target, 2).Count <= 1) //getNearbyTroop will return AI troops
                        {
                            actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.rainOfArrows, target.gameObject));
                            staminaLeft -= unit.getRainOfArrowsStaminaCost();
                        } else
                        {
                            actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.quickDraw, target.gameObject));
                            staminaLeft -= unit.getQuickDrawStaminaCost();
                        }
                    }
                }
            }
        }
    }
    void guard()
    {

        foreach (Person unit in halberdiers)
        {
            if (unit.troop != null)
            {
                actionQueue.Enqueue(new AIAction(unit.troop, TroopSkill.guard));
            }
        }
    }





    int getOffsetX(int num)
    {
        if (attackMode == AIAttackMode.aggressive)
        {
            return (int) (mapSize.x / (num + 1));
        } else if (attackMode == AIAttackMode.cautious) {
            return 1;
        } else
        {
            return (int) (mapSize.x / (2 * num));
        }
    }

    Grid findEmptyBlockGrid(Grid center, int offset)
    {
        Grid result = null;
        float highestBlockRate = 0;
        for (int i = -offset; i <= offset; i++)
        {
            for (int j = -offset; j <= offset; j++)
            {
                if (center.x + i >= 0 && center.x + i <= mapSize.x - 1 && center.z + j >= 0 && center.z + j <= mapSize.y - 1)
                {
                    if (BattleCentralControl.map[center.x + i, center.z + j].personOnGrid == null
                        && !futureGrids.Contains(BattleCentralControl.map[center.x + i, center.z + j])
                            && BattleCentralControl.map[center.x + i, center.z + j].blockRate > highestBlockRate)
                    {
                        highestBlockRate = BattleCentralControl.map[center.x + i, center.z + j].blockRate;
                        result = BattleCentralControl.map[center.x + i, center.z + j];
                    }
                }
            }
        }
        return result;
    }
    Grid findEmptyGrid(Grid toCheck, Direction dir)
    {
        Grid result = toCheck;
        while (result.personOnGrid != null || futureGrids.Contains(result))
        {
            if (result.z >= mapSize.y - 1 || result.z <= 0 
                || result.x >= mapSize.x - 1 || result.x <= 0)
            {
                return result;
            } else
            {
                switch(dir)
                {
                    case Direction.negX:
                        result = BattleCentralControl.map[result.x - 1, result.z];
                        break;
                    case Direction.posX:
                        result = BattleCentralControl.map[result.x + 1, result.z];
                        break;
                    case Direction.negZ:
                        result = BattleCentralControl.map[result.x, result.z - 1];
                        break;
                    case Direction.posZ:
                        result = BattleCentralControl.map[result.x, result.z + 1];
                        break;
                }
                
            }
        }
        return result;
    }
    void decideLinesBlind()
    {
        int offSetZ = (int) mapSize.y / 10;
        switch (attackMode) {
            case AIAttackMode.aggressive:
                backLine = frontLine;
                frontLine -= 2 * offSetZ;
                midLine -= 2 * offSetZ;
                break;
            case AIAttackMode.cautious:
                //frontLine -= offSetZ;
                midLine -= 2 * offSetZ;
                //backLine -= offSetZ;
                break;
            case AIAttackMode.neutral:
                backLine = frontLine;
                frontLine = backLine - offSetZ;
                midLine = frontLine - 2 * offSetZ;
                break;
        }
        frontLine = Mathf.Clamp(frontLine, 2, (int)mapSize.y - 2);
        midLine = Mathf.Clamp(midLine, 2, (int)mapSize.y - 2);
        backLine = Mathf.Clamp(backLine, 2, (int)mapSize.y - 2);
    }
    void decideLinesSeen()
    {
        int offSetZ = (int)mapSize.y / 10;
        int playerTroopFront = 0;
        int playerTroopBack = (int)mapSize.y - 1;
        foreach (Troop p in getSeen())
        {
            if (playerTroopFront < p.curGrid.z)
            {
                playerTroopFront = (int)p.curGrid.z;
            }
            if (playerTroopBack > p.curGrid.z)
            {
                playerTroopBack = (int)p.curGrid.z;
            }

        }
        switch (attackMode)
        {
            case AIAttackMode.aggressive:
                frontLine = playerTroopFront;
                midLine = (playerTroopBack + playerTroopFront) / 2;
                backLine -= offSetZ;
                break;
            case AIAttackMode.cautious:
                //frontLine -= offSetZ;
                midLine = playerTroopFront;
                //backLine -= offSetZ;
                break;
            case AIAttackMode.neutral:
                frontLine = playerTroopFront;
                midLine = playerTroopFront - 1;
                //backLine -= offSetZ;
                break;
        }
        frontLine = Mathf.Clamp(frontLine, 2, (int) mapSize.y - 2);
        midLine = Mathf.Clamp(midLine, 2, (int) mapSize.y - 2);
        backLine = Mathf.Clamp(backLine, 2, (int) mapSize.y - 2);
    }
    Troop getNearest(Troop troop)
    {
        List<Troop> seenTroop = getSeen();
        if (seenTroop.Count > 0)
        {
            float leastDistance = Vector3.Distance(seenTroop[0].gameObject.transform.position, troop.gameObject.transform.position);
            Troop closestTroop = seenTroop[0];
            foreach (Troop playerTroop in seenTroop)
            {
                if (leastDistance > Vector3.Distance(playerTroop.gameObject.transform.position, troop.gameObject.transform.position))
                {
                    leastDistance = Vector3.Distance(playerTroop.gameObject.transform.position, troop.gameObject.transform.position);
                    closestTroop = playerTroop;
                }
            }
            return closestTroop;
        }
        return null;
    }
    Troop getHPLeast()
    {
        List<Troop> seenTroop = getSeen();
        if (seenTroop.Count > 0)
        {
            float leastHP = seenTroop[0].person.health;
            Troop leastTroop = seenTroop[0];
            foreach (Troop playerTroop in seenTroop)
            {
                if (leastHP > playerTroop.person.health)
                {
                    leastHP = playerTroop.person.health;
                    leastTroop = playerTroop;
                }
            }
            return leastTroop;
        }
        return null;
    }
    Troop getHPLeast(List<Troop> checkedTroop)
    {
        List<Troop> seenTroop = getSeen();
        if (seenTroop.Count > 0)
        {
            float leastHP = seenTroop[0].person.health;
            Troop leastTroop = seenTroop[0];
            foreach (Troop playerTroop in seenTroop)
            {
                if (leastHP > playerTroop.person.health && checkedTroop.Contains(playerTroop))
                {
                    leastHP = playerTroop.person.health;
                    leastTroop = playerTroop;
                }
            }
            return leastTroop;
        }
        return null;
    }
    List<Troop> getNearbyTroop(Troop troop, int offset)
    {
        List<Troop> result = new List<Troop>();
        Grid center = troop.curGrid;
        for (int i = -offset; i <= offset; i++)
        {
            for (int j = -offset; j <= offset; j++)
            {
                if (center.x + i >= 0 && center.x + i <= mapSize.x - 1 && center.z + j >= 0 && center.z + j <= mapSize.y - 1)
                {
                    if (BattleCentralControl.map[center.x + i, center.z + j].personOnGrid != null
                        && BattleCentralControl.map[center.x + i, center.z + j].personOnGrid.faction != troop.person.faction)
                    {
                        result.Add(BattleCentralControl.map[center.x + i, center.z + j].personOnGrid.troop);
                    }
                }
            }
        }
        return result;
    }



    List<Troop> getSeen()
    {
        List<Troop> result = new List<Troop>();
        foreach(Person unit in BattleCentralControl.playerParty.partyMember)
        {
            if (unit.troop != null && unit.troop.seen && !result.Contains(unit.troop))
            {
                result.Add(unit.troop);
            }
        }
        return result;
    }
    void categorizeTroop()
    {
        halberdiers.Clear();
        cavalries.Clear();
        swordsmen.Clear();
        crossbowmen.Clear();
        musketeer.Clear();

        foreach(Person p in BattleCentralControl.enemyParty.partyMember)
        {
            switch (p.troopType)
            {
                case TroopType.halberdier:
                    halberdiers.Add(p);
                    break;
                case TroopType.cavalry:
                    cavalries.Add(p);
                    break;
                case TroopType.swordsman:
                    swordsmen.Add(p);
                    break;
                case TroopType.crossbowman:
                    crossbowmen.Add(p);
                    break;
                case TroopType.musketeer:
                    musketeer.Add(p);
                    break;
                case TroopType.recruitType:
                    swordsmen.Add(p);
                    break;
            }
        }
        if (enemyPlaced)
        {
            halberdiers = sortListByGridX(halberdiers);
            cavalries = sortListByGridX(cavalries);
            swordsmen = sortListByGridX(swordsmen);
            musketeer = sortListByGridX(musketeer);
            crossbowmen = sortListByGridX(crossbowmen);
        }
    }
    void placeTroop(int posX, int posZ, Person unit)
    {
        var pos = new Vector3(posX, 1, posZ);
        var rot = new Quaternion(0, 0, 0, 0);
        GameObject unitToPlace = enemyBaseCharacter;
        GameObject model = Object.Instantiate(TroopDataBase.troopDataBase.getTroopObject(unit.faction, unit.troopType, unit.ranking), new Vector3(0, 0, 0), rot);
        unitToPlace.GetComponent<Troop>().model = model;
        model.transform.SetParent(unitToPlace.transform, false);
        model.SetActive(true);
        GameObject gridToPlace = BattleCentralControl.map[posX, posZ].gridObject;
        GameObject duplicatedUnitToPlace = gridToPlace.GetComponent<GridObject>().placeTroopOnGrid(unitToPlace, pos, rot);
        duplicatedUnitToPlace.GetComponent<Troop>().placed(unit, BattleCentralControl.map[posX, posZ]);
        Object.DestroyImmediate(model);
    }
    void decideAttackMode()
    {
        int cautiousChance = 10;
        int aggressiveChance = 10;
        int neutralChance = 20;
        if (BattleCentralControl.enemyParty.leader.troopType == TroopType.musketeer
            && BattleCentralControl.enemyParty.leader.troopType == TroopType.crossbowman)
        {
            cautiousChance += 10;
            aggressiveChance -= 5;
            neutralChance -= 5;
        }
        else if (BattleCentralControl.enemyParty.leader.troopType == TroopType.swordsman
          && BattleCentralControl.enemyParty.leader.troopType == TroopType.cavalry)
        {
            cautiousChance -= 5;
            aggressiveChance += 10;
            neutralChance -= 5;
        }
        else
        {
            cautiousChance -= 5;
            aggressiveChance -= 5;
            neutralChance += 10;
        }
        int rand = Random.Range(1, cautiousChance + aggressiveChance + neutralChance);
        if (rand <= cautiousChance)
        {
            attackMode = AIAttackMode.cautious;
        }
        else if (rand > cautiousChance && rand <= aggressiveChance)
        {
            attackMode = AIAttackMode.aggressive;
        }
        else
        {
            attackMode = AIAttackMode.neutral;
        }
    }
    float getDistance(Grid gridOne, Grid gridTwo)
    {
        return Vector2.Distance(new Vector2(gridOne.x, gridOne.z), new Vector2(gridTwo.x,  gridTwo.z));
    }
    void clockTick()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            tick = true;
            timer -= 0.5f;
        }
        else
        {
            tick = false;
        }
    }
    List<Person> sortListByGridX(List<Person> personL)
    {
        if (personL.Count > 0)
        {
            float smallestX, comparingX;
            int smallestIndex = 0;
            Person temp;
            for (int i = 0; i < personL.Count - 1; i++)
            {
                if (personL[i].troop != null)
                {
                    smallestX = personL[i].troop.curGrid.x;
                    smallestIndex = i;
                    for (int j = i + 1; j < personL.Count; j++)
                    {
                        if (personL[j].troop != null)
                        {
                            comparingX = personL[j].troop.curGrid.x;
                            if (smallestX >= comparingX)
                            {
                                smallestIndex = j;
                                smallestX = comparingX;
                            }
                        }
                    }
                    temp = personL[i];
                    personL[i] = personL[smallestIndex];
                    personL[smallestIndex] = temp;
                }
                
            }
        }
        return personL;
    }
}
/**
 * An object that stores instructions for enemy, including: wa
 */
public class AIAction {
    public Troop troop;
    public Troop targetTroop;
    public GameObject target;
    public TroopSkill skillMode;
    public Grid destination;
    public bool aimingAtTroop = false;
    public bool aimingAtGrid = false;
    public bool finished;
    public AIAction(Troop troopI, TroopSkill skillModeI)
    {
        troop = troopI;
        skillMode = skillModeI;
    }
    public AIAction(Troop troopI, TroopSkill skillModeI, GameObject targetI)
    {
        troop = troopI;
        skillMode = skillModeI;
        target = targetI;
        if (target.GetComponent<Troop>() != null)
        {
            aimingAtTroop = true;
            aimingAtGrid = false;
            targetTroop = target.GetComponent<Troop>();
        } else if (target.GetComponent<GridObject>() != null)
        {
            aimingAtTroop = false;
            aimingAtGrid = true;
        }
    }
    public AIAction(Troop troopI, TroopSkill skillModeI, Grid destinationI)
    {
        troop = troopI;
        skillMode = skillModeI;
        destination = destinationI;
    }
    /**
     * enemy walk
     * enemy character aim with hitbox
     */
    public void doAIAction()
    {
        if (troop.activated)
        {
            troop.aiControlled = true;
            switch (skillMode)
            {
                case TroopSkill.none:
                    BattleAIControl.enemyFinished = true;
                    break;
                case TroopSkill.walk:
                    //Debug.Log(destination.x + " " + destination.z);
                    BattleAIControl.futureGrids.Remove(troop.curGrid);
                    troop.troopMoveToPlace(destination);
                    break;
                case TroopSkill.lunge:
                    if (aimingAtTroop)
                    {
                        if (targetTroop == null || !targetTroop.activated)
                        {
                            BattleAIControl.skippThisAction = true;
                            BattleAIControl.shouldRethink = true;
                            return;
                        }
                    }
                    if (troop.person.stamina >= troop.person.getLungeStaminaCost())
                    {
                        lungeAttack(troop, target);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.whirlwind:
                    if (aimingAtTroop)
                    {
                        if (targetTroop == null || !targetTroop.activated)
                        {
                            BattleAIControl.skippThisAction = true;
                            BattleAIControl.shouldRethink = true;
                            return;
                        }
                    }
                    if (troop.person.stamina >= troop.person.getWhirlwindStaminaCost())
                    {
                        whirlwindAttack(troop, target);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.execute:
                    if (troop.person.stamina >= troop.person.getExecuteStaminaCost())
                    {
                        executeAttack(troop);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.guard:
                    if (troop.person.stamina >= troop.person.getGuardStaminaCost())
                    {
                        guardAttack(troop);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.holdSteady:
                    break;
                case TroopSkill.fire:
                    if (aimingAtTroop)
                    {
                        if (targetTroop == null || !targetTroop.activated)
                        {
                            BattleAIControl.skippThisAction = true;
                            BattleAIControl.shouldRethink = true;
                            return;
                        }
                    }
                    if (troop.person.stamina >= troop.person.getFireStaminaCost())
                    {
                        fireAttack(troop, target);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.quickDraw:
                    if (aimingAtTroop)
                    {
                        if (targetTroop == null || !targetTroop.activated)
                        {
                            BattleAIControl.skippThisAction = true;
                            BattleAIControl.shouldRethink = true;
                            return;
                        }
                    }
                    if (troop.person.stamina >= troop.person.getQuickDrawStaminaCost())
                    {
                        quickDrawAttack(troop, target);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.rainOfArrows:
                    if (aimingAtTroop)
                    {
                        if (targetTroop == null || !targetTroop.activated)
                        {
                            BattleAIControl.skippThisAction = true;
                            BattleAIControl.shouldRethink = true;
                            return;
                        }
                    }
                    if (troop.person.stamina >= troop.person.getRainOfArrowsStaminaCost())
                    {
                        rainOfArrowAttack(troop, target);
                    }
                    else
                    {
                        BattleAIControl.skippThisAction = true;
                        BattleAIControl.shouldRethink = true;
                    }
                    break;
                case TroopSkill.charge:
                    break;
            }
        }
        
    }
    /**
     * where characters actually do animation and dmg, except walk
     */
    public void finishAIAction()
    {
        switch (skillMode)
        {
            case TroopSkill.none:
                break;
            case TroopSkill.walk:
                break;
            case TroopSkill.lunge:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.lunge);
                troop.hideIndicators();
                break;
            case TroopSkill.whirlwind:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.whirlwind);
                troop.hideIndicators();
                break;
            case TroopSkill.execute:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.execute);
                troop.hideIndicators();
                break;
            case TroopSkill.guard:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.guard);
                troop.hideIndicators();
                break;
            case TroopSkill.charge:
                charge(troop);
                break;
            case TroopSkill.holdSteady:
                if (troop.person.stamina >= troop.person.getFireStaminaCost() + troop.person.getHoldSteadyStaminaCost())
                {
                    holdSteadyAttack(troop);
                }
                break;
            case TroopSkill.fire:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.fire);
                troop.hideIndicators();
                break;
            case TroopSkill.quickDraw:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.quickDraw);
                troop.hideIndicators();
                break;
            case TroopSkill.rainOfArrows:
                troop.doSkill(troop.indicatedGrid(), TroopSkill.rainOfArrows);
                troop.hideIndicators();
                break;
        }
        
    }
    

    void lungeAttack(Troop unit, GameObject attacked)
    {
        if (!unit.lungeIndicator.activeSelf)
        {
            unit.lungeIndicator.SetActive(true);
        }
        unit.curIndicator = unit.lungeIndicator;
        lookAtObject(unit.curIndicator, attacked);
        unit.lookTarget = unit.lungeIndicator.transform.Find("Model").transform.position - unit.gameObject.transform.position;
        lookAtObject(unit.gameObject, attacked);
    }
    void whirlwindAttack(Troop unit, GameObject attacked)
    {
        if (!unit.whirlwindIndicator.activeSelf)
        {
            unit.whirlwindIndicator.SetActive(true);
        }
        unit.curIndicator = unit.whirlwindIndicator;
        unit.lookTarget = attacked.transform.position - unit.gameObject.transform.position;
    }
    void executeAttack(Troop unit)
    {
        if (!unit.executeIndicator.activeSelf)
        {
            unit.executeIndicator.SetActive(true);
        }
        unit.curIndicator = unit.executeIndicator;
    }
    void guardAttack(Troop unit)
    {
        if (!unit.guardIndicator.activeSelf)
        {
            unit.guardIndicator.SetActive(true);
        }
        unit.curIndicator = unit.guardIndicator;
    }
    void holdSteadyAttack(Troop unit)
    {
        unit.holdSteady();
    }
    void charge(Troop unit)
    {
        unit.charge();
    }
    void fireAttack(Troop unit, GameObject attacked)
    {
        if (!unit.fireIndicator.activeSelf)
        {
            unit.fireIndicator.SetActive(true);
        }
        unit.curIndicator = unit.fireIndicator;
        lookAtObject(unit.curIndicator, attacked);
        unit.lookTarget = unit.fireIndicator.transform.Find("Model").transform.position - unit.gameObject.transform.position;
        lookAtObject(unit.gameObject, attacked);
    }
    void quickDrawAttack(Troop unit, GameObject attacked)
    {
        if (!unit.quickDrawIndicator.activeSelf)
        {
            unit.quickDrawIndicator.SetActive(true);
        }
        unit.curIndicator = unit.quickDrawIndicator;
        lookAtObject(unit.curIndicator, attacked);
        unit.lookTarget = unit.quickDrawIndicator.transform.Find("Model").transform.position - unit.gameObject.transform.position;
        lookAtObject(unit.gameObject, attacked);
    }
    void rainOfArrowAttack(Troop unit, GameObject attacked)
    {
        if (!unit.rainOfArrowIndicator.activeSelf)
        {
            unit.rainOfArrowIndicator.SetActive(true);
        }
        unit.curIndicator = unit.rainOfArrowIndicator;
        lookAtObject(unit.gameObject, attacked);
        unit.lookTarget = unit.rainOfArrowIndicator.transform.Find("Model").transform.position - unit.gameObject.transform.position;
        followObject(unit.rainOfArrowIndicator, attacked);
    }
    void lookAtObject(GameObject looker, GameObject obj)
    {
        Vector3 v = obj.transform.position - looker.transform.position;
        v.x = v.z = 0.0f;
        looker.gameObject.transform.LookAt(obj.transform.position - v);
        looker.gameObject.transform.Rotate(0, 180, 0);
    }
    void followObject(GameObject objToMove, GameObject obj)
    {
        Vector3 pos = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
        objToMove.transform.position = Vector3.Slerp(obj.transform.position, pos, Time.deltaTime * 1000);
    }

}
public enum AIAttackMode
{
    aggressive,
    cautious,
    neutral
}

public enum Direction
{
    negX, //right
    posX, //left
    negZ, //front
    posZ //back
}