using System.Collections.Generic;
using System.Linq;
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

    public Path FindPath(Vector2 door1, Vector2 door2, GameObject[,] backgroundLayer, int maxX, int maxY)
    {

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
        PathNode startNode = grid[(int)door1.x, (int)door1.y];
        PathNode endNode = grid[(int)door2.x, (int)door2.y];


        List<PathNode> openList = new() { startNode };
        List<PathNode> closedList = new();

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
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {

                //reached final node
                Path path =  CalculatePath(endNode);
                return path;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode NeighbourNode in getNeighboursList(currentNode))
            {
                if (closedList.Contains(NeighbourNode)) continue;
                if (NeighbourNode != null)
                {
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
        List<PathNode> neighbourList = new();

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
    private Path CalculatePath(PathNode endNode)
    {
        Path path = new();
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

        return MOVE_STRAIGHT_COST * remaining;
    }


    public Vector2Int GetClosestDoorInDifferentRoom(Dictionary<Vector2Int, TileController> doors, DoorController door, Vector2Int location){
        Vector2Int closest = new(-1, -1);

        foreach (Vector2Int doorLocation in doors.Keys){
            DoorController otherDoor = doors[doorLocation] as DoorController;
            if (otherDoor.GetParent() == door.GetParent()) continue;
            if (closest == new Vector2Int(-1, -1) || Vector2Int.Distance(doorLocation, location) < Vector2Int.Distance(closest, location)){
                closest = doorLocation;
            }
        }

        return closest;
    }


    private void PlacePath(Path path, GameObject[,] backgroundLayer, GridController gridController){
        for (int pathIndex = 1; pathIndex < path.Count() - 1; pathIndex++)
                    {
                        PathNode p = path.get(pathIndex);
                        GameObject obj = Instantiate(walkway, new Vector3(p.x * gridController.cellSize, p.y * gridController.cellSize, 0), Quaternion.identity, gameObject.transform);
                        backgroundLayer[p.x,p.y] = obj;
                        obj.GetComponent<TileController>().Init(gridController.cellSize - gridController.cellSpacing * 2);
                        gridController.GetRecorder().AddTile(new RecorderTile("floor", p.x, p.y, -1));
                    
                }
    }

    public void ConnectAllRooms(GameObject[,] backgroundLayer, GameObject[] rooms, int maxX, int maxY, Transform transformParent, GameObject[,] gridFogLayer, float size, float margin)
    {
        DoorController door1;
        Vector2Int door1Coords;
        Vector2Int door2Coords;
        Dictionary<Vector2Int, TileController> doors = new();
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
                        Vector2Int gridCoordinates = new(i, j); // Convert the grid coordinates to Vector2Int
                        doors.Add(gridCoordinates, tile); // Add the door to the dictionary
                    }
                }
            }
        }
        var DoorList = doors.Values.ToList();
        var DoorCoords = doors.Keys.ToList();


        List<Path> paths = new();

        for (int i = 0; i < doors.Count - 1; i++)
        { //Connect each door to the closest door in a different room, if the distance is < maxLength
                door1 = DoorList[i] as DoorController;
                door1Coords = DoorCoords[i];
                door2Coords = GetClosestDoorInDifferentRoom(doors, door1, DoorCoords[i]);
                
                
                Path path = FindPath(door1Coords, door2Coords, backgroundLayer, maxX, maxY);
                if (path != null && path.Count() <= maxLength)
                {
                    path.SetRooms(backgroundLayer);
                    paths.Add(path);
                    
            }

        }
        

        List<HashSet<RoomController>> disjointGroups = GetDisjointRoomGroups(rooms);

        while (disjointGroups.Count() > 1)
        { //Connect each disjoint group of rooms, ignoring max path length
            List<RoomController> firstGroup = new(disjointGroups[0]);
            List<RoomController> controllers = new();
            foreach (GameObject gameObject in rooms){
                RoomController controller =  gameObject.GetComponent<RoomController>();
                if (!firstGroup.Contains(controller)) {
                    controllers.Add(gameObject.GetComponent<RoomController>());
                }
            }

            FindClosestDoors(firstGroup, controllers, out Vector2Int originDoor, out Vector2Int destinationDoor);

            Path path = FindPath(originDoor, destinationDoor, backgroundLayer, maxX, maxY);

            if (path == null)
            { //There is not valid path between the two doors. Must return otherwise the code will infinitely loop
                Debug.Log("Could not generate path between closest doors in disjoint groups of rooms");
                Debug.Log(originDoor);
                Debug.Log(destinationDoor);
                return;
            }

            path.SetRooms(backgroundLayer);

            paths.Add(path);
            
            disjointGroups = GetDisjointRoomGroups(rooms);

        }
        

        foreach(Path path in paths)
        {
            PlacePath(path, backgroundLayer, gridController);
            path.CreateFog(transformParent, gridFogLayer, size, margin);
        }

    }

    private void FindClosestDoors(List<RoomController> origins, List<RoomController> destinations, out Vector2Int originDoor, out Vector2Int destinationDoor){
        float distance = -1f;
        Vector2Int closestOriginDoor = new(-1, -1);
        Vector2Int closestDestinationDoor = new(-1, -1);
        foreach (RoomController originRoom in origins){
            foreach (RoomController destinationRoom in destinations){
                float currentDistance = FindClosestDoorsInRooms(originRoom, destinationRoom, out Vector2Int origin, out Vector2Int destination);
                if (distance == -1f || currentDistance < distance){
                    distance = currentDistance;
                    closestOriginDoor = origin;
                    closestDestinationDoor = destination;
                }
            }
        }

        originDoor = closestOriginDoor;
        destinationDoor = closestDestinationDoor;
        Debug.Log(distance);
    }

    private float FindClosestDoorsInRooms(RoomController originRoom, RoomController destinationRoom, out Vector2Int originDoor, out Vector2Int destinationDoor){
        float distance = -1f;
        Vector2Int closestOrigin = new(-1, -1);
        Vector2Int closestDestination = new(-1, -1);
        foreach(Vector2 originVector in originRoom.doors){
            Vector2Int origin = new((int)originVector.x + originRoom.GetX(), (int)originVector.y + originRoom.GetY());
            foreach (Vector2 destinationVector in destinationRoom.doors){
                Vector2Int destination = new((int)destinationVector.x + destinationRoom.GetX(), (int)destinationVector.y + destinationRoom.GetY());
                float currentDistance = Vector2Int.Distance(origin, destination);
                if (distance == -1f || currentDistance < distance){
                    distance = currentDistance;
                    closestOrigin = origin;
                    closestDestination = destination;
                }

            }
        }

        originDoor = closestOrigin;
        destinationDoor = closestDestination;

        return distance;


    }
    
    private List<HashSet<RoomController>> GetDisjointRoomGroups(GameObject[] rooms){
        List<HashSet<RoomController>> roomGroups = new();

        List<RoomController> unvisitedRooms = new();

        foreach (GameObject gameObject in rooms){
            unvisitedRooms.Add(gameObject.GetComponent<RoomController>());
        }


        while (unvisitedRooms.Count > 0){
            HashSet<RoomController> visitedRooms = new();

            if (unvisitedRooms[0].paths.Count > 0){
                visitedRooms = unvisitedRooms[0].paths[0].GetReachableRooms(visitedRooms);
            }
            else{
                visitedRooms.Add(unvisitedRooms[0]);
            }
            roomGroups.Add(visitedRooms);
            
            unvisitedRooms.RemoveAll((room) => visitedRooms.Any((otherRoom) => room == otherRoom));
        }

        return roomGroups;
    }

}