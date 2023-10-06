using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandlerController : MonoBehaviour
{
    Vector3 mousePosition;
    bool mouseDown;

    float cameraExtendVertical;

    float cameraExtendHorizontal;

    float topBound, leftBound, bottomBound, rightBound;

    GridController controller;

    GameObject cameraObject;

    GameObject grabbedEntity;
    Vector2 entityOrigin;



    // Start is called before the first frame update
    void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
        cameraExtendVertical = cameraObject.GetComponent<Camera>().orthographicSize;
        cameraExtendHorizontal = (cameraExtendVertical * Screen.width) / Screen.height;
        controller = gameObject.GetComponent<GridController>();
        controller.getGridBounds(out topBound, out leftBound, out bottomBound, out rightBound);

        grabbedEntity = null;

    }

    // Update is called once per frame
    void LateUpdate(){
        HandleMoveCamera();
    }

    void Update()
    {
        if (!HandleRemoveFog()){
            HandleDoors();
        }
        HandleMoveEntity();
        checkForSpace();
    }

    void HandleDoors(){
        if (Input.GetMouseButtonDown(1) && !controller.isInPlayerView()){
            DoorController doorController = controller.GetBackgroundObject(controller.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition))).GetComponent<DoorController>();

            if (doorController == null) return;

            doorController.Switch();

        }
    }

    bool HandleRemoveFog()
    {
        if (Input.GetMouseButtonDown(1) && !controller.isInPlayerView()){
            return controller.HandleFog(controller.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
        return false;
    }

    void checkForSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.ChangePlayerDMView();
        }
    }


    private void HandleMoveEntity()
    {
        if (Input.GetMouseButtonDown(0))
        {
            entityOrigin = controller.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (entityOrigin == new Vector2(-1, -1)) return;

            GameObject gameObject = controller.GetForegroundObject(entityOrigin);

            if (gameObject == null) return;

            EntityController entityController = gameObject.GetComponent<EntityController>();

            if (entityController == null) return;
            if (entityController.canBeMovedByPlayer() || !controller.isInPlayerView()){
                grabbedEntity = gameObject;
            }

            
        }

        if (Input.GetMouseButton(0) && grabbedEntity != null)
        {
            grabbedEntity.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -5);
        }
        else if (grabbedEntity != null)
        {
            Vector2 destination = controller.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            controller.DropEntity(grabbedEntity, entityOrigin, destination);

            grabbedEntity = null;
        }


    }


    // This is a separate function, 
    // so that the movement of players/monsters can be added as another function 
    // without requiring too much of a rewrite

    void HandleMoveCamera(){
        if (mouseDown){
                Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 distance = currentMousePos - mousePosition;
                Vector3 cameraPosition = cameraObject.transform.position;
                cameraPosition -= distance;

                cameraPosition.x = Mathf.Min(rightBound, Mathf.Max(leftBound, cameraPosition.x));
                cameraPosition.y = Mathf.Min(topBound, Mathf.Max(bottomBound, cameraPosition.y));

                cameraObject.transform.position = cameraPosition;
            }


            if (OverMap() && Input.GetMouseButtonDown(0) && grabbedEntity == null){
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseDown = true;
            }
            else if (!Input.GetMouseButton(0)){
                mouseDown = false;
            }
        }

        // This just makes sure that the camera is only moved when map is dragged
        bool OverMap(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);;
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
		    {
			    return hit.collider.name == "Map";
		    }

            return false;
        }
    }
