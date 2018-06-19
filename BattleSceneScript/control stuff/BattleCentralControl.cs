using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleCentralControl : MonoBehaviour {
    

    public static bool battleStart, playerTurn;
    public static Grid[,] map;
    public static int gridXMax, gridZMax, curRound, oldRound;
    public static MainParty playerParty;
    public static Party enemyParty;
    public static List<BattlefieldType> battlefieldTypes;
    public static Dictionary<Person, GameObject> playerTroopOnField, enemyTroopOnField;
    public static List<Person> deadEnemy, deadPlayer;
    public static int playerTotal, enemyTotal;
    public GameObject mapCenter;
    //ADD NEW GRID
    public GameObject baseGround, barrel, brick, bush, deadTree, deadFarmPlants, farmPlants, farmPlants2, 
        fence, flatGrass, hugeRock, hugeTree, logs, pillar, puddle, puddleAndTree, rock, rockAndGrass,
        rockAndTree, rockyPlain, singleTree, stonePillar, woodBoard;
    List<GridType> woods, farmland, mountain, grassland, hills, marsh, city, common;
    bool groundInitialized = false;
    private void Awake()
    {
        //gridToObj = new Dictionary<Grid, GameObject>();
        //objToGrid = new Dictionary<GameObject, Grid>();
        playerTroopOnField = new Dictionary<Person, GameObject>();
        enemyTroopOnField = new Dictionary<Person, GameObject>();
        deadEnemy = new List<Person>();
        deadPlayer = new List<Person>();
        if (battlefieldTypes == null || battlefieldTypes.Count == 0)
        {
            battlefieldTypes = new List<BattlefieldType>();
            battlefieldTypes.Add(BattlefieldType.Common);
        }
        
        playerTurn = true;
        battleStart = false;
    }
    // Use this for initialization
    void Start()
    {
        categorizeGridTypes();
    }

    // Update is called once per frame
    void Update()
    {
        if (!groundInitialized && playerParty != null && enemyParty != null)
        {
            //TEMP
            gridXMax = playerParty.partyMember.Count + enemyParty.partyMember.Count;
            gridZMax = playerParty.partyMember.Count + enemyParty.partyMember.Count;
            map = new Grid[gridXMax, gridZMax];
            generateMap(gridXMax, gridZMax);
            placeOnMap(gridXMax, gridZMax);
            groundInitialization();
            groundInitialized = true;
            BattleCamera.mapCenter = map[gridXMax / 2, gridZMax / 2].gridObject;
            mapCenter.transform.position = BattleCamera.mapCenter.transform.position;
            BattleCamera.target = BattleCamera.mapCenter;
        }
    }

    public static void endTurnPrep()
    {
        if (playerTurn)
        {
            foreach (KeyValuePair<Person, GameObject> t in BattleCentralControl.playerTroopOnField)
            {
                Troop troop = t.Value.GetComponent<Troop>();
                if (troop.activated)
                {
                    t.Key.stamina = t.Key.getStaminaMax();
                    troop.stealthCheckRefresh();
                    troop.charging = false;
                    troop.holdSteadying = false;
                }
            }
            foreach (KeyValuePair<Person, GameObject> t in BattleCentralControl.enemyTroopOnField)
            {
                Troop troop = t.Value.GetComponent<Troop>();
                if (troop.activated)
                {
                    troop.stealthCheckRefresh();
                    troop.clearGuard();
                }
            }
            if (BattleInteraction.curControlled != null)
            {
                BattleInteraction.curControlled.GetComponent<Troop>().cameraFocusOnExit();
                BattleInteraction.curControlled = null;
            }
            foreach (Person p in BattleCentralControl.playerParty.partyMember)
            {
                if (p.troop == null)
                {
                    p.health = Mathf.Clamp(p.health + p.healthRegenPerTurn(), p.health, p.getHealthMax());
                } else
                {
                    p.troop.changeHealth(p.healthRegenPerTurn(), 0);
                }
            }
        }
        if (!playerTurn)
        {
            foreach (KeyValuePair<Person, GameObject> t in BattleCentralControl.enemyTroopOnField)
            {
                Troop troop = t.Value.GetComponent<Troop>();
                if (troop.activated)
                {
                    t.Key.stamina = t.Key.getStaminaMax();
                    troop.stealthCheckRefresh();
                    troop.charging = false;
                    troop.holdSteadying = false;
                }
            }
            foreach (KeyValuePair<Person, GameObject> t in BattleCentralControl.playerTroopOnField)
            {
                Troop troop = t.Value.GetComponent<Troop>();
                if (troop.activated)
                {
                    troop.stealthCheckRefresh();
                    troop.clearGuard();
                }
            }
            foreach (Person p in BattleCentralControl.enemyParty.partyMember)
            {
                if (p.troop == null)
                {
                    p.health = Mathf.Clamp(p.health + p.healthRegenPerTurn(), p.health, p.getHealthMax());
                }
                else
                {
                    p.troop.changeHealth(p.healthRegenPerTurn(), 0);
                }
            }
        }
    }
    public static void startTurnPrep()
    {
        if (playerTurn)
        {
            foreach (KeyValuePair<Person, GameObject> pair in BattleCentralControl.playerTroopOnField)
            {
                BattleInteraction.curControlled = pair.Value;
                BattleInteraction.curControlled.GetComponent<Troop>().cameraFocusOn();
                break;
            }
        }
        if (!playerTurn)
        {
        }
    }
    public static void endBattle()
    {
        Time.timeScale = 1.0f;
        //SaveLoadSystem.saveLoadSystem.tempSave();
        foreach(Person p in BattleCentralControl.playerParty.partyMember)
        {
            p.inBattle = false;
            p.increaseExp((int) (playerParty.expToDistribute / BattleCentralControl.playerParty.partyMember.Count));
        }
        foreach (Person p in BattleCentralControl.enemyParty.partyMember)
        {
            p.inBattle = false;
            p.increaseExp((int)(enemyParty.expToDistribute / BattleCentralControl.enemyParty.partyMember.Count));
        }
        playerParty.expToDistribute = 0;
        enemyParty.expToDistribute = 0;
        BattleInspectPanel.person = null;
        GridInspectPanel.grid = null;
        MapManagement.parties.Remove(BattleCentralControl.enemyParty);
        
        SaveLoadSystem.saveLoadSystem.tempSave();
        //Debug.Log(SaveLoadSystem.saveLoadSystem.mainParty.name);
        SceneManager.LoadScene("MapScene");
        SceneManager.UnloadSceneAsync("BattleScene");
        MapManagement.mapManagement.endOfBattle(BattleCentralControl.enemyParty, EndTurnPanel.battleResult);
        
    }
    public static List<GameObject> gridInLine(GameObject start, GameObject end)
    {
        List<GameObject> result = new List<GameObject>();
        RaycastHit[] hits;
        var direction = end.transform.position - start.transform.position;
        var distance = Vector3.Distance(start.transform.position, end.transform.position);
        hits = Physics.RaycastAll(start.transform.position, direction, distance);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            result.Add(hit.transform.gameObject);
        }
        return result;
    }
    void generateMap(int x, int z)
    {
        for (int ix = 0; ix < x; ix++)
        {
            for (int iz = 0; iz < z; iz++)
            {
                mapGridDecider(ix, iz, battlefieldTypes);
            }
        }
    }
    void placeOnMap(int x, int z)
    {
        for (int ix = 0; ix < x; ix++)
        {
            for (int iz = 0; iz < z; iz++)
            {
                var pos = new Vector3(ix, 0, iz);
                var rot = new Quaternion(0, 0, 0, 0);
                int rand = Random.Range(0, 3);
                var obj = Instantiate(map[ix, iz].mapSettingModel, pos, rot);
                obj.transform.rotation *= Quaternion.Euler(0, rand * 90f, 0);
                map[ix, iz].gridObject = obj;
                obj.GetComponent<GridObject>().grid = map[ix, iz];
                obj.GetComponent<GridObject>().becomeUnseen();
                if (iz <= BattleCentralControl.playerParty.leader.getTroopPlacingRange(BattleCentralControl.gridZMax))
                {
                    obj.GetComponent<GridObject>().becomeSeen();
                }
                
                //gridToObj.Add(map[ix, iz], obj);
                //objToGrid.Add(obj, map[ix, iz]);
            }
        }
    }
    void mapGridDecider(int x, int z, List<BattlefieldType> bt)
    {
        int rand = Random.Range(0, bt.Count);
        List<GridType> gridTypesToCreate;
        switch(bt[rand])
        {
            case BattlefieldType.City:
                gridTypesToCreate = city;
                break;
            case BattlefieldType.Farmland:
                gridTypesToCreate = farmland;
                break;
            case BattlefieldType.Grassland:
                gridTypesToCreate = grassland;
                break;
            case BattlefieldType.Hills:
                gridTypesToCreate = hills;
                break;
            case BattlefieldType.Marsh:
                gridTypesToCreate = marsh;
                break;
            case BattlefieldType.Mountain:
                gridTypesToCreate = mountain;
                break;
            case BattlefieldType.Woods:
                gridTypesToCreate = woods;
                break;
            default:
                gridTypesToCreate = common;
                break;

        }
        //TEMP
        gridTypesToCreate = common;
        Grid temp = makeLandscape(x, z, gridTypesToCreate);
        
        
        map[x, z] = temp;
    }
    //ADD NEW GRID
    Grid makeLandscape(int x, int z, List<GridType> gridTypes)
    {
        int rand = (int)Random.Range(0, gridTypes.Count);
        Grid temp = new Grid(x, z, flatGrass, GridType.flatGrass);
        switch (gridTypes[rand])
        {
            case GridType.baseGround:
                temp = new Grid(x, z, baseGround, GridType.baseGround);
                break;
            case GridType.barrel:
                temp = new Grid(x, z, barrel, GridType.barrel);
                break;
            case GridType.brick:
                temp = new Grid(x, z, brick, GridType.brick);
                break;
            case GridType.bush:
                temp = new Grid(x, z, bush, GridType.bush);
                break;
            case GridType.deadTree:
                temp = new Grid(x, z, deadTree, GridType.deadTree);
                break;
            case GridType.deadFarmPlants:
                temp = new Grid(x, z, deadFarmPlants, GridType.deadFarmPlants);
                break;
            case GridType.farmPlants:
                temp = new Grid(x, z, farmPlants, GridType.farmPlants);
                break;
            case GridType.farmPlants2:
                temp = new Grid(x, z, farmPlants2, GridType.farmPlants2);
                break;
            case GridType.fence:
                temp = new Grid(x, z, fence, GridType.fence);
                break;
            case GridType.flatGrass:
                temp = new Grid(x, z, flatGrass, GridType.flatGrass);
                break;
            case GridType.hugeRock:
                temp = new Grid(x, z, hugeRock, GridType.hugeRock);
                break;
            case GridType.hugeTree:
                temp = new Grid(x, z, hugeTree, GridType.hugeTree);
                break;
            case GridType.logs:
                temp = new Grid(x, z, logs, GridType.logs);
                break;
            case GridType.pillar:
                temp = new Grid(x, z, pillar, GridType.pillar);
                break;
            case GridType.puddle:
                temp = new Grid(x, z, puddle, GridType.puddle);
                break;
            case GridType.puddleAndTree:
                temp = new Grid(x, z, puddleAndTree, GridType.puddleAndTree);
                break;
            case GridType.rock:
                temp = new Grid(x, z, rock, GridType.rock);
                break;
            case GridType.rockAndGrass:
                temp = new Grid(x, z, rockAndGrass, GridType.rockAndGrass);
                break;
            case GridType.rockAndTree:
                temp = new Grid(x, z, rockAndTree, GridType.rockAndTree);
                break;
            case GridType.rockyPlain:
                temp = new Grid(x, z, rockyPlain, GridType.rockyPlain);
                break;
            case GridType.singleTree:
                temp = new Grid(x, z, singleTree, GridType.singleTree);
                break;
            case GridType.stonePillar:
                temp = new Grid(x, z, stonePillar, GridType.stonePillar);
                break;
            case GridType.woodBoard:
                temp = new Grid(x, z, fence, GridType.woodBoard);
                break;
        }
        return temp;
    }
    
    void groundInitialization()
    {
        flatGrass.SetActive(false);
        rockAndTree.SetActive(false);
        singleTree.SetActive(false);
        deadTree.SetActive(false);
        rockyPlain.SetActive(false);
        fence.SetActive(false);
        //Terrain terrain = ground.GetComponent<Terrain>();
        //ground.GetComponent<MeshRenderer>().enabled = false;
    }
    //ADD NEW GRID
    void categorizeGridTypes()
    {
        woods = new List<GridType>();
        woods.AddRange(new GridType[] { GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.farmPlants, GridType.farmPlants2, GridType.fence, GridType.flatGrass, GridType.hugeRock, GridType.hugeTree,
            GridType.logs, GridType.pillar, GridType.puddleAndTree, GridType.rock, GridType.rockAndGrass,
            GridType.rockAndTree, GridType.rockyPlain, GridType.singleTree, GridType.stonePillar, GridType.woodBoard });

        farmland = new List<GridType>();
        farmland.AddRange(new GridType[] { GridType.barrel, GridType.brick, GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.farmPlants, GridType.farmPlants2, GridType.fence, GridType.flatGrass,
            GridType.logs, GridType.pillar, GridType.rock, GridType.rockAndGrass,
            GridType.rockyPlain, GridType.singleTree, GridType.woodBoard });

        mountain = new List<GridType>();
        mountain.AddRange(new GridType[] { GridType.bush, GridType.deadTree, GridType.fence, GridType.flatGrass, GridType.hugeRock, GridType.hugeTree,
            GridType.logs, GridType.pillar, GridType.puddle, GridType.puddleAndTree, GridType.rock, GridType.rockAndGrass,
            GridType.rockAndTree, GridType.rockyPlain, GridType.singleTree, GridType.woodBoard});

        grassland = new List<GridType>();
        grassland.AddRange(new GridType[] { GridType.brick, GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.farmPlants,  GridType.flatGrass,
            GridType.logs, GridType.puddle, GridType.rock, GridType.rockAndGrass,
            GridType.rockyPlain, GridType.singleTree, GridType.woodBoard });

        hills = new List<GridType>();
        hills.AddRange(new GridType[] { GridType.barrel, GridType.brick, GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.farmPlants, GridType.farmPlants2, GridType.fence, GridType.flatGrass, GridType.hugeRock, GridType.hugeTree,
            GridType.logs, GridType.pillar, GridType.puddle, GridType.puddleAndTree, GridType.rock, GridType.rockAndGrass,
            GridType.rockAndTree, GridType.rockyPlain, GridType.singleTree, GridType.stonePillar, GridType.woodBoard });

        marsh = new List<GridType>();
        marsh.AddRange(new GridType[] { GridType.barrel, GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.flatGrass, GridType.hugeRock, GridType.hugeTree,
            GridType.logs, GridType.puddle, GridType.puddleAndTree, GridType.rock, GridType.rockAndGrass,
            GridType.rockAndTree, GridType.rockyPlain, GridType.singleTree, GridType.woodBoard});

        city = new List<GridType>();
        city.AddRange(new GridType[] {GridType.barrel, GridType.brick, GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.farmPlants, GridType.farmPlants2, GridType.fence, GridType.flatGrass, 
            GridType.logs, GridType.pillar, GridType.rock, GridType.rockAndGrass,
            GridType.rockAndTree, GridType.rockyPlain, GridType.singleTree, GridType.stonePillar, GridType.woodBoard });

        common = new List<GridType>();
        common.AddRange(new GridType[] { GridType.barrel, GridType.brick, GridType.bush, GridType.deadTree, GridType.deadFarmPlants,
            GridType.farmPlants, GridType.farmPlants2, GridType.fence, GridType.flatGrass, GridType.hugeRock, GridType.hugeTree,
            GridType.logs, GridType.pillar, GridType.puddle, GridType.puddleAndTree, GridType.rock, GridType.rockAndGrass,
            GridType.rockAndTree, GridType.rockyPlain, GridType.singleTree, GridType.stonePillar, GridType.woodBoard});
    }
}
//ADD NEW GRID
public enum GridType
{
    baseGround, barrel, brick, bush, deadTree, deadFarmPlants, farmPlants, farmPlants2,
    fence, flatGrass, hugeRock, hugeTree, logs, pillar, puddle, puddleAndTree, rock, rockAndGrass,
    rockAndTree, rockyPlain, singleTree, stonePillar, woodBoard
}