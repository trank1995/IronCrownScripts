using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInteractable : MonoBehaviour {

	public virtual void cameraFocusOn ()
    {
        if (gameObject != null)
        {
            BattleCamera.target = gameObject;
            BattleCamera.cameraMode = CameraMode.mapObject;
            Info.clearInfo();
        }
    }
    public virtual void cameraFocusOnExit()
    {
    }
}
