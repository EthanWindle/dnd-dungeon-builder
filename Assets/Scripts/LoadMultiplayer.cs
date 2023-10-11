using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMultiplayer : MonoBehaviour
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

}
