using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Path
{
	private List<PathNode> nodes {get;}

	public RoomController originRoom{get; private set;}
	public RoomController destinationRoom{get; private set; }
    private Boolean hidden = true;

    private ArrayList pathWallFog;

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

	public void CreateFog(Transform transformParent, GameObject[,] gridFogLayer, GameObject[,] backgroundLayer, float size, float margin, int width, int height)
    {
        pathWallFog = new ArrayList();
        foreach (PathNode pathnode in nodes)
        {
            pathnode.CreateFog(transformParent, originRoom.fogPrefab, gridFogLayer, size, margin);

            // Creating fog for path walls
            var x = pathnode.x;
            var y = pathnode.y;
            CreateAdjacentFogTiles(transformParent, gridFogLayer, x, y, backgroundLayer, width, height, size, margin);
            /*
            foreach (TileController controller in GetAdjacentControllers(x, y, backgroundLayer, width, height))
            {
                if (controller is WallController) {
                    var fog = UnityEngine.Object.Instantiate(originRoom.fogPrefab, new Vector3((x) * (size + margin), (y) * (size + margin), -2), Quaternion.identity, transformParent);
                    fog.GetComponent<TileController>().Init(size - margin * 2);
                    gridFogLayer[x, y] = fog;
                    pathWallFog.Add(fog);
                }
            }*/
        }
    }

    private void CreateAdjacentFogTiles(Transform transformParent, GameObject[,] gridFogLayer, int x, int y, GameObject[,] backgroundLayer, int width, int height, float size, float margin)
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
                else
                {
                    //controllers[index] = backgroundLayer[xi, yi].GetComponent<TileController>();
                    if (backgroundLayer[xi, yi].GetComponent<TileController>() is WallController)
                    {
                        var fog = UnityEngine.Object.Instantiate(originRoom.fogPrefab, new Vector3((xi) * (size + margin), (yi) * (size + margin), -2), Quaternion.identity, transformParent);
                        fog.GetComponent<TileController>().Init(size - margin * 2);
                        gridFogLayer[xi, yi] = fog;
                        pathWallFog.Add(fog);
                    }
                }

                index++;

            }
        }

    }




        /* GetAdjacentControllers will return the adjacent tiles. 
         * Perform this function on each tile, if the tile is a walltile then
         *		Check if the gridcontroller fog array contains a fogtile, if so
         *			Set the fogtile as permanently inactive
         *		
         * 
         */


        /*
         * Removes the fog for paths connecting to room
         * Called by right-clicking the room
         */
    public void ClearFogTiles()
    {
        hidden = false;
        foreach (PathNode pathnode in nodes)
        {
            pathnode.ClearFogTile();
        }
    }

    /*
     * Hides all fog tiles
     * Called when switching to DM view
     */
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
     * Called when switching to player view
     */
    public void ShowFogTiles()
    {
        if (hidden == false) return;
        foreach (PathNode pathnode in nodes)
        {
            pathnode.ShowFogTile();
        }
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