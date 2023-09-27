using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandlerController : MonoBehaviour
{
    public GameObject map;
    Vector3 mousePosition;
    bool mouseDown;

    float cameraExtendVertical;

    float cameraExtendHorizontal;

    float topBound, leftBound, bottomBound, rightBound;


    GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = GameObject.Find("Main Camera");
        cameraExtendVertical = cameraObject.GetComponent<Camera>().orthographicSize;
        cameraExtendHorizontal = (cameraExtendVertical * Screen.width) / Screen.height;
        gameObject.GetComponent<GridController>().getGridBounds(out topBound, out leftBound, out bottomBound, out rightBound);
    }

    // Update is called once per frame
    void LateUpdate(){
        HandleMoveCamera();
    }

    void Update()
    {
        HandleRemoveFog();

    }

    void HandleRemoveFog()
    {
        if (Input.GetMouseButtonDown(1))
            gameObject.GetComponent<GridController>().HandleFog(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

        // This just makes sure that the camera is only moved when map is dragged
        bool OverMap(){
            //Mesh mesh = map.GetComponent<MeshFilter>().mesh;
            //Vector3[] vert = mesh.vertices;
            
            //Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            return false;//Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask);
            //Ray ray = map.ScreenToWorldPoint(Input.mousePosition);
            //return Physics.Raycast(ray, out hit);
        }
    }
