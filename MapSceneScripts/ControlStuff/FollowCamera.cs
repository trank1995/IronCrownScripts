using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target = null;
    public GameObject playerParty = null;
    public bool orbity = false;
    private Vector3 positionOffset = Vector3.zero;
    private Vector3 positionOffsetBase = Vector3.zero;
    private int multiplier;
    private string current_mode = "TOGGLE_TO_PLAYER";
    private Vector3 DEFAULT_OFFSET_BASE = new Vector3(2, 3f, -2);
    int counter;
    //free move paras
    float FREE_MOVE_SPEED = 30f;
    private float freeMoveYOffset;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis
    private void Start()
    {
        multiplier = 4;

        positionOffsetBase = DEFAULT_OFFSET_BASE;
        positionOffset = multiplier * positionOffsetBase;
        target = playerParty;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (target != null && WorldInteraction.playerControllable)
        {
            switch (current_mode)
            {
                case "TOGGLE_TO_PLAYER":

                    transform.LookAt(target.transform);
                    transform.position = Vector3.Slerp(transform.position, target.transform.position + positionOffset, Time.deltaTime * 3f);
                    zoom();
                    rotate();
                    break;
                case "FREE_MOVE":
                    if (Camera.current != null)
                    {
                        Vector3 dir = transform.forward;
                        if (Input.GetKey(KeyCode.W)) //forward
                        {
                            transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(dir.x, 0, dir.z), Time.deltaTime * FREE_MOVE_SPEED);
                        }
                        if (Input.GetKey(KeyCode.S)) //backward
                        {
                            transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(-dir.x, 0, -dir.z), Time.deltaTime * FREE_MOVE_SPEED);
                        }
                        if (Input.GetKey(KeyCode.A)) //left
                        {
                            transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(-dir.z, 0, dir.x), Time.deltaTime * FREE_MOVE_SPEED);
                        }
                        if (Input.GetKey(KeyCode.D)) //right
                        {
                            transform.position = Vector3.Slerp(transform.position, transform.position + new Vector3(dir.z, 0, -dir.x), Time.deltaTime * FREE_MOVE_SPEED);
                        }

                    }
                    //freeMove();
                    freeMoveRotate();
                    if (!TroopPlacing.pointerInPlacingPanel)
                    {
                        freeMoveZoom();
                    }
                    //freeMove();
                    //zoom();
                    break;
                case "RANDOM_MAP_OBJECT":
                    break;

            }


            if (Input.GetKey(KeyCode.Space) && current_mode != "TOGGLE_TO_PLAYER")
            {
                current_mode = "TOGGLE_TO_PLAYER";
                positionOffsetBase = DEFAULT_OFFSET_BASE;
                target = playerParty;
                positionOffset = 4 * positionOffsetBase;
            }
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && current_mode != "FREE_MOVE")
            {
                current_mode = "FREE_MOVE";
            }
            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                current_mode = "TOGGLE_TO_PLAYER";
                Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit interactionInfo;
                if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
                {

                    GameObject interactedObject = interactionInfo.collider.gameObject;
                    if (interactedObject.tag == "Interactable Object" || interactedObject.tag == "NPC" || interactedObject.tag == "City" || interactedObject.tag == "Town")
                    {
                        target = interactedObject;
                    }
                }
            }
        }
    }

    private void zoom()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f) //scroll up
        {
            multiplier -= 2;
            positionOffsetBase.y = Mathf.Clamp(positionOffsetBase.y - 1f, DEFAULT_OFFSET_BASE.y - 2, DEFAULT_OFFSET_BASE.y + 2);
        }
        if (d < 0f)
        {
            multiplier += 2;
            positionOffsetBase.y = Mathf.Clamp(positionOffsetBase.y + 1f, DEFAULT_OFFSET_BASE.y - 2, DEFAULT_OFFSET_BASE.y + 2);
        }
        multiplier = Mathf.Clamp(multiplier, 1, 4);
        positionOffset = multiplier * positionOffsetBase;
    }
    private void rotate()
    {

        if (Input.mousePosition.y > Screen.height - 2) //up, camera zoom in
        {
            counter++;
            if (counter == 4)
            {
                multiplier = Mathf.Clamp(multiplier - 1, 1, 4);
                positionOffsetBase.y = Mathf.Clamp(positionOffsetBase.y - .5f, DEFAULT_OFFSET_BASE.y - 2, DEFAULT_OFFSET_BASE.y + 2);
                counter = 0;
            }
        }
        if (Input.mousePosition.y < 2) //down, camera zoom out
        {
            counter++;
            if (counter == 4)
            {
                multiplier = Mathf.Clamp(multiplier + 1, 1, 4);
                positionOffsetBase.y = Mathf.Clamp(positionOffsetBase.y + .5f, DEFAULT_OFFSET_BASE.y - 2, DEFAULT_OFFSET_BASE.y + 2);
                counter = 0;
            }
        }
        if (Input.mousePosition.x < 2) //left
        {
            positionOffsetBase = Quaternion.Euler(0, 1, 0) * positionOffsetBase;

        }
        if (Input.mousePosition.x > Screen.width - 2) //right
        {
            positionOffsetBase = Quaternion.Euler(0, -1, 0) * positionOffsetBase;
        }

        positionOffset = multiplier * positionOffsetBase;
    }


    private void freeMove()
    {
        float speed = 20f;
        var d = Input.GetAxis("Mouse ScrollWheel");

        //WASD
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        if (Camera.current != null)
        {
            //transform.Translate(transform.forward*Time.deltaTime*20);
            var curPos = transform.position.y;
            transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
            var pos = transform.position;

            pos.y = Mathf.Clamp(transform.position.y, curPos, curPos);
            transform.position = pos;
        }


        if (d > 0f && transform.position.z < 200) //scroll up
        {
            target.transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));

        }
        if (d < 0f && transform.position.z > 4)
        {
            target.transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
    }
    private void freeMoveZoom()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f) //scroll up
        {
            freeMoveYOffset -= 3;
            freeMoveYOffset = Mathf.Clamp(freeMoveYOffset, 5, 40);
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, freeMoveYOffset, transform.position.z), Time.deltaTime * 10f);
        }
        if (d < 0f)
        {
            freeMoveYOffset += 3;
            freeMoveYOffset = Mathf.Clamp(freeMoveYOffset, 5, 40);
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, freeMoveYOffset, transform.position.z), Time.deltaTime * 10f);
        }

    }
    private void freeMoveRotate()
    {
        if ((Input.mousePosition.x > Screen.width - 2) || (Input.GetKey(KeyCode.E))) //right
        {
            rotY += 100f * Time.deltaTime;
            //rotX += 10f * mouseSensitivity * Time.deltaTime;
        }
        if (Input.mousePosition.x < 2 || (Input.GetKey(KeyCode.Q))) //left
        {
            rotY -= 100f * Time.deltaTime;
            //positionOffsetBase = Quaternion.Euler(0, 1, 0) * positionOffsetBase;
        }
        if (Input.mousePosition.y > Screen.height - 2) //up, camera zoom in
        {
            rotX -= 20f * Time.deltaTime;
        }
        if (Input.mousePosition.y < 2) //down, camera zoom out
        {
            rotX += 20f * Time.deltaTime;
        }

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}
