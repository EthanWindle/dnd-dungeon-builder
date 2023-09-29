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
    public GameObject fogPrefab;
    public GameObject[] propOptions;
    public GameObject[] monsterOptions;


    private GameObject[,] fogLayer;
    public Shape[] shapes;
    public Shape[] props; //For now props are assumed to be 1x1.
    public Vector2[] monsters; //locations for monsters.
    public Vector2[] doors; //Should be updated to being possible door locations.
    public int width;
    public int height;
    public bool hasPath = false;

    private int x;
    private int y;
    private Boolean hidden = false;

    private System.Random random;

    private static int roomCount = 0; //Counts the number for each room for recorder, do not modify for anything else


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
    public void PlaceRoom(Transform transformParent, GameObject[,] background, GameObject[,] foreground, GameObject[,] gridFogLayer, float size, float margin, Recorder recorder)
    {
        recorder.AddRoom(new RecorderRoom(++roomCount, this.x, this.y));
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
                    recorder.AddTile(new RecorderTile("floor", x + this.x, y + this.y, roomCount));
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
            recorder.AddTile(new RecorderTile("door", (int)doorLoc.x + this.x, (int)doorLoc.y + this.y, roomCount));
        }
        
        //Randomly select props for each prop location in the room.
        foreach (Shape propLocation in props)
        {


            GameObject prop = Instantiate(propOptions[random.Next(propOptions.Length)], new Vector3((propLocation.x1 + this.x) * (size + margin), (propLocation.y1 + this.y) * (size + margin), -1), Quaternion.identity, transformParent);
            prop.GetComponent<PropController>().Init(size - margin * 2);
            foreground[propLocation.x1 + this.x, propLocation.y1 + this.y] = prop;
            recorder.AddTile(new RecorderTile("prop", propLocation.x1 + this.x, propLocation.y1 + this.y, roomCount));
        }


        foreach (Vector2 monsterLoc in monsters)
        {
            GameObject monster = Instantiate(monsterOptions[random.Next(monsterOptions.Length)], new Vector3((monsterLoc.x + this.x) * (size + margin), (monsterLoc.y + this.y) * (size + margin), -1), Quaternion.identity, transformParent);
            monster.GetComponent<MonsterController>().Init(size - margin * 2);
            foreground[(int)(monsterLoc.x + x), (int)(monsterLoc.y + y)] = monster;
            //TODO add recorder stuff 
        }

        fogLayer = new GameObject[width + 2, height + 2];
        /*
        int wStart = 0;
        if (this.x > 0) wStart = -1;
        int wEnd = width;
        if (width < gridWidth) wEnd = width + 1;
        int hStart = 0;
        if (this.y > 0) hStart = -1;
        int hEnd = 0;
        if (height < gridHeight) hEnd = height + 1;

        while (wStart < wEnd)
        {
            while (hStart < hEnd)
            {
                GameObject fog = Instantiate(fogPrefab, new Vector3((wStart + this.x) * (size + margin), (hStart + this.y) * (size + margin), 0), Quaternion.identity, transformParent);
                fog.GetComponent<TileController>().Init(size - margin * 2);
                fogLayer[wStart + 1, hStart + 1] = fog;
                Debug.Log("index " + wStart + " : " + hStart + " x: " + this.x + " y " + this.y);
                gridFogLayer[wStart + this.x, hStart + this.y] = fog;
                hStart++;
            }
            wStart++;
        }
        */

        
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                GameObject fog = Instantiate(fogPrefab, new Vector3((w + this.x) * (size + margin), (h + this.y) * (size + margin), 0), Quaternion.identity, transformParent);
                fog.GetComponent<TileController>().Init(size - margin * 2);
                fogLayer[w, h] = fog;
                gridFogLayer[w + this.x, h + this.y] = fog;
            }
        }
        
        
    }

    /*
     * Removes the fog if point is inside room
     */
    public void ClearFog(Vector3 mousePos, GameObject[,] gridFogLayer, int gridWidth, int gridHeight)
    {
        if (InsideRoom(mousePos) && !hidden)
        {
            hidden = true;
            HideFogTiles(gridFogLayer, gridWidth, gridHeight);
            //fogLayer = new GameObject[width, height];
        }
    }

    /*
     * Checks if point is within this room
     */
    public Boolean InsideRoom(Vector3 mousePos)
    {
        if (mousePos.x + 35 <= (this.x + this.width)
            && mousePos.x + 35 >= this.x
            && mousePos.y + 35 <= (this.y + this.height)
            && mousePos.y + 35 >= this.y)
        {
            return true;
        }
        return false;
    }

    /*
     * Hides all fog tiles
     */
    public void HideFogTiles(GameObject[,] gridFogLayer, int gridWidth, int gridHeight)
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                fogLayer[w, h].SetActive(false);
            }
        }

        for (int w = -1; w < width + 1; w++)
        {
            for (int h = -1; h < height + 1; h++)
            {
                if (w > -1 && w < width && h > -1 && h < height) continue;
                gridFogLayer[w + this.x, h + this.y].GetComponent<FogController>().lowerOpacity();
            }
        }
        /*
        int wStart = 0;
        if (this.x > 0) wStart = -1;
        int wEnd = width;
        if (width < gridWidth) wEnd = width + 1;
        int hStart = 0;
        if (this.y > 0) hStart = -1;
        int hEnd = 0;
        if (height < gridHeight) hEnd = height + 1;


        while (wStart < wEnd)
        {
            while (hStart < hEnd)
            {
                if (wStart  > -1 && wStart < width && hStart > -1 && hStart < height) continue;
                gridFogLayer[wStart + this.x, hStart + this.y].GetComponent<FogController>().lowerOpacity();
                hStart++;
            }
            wStart++;
        }
        */

        /*
        for (int w = -1; w < width + 1; w++)
        {
            for (int h = -1; h < height + 1; h++)
            {

                if (w > -1 && w < width && h > -1 && h < height) fogLayer[w, h].SetActive(false);
                else fogLayer[w, h].GetComponent<FogController>().lowerOpacity();

                //fogLayer[w, h].GetComponent<Renderer>().material.color.a = 20;
                //fogLayer[w, h].SetActive(false);
                    //fogLayer[w, h].GetComponent<FogController>().lowerOpacity();
                //.lowerOpacity();
                // gameObject.GetComponent<Renderer>().material.color.a = 0
            }
        }
        */
    }

    /* Need to:
     * Generate map of fog tiles in gridcontroller which covers whole map
     * Create function in gridcontroller to show/hide these for dm/player (function is kinda already existing)
     *  - Will no longer need to show/hide in the individual room for dm/player
     * Roomcontroller will contain an array of references to the objects stored in grid so the grid show/hide still works
     * - array will need to be 1 greater in each direction to have the low opacity tile
     * Rooms will also have to deal with removing fog for neighbouring paths
     * 
     */

    /*
     * Shows all fog tiles
     */
    public void ShowFogTiles(GameObject[,] gridFogLayer, int gridWidth, int gridHeight)
    {
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                fogLayer[w, h].SetActive(true);
            }
        }
        /*
        for (int w = -1; w < width + 1; w++)
        {
            for (int h = -1; h < height + 1; h++)
            {
                if (w > -1 && w < width && h > -1 && h < height) continue;
                gridFogLayer[w + this.x, h + this.y].GetComponent<FogController>().raiseOpacity();
            }
        }
        */
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
