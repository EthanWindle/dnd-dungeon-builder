using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public GameObject tilePrefab;


    public void main(GameObject[,] backgroundLayer, GameObject[] rooms, int maxX, int maxY)
    {
        Vector2 door1Loc;
        Vector2 door2Loc;
        foreach (GameObject room in rooms)
        {
            RoomController roomController = room.GetComponent<RoomController>();
            int roomCenterX = roomController.GetX();
            int roomCenterY = roomController.GetY();
            Vector2[] doors = roomController.doors;
            int doorCount = doors.Length;
            if (roomController.hasPath == false)
            {
                foreach (Vector2 door in doors)
                {
                    //Debug.LogError(door);
                    door1Loc = door;
                    for (int x = 0; x < maxX; x++) //for each grid cell across the whole map
                    {
                        for (int y = 0; y < maxY; y++) //for each grid cell down the whole map
                        {
                            GameObject cellObject = backgroundLayer[x, y]; //the object in the current cell
                            if (cellObject != null && cellObject.GetComponent<DoorController>() != null) //check if the object is a door
                            {
                                //Debug.LogError("6");
                                DoorController doorController = cellObject.GetComponent<DoorController>(); //get the door controller
                                if (!IsDoorInCurrentRoom(doorController, roomController)) //check if the door is in the current room
                                {
                                    //Debug.LogError("7");
                                    door2Loc = backgroundLayer[x, y].transform.position; //set the second door location
                                    // This is a door that is not part of the current room.
                                    //Debug.Log("Found a door not part of the current room.");
                                    GridController gridController = gameObject.GetComponent<GridController>();  
                                    List<Vector2> path = GeneratePath(backgroundLayer, maxX, maxY);
                                    foreach (Vector2 p in path)
                                    {
                                        Debug.Log(p);
                                    }
                                    if (path != null)
                                    {
                                        Debug.LogError("8");
                                        foreach (Vector2 pathLoc in path)
                                        {
                                            Debug.LogError("9");
                                            InstantiateTilePrefab(gameObject.transform, backgroundLayer, gridController.cellSize, gridController.cellSpacing,(int)pathLoc.x, (int)pathLoc.y, gridController.GetRecorder());
                                        }
                                    }
                                    else
                                    {
                                        //Debug.LogError("No path between doors found");
                                    }
                                    
                                    //break;
                                }
                            }
                            //break;
                        }
                       // break;
                    }
                    //break;
                }
                roomController.setHasPathTrue();
            }

        }
    }

    public List<Vector2> GeneratePath(GameObject[,] backgroundLayer, int maxX, int maxY)
    {
        List<Vector2> doorLocations = new List<Vector2>();

        // Step 1: Traverse through the grid and find occupied tiles with doors
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                GameObject cellObject = backgroundLayer[x, y];
                if (cellObject != null && cellObject.GetComponent<DoorController>() != null)
                {
                    doorLocations.Add(new Vector2(x, y));
                }
            }
        }

        // Step 2: Store door locations and calculate paths
        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < doorLocations.Count; i++)
        {
            Vector2 door1Loc = doorLocations[i];
            for (int j = i + 1; j < doorLocations.Count; j++)
            {
                Vector2 door2Loc = doorLocations[j];

                // Step 3: Calculate path between two doors
                List<Vector2> doorPath = CalculateDoorPath(backgroundLayer, door1Loc, door2Loc, maxX, maxY);

                if (doorPath != null)
                {
                    // Step 4: Add the path to the result
                    path.AddRange(doorPath);
                }
            }
        }

        // Step 5: Return the generated path
        return path;
    }

    private List<Vector2> CalculateDoorPath(GameObject[,] backgroundLayer, Vector2 door1Loc, Vector2 door2Loc, int maxX, int maxY)
    {
        float[,] distances = new float[maxX, maxY];
        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                distances[x, y] = float.PositiveInfinity;
            }
        }

        // Create a queue for Dijkstra's algorithm
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(door1Loc);
        distances[(int)door1Loc.x, (int)door1Loc.y] = 0;

        // Create a dictionary to store previous positions for path reconstruction
        Dictionary<Vector2, Vector2> previous = new Dictionary<Vector2, Vector2>();

        // Define possible neighboring directions
        Vector2[] directions = new Vector2[]
        {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
        };

        // Dijkstra's algorithm
        while (queue.Count > 0)
        {
            Vector2 current = queue.Dequeue();

            // Stop when we reach the destination door
            if (current == door2Loc)
            {
                break;
            }

            // Explore neighbors
            foreach (Vector2 direction in directions)
            {
                Vector2 neighbor = current + direction;

                // Check if the neighbor is within bounds and not occupied
                if (neighbor.x >= 0 && neighbor.x < maxX && neighbor.y >= 0 && neighbor.y < maxY &&
                    backgroundLayer[(int)neighbor.x, (int)neighbor.y] == null)
                {
                    float alt = distances[(int)current.x, (int)current.y] + 1; // Assuming equal cost for all moves

                    if (alt < distances[(int)neighbor.x, (int)neighbor.y])
                    {
                        distances[(int)neighbor.x, (int)neighbor.y] = alt;
                        previous[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        // Reconstruct the path
        List<Vector2> path = new List<Vector2>();
        Vector2 currentPos = door2Loc;
        while (previous.ContainsKey(currentPos))
        {
            path.Insert(0, currentPos);
            currentPos = previous[currentPos];
        }

        // Check if a path exists
        if (path.Count > 0)
        {
            return path;
        }
        else
        {
            // No path found
            return null;
        }
    }

    private void InstantiateTilePrefab(Transform transformParent, GameObject[,] background, float size, float margin, int x, int y, Recorder recorder)
    {
        // Instantiate the tile prefab at the specified position
        GameObject tile = Instantiate(tilePrefab, new Vector3((x) * (size + margin), (y) * (size + margin), 0), Quaternion.identity, transformParent);
        tile.GetComponent<TileController>().Init(size - margin * 2);
        background[x, y] = tile;
        recorder.AddTile(new RecorderTile("floor", x, y));
        // Customize or configure the instantiated tile as needed
        // For example, you can set its properties, add components, etc.
    }

    private bool IsDoorInCurrentRoom(DoorController door, RoomController currentRoom)
    {
        Vector3 doorPosition = door.transform.position; // Convert the door's position to Vector3
        foreach (Vector2 roomDoor in currentRoom.doors)
        {
            Vector3 roomDoorPosition = new Vector3(roomDoor.x, roomDoor.y, 0f); // Convert roomDoor to Vector3
            if (doorPosition == roomDoorPosition)
            {
                return true; // The door is in the current room
            }
        }
        return false; // The door is not in the current room
    }


    // Check if a tile is within bounds and not occupied
    private bool IsValidTile(Vector2 tile, int maxX, int maxY, GameObject[,] backgroundLayer)
    {
        int x = (int)tile.x;
        int y = (int)tile.y;
        if (x >= 0 && x < maxX && y >= 0 && y < maxY)
        {
            // Check if the tile is not occupied by something else
            return backgroundLayer[x, y] == null;
        }
        return false;
    }

}
