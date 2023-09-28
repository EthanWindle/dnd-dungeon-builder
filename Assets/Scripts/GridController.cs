using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

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
    private GameObject[,] fogLayer; //Layer for the fog

    public GameObject[] rooms; //Each instance of a room object is stored here
    public int[] xOffsets; //x location of room, correlates with rooms array
    public int[] yOffsets; //y location of room, correlates with rooms array
    public GameObject wallTile;
    public GameObject playerEntity;


    private Recorder recorder; //Records events to save the current game state
    private GameObject tilePrefab;
    private GameObject doorPrefab;

    private bool inPlayerView = false;

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
        fogLayer = new GameObject[width, height];

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

        fogLayer = new GameObject[width, height];

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
            rooms[i].GetComponent<RoomController>().PlaceRoom(gameObject.transform, backgroundLayer, foregroundLayer, fogLayer, cellSize, cellSpacing, recorder);
            RoomController roomController = rooms[i].GetComponent<RoomController>();
            this.tilePrefab = roomController.tilePrefab;
            this.doorPrefab = roomController.doorPrefab;
        }

        List<Vector2> wallLocations = new();

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (ShouldBeWall(x, y))
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
            wall.GetComponent<WallController>().Init(cellSize - cellSpacing * 2);
            wall.GetComponent<WallController>().SetTexture(GetAdjacentControllers(x, y));
            backgroundLayer[x, y] = wall;
            recorder.AddTile(new RecorderTile("wall", x, y, -1));
        }

        Vector2 playerPosition =  PlacePlayer();

        
        PathGenerator pathGen = gameObject.GetComponent<PathGenerator>();
        pathGen.main(backgroundLayer, rooms, width, height);
        gameObject.transform.position -= new Vector3(playerPosition.x * cellSize, playerPosition.y * cellSize, 0); //Try to center the grid in the game space.    
        
        //Test load from file
        //Recorder deserializedRecorder = GridControllerJsonSerializer.DeserializeFromJson("testFile2.json");
        //SetObjects(deserializedRecorder);

        //Test save to file
        //GridControllerJsonSerializer.SerializeToJson(this, "testFile2.json", recorder);

    }

    

    private Vector2 PlacePlayer(){
        GameObject firstRoom = rooms[0];
        RoomController firstRoomController = firstRoom.GetComponent<RoomController>();

        for (int x = firstRoomController.GetX(); x < firstRoomController.width + firstRoomController.GetX(); x++){
            for (int y = firstRoomController.GetY(); y < firstRoomController.height + firstRoomController.GetY(); y++){
                if (backgroundLayer[x,y] != null && backgroundLayer[x,y].GetComponent<TileController>() is FloorController && foregroundLayer[x,y] == null){
                    GameObject player = Instantiate(playerEntity,new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 0), Quaternion.identity, gameObject.transform);
                    foregroundLayer[x,y] = player;
                    player.GetComponent<PlayerController>().Init(cellSize - cellSpacing * 2);
                    firstRoomController.HideTiles();
                    return new Vector2(x,y);
                }
            }
        }

        return new Vector2(-1,-1);
    }


    /*
     * Helpers
     */
    private Vector3 GetWorldLocation(int x, int y)
    {
        return new Vector3(x * cellSize + gameObject.transform.position.x, y * cellSize + gameObject.transform.position.y, -0.5f);
    }


    private Vector3 GetWorldLocation(Vector2 position)
    {
        return GetWorldLocation((int)position.x, (int)position.y);
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
    private bool ShouldBeWall(int x, int y)
    {

        if (backgroundLayer[x, y] != null) return false;

        
        foreach (TileController controller in GetAdjacentControllers(x, y))
        {
            if (controller is FloorController) { return true; }
        }

        return false;
    }

    private TileController[] GetAdjacentControllers(int x, int y)
    {
        int index = 0;
        TileController[] controllers = new TileController[8];
        for (int xi = x - 1; xi <= x + 1; xi++)
        {
            
            for (int yi = y - 1; yi <= y + 1; yi++)
            {
                if (xi == x && yi == y) continue;

                if (xi < 0 || xi >= width) controllers[index] = null;
                else if (yi < 0 || yi >= height) controllers[index] = null;
                else if (backgroundLayer[xi, yi] == null) controllers[index] = null;
                else controllers[index] = backgroundLayer[xi, yi].GetComponent<TileController>();
                index++;
                
            }
        }

        return controllers;
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
        if (!inPlayerView) inPlayerView = true;
        else return;
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().ShowTiles();
        }
    }

    public void ChangeToDMView()
    {
        if (inPlayerView) inPlayerView = false;
        else return;
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().HideTiles();
        }
    }

    public bool isInPlayerView()
    {
        return inPlayerView;
    }

    public Vector2 GetGridPosition(Vector3 worldPosition)
    {

        if (worldPosition.x > gameObject.transform.position.x + (width * cellSize)
            || worldPosition.y > gameObject.transform.position.y + (height * cellSize)
            || worldPosition.x < gameObject.transform.position.x
            || worldPosition.y < gameObject.transform.position.y) return new Vector2(-1, -1);

        return new Vector2(Mathf.Ceil((worldPosition.x - gameObject.transform.position.x) / (cellSize) - 0.5f * cellSize),
            Mathf.Ceil(((worldPosition.y - gameObject.transform.position.y) / cellSize) - 0.5f * cellSize));
    }


    public GameObject GetForegroundObject(Vector2 position)
    {
        return foregroundLayer[(int)position.x, (int)position.y];
    }

    public GameObject GetBackgroundObject(Vector2 position)
    {
        return backgroundLayer[(int)position.x, (int)position.y];
    }

    private bool FogIsActive(Vector2 position)
    {
        return fogLayer[(int)position.x, (int)position.y].activeSelf;
    }
    public GameObject grabEntity(Vector2 position){
        if (isInPlayerView() && FogIsActive(position)) return null;
        return GetForegroundObject(position);

    }


    public void DropEntity(GameObject entity, Vector2 origin, Vector2 destination)
    {
        if (entity != GetForegroundObject(origin)) throw new ArgumentException("Dropped entity has the wrong origin position");

        if (GetBackgroundObject(destination) == null //There is no tile at destination
            || !GetBackgroundObject(destination).GetComponent<TileController>().CanEnter() //The destination cannot be entered (i.e. is a wall or closed door)
            || GetForegroundObject(destination) != null
            || (isInPlayerView() && FogIsActive(destination))) //There is already a prop or other entity at the destination.
        {
            entity.transform.position = GetWorldLocation(origin);
        }
        else
        {
            foregroundLayer[(int)origin.x, (int)origin.y] = null;
            foregroundLayer[(int)destination.x, (int)destination.y] = entity;

            entity.transform.position = GetWorldLocation(destination);
        }
    }

}
