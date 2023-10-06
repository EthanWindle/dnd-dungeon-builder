using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Path
{
	private List<PathNode> nodes {get;}

	public RoomController originRoom{get; private set;}
	public RoomController destinationRoom{get; private set; }
    private Boolean hidden = true;

    public Path(){
		//nodes = new List<PathNode>();
        this.nodes = new();
    }

	public void Add(PathNode pathNode){
		nodes.Add(pathNode);
	}

	public void Reverse(){
		nodes.Reverse();
	}

	public int Count(){
		return nodes.Count;
	}

	public PathNode get(int i){
		return nodes[i];
	}

	public void CreateFog(Transform transformParent, GameObject[,] gridFogLayer, float size, float margin)
    {
        foreach (PathNode pathnode in nodes)
        {
            pathnode.CreateFog(transformParent, originRoom.fogPrefab, gridFogLayer, size, margin);
        }
    }

    public void ClearFogTiles()
    {
        hidden = false;
        foreach (PathNode pathnode in nodes)
        {
            pathnode.ClearFogTile();
        }
    }

    public void HideFogTiles()
    {
        if (hidden == false) return;
        foreach (PathNode pathnode in nodes)
        {
            pathnode.HideFogTile();
        }
    }

	/*
     * Shows all fog tiles
     */
	public void ShowFogTiles()
    {
        if (hidden == false) return;
        foreach (PathNode pathnode in nodes)
        {
            pathnode.ShowFogTile();
        }
    }

    /*
            var leftMostX = get(0).x;
            var rightMostX = get(0).x;
            var lowestY = get(0).y;
            var highestY = get(0).y;

            foreach (PathNode pathnode in nodes)
            {
                if (pathnode.x < leftMostX) leftMostX = pathnode.x;
                if (pathnode.x > rightMostX) rightMostX = pathnode.x;
                if (pathnode.y < lowestY) lowestY = pathnode.y;
                if (pathnode.y > highestY) highestY = pathnode.y;
            }
            */

    //fogLayer = new GameObject[rightMostX - leftMostX, highestY - lowestY];
    //fogLayer = new GameObject[gridFogLayer.GetLength(0), gridFogLayer.GetLength(1)];

    /*

    foreach (PathNode pathnode in nodes)
    {
        var x = pathnode.x;
        var y = pathnode.y;

        GameObject fog = UnityEngine.Object.Instantiate(originRoom.fogPrefab, new Vector3((x) * (size + margin), (y) * (size + margin), -2), Quaternion.identity, transformParent);
        fog.GetComponent<TileController>().Init(size - margin * 2);

        if (fog == null)
        {
            Debug.Log("null");
            continue;
        }
        //Debug.Log(x + " : " + y + " : " + (rightMostX - leftMostX) + " :" + (highestY - lowestY));
        fogLayer[x, y] = fog;
        gridFogLayer[x, y] = fog;
        Debug.Log("creating fog tiles");
    }

    for (int i = 0; i < gridFogLayer.GetLength(0); i++)
    {
        for (int j = 0; j < gridFogLayer.GetLength(0); j++)
        {
            fogLayer[i][j]

        }
    }*/

    public void SetRooms(GameObject[,] backgroundLayer){
		int ox = nodes[0].x;
		int oy = nodes[0].y;

		int dx = nodes[^1].x;
		int dy = nodes[^1].y;
		originRoom = backgroundLayer[ox, oy].GetComponent<DoorController>().GetParent();
		originRoom.paths.Add(this);
		destinationRoom = backgroundLayer[dx, dy].GetComponent<DoorController>().GetParent();
		destinationRoom.paths.Add(this);
	}

	public HashSet<RoomController> GetReachableRooms(HashSet<RoomController> rooms){
		if (!rooms.Contains(originRoom)){
			rooms.Add(originRoom);
			foreach (Path path in originRoom.paths){
				path.GetReachableRooms(rooms);
			}
		}
		if (!rooms.Contains(destinationRoom)){
			rooms.Add(destinationRoom);
			foreach (Path path in destinationRoom.paths){
				path.GetReachableRooms(rooms);
			}
		}

		return rooms;
	}

}