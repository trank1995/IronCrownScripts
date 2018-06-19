using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : BattleInteractable {
    public bool seen = false;
    public Color originalColor, guardedByEnemyIndicatorOriginalColor;
    public Material[] originalMaterials;
    public Material invisibleMaterial;
    public MeshRenderer meshRenderer;
    public GameObject infoPanel;
    public GameObject guardedByPlayerIndicator, guardedByEnemyIndicator;
    public Grid grid;
    public void Awake()
    {
        //originalColor = meshRenderer.material.color;
        
    }
    private void Update()
    {
        /**if (BattleCentralControl.objToGrid.ContainsKey(gameObject))
        {
            if (BattleCentralControl.objToGrid[gameObject].enemyTempStaminaCost > 0)
            {
                guardedByPlayer();
            }
            else if (BattleCentralControl.objToGrid[gameObject].playerTempStaminaCost > 0)
            {
                guardedByEnemy();
            }
        }**/
    }

    private void OnEnable()
    {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        originalMaterials = meshRenderer.materials;
        guardedByPlayerIndicator.SetActive(false);
        guardedByEnemyIndicator.SetActive(false);
        
    }
    public override void cameraFocusOn()
    {
        //base.cameraFocusOn();
        infoPanel.SetActive(true);
        
        if (seen)
        {
            GridInspectPanel.unknown = false;
        } else
        {
            GridInspectPanel.unknown = true;
        }
        GridInspectPanel.grid = grid;
        BattleInspectPanel.person = null;
    }
    public override void cameraFocusOnExit()
    {
        base.cameraFocusOnExit();
        infoPanel.SetActive(false);
        GridInspectPanel.grid = null;
    }
    public GameObject placeTroopOnGrid(GameObject toPlace, Vector3 pos, Quaternion rot)
    {
        return Instantiate(toPlace, pos, rot);
    }
    public bool inGrid(float x, float z)
    {
        float gridPosX = grid.x;
        float gridPosZ = grid.x;
        if (x > gridPosX- .5f && x < gridPosX + .5f &&
            z > gridPosZ - .5f && z < gridPosZ + .5f)
        {
            return true;
        }
        return false;
    }

    public void moveTroopToGrid(GameObject toMove)
    {

        toMove.GetComponent<Troop>().troopMoveToPlace(grid);
        
    }
    public void checkTroopOnGrid(Troop troop)
    {
        if (grid.personOnGrid != null)
        {
            grid.checkPersonStealth(troop);
        }
    }
    public void guardedByPlayer(bool guarded, Person p)
    {
        guardedByPlayerIndicator.SetActive(guarded);
        if (guarded)
        {
            grid.guarded(p);
        } else
        {
            grid.unguarded(p);
        }
    }
    public void guardedByEnemy(bool guarded, Person p)
    {
        guardedByEnemyIndicator.SetActive(guarded);
        if (guarded)
        {
            grid.guarded(p);
        }
        else
        {
            grid.unguarded(p);
        }
    }
    public void becomeUnseen()
    {
        meshRenderer.material.color = new Color(0f, 0f, 0f);
        Material[] newMaterials = (Material[]) meshRenderer.materials.Clone();
        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            newMaterials[i] = invisibleMaterial;
        }
        meshRenderer.materials = newMaterials;
        seen = false;
    }
    public void becomeSeen()
    {
        meshRenderer.materials = originalMaterials;
        //foreach (Person p in grid.guardingPersons)
        //{
        //    if (p.troop != null && p.troop.seen)
        //    {
        //        guardedByEnemyIndicator.GetComponent<MeshRenderer>().material.color = guardedByEnemyIndicatorOriginalColor;
        //        break;
        //    }
        //}
        seen = true;
    }
}
