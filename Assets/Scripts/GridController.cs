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
    public int[] xOffsets; //x location of room, correlates with rooms array
    public int[] yOffsets; //y location of room, correlates with rooms array
    public GameObject wallTile;

    private Recorder recorder; //Records events to save the current game state
    private GameObject tilePrefab;
    private GameObject doorPrefab;

    /*
     * Loads a save file from recorder
     */
    public void SetObjects(Recorder deserializedRecorder)
    {
        width = deserializedRecorder.width;
        height = deserializedRecorder.height;
        cellSize = deserializedRecorder.cellSize;
        cellSpacing = deserializedRecorder.cellSpacing;

        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        backgroundLayer = new GameObject[width, height];
        foregroundLayer = new GameObject[width, height];

        rooms = new GameObject[30];
        xOffsets = new int[30];
        yOffsets = new int[30];

        foreach (RecorderTile tile in deserializedRecorder.tiles)
        {
            if (tile.type.Equals("floor"))
            {
                // Update the backgroundLayer with floor tiles
                GameObject floorTile = Instantiate(tilePrefab, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), 0), Quaternion.identity, gameObject.transform);
                floorTile.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
                backgroundLayer[tile.x, tile.y] = floorTile;
            }
            else if (tile.type.Equals("wall"))
            {
                // Update the backgroundLayer with wall tiles
                wallTile = Instantiate(wallTile, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                wallTile.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
                backgroundLayer[tile.x, tile.y] = wallTile;
            }
            else if (tile.type.Equals("door"))
            {
                // Update the backgroundLayer with door tiles
                doorPrefab = Instantiate(doorPrefab, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                doorPrefab.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
                backgroundLayer[tile.x, tile.y] = doorPrefab;
            }
        }

    }

    public void Awake()
    {
        backgroundLayer = new GameObject[width, height];

        foregroundLayer = new GameObject[width, height];

        GenerateNewMap();
    }

    public void GenerateNewMap()
    {
        this.recorder = new Recorder(this);
        //Updates the arrays with the generated dungeon values
        DungeonGenerator dungeonGenerator = gameObject.GetComponent<DungeonGenerator>();
        rooms = dungeonGenerator.GenerateDungeon(rooms, width, height);

        for (int i = 0; i < rooms.Length; i++) //Place each room in the Grid
        {
            //int offsetx = xOffsets[i];
            rooms[i].GetComponent<RoomController>().PlaceRoom(gameObject.transform, backgroundLayer, foregroundLayer, cellSize, cellSpacing, recorder);
            RoomController roomController = rooms[i].GetComponent<RoomController>();
            this.tilePrefab = roomController.tilePrefab;
            this.doorPrefab = roomController.doorPrefab;
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
            recorder.AddTile(new RecorderTile("wall", x, y, -1));
        }

        gameObject.transform.position -= new Vector3(width * cellSize / 2, width * cellSize / 2, 0); //Try to center the grid in the game space.

        //Test load from file
        //Recorder deserializedRecorder = GridControllerJsonSerializer.DeserializeFromJson("testFile2.json");
        //SetObjects(deserializedRecorder);
        
        //Test save to file
        //GridControllerJsonSerializer.SerializeToJson(this, "testFile2.json", recorder);

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


    public void getGridBounds(out float topBound, out float leftBound, out float bottomBound, out float rightBound){
        topBound = gameObject.transform.position.y + (height * cellSize);
        leftBound = gameObject.transform.position.x;
        bottomBound = gameObject.transform.position.y;
        rightBound = gameObject.transform.position.x + (width * cellSize);
    }

    public void HandleFog(Vector3 mousePos)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().ClearFog(mousePos);
        }
    }

    public void ChangeToPlayerView()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().ShowTiles();
        }
    }

    public void ChangeToDMView()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().HideTiles();
        }
    }

}
