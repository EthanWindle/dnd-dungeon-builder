using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{

    public GameObject walkway;
    public GameObject wallPrefab;
    PathNode[,] grid;
    int gridMaxX = 0;
    int gridMaxY = 0;
    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

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
            if (currentNode.y - 1 >= 0)//left down
            {
                neighbourList.Add(grid[currentNode.x - 1, currentNode.y - 1]);
            }
            if (currentNode.y + 1 < gridMaxY)//left up
            {
                neighbourList.Add(grid[currentNode.x - 1, currentNode.y + 1]);
            }
        }
        if (currentNode.x + 1 < gridMaxX)//Right
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
        int pathCount = 0;
        DoorController door1;
        DoorController door2;
        Vector2Int door1Coords;
        Vector2Int door2Coords;
        Dictionary<Vector2Int, TileController> doors = new Dictionary<Vector2Int, TileController>();
        Dictionary<Vector2Int, TileController> doorsInDifferentRooms = new Dictionary<Vector2Int, TileController>();
        PathNode prevPathNode = null;
        List<PathNode> path;
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
        for (int i = 0; i < doors.Count - 1; i++)
        {
            door1 = DoorList[i] as DoorController;
            door2 = DoorList[i + 1] as DoorController;

            if (door2.GetParent() != door1.GetParent())
            {
                door1Coords = DoorCoords[i];
                door2Coords = DoorCoords[i + 1];
                path = findPath(door1Coords, door2Coords, backgroundLayer, maxX, maxY);
                if (path != null)
                {
                    pathCount++;
                    //Debug.Log(path.Count);
                    foreach (PathNode p in path)
                    {
                        if (prevPathNode != null && IsDiagonal(prevPathNode, p))
                        {
                            Vector3 tileBelowPosition = new Vector3(p.x * (gridController.cellSize + (gridController.cellSpacing * 6)), (p.y - 1) * (gridController.cellSize + (gridController.cellSpacing * 6)), 0);

                            // Check if the tile below is occupied
                            if (!IsTileOccupied(tileBelowPosition, gridController.cellSize - gridController.cellSpacing * 2))
                            {
                                InstantiateTile(tileBelowPosition, gridController.cellSize - gridController.cellSpacing * 2);
                            }
                            else
                            {
                                // If the tile below is occupied, try to find an unoccupied neighboring tile
                                Vector3 unoccupiedPosition = FindUnoccupiedNeighbor(tileBelowPosition, gridController.cellSize - gridController.cellSpacing * 2, gridController);
                                if (unoccupiedPosition != Vector3.zero)
                                {
                                    InstantiateTile(unoccupiedPosition, gridController.cellSize - gridController.cellSpacing * 2);
                                }
                            }

                            Vector3 diagRightNeighborPosition = new Vector3(p.x + 1, p.y, 0);
                            Vector3 diagLeftNeighborPosition = new Vector3(p.x - 1, p.y, 0);

                            if (!IsTileOccupied(diagRightNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2))
                            {
                                InstantiateWall(diagRightNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2);
                            }

                            if (!IsTileOccupied(diagLeftNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2))
                            {
                                InstantiateWall(diagLeftNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2);
                            }
                        }

                        // Check right and left neighbors
                        Vector3 rightNeighborPosition = new Vector3(p.x + 1, p.y, 0);
                        Vector3 leftNeighborPosition = new Vector3(p.x - 1, p.y, 0);
                        Vector3 topNeighborPosition = new Vector3(p.x, p.y + 1, 0);
                        Vector3 bottomNeighborPosition = new Vector3(p.x, p.y - 1, 0);

                        if (!IsTileOccupied(rightNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2))
                        {
                            InstantiateWall(rightNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2);
                        }

                        if (!IsTileOccupied(leftNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2))
                        {
                            InstantiateWall(leftNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2);
                        }
                        if (!IsTileOccupied(topNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2))
                        {
                            InstantiateWall(topNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2);
                        }

                        if (!IsTileOccupied(bottomNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2))
                        {
                            InstantiateWall(bottomNeighborPosition, gridController.cellSize - gridController.cellSpacing * 2);
                        }
                        //Debug.Log("X: "+p.x);
                        //Debug.Log("Y: "+p.y);                  
                        //Debug.Log(pathCount);
                        GameObject obj = Instantiate(walkway, new Vector3(p.x * (gridController.cellSize + (gridController.cellSpacing * 6)), p.y * (gridController.cellSize + (gridController.cellSpacing * 6)), 0), Quaternion.identity, gameObject.transform);
                        obj.GetComponent<TileController>().Init(gridController.cellSize - gridController.cellSpacing * 2);
                        prevPathNode = p;
                    }
                }
            }

        }

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

    private bool IsTileOccupied(Vector3 position, float size)
    {
        // Check if the tile at the specified position is occupied by something else
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(size, size), 0f);
        return colliders.Length > 0;
    }

    private bool IsDiagonal(PathNode node1, PathNode node2)
    {
        // Check if the nodes are diagonal by comparing their X and Y differences
        int deltaX = Mathf.Abs(node1.x - node2.x);
        int deltaY = Mathf.Abs(node1.y - node2.y);
        return deltaX == 1 && deltaY == 1;
    }

    private void InstantiateTile(Vector3 position, float size)
    {
        // Instantiate the tile prefab at the specified position
        GameObject obj = Instantiate(walkway, position, Quaternion.identity, gameObject.transform);
        obj.GetComponent<TileController>().Init(size);
    }

    private void InstantiateWall(Vector3 position, float size)
    {
        // Instantiate the tile prefab at the specified position
        GameObject obj = Instantiate(wallPrefab, position, Quaternion.identity, gameObject.transform);
        obj.GetComponent<TileController>().Init(6 * size);
    }

    private Vector3 FindUnoccupiedNeighbor(Vector3 currentPosition, float size, GridController gridController)
    {
        // Define possible neighbor positions (top, left, right, bottom)
        Vector3[] possibleNeighbors = new Vector3[]
        {
        new Vector3(currentPosition.x, currentPosition.y + size + (gridController.cellSpacing * 6), 0), // Top neighbor
        new Vector3(currentPosition.x - size - (gridController.cellSpacing * 6), currentPosition.y, 0), // Left neighbor
        new Vector3(currentPosition.x + size + (gridController.cellSpacing * 6), currentPosition.y, 0), // Right neighbor
        new Vector3(currentPosition.x, currentPosition.y - size - (gridController.cellSpacing * 6), 0)  // Bottom neighbor
        };

        foreach (Vector3 neighborPosition in possibleNeighbors)
        {
            // Check if the neighbor tile is not occupied
            if (!IsTileOccupied(neighborPosition, size))
            {
                return neighborPosition; // Return the unoccupied neighbor position
            }
        }

        return Vector3.zero; // Return Vector3.zero if no unoccupied neighbor is found
    }

}