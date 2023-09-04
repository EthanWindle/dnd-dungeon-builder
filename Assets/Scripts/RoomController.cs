using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * Controller for a Room. Contains a list of shapes and a list of prop locations
 */
public class RoomController : MonoBehaviour
{

    public GameObject tilePrefab;
    public GameObject doorPrefab;
    public GameObject[] propOptions;
    public Shape[] shapes;
    public Shape[] props; //For now props are assumed to be 1x1.
    public Vector2[] doors; //Should be updated to being possible door locations.


    private System.Random random;


    public RoomController()
    {   
        random = new System.Random();
        //TODO: Also make sure that the shapes are not overlapping
    }

    /**
     * Place all of the tiles and props into the game, and also insert them into the arrays for foreground and background objects.
     */
    public void PlaceRoom(Transform transformParent, GameObject[,] background, GameObject[,] foregound, float size, float margin, int offsetX, int offsetY)
    {


        
        //Place the floors for each shape that makes up the room
        foreach (Shape shape in shapes)
        {
            for (int x = shape.x1;  x < shape.x2; x++)
            {
                for (int y = shape.y1;  y < shape.y2; y++)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3((x + offsetX) * (size + margin), (y + offsetY) * (size + margin), 0), Quaternion.identity, transformParent);
                    tile.GetComponent<TileController>().Init(size - margin * 2);
                    background[x + offsetX, y + offsetY] = tile;
                }
            }
        }

        //Place the doors for each door in the room
        foreach (Vector2 doorLoc in doors)
        {
            GameObject door = Instantiate(doorPrefab, new Vector3((doorLoc.x + offsetX) * (size + margin), (doorLoc.y + offsetY) * (size + margin), 1), Quaternion.identity, transformParent);
            door.GetComponent<TileController>().Init(size - margin * 2);
            background[(int)doorLoc.x + offsetX, (int)doorLoc.y + offsetY] = door;
        }
        //Randomly select props for each prop location in the room.
        foreach(Shape propLocation in props)
        {


            GameObject prop = Instantiate(propOptions[random.Next(propOptions.Length)], new Vector3((propLocation.x1 + offsetX) * (size + margin), (propLocation.y1 + offsetY) * (size + margin), -1), Quaternion.identity, transformParent);
            prop.GetComponent<PropController>().Init(size - margin * 2);
            foregound[propLocation.x1 + offsetX, propLocation.y1 + offsetY] = prop;
        }
    }

    
}
