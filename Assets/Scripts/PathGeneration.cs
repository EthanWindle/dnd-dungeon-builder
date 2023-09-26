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
        Dictionary<Vector2Int, TileController> doors = new Dictionary<Vector2Int, TileController>();

        for (int i = 0; i < maxX; i++)//for each grid cell across the whole map
        {
            for(int j = 0; j < maxY; j++)//for each grid cell down the whole map
            {
                TileController tile = backgroundLayer[i, j].GetComponent<TileController>();//get the tile at the current location
                if (tile is DoorController)//if the tile is a door
                {
                    Vector2Int gridCoordinates = new Vector2Int(i, j); // Convert the grid coordinates to Vector2Int
                    doors.Add(gridCoordinates, tile); // Add the door to the dictionary
                }
                
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
