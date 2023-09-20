using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandlerController : MonoBehaviour
{

    Vector3 mousePosition;
    bool mouseDown;

    GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void LateUpdate(){
        HandleMoveCamera();
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
