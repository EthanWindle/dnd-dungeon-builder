using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/*
 * Grid full of tiles
 * 
 */
public class GridController : MonoBehaviour{

    public int width;
    public int height;
    public float cellSize; //Cells will always be this size, including spacing.
    public float cellSpacing;
    private GameObject[,] backgroundLayer;
    private GameObject[,] foregroundLayer;

    public GameObject[] rooms;
    public GameObject wallTile;

    public void Awake()
    {
        backgroundLayer = new GameObject[width, height];

        foregroundLayer = new GameObject[width, height];
        

        foreach (GameObject room in rooms)
        {
            room.GetComponent<RoomController>().PlaceRoom(gameObject.transform, backgroundLayer, foregroundLayer, cellSize, cellSpacing);
        }

        List<Vector2> wallLocations = new List<Vector2>();


        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (shouldBeWall(x, y))
                {
                    wallLocations.Add(new Vector2(x, y));
                }
            }
        }

        foreach (Vector2 wallLocation in wallLocations)
        {
            int x = (int)wallLocation.x;
            int y = (int)wallLocation.y;
            GameObject wall = Instantiate(wallTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
            wall.GetComponent<TileController>().Init(cellSize - cellSpacing * 2);
            backgroundLayer[x, y] = wall;
        }

        gameObject.transform.position -= new Vector3(width * cellSize / 2, width * cellSize / 2, 0);
    }

    /*
     * Helpers
     */
    private Vector2 GetWorldLocation(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }


    private bool shouldBeWall(int x, int y)
    {

        if (backgroundLayer[x, y] != null) return false;

        for (int xi = x-1; xi <= x+1; xi++)
        {
            if (xi < 0 || xi >= width) continue;
            for (int yi = y-1; yi <= y+1; yi++)
            {
                if (yi < 0 || yi >= height || (yi == y && xi == x)) continue;
                if (backgroundLayer[xi, yi] != null) return true;
            }
        }

        return false;
    }

}
