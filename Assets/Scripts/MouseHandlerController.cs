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
    }

    // Update is called once per frame
    void LateUpdate(){
        HandleMoveCamera();
    }

    void Update()
    {
        HandleRemoveFog();
        HandleMoveEntity();
    }

    void HandleRemoveFog()
    {
        if (Input.GetMouseButtonDown(1))
            controller.HandleFog(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }




    private void HandleMoveEntity()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(controller.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
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


            if (Input.GetMouseButtonDown(0)){
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseDown = true;
            }
            else if (!Input.GetMouseButton(0)){
                mouseDown = false;
            }
        }
    }
