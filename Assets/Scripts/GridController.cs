using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/*
 * Controller for a Grid of tiles and props.
 */
public class GridController : MonoBehaviour
{

    public int width;
    public int height;
    public float cellSize; //Cells will always be this size, including spacing.
    public float cellSpacing;
    private GameObject[,] backgroundLayer; //Layer for tiles like walls, floors, doors.
    private GameObject[,] foregroundLayer; //Layer for props and entities like players and monsters

    public GameObject[] rooms; //Each instance of a room object is stored here
    public GameObject wallTile;

    private Recorder recorder; //Records events to save the current game state
    public GameObject placeHolderTile;


    public void Awake()
    {
        this.recorder = new Recorder(this);

        backgroundLayer = new GameObject[width, height];

        foregroundLayer = new GameObject[width, height];

        //Updates the arrays with the generated dungeon values
        DungeonGenerator dungeonGenerator = gameObject.GetComponent<DungeonGenerator>();
        rooms = dungeonGenerator.GenerateDungeon(rooms, width, height);

        for (int i = 0; i < rooms.Length; i++) //Place each room in the Grid
        {
            //int offsetx = xOffsets[i];
            rooms[i].GetComponent<RoomController>().PlaceRoom(gameObject.transform, backgroundLayer, foregroundLayer, cellSize, cellSpacing, recorder);
        }

        List<Vector2> wallLocations = new();

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (shouldBeWall(x, y))
                { //Place walls in locations that are next to floors or doors.
                    wallLocations.Add(new Vector2(x, y));
                }
            }
        }

        foreach (Vector2 wallLocation in wallLocations)
        {
            int x = (int)wallLocation.x;
            int y = (int)wallLocation.y;
            GameObject wall = Instantiate(wallTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
            wall.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
            backgroundLayer[x, y] = wall;
            recorder.AddTile(new RecorderTile("wall", x, y));
        }

        //instantiates the default background objects as the backgroundlayer is created
        //for (int x = 0; x < width; x++) //for each grid cell across the whole map
        //{
            //for (int y = 0; y < height; y++) //for each grid cell down the whole map
            //{
                //if (backgroundLayer[x, y] != null) continue; //If there is already a tile here, skip it.
                //GameObject tile = Instantiate(placeHolderTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                //tile.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
                //backgroundLayer[x, y] = tile;
                //recorder.AddTile(new RecorderTile("defaultTile", x, y));
            //}
        //}

        gameObject.transform.position -= new Vector3(width * cellSize / 2, width * cellSize / 2, 0); //Try to center the grid in the game space.
        PathGenerator pathGen = gameObject.GetComponent<PathGenerator>();
        pathGen.main(backgroundLayer, rooms, width, height);    
    }

    /*
     * Helpers
     */
    private Vector2 GetWorldLocation(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }

    /*
     * Returns the current Recorder
     */
    public Recorder GetRecorder()
    {
        return this.recorder;
    }

    /**
     * Checks if a location should be a wall by checking if any of the 8 adjacent tiles are not null.
     */
    private bool shouldBeWall(int x, int y)
    {

        if (backgroundLayer[x, y] != null) return false;

        for (int xi = x-1; xi <= x+1; xi++)
        {
            if (xi < 0 || xi >= width) continue;
            for (int yi = y-1; yi <= y+1; yi++)
            {
                if (yi < 0 || yi >= height || (yi == y && xi == x)) continue;
                if (backgroundLayer[xi, yi] == null) continue;
                DoorController doorTile = backgroundLayer[xi, yi].GetComponent<DoorController>();
                if (doorTile != null) continue;
                return true;
            }
        }


        return false;
    }

}
