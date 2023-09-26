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
    public int width;
    public int height;
    public bool hasPath = false;

    private int x;
    private int y;

    private System.Random random;


    public RoomController()
    {   
        random = new System.Random();
        //TODO: Also make sure that the shapes are not overlapping
    }


    public void SetPosition(int x, int y){
        this.x = x;
        this.y = y;
    }

    /**
     * Place all of the tiles and props into the game, and also insert them into the arrays for foreground and background objects.
     */
    public void PlaceRoom(Transform transformParent, GameObject[,] background, GameObject[,] foregound, float size, float margin, Recorder recorder)
    {

        //Place the floors for each shape that makes up the room
        foreach (Shape shape in shapes)
        {
            for (int x = shape.x1;  x < shape.x2; x++)
            {
                for (int y = shape.y1;  y < shape.y2; y++)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3((x + this.x) * (size + margin), (y + this.y) * (size + margin), 0), Quaternion.identity, transformParent);
                    tile.GetComponent<TileController>().Init(size - margin * 2);
                    background[x + this.x, y + this.y] = tile;
                    recorder.AddTile(new RecorderTile("floor", x + this.x, y + this.y));
                }
            }
        }
        
        //Place the doors for each door in the room
        foreach (Vector2 doorLoc in doors)
        {
            GameObject door = Instantiate(doorPrefab, new Vector3((doorLoc.x + this.x) * (size + margin), (doorLoc.y + this.y) * (size + margin), 1), Quaternion.identity, transformParent);
            door.GetComponent<TileController>().Init(size - margin * 2);
            door.GetComponent<DoorController>().SetParent(this);
            background[(int)doorLoc.x + this.x, (int)doorLoc.y + this.y] = door;
            recorder.AddTile(new RecorderTile("door", (int)doorLoc.x + this.x, (int)doorLoc.y + this.y));
        }
        
        //Randomly select props for each prop location in the room.
        foreach (Shape propLocation in props)
        {


            GameObject prop = Instantiate(propOptions[random.Next(propOptions.Length)], new Vector3((propLocation.x1 + this.x) * (size + margin), (propLocation.y1 + this.y) * (size + margin), -1), Quaternion.identity, transformParent);
            prop.GetComponent<PropController>().Init(size - margin * 2);
            foregound[propLocation.x1 + this.x, propLocation.y1 + this.y] = prop;
            recorder.AddTile(new RecorderTile("prop", propLocation.x1 + this.x, propLocation.y1 + this.y));
        }
    }

    public void setHasPathTrue()
    {
        this.hasPath = true;
    }

    public int GetX(){
        return this.x;
    }

    public int GetY(){
        return this.y;
    }
  
}
