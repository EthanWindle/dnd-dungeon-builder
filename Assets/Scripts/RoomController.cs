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
    public GameObject propPrefab; //TODO: make this a list of possible props - ideally a global list of some kind that is shared between rooms?
    public Shape[] shapes;
    public Shape[] props; //For now props are assumed to be 1x1.
    public Vector2[] doors; //Should be updated to being possible door locations.


    private int maxWidth;
    private int maxHeight;


    void Awake()
    {
        maxWidth = 0;
        maxHeight = 0;
        foreach (Shape shape in shapes)
        {
            maxWidth = Mathf.Max(maxWidth, shape.x1);
            maxWidth = Mathf.Max(maxWidth, shape.x2);
            maxHeight = Mathf.Max(maxHeight, shape.y1);
            maxHeight = Mathf.Max(maxHeight, shape.y2);
        }

        //TODO: Also make sure that the shapes are not overlapping
    }

    /**
     * Place all of the tiles and props into the game, and also insert them into the arrays for foreground and background objects.
     */
    public void PlaceRoom(Transform transformParent, GameObject[,] background, GameObject[,] foregound, float size, float margin)
    {


        

        foreach (Shape shape in shapes)
        {
            for (int x = shape.x1;  x < shape.x2; x++)
            {
                for (int y = shape.y1;  y < shape.y2; y++)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3(x * (size + margin), y * (size + margin), 0), Quaternion.identity, transformParent);
                    tile.GetComponent<TileController>().Init(size - margin * 2);
                    background[x, y] = tile;
                }
            }
        }


        foreach (Vector2 doorLoc in doors)
        {
            GameObject door = Instantiate(doorPrefab, new Vector3(doorLoc.x * (size + margin), doorLoc.y * (size + margin), 1), Quaternion.identity, transformParent);
            door.GetComponent<TileController>().Init(size - margin * 2);
            background[(int)doorLoc.x, (int)doorLoc.y] = door;
        }

        foreach(Shape propLocation in props)
        {
            GameObject prop = Instantiate(propPrefab, new Vector3(propLocation.x1 * (size + margin), propLocation.y1 * (size + margin), -1), Quaternion.identity, transformParent);
            prop.GetComponent<PropController>().Init(size - margin * 2);
            foregound[propLocation.x1, propLocation.y1] = prop;
        }
    }

    
}
