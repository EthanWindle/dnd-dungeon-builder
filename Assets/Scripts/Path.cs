using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Path{
	private List<PathNode> nodes {get;}

	public RoomController originRoom{get; private set;}
	public RoomController destinationRoom{get; private set;}

	public Path(){
		nodes = new();
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