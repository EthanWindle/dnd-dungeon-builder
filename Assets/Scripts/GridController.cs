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

    public void Awake()
    {
        backgroundLayer = new GameObject[width, height];

        foregroundLayer = new GameObject[width, height];
        

        foreach (GameObject room in rooms)
        {
            room.GetComponent<RoomController>().PlaceRoom(gameObject.transform, backgroundLayer, foregroundLayer, cellSize, cellSpacing);
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

}
