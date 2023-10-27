using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.MaterialProperty;
using UnityEngine.U2D;

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

    public Camera mainCamera;
    public GameObject playerClone;

    public GameObject[] rooms; //Each instance of a room object is stored here
    public int[] xOffsets; //x location of room, correlates with rooms array
    public int[] yOffsets; //y location of room, correlates with rooms array
    public GameObject wallTile;
    public GameObject playerEntity;
    public String spritesheetName;

    
    public GameObject fogTile;

    private Recorder recorder; //Records events to save the current game state
    private GameObject tilePrefab;
    private GameObject doorPrefab;
    private GameObject fogPrefab;
    private GameObject[] propOptions;
    private GameObject[] monsterOptions;
    private SpritesheetManager spritesheetManager;

    private bool inPlayerView = true;

    private CustomGeneration customGeneration = new CustomGeneration(GlobalVariables.getRoomCount(), GlobalVariables.hasMonsters(), GlobalVariables.hasProps()); //current default generation parameters

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
                floorTile.GetComponent<FloorController>().Init(cellSize - cellSpacing * 2, spritesheetManager);
                backgroundLayer[tile.x, tile.y] = floorTile;
            }
            else if (tile.type.Equals("wall"))
            {
                // Update the backgroundLayer with wall tiles
                GameObject newWallTile = Instantiate(wallTile, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                newWallTile.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
                backgroundLayer[tile.x, tile.y] = newWallTile;
            }
            else if (tile.type.Equals("door"))
            {
                // Update the backgroundLayer with door tiles
                GameObject newDoorPrefab = Instantiate(doorPrefab, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                newDoorPrefab.GetComponent<DoorController>().Init(cellSize - cellSpacing * 2, spritesheetManager);
                backgroundLayer[tile.x, tile.y] = newDoorPrefab;
            }
            else if (tile.type.Equals("prop"))
            {
                // Update the backgroundLayer with prop tiles
                String propPath = "Assets/Prefabs/Prop Prefabs/" + tile.option + ".prefab";
                GameObject propPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(propPath, typeof(GameObject));

                GameObject newPropPrefab = Instantiate(propPrefab, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), -2), Quaternion.identity, gameObject.transform);
                newPropPrefab.GetComponent<PropController>().Init(cellSize - cellSpacing * 2);
                foregroundLayer[tile.x, tile.y] = newPropPrefab;
            }
            else if (tile.type.Equals("monster"))
            {
                // Update the backgroundLayer with monster tiles
                String monsterPath = "Assets/Prefabs/Entity Prefabs/" + tile.option + ".prefab";
                GameObject mosterPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(monsterPath, typeof(GameObject));

                GameObject newMonsterPrefab = Instantiate(mosterPrefab, new Vector3(tile.x * (cellSize + cellSpacing), tile.y * (cellSize + cellSpacing), -2), Quaternion.identity, gameObject.transform);
                Debug.Log(newMonsterPrefab);
                newMonsterPrefab.GetComponent<MonsterController>().Init(cellSize - cellSpacing * 2);
                foregroundLayer[tile.x, tile.y] = newMonsterPrefab;
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = backgroundLayer[x, y];
                if (tile != null && tile.GetComponent<TileController>() is WallController)
                {
                    tile.GetComponent<WallController>().SetTexture(GetAdjacentControllers(x, y), spritesheetManager);
                }
            }
        }

    }

    public void Awake()
    {
        backgroundLayer = new GameObject[width, height];

        foregroundLayer = new GameObject[width, height];

        fogLayer = new GameObject[width, height];

        spritesheetManager = new SpritesheetManager(spritesheetName);

        GenerateNewMap();
    }

    public void GenerateNewMap()
    {
        this.recorder = new Recorder(this);
        //Updates the arrays with the generated dungeon values
        DungeonGenerator dungeonGenerator = gameObject.GetComponent<DungeonGenerator>();
        rooms = dungeonGenerator.GenerateDungeon(rooms, width, height, customGeneration);

        for (int i = 0; i < rooms.Length; i++) //Place each room in the Grid
        {
            //int offsetx = xOffsets[i];
            /*
            if (rooms[i] == null)
            {
                Debug.Log("rooms[" + i + "] is null");
            }
            if (gameObject == null)
            {
                Debug.Log("gameObject is null");
            }
            if (backgroundLayer == null)
            {
                Debug.Log("backgroundLayer is null");
            }
            if (foregroundLayer == null)
            {
                Debug.Log("foregroundLayer is null");
            }
            if (fogLayer == null)
            {
                Debug.Log("fogLayer is null");
            }
            if (customGeneration == null)
            {
                Debug.Log("customGeneration is null");
            }
            if (spritesheetManager == null)
            {
                Debug.Log("spritesheetManager is null");
            }*/

            rooms[i].GetComponent<RoomController>().PlaceRoom(gameObject.transform, backgroundLayer, foregroundLayer, fogLayer, cellSize, cellSpacing, recorder, customGeneration, spritesheetManager);
            RoomController roomController = rooms[i].GetComponent<RoomController>();
            this.tilePrefab = roomController.tilePrefab;
            this.doorPrefab = roomController.doorPrefab;
            this.fogPrefab = roomController.fogPrefab;
            this.propOptions = roomController.propOptions;
            this.monsterOptions = roomController.monsterOptions;
        }

        PlaceWalls();

        PathGenerator pathGen = gameObject.GetComponent<PathGenerator>();
        pathGen.ConnectAllRooms(backgroundLayer, rooms, width, height, gameObject.transform, fogLayer, cellSize, cellSpacing, spritesheetManager);

        PlaceWalls();

        pathGen.CreatePathFog(backgroundLayer, width, height, gameObject.transform, fogLayer, cellSize, cellSpacing);
    
        PlaceBackgroundFog();
        Vector2 playerPosition = PlacePlayer();

        gameObject.transform.position -= new Vector3(0,0,0); //actually center the grid in the game space.

        MoveCameraToPlayer();

        //Test load from file
        if (!string.IsNullOrWhiteSpace(GlobalVariables.getMap()))
        {
            Debug.Log("Loading Map");
            Recorder deserializedRecorder = GridControllerJsonSerializer.DeserializeFromJson(GlobalVariables.getMap());
            GlobalVariables.clearMap();
            SetObjects(deserializedRecorder);
        }
        ChangeToPlayerView();

        //Test save to file
        //GridControllerJsonSerializer.SerializeToJson(this, "testFile.json", recorder);

        //Test save as PNG
        //SaveAsPNG("testImage");

    }

    private void PlaceBackgroundFog()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (fogLayer[x, y] != null)
                {
                    fogLayer[x, y] = null;
                    if (backgroundLayer[x, y] == null)
                    {
                        GameObject cornerFog = Instantiate(fogTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                        fogLayer[x, y] = cornerFog;
                    }
                    continue;
                }
                GameObject fog = Instantiate(fogTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                fogLayer[x, y] = fog;

            }
        }
    }

    private void PlaceWalls()
    {
        List<Vector2> wallLocations = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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

            GameObject wall;

            if (backgroundLayer[x,y] != null){
                wall = backgroundLayer[x,y];
            }
            else{
                wall = Instantiate(wallTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
                recorder.AddTile(new RecorderTile("wall", x, y, -1));
            }

            wall.GetComponent<WallController>().Init(cellSize - cellSpacing * 2);
            wall.GetComponent<WallController>().SetTexture(GetAdjacentControllers(x, y), spritesheetManager);
            backgroundLayer[x, y] = wall;
            
        }

    }

    private Vector2 PlacePlayer(){
        GameObject firstRoom = rooms[0];
        RoomController firstRoomController = firstRoom.GetComponent<RoomController>();

        for (int x = firstRoomController.GetX(); x < firstRoomController.width + firstRoomController.GetX(); x++){
            for (int y = firstRoomController.GetY(); y < firstRoomController.height + firstRoomController.GetY(); y++){
                if (backgroundLayer[x,y] != null && backgroundLayer[x,y].GetComponent<TileController>() is FloorController && foregroundLayer[x,y] == null){
                    GameObject player = Instantiate(playerEntity,new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), -1), Quaternion.identity, gameObject.transform);
                    foregroundLayer[x,y] = player;
                    player.GetComponent<PlayerController>().Init(cellSize - cellSpacing * 2);
                    firstRoomController.ClearFog(new Vector3(x, y, 0), fogLayer, width, height);
                    this.playerClone = player;
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

        if (backgroundLayer[x, y] != null){

            return backgroundLayer[x,y].GetComponent<TileController>() is WallController;
        } 


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

    public void getGridBounds(out float topBound, out float leftBound, out float bottomBound, out float rightBound)
    {
        topBound = gameObject.transform.position.y + (height * cellSize);
        leftBound = gameObject.transform.position.x;
        bottomBound = gameObject.transform.position.y;
        rightBound = gameObject.transform.position.x + (width * cellSize);
    }

    public bool HandleFog(Vector3 mousePos)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            bool clear = rooms[i].GetComponent<RoomController>().ClearFog(mousePos, fogLayer, width, height);
            if (clear) return true;            
        }

        return false;
    }

    public void ChangePlayerDMView()
    {
        if (!inPlayerView)
        {
            inPlayerView = true;
            ChangeToPlayerView();
        } else if (inPlayerView)
        {
            inPlayerView = false;
            ChangeToDMView();
        }
    }

    public void ChangeToPlayerView()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (fogLayer[x, y] == null) continue;
                fogLayer[x, y].SetActive(true);
            }
        }
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().ShowFogTiles(fogLayer, width, height);
        }
    }

    public void ChangeToDMView()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (fogLayer[x, y] == null) continue;
                fogLayer[x, y].SetActive(false);
            }
        }

        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].GetComponent<RoomController>().HideFogTiles(fogLayer, width, height);
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
        if (fogLayer[(int)position.x, (int)position.y] == null) return false;
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

    public void Save(string filePath){
        string json = filePath + ".json";
        string png = filePath +".png";
        GridControllerJsonSerializer.SerializeToJson(this, json, recorder);
        GameObject topDownCamera = GameObject.Find("topDownCamera");
        Camera camera = topDownCamera.GetComponent<Camera>();
        GridControllerJsonSerializer.SaveSceneAsPNG(png, 3840, 2160, camera);
    }

    public void SaveAsPNG(string fileName)
    {
        ChangeToDMView();
        GameObject topDownCamera = GameObject.Find("Top-Down Camera");
        Camera camera = topDownCamera.GetComponent<Camera>();
        GridControllerJsonSerializer.SaveSceneAsPNG("saves/" + fileName + ".png", 6000, 3000, camera);
        ChangeToPlayerView();
    }

    public void MoveCameraToPlayer()
    {
        if (playerClone != null && mainCamera != null)
        {
            // Set the camera's position to match the target's position
            mainCamera.transform.position = new Vector3(
                playerClone.transform.position.x,
                playerClone.transform.position.y,
                mainCamera.transform.position.z
            );
            //Debug.Log("Player Position: " + playerClone.transform.position);
            //Debug.Log("Camera Position: " + mainCamera.transform.position);
        }
    }
}
