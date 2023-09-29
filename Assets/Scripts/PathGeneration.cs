using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject walkway;
    public GameObject wallPrefab;
    private readonly int maxLength = 20;
    PathNode[,] grid;
    int gridMaxX = 0;
    int gridMaxY = 0;
    const int MOVE_STRAIGHT_COST = 10;

    public List<PathNode> findPath(Vector2 door1, Vector2 door2, GameObject[,] backgroundLayer, int maxX, int maxY)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        bool[,] walkable = new bool[maxX, maxY];
        grid = new PathNode[maxX, maxY];

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (backgroundLayer[x, y] == null)
                {
                    walkable[x, y] = true;
                    grid[x, y] = new PathNode(backgroundLayer, x, y);
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
                    walkable[x, y] = false;
                    if (backgroundLayer[x, y].GetComponent<DoorController>())
                    {
                        walkable[x, y] = true;
                        grid[x, y] = new PathNode(backgroundLayer, x, y);
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
        //Debug.LogError(door1.x +","+ door1.y);
        //Debug.LogError(door2.x + "," + door2.y);
        PathNode startNode = grid[(int)door1.x, (int)door1.y];
        PathNode endNode = grid[(int)door2.x, (int)door2.y];


        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                if (walkable[x, y] == true)
                {
                    PathNode node = grid[x, y];
                    node.gCost = int.MaxValue;
                    node.CalculateFCost();
                    node.previousNode = null;
                }
            }
        }

        if (startNode == null || endNode == null) return null;

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        //Debug.LogError(startNode.hCost);
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

            foreach (PathNode NeighbourNode in getNeighboursList(currentNode))
            {
                if (closedList.Contains(NeighbourNode)) continue;
                if (NeighbourNode != null)
                {
                    //Debug.Log(NeighbourNode);
                    //Debug.Log(currentNode);
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, NeighbourNode);
                    if (tentativeGCost < NeighbourNode.gCost)
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
        }
        //out of nodes on the openList
        return null;
    }

    private List<PathNode> getNeighboursList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)//Left
        {
            neighbourList.Add(grid[currentNode.x - 1, currentNode.y]);
        }
        if (currentNode.x + 1 < gridMaxX)//Right
        {
            neighbourList.Add(grid[currentNode.x + 1, currentNode.y]);
        }
        if (currentNode.y - 1 >= 0)//Down
        {
            neighbourList.Add(grid[currentNode.x, currentNode.y - 1]);
        }
        if (currentNode.y + 1 < gridMaxY)//Up
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

        if (path.Count() > maxLength) return null;

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

        return MOVE_STRAIGHT_COST * remaining;
    }


    public Vector2Int GetClosestDoorInDifferentRoom(Dictionary<Vector2Int, TileController> doors, DoorController door, Vector2Int location){
        Vector2Int closest = new Vector2Int(-1, -1);

        foreach (Vector2Int doorLocation in doors.Keys){
            DoorController otherDoor = doors[doorLocation] as DoorController;
            if (otherDoor.GetParent() == door.GetParent()) continue;
            if (closest == new Vector2Int(-1, -1) || Vector2Int.Distance(doorLocation, location) < Vector2Int.Distance(closest, location)){
                closest = doorLocation;
            }
        }

        return closest;
    }

    public void main(GameObject[,] backgroundLayer, GameObject[] rooms, int maxX, int maxY)
    {
        int pathCount = 0;
        DoorController door1;
        DoorController door2;
        Vector2Int door1Coords;
        Vector2Int door2Coords;
        Dictionary<Vector2Int, TileController> doors = new Dictionary<Vector2Int, TileController>();
        GridController gridController = gameObject.GetComponent<GridController>();

        for (int i = 0; i < maxX; i++)//for each grid cell across the whole map
        {
            for (int j = 0; j < maxY; j++)//for each grid cell down the whole map
            {
                if (backgroundLayer[i, j] != null)
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
        var DoorList = doors.Values.ToList();
        var DoorCoords = doors.Keys.ToList();


        List<List<PathNode>> paths = new List<List<PathNode>>();

        for (int i = 0; i < doors.Count - 1; i++)
        {
                door1 = DoorList[i] as DoorController;
                door1Coords = DoorCoords[i];
                door2Coords = GetClosestDoorInDifferentRoom(doors, door1, DoorCoords[i]);
                door2 = doors[door2Coords] as DoorController;
                
                
                List<PathNode> path = findPath(door1Coords, door2Coords, backgroundLayer, maxX, maxY);
                if (path != null)
                {
                    paths.Add(path);
                    
            }

        }

        foreach(List<PathNode> path in paths){
            for (int pathIndex = 1; pathIndex < path.Count - 1; pathIndex++)
                    {
                        PathNode p = path[pathIndex];
                        //Debug.Log("X: "+p.x);
                        //Debug.Log("Y: "+p.y);                  
                        //Debug.Log(pathCount);
                        GameObject obj = Instantiate(walkway, new Vector3(p.x * gridController.cellSize, p.y * gridController.cellSize, 0), Quaternion.identity, gameObject.transform);
                        backgroundLayer[p.x,p.y] = obj;
                        obj.GetComponent<TileController>().Init(gridController.cellSize - gridController.cellSpacing * 2);
                    
                }
        }

    }


    private bool IsTileOccupied(Vector3 position, float size)
    {
        // Check if the tile at the specified position is occupied by something else
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(size, size), 0f);
        return colliders.Length > 0;
    }




    // private Vector3 FindUnoccupiedNeighbor(Vector3 currentPosition, float size, GridController gridController)
    // {
    //     // Define possible neighbor positions (top, left, right, bottom)
    //     Vector3[] possibleNeighbors = new Vector3[]
    //     {
    //     new Vector3(currentPosition.x, currentPosition.y + size + (gridController.cellSpacing * 6), 0), // Top neighbor
    //     new Vector3(currentPosition.x - size - (gridController.cellSpacing * 6), currentPosition.y, 0), // Left neighbor
    //     new Vector3(currentPosition.x + size + (gridController.cellSpacing * 6), currentPosition.y, 0), // Right neighbor
    //     new Vector3(currentPosition.x, currentPosition.y - size - (gridController.cellSpacing * 6), 0)  // Bottom neighbor
    //     };

    //     foreach (Vector3 neighborPosition in possibleNeighbors)
    //     {
    //         // Check if the neighbor tile is not occupied
    //         if (!IsTileOccupied(neighborPosition, size))
    //         {
    //             return neighborPosition; // Return the unoccupied neighbor position
    //         }
    //     }

    //     return Vector3.zero; // Return Vector3.zero if no unoccupied neighbor is found
    // }

}