using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject floor;
    PathNode[,] grid;
    int gridMaxX = 0;
    int gridMaxY = 0;
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    public List<PathNode> findPath(Vector2 door1, Vector2 door2, GameObject[,] backgroundLayer, int maxX, int maxY)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        bool[,]walkable = new bool[maxX, maxY];
        grid = new PathNode[maxX, maxY];

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (backgroundLayer[x,y] == null)
                {
                    walkable[x,y] = true;
                    grid[x,y] = new PathNode(backgroundLayer, x, y);
                    if (x > gridMaxX)
                    {
                        gridMaxX = x;
                    }
                    if (y > gridMaxY)
                    {
                        gridMaxY = y;
                    }
                }
                else
                {
                    walkable[x,y] = false;
                    if (backgroundLayer[x, y].GetComponent<DoorController>())
                    {
                        walkable[x,y] = true;
                        grid[x,y] = new PathNode(backgroundLayer, x, y);
                        if (x > gridMaxX)
                        {
                            gridMaxX = x;
                        }
                        if (y > gridMaxY)
                        {
                            gridMaxY = y;
                        }
                    }
                }
            }
        }
        Debug.LogError(door1.x +","+ door1.y);
        Debug.LogError(door2.x + "," + door2.y);
        PathNode startNode = grid[(int)door1.x, (int)door1.y];
        PathNode endNode = grid[(int)door2.x, (int)door2.y];

        /*
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (walkable[x,y] == true)
                {
                    PathNode node = grid[x, y];
                    node.gCost = int.MaxValue;
                    node.CalculateFCost(); 
                    node.previousNode = null;
                }
            }
        }startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);        
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                //reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode NeighbourNode in getNeighboursList(currentNode)){
                if(closedList.Contains(NeighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, NeighbourNode);
                if(tentativeGCost < NeighbourNode.gCost)
                {
                    NeighbourNode.previousNode = currentNode;
                    NeighbourNode.gCost = tentativeGCost;
                    NeighbourNode.hCost = CalculateDistanceCost(NeighbourNode, endNode);
                    NeighbourNode.CalculateFCost();

                    if (!openList.Contains(NeighbourNode))
                    {
                        openList.Add(NeighbourNode);
                    }
                }
            }
        }
        //out of nodes on the openList
        */
        return null;
    }

    private List<PathNode> getNeighboursList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if(currentNode.x - 1 >= 0)//Left
        {
            neighbourList.Add(grid[currentNode.x - 1, currentNode.y]);
            if (currentNode.y - 1 >= 0)//left down
            {
                neighbourList.Add(grid[currentNode.x - 1, currentNode.y - 1]);
            }
            if(currentNode.y + 1 < gridMaxY)//left up
            {
                neighbourList.Add(grid[currentNode.x - 1, currentNode.y + 1]);
            }
        }if(currentNode.x + 1 < gridMaxX)//Right
        {
            neighbourList.Add(grid[currentNode.x + 1, currentNode.y]);
            if (currentNode.y - 1 >= 0)//right down
            {
                neighbourList.Add(grid[currentNode.x + 1, currentNode.y - 1]);
            }
            if (currentNode.y + 1 < gridMaxY)//right up
            {
                neighbourList.Add(grid[currentNode.x + 1, currentNode.y + 1]);
            }
        }if(currentNode.y - 1 >= 0)//Down
        {
            neighbourList.Add(grid[currentNode.x, currentNode.y - 1]);
        }if(currentNode.y + 1 < gridMaxY)//Up
        {
            neighbourList.Add(grid[currentNode.x, currentNode.y + 1]);
        }
        return neighbourList;
    }
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private int CalculateDistanceCost(PathNode startNode, PathNode endNode)
    {
        int XDistance = Mathf.Abs(startNode.x - endNode.x);
        int YDistance = Mathf.Abs(startNode.y - endNode.y);
        int remaining = Mathf.Abs(XDistance - YDistance);   

        return MOVE_DIAGONAL_COST * Mathf.Min(XDistance, YDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    public void main(GameObject[,] backgroundLayer, GameObject[] rooms, int maxX, int maxY)
    {
        Vector2 door1Loc;
        Vector2 door2Loc;

        foreach (GameObject room in rooms)
        {
            for ()
            {
                for ()
                {
                    GameObject cell = backgroundLayer[x, y];
                    if(cell == null)
                    {
                        continue;
                    }
                    TileController controller = cell.GetComponent<TileController>(); // tile controller of the cell
                    if(controller.Is)
                }
            }
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
                    door1Loc = door;//in respect to the room so wrong
                    for (int x = 0; x < maxX; x++) //for each grid cell across the whole map
                    {
                        //Debug.LogError("5");
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
                                    List<PathNode> path = findPath(door1Loc, door2Loc, backgroundLayer, maxX, maxY);
                                    Debug.LogError(path.Count);
                                    foreach (PathNode node in path)
                                    {
                                        //Debug.LogError("8");
                                        InstantiateTilePrefab(backgroundLayer, gridController.cellSize, gridController.cellSpacing, node.x, node.y);
                                    }
                                    //break;
                                }
                            }
                            //break;
                        }
                        //break;
                    }
                    //break;
                }
                roomController.setHasPathTrue();
            }

        }
    }

    private void InstantiateTilePrefab(GameObject[,] background, float size, float margin, int x, int y)
    {
        // Instantiate the tile prefab at the specified position
        GameObject tile = Instantiate(floor, new Vector3((x) * (size + margin), (y) * (size + margin), 0), Quaternion.identity);
        //tile.GetComponent<TileController>().Init(size - margin * 2);
        //background[x, y] = tile;
        //recorder.AddTile(new RecorderTile("floor", x, y));
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
    private bool IsValidTile(Vector2 tile, int maxX, int maxY)
    {
        int x = (int)tile.x;
        int y = (int)tile.y;
        return x >= 0 && x < maxX && y >= 0 && y < maxY;
    }

}
