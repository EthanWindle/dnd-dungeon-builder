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
    public float cellSize;
    private GameObject[,] gridArray;

    public GameObject[] rooms;

    public void Awake()
    {
        gridArray = new GameObject[width, height];

        

        foreach (GameObject room in rooms)
        {
            room.GetComponent<RoomController>().PlaceTiles(gameObject.transform, gridArray, cellSize);
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
